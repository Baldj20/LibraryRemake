using Application.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Tests.Mocks;
using Xunit;
using Application.DTO.Request;
using Newtonsoft.Json;
using Application.Mappers;
using Application.Interfaces.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.DTO.Response;

namespace Tests.AuthorTests
{
    public class GetAllAuthorsTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;

        public GetAllAuthorsTest()
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
        public async Task Get_All_Authors_Should_Get_All_Authors()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var books1 = new List<BookRequest>()
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

            var books2 = new List<BookRequest>()
            {
                new BookRequest
                {
                   ISBN = "978-1-4028-9462-6",
                   Title = "Coffee business",
                   Genre = "Self-development",
                   Description = "",
                   Count = 70
                }
            };

            var authorsList = new List<AuthorRequest>
            {
                 new AuthorRequest
                 {
                    Name = "Andrey",
                    Surname = "Zaitsev",
                    BirthDate = new DateTime(2004, 4, 13),
                    Country = "Belarus",
                    BooksJson = JsonConvert.SerializeObject(books1, Formatting.Indented),
                    Image = ImageFileMock.Get().Object
                 },

                 new AuthorRequest
                 {
                    Name = "Dmitry",
                    Surname = "Zaitsev",
                    BirthDate = new DateTime(2004, 1, 12),
                    Country = "Belarus",
                    BooksJson = JsonConvert.SerializeObject(books2, Formatting.Indented),
                    Image = ImageFileMock.Get().Object
                 }
            };

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new AddAuthorHandler(aValidator, unitOfWork, CreateMapper(), imageService);

                foreach (var author in authorsList)
                {
                    var useCase = new AddAuthorUseCase
                    {
                        AuthorRequest = author,
                    };

                    await handler.Handle(useCase, CancellationToken.None);
                }
            }

            // Act
            var list = new List<AuthorResponse>();
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new GetAllAuthorsHandler(unitOfWork, CreateMapper());

                var useCase = new GetAllAuthorsUseCase
                {

                };

                list = (await handler.Handle(useCase, CancellationToken.None)).ToList();
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                Assert.Equal(list.Count, authorsList.Count);

                foreach (var author in authorsList)
                {
                    var books = JsonConvert.DeserializeObject<List<BookRequest>>(author.BooksJson);

                    Assert.Contains(list, response => 
                        response.Name == author.Name &&
                        response.Surname == author.Surname &&
                        response.Country == author.Country &&
                        response.BirthDate == author.BirthDate &&
                        books.All(b => response.Books.Any(book =>
                           book.Title == b.Title &&
                           book.Genre == b.Genre &&
                           book.Description == b.Description &&
                           book.Count == b.Count)));
                }
            }
        }
    }
}
