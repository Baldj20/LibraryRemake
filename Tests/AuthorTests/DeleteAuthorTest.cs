using Application.Handlers.AuthorHandlers;
using Application.Interfaces.Services;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Application.Mappers;
using Application.DTO.Request;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tests.Mocks;
using Newtonsoft.Json;

namespace Tests.AuthorTests
{
    public class DeleteAuthorTest
    {
        private readonly IImageService imageService;
        private readonly IValidator<AuthorRequest> aValidator;
        public DeleteAuthorTest()
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
        public async Task DeleteAuthor_Should_Delete_Author_And_His_Books()
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

            var id = Guid.NewGuid();

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
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var handler = new DeleteAuthorHandler(unitOfWork);

                var useCase = new DeleteAuthorUseCase
                {
                    Id = id,
                };

                await handler.Handle(useCase, CancellationToken.None);
            }

            // Assert
            using (var context = new LibraryDbContext(options))
            {
                var unitOfWork = new UnitOfWork(context);

                var deletedAuthor = await unitOfWork.Authors.GetById(id, cancellationToken);

                Assert.Null(deletedAuthor);

                foreach (var book in books)
                {
                    Assert.Null(await unitOfWork.Books.GetByISBN(book.ISBN));
                }
            }
        }
    }
}
