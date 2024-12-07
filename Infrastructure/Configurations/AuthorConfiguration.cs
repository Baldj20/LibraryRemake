using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .HasKey(author => author.Id);

            builder.
                HasMany(author => author.Books)
                .WithOne(book => book.Author)
                .HasForeignKey(book => book.AuthorId);

            builder
                .Property(author => author.Name)
                .IsRequired();

            builder
                .Property(author => author.Surname)
                .IsRequired();

            builder
                .Property(author => author.BirthDate)
                .IsRequired();

            builder
                .Property(author => author.Country)
                .IsRequired();

            builder
                .Property(author => author.ImagePath)
                .IsRequired();
        }
    }
}
