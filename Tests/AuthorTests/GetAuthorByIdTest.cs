using Application.DTO.Request;
using Application.Exceptions;
using Application.Handlers.AuthorHandlers;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.UseCases.AuthorUseCases;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Mocks;
using Xunit;
using Newtonsoft.Json;
using Application.DTO.Response;

namespace Tests.AuthorTests
{
    public class GetAuthorByIdTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;

        public GetAuthorByIdTest()
        {
            var services = new ServiceCollection();

            services.AddScoped<IValidator<AuthorRequest>, AuthorRequestValidator>();

            var serviceProvider = services.BuildServiceProvider();

            imageService = ImageServiceMock.Get().Object;
            aValidator = serviceProvider.GetRequiredService<IValidator<AuthorRequest>>();
        }
        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AuthorProfile());
                cfg.AddProfile(new BookProfile());
            });

            return config.CreateMapper();
        }

        [Fact]
        public async Task Get_Author_By_Id_Should_Return_Author()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var id = Guid.NewGuid();

            var books = new List<BookRequest>()
            {
                new BookRequest
                {
                   ISBN = "978-3-16-148410-0",
                   Title = "Financial literacy",
                   Genre = "Self-development",
                   Description = "",
                   Count = 50
                },
                new BookRequest
                {
                   ISBN = "978-0-307-74172-3",
                   Title = "Financial literacy vol.2",
                   Genre = "Self-development",
                   Description = "",
                   Count = 30
                }
            };

            var booksJson = JsonConvert.SerializeObject(books, Formatting.Indented);

            var newAuthor = new AuthorRequest
            {
                Name = "Andrey",
                Surname = "Zaitsev",
                BirthDate = new DateTime(2004, 4, 13),
                Country = "Belarus",
                BooksJson = booksJson,
                Image = ImageFileMock.Get().Object
            };

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var useCase = new AddAuthorUseCase
                {
                    AuthorRequest = newAuthor
                };

                var handler = new AddAuthorHandler(aValidator, unitOfWork, CreateMapper(), imageService);
                await handler.Handle(useCase, CancellationToken.None);

                id = (await unitOfWork.Authors.GetAll(CancellationToken.None)).FirstOrDefault().Id;
            }

            // Act
            var author = new AuthorResponse();

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new GetAuthorByIdHandler(unitOfWork, CreateMapper());

                var useCase = new GetAuthorByIdUseCase
                {
                    Id = id,
                };

                author = await handler.Handle(useCase, CancellationToken.None);
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                Assert.Equal(author.Books.Count, books.Count);

                Assert.Equal(author.Name, newAuthor.Name);
                Assert.Equal(author.Surname, newAuthor.Surname);
                Assert.Equal(author.BirthDate, newAuthor.BirthDate);
                Assert.Equal(author.Country, newAuthor.Country);

                for (int i = 0; i < author.Books.Count; i++)
                {
                    Assert.Equal(author.Books.ToList()[i].Title, books[i].Title);
                    Assert.Equal(author.Books.ToList()[i].Genre, books[i].Genre);
                    Assert.Equal(author.Books.ToList()[i].Description, books[i].Description);
                    Assert.Equal(author.Books.ToList()[i].Count, books[i].Count);
                }
            }
        }

        [Fact]
        public async Task Get_Author_By_Id_Should_Return_Null()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var id = Guid.NewGuid();

            // Act & Assert

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new GetAuthorByIdHandler(unitOfWork, CreateMapper());

                var useCase = new GetAuthorByIdUseCase
                {
                    Id = id,
                };

                var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
                    await handler.Handle(useCase, CancellationToken.None));
            }
        }
    }
}
