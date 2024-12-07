using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasKey(book => book.ISBN);

            builder
                .HasMany(book => book.TakenBooks)
                .WithOne(userbook => userbook.Book)
                .HasForeignKey(userbook => userbook.BookISBN);

            builder
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId);

            builder
                .Property(book => book.Title)
                .IsRequired();

            builder
                .Property(book => book.Genre)
                .IsRequired();

            builder
                .Property(book => book.Description)
                .IsRequired();

            builder
                .Property(book => book.Count)
                .IsRequired();
        }
    }
}
