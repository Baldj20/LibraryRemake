using Application.DTO.Request;
using Application.Handlers.AuthorHandlers;
using Application.Handlers.BookHandlers;
using Application.Handlers.UserHandlers;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.Interfaces.Handlers.BookHandlers;
using Application.Interfaces.Handlers.UserHandlers;
using Application.Interfaces.Services;
using Application.Validators;
using Domain.Interfaces.Repositories;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace API.ProgramConfiguration
{
    public static class ApplicationServices
    {
        public static void AddApplicationServices(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserBookRepository, UserBookRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IHashPasswordService, HashPasswordService>();

            services.AddScoped<IAddAuthorHandler, AddAuthorHandler>();
            services.AddScoped<IDeleteAuthorHandler, DeleteAuthorHandler>();
            services.AddScoped<IGetAllAuthorsHandler, GetAllAuthorsHandler>();
            services.AddScoped<IGetAuthorBooksHandler, GetAuthorBooksHandler>();
            services.AddScoped<IGetAuthorByIdHandler, GetAuthorByIdHandler>();
            services.AddScoped<IUpdateAuthorHandler, UpdateAuthorHandler>();
            services.AddScoped<IGetPagedAuthorsHandler, GetPagedAuthorsHandler>();

            services.AddScoped<IAddBookHandler, AddBookHandler>();
            services.AddScoped<IDeleteBookHandler, DeleteBookHandler>();
            services.AddScoped<IGetAllBooksHandler, GetAllBooksHandler>();
            services.AddScoped<IGetBookByISBNHandler, GetBookByISBNHandler>();
            services.AddScoped<IUpdateBookHandler, UpdateBookHandler>();
            services.AddScoped<IGetPagedBooksHandler, GetPagedBooksHandler>();

            services.AddScoped<IAddUserHandler, AddUserHandler>();
            services.AddScoped<IDeleteUserHandler, DeleteUserHandler>();
            services.AddScoped<IGetAllUsersHandler, GetAllUsersHandler>();
            services.AddScoped<IGetUserByLoginHandler, GetUserByLoginHandler>();
            services.AddScoped<IUpdateUserHandler, UpdateUserHandler>();
            services.AddScoped<IAuthorizeHandler, AuthorizeHandler>();
            services.AddScoped<IRegisterBookForUserHandler, RegisterBookForUserHandler>();
            services.AddScoped<IGetUserTakenBooksHandler, GetUserTakenBooksHandler>();
            services.AddScoped<IRegisterHandler, RegisterHandler>();
            services.AddScoped<IGetPagedUsersHandler, GetPagedUsersHandler>();

            services.AddScoped<IValidator<AuthorRequest>, AuthorRequestValidator>();
            services.AddScoped<IValidator<BookRequest>, BookRequestValidator>();
            services.AddScoped<IValidator<UserRequest>, UserRequestValidator>();
            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
            services.AddScoped<IValidator<AddUserRequest>, AddUserRequestValidator>();
        }
    }
}
