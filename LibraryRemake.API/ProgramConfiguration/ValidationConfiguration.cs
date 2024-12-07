using Application.Validators;
using FluentValidation;

namespace API.ProgramConfiguration
{
    public static class ValidationConfiguration
    {
        public static void AddValidators(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<AuthorRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<BookRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<AddUserRequestValidator>();
        }
    }
}
