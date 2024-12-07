using Application.DTO.Request;
using Application.Handlers.AuthorHandlers;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.UseCases.AuthorUseCases;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Tests.Mocks;
using Xunit;

namespace Tests.AuthorTests
{
    public class AddAuthorTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;

        public AddAuthorTest()
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
        public async Task AddAuthor_Should_Add_New_Author()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

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

            // Act
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new AddAuthorHandler(aValidator, unitOfWork, CreateMapper(), imageService);

                var useCase = new AddAuthorUseCase
                {
                    AuthorRequest = newAuthor
                };

                await handler.Handle(useCase, CancellationToken.None);
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var addedAuthor = (await unitOfWork.Authors.GetAll(CancellationToken.None)).FirstOrDefault();

                Assert.NotNull(addedAuthor);
                Assert.Equal(newAuthor.Name, addedAuthor.Name);
                Assert.Equal(newAuthor.Surname, addedAuthor.Surname);
                Assert.Equal(newAuthor.BirthDate, addedAuthor.BirthDate);
                Assert.Equal(newAuthor.Country, addedAuthor.Country);

                for (int i = 0; i < addedAuthor.Books.Count; i++)
                {
                    Assert.Equal(addedAuthor.Books.ToList()[i].ISBN, books[i].ISBN);
                    Assert.Equal(addedAuthor.Books.ToList()[i].Title, books[i].Title);
                    Assert.Equal(addedAuthor.Books.ToList()[i].Genre, books[i].Genre);
                    Assert.Equal(addedAuthor.Books.ToList()[i].Description, books[i].Description);
                    Assert.Equal(addedAuthor.Books.ToList()[i].Count, books[i].Count);
                }
            }
        }
    }
}
