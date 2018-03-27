using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class LibraryContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });

            builder.Entity<BookAuthor>().HasOne(ba => ba.Book).WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            builder.Entity<BookAuthor>().HasOne(ba => ba.Author).WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);
            
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
