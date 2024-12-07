using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder
                .HasKey(userbook => new { userbook.UserLogin, userbook.BookISBN });

            builder
                .HasOne(userbook => userbook.User)
                .WithMany(user => user.TakenBooks)
                .HasForeignKey(userbook => userbook.UserLogin);

            builder
                .HasOne(userbook => userbook.Book)
                .WithMany(book => book.TakenBooks)
                .HasForeignKey(userbook => userbook.BookISBN);

            builder
                .Property(userbook => userbook.ReceiptDate)
                .IsRequired();

            builder
                .Property(userbook => userbook.ReturnDate)
                .IsRequired();
        }
    }
}
