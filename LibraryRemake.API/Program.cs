using API;
using API.ProgramConfiguration;
using Application.Mappers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryRemake
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddPolicyBasedAuthorization(builder.Configuration);

            builder.Services.AddSwaggerGenConfiguration(builder.Configuration);

            builder.Services.AddValidators(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAutoMapper(typeof(AuthorProfile),
                typeof(BookProfile), typeof(UserBookProfile), typeof(UserProfile));

            builder.Services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
