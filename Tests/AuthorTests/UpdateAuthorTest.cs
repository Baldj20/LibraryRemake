using Application.DTO.Request;
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

namespace Tests.AuthorTests
{
    public class UpdateAuthorTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;
        public UpdateAuthorTest()
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
        public async Task Update_Author_Should_Return_Updated_Author()
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

            var cancellationToken = CancellationToken.None;

            // Act

            var updatedBooks = new List<BookRequest>()
            {
                new BookRequest
                {
                    ISBN = "978-3-16-148410-0",
                    Title = "Coffee business",
                    Genre = "Self-development",
                    Description = "",
                    Count = 150
                },
            };

            var authorWithUpdatedInfo = new AuthorRequest
            {
                Name = "Dmitry",
                Surname = "Zaitsev",
                BirthDate = new DateTime(2004, 1, 12),
                Country = "Belarus",
                BooksJson = JsonConvert.SerializeObject(updatedBooks, Formatting.Indented),
            };

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new UpdateAuthorHandler(unitOfWork, CreateMapper(), aValidator, imageService);

                var useCase = new UpdateAuthorUseCase
                {
                    Id = id,
                    AuthorRequest = authorWithUpdatedInfo,
                };

                await handler.Handle(useCase, CancellationToken.None);
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var updatedAuthor = await unitOfWork.Authors.GetByIdWithBooks(id, cancellationToken);                

                Assert.Equal(updatedBooks.Count, updatedAuthor.Books.Count);

                Assert.Equal(authorWithUpdatedInfo.Name, updatedAuthor.Name);
                Assert.Equal(authorWithUpdatedInfo.Surname, updatedAuthor.Surname);
                Assert.Equal(authorWithUpdatedInfo.BirthDate, updatedAuthor.BirthDate);
                Assert.Equal(authorWithUpdatedInfo.Country, updatedAuthor.Country);

                for (int i = 0; i < updatedBooks.Count; i++)
                {
                    Assert.Equal(updatedBooks[i].Title, updatedAuthor.Books.ToList()[i].Title);
                    Assert.Equal(updatedBooks[i].Genre, updatedAuthor.Books.ToList()[i].Genre);
                    Assert.Equal(updatedBooks[i].Description, updatedAuthor.Books.ToList()[i].Description);
                    Assert.Equal(updatedBooks[i].Count, updatedAuthor.Books.ToList()[i].Count);
                }
            }
        }
    }
}
