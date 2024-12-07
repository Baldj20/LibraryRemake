using Application.DTO.Request;
using Application.Handlers.AuthorHandlers;
using Application.Interfaces.Services;
using Application.Mappers;
using Application.UseCases.AuthorUseCases;
using Application.Validators;
using AutoMapper;
using Domain;
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
    public class GetPagesAuthorsTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;
        public GetPagesAuthorsTest()
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
        public async Task GetPagedAuthors_Should_Return_One_Author()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();

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
                    ISBN = "978-3-16-148410-1",
                    Title = "Financial literacy",
                    Genre = "Self-development",
                    Description = "",
                    Count = 50
                },
                new BookRequest
                {
                    ISBN = "978-0-307-74172-4",
                    Title = "Financial literacy vol.2",
                    Genre = "Self-development",
                    Description = "",
                    Count = 30
                }
            };

            var books3 = new List<BookRequest>()
            {
                new BookRequest
                {
                    ISBN = "978-3-16-148410-5",
                    Title = "Financial literacy",
                    Genre = "Self-development",
                    Description = "",
                    Count = 50
                },
                new BookRequest
                {
                    ISBN = "978-0-307-74172-9",
                    Title = "Financial literacy vol.2",
                    Genre = "Self-development",
                    Description = "",
                    Count = 30
                }
            };

            var newAuthor1 = new AuthorRequest
            {
                Name = "Andrey",
                Surname = "Zaitsev",
                BirthDate = new DateTime(2004, 4, 13),
                Country = "Belarus",
                BooksJson = JsonConvert.SerializeObject(books1, Formatting.Indented),
            };

            var newAuthor2 = new AuthorRequest
            {
                Name = "Dmitry",
                Surname = "Zaitsev",
                BirthDate = new DateTime(2004, 4, 13),
                Country = "Belarus",
                BooksJson = JsonConvert.SerializeObject(books2, Formatting.Indented)
            };

            var newAuthor3 = new AuthorRequest
            {
                Name = "Roman",
                Surname = "Zaitsev",
                BirthDate = new DateTime(2004, 4, 13),
                Country = "Belarus",
                BooksJson = JsonConvert.SerializeObject(books3, Formatting.Indented)
            };

            var authors = new List<AuthorRequest> { newAuthor1, newAuthor2, newAuthor3 };

            var paginationParams = new PaginationParams
            {
                PageSize = 2,
                PageNumber = 2
            };

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new AddAuthorHandler(aValidator, unitOfWork, CreateMapper(), imageService);

                foreach (var author in authors)
                {
                    var useCase = new AddAuthorUseCase
                    {
                        AuthorRequest = author,
                    };

                    await handler.Handle(useCase, CancellationToken.None);
                }
            }

            var cancellationToken = CancellationToken.None;

            // Act
            var pagedAuthors = new List<AuthorResponse>();

            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new GetPagedAuthorsHandler(unitOfWork, CreateMapper());

                var useCase = new GetPagedAuthorsUseCase
                {
                    PaginationParams = paginationParams,
                };

                pagedAuthors = (await handler.Handle(useCase, CancellationToken.None)).ToList();
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                var result = authors.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Take(paginationParams.PageSize).ToList();
                var resultBooks = result
                    .Select(res => JsonConvert.DeserializeObject<List<BookRequest>>(res.BooksJson))
                    .ToList();

                Assert.Equal(result.Count, pagedAuthors.Count);

                for (int i = 0; i < result.Count; i++)
                {
                    Assert.Equal(result[i].Name, pagedAuthors[i].Name);
                    Assert.Equal(result[i].Surname, pagedAuthors[i].Surname);
                    Assert.Equal(result[i].BirthDate, pagedAuthors[i].BirthDate);
                    Assert.Equal(result[i].Country, pagedAuthors[i].Country);

                    for (int j = 0; j < resultBooks[i].Count; j++)
                    {
                        Assert.Contains(pagedAuthors[i].Books, b =>
                        b.Title == resultBooks[i][j].Title &&
                        b.Genre == resultBooks[i][j].Genre &&
                        b.Description == resultBooks[i][j].Description &&
                        b.Count == resultBooks[i][j].Count);
                    }
                }

            }
        }
    }
}
