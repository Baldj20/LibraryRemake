using Application.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Tests.Mocks;
using Xunit;
using Application.DTO.Request;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Application.DTO.Response;

namespace Tests.AuthorTests
{
    public class GetAuthorBooksTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;

        public GetAuthorBooksTest()
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
        public async Task GetAuthorBooks_Should_Return_Author_Books()
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
                var handler = new AddAuthorHandler(aValidator, unitOfWork, CreateMapper(), imageService);

                var useCase = new AddAuthorUseCase
                {
                    AuthorRequest = newAuthor
                };

                await handler.Handle(useCase, CancellationToken.None);

                id = (await unitOfWork.Authors.GetAll(CancellationToken.None)).FirstOrDefault().Id;
            }

            // Act
            var authorBooks = new List<BookResponse>();

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var getAuthorBooksUseCase = new GetAuthorBooksHandler(unitOfWork, CreateMapper());

                var useCase = new GetAuthorBooksUseCase
                {
                    Id = id,
                };

                authorBooks = (await getAuthorBooksUseCase.Handle(useCase, CancellationToken.None)).ToList();
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                Assert.Equal(authorBooks.Count, books.Count);

                foreach (var book in books)
                {
                    Assert.Contains(authorBooks, authorBook =>
                    authorBook.Title == book.Title &&
                    authorBook.Genre == book.Genre &&
                    authorBook.Description == book.Description &&
                    authorBook.Count == book.Count);
                }
            }
        }
    }
}
