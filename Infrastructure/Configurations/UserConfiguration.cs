using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(user => user.Login);

            builder
                .HasMany(user => user.RefreshTokens)
                .WithOne(token => token.User)
                .HasForeignKey(token => token.UserLogin);

            builder
                .HasMany(user => user.TakenBooks)
                .WithOne(userbook => userbook.User)
                .HasForeignKey(userbook => userbook.UserLogin);

            builder
                .Property(user => user.HashedPassword)
                .IsRequired();

            builder
                .Property(user => user.Role)
                .IsRequired();
        }
    }
}
