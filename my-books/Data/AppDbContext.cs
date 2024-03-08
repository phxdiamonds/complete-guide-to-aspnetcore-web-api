using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using my_books.Data.Models;

namespace my_books.Data
{
    //this is the database context which inherits from DbContext
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //this is the bridge between the c#  modelsclasses and the sql database tables
        //this is a constructor

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  //here bass is the dbcontext class
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Here we have defined the relationship between the book and the book author
            modelBuilder.Entity<Book_Author>().HasOne(b=>b.Book).WithMany(ba => ba.Book_Authors).HasForeignKey(b => b.BookId);

            //Here we have defined the relationship between the author and the book author

            modelBuilder.Entity<Book_Author>().HasOne(b => b.Author).WithMany(ba => ba.Book_Authors).HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Log>().HasKey(n => n.Id); //id is unique identifier

            base.OnModelCreating(modelBuilder);
        }

        //Need to define the tables names for the c# models

        public DbSet<Book> Books { get; set; } //We are using this books name for getting and setting from the database

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book_Author> Book_Authors { get; set; }

        public DbSet<Publisher  > Publishers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    object value = optionsBuilder.UseSqlServer(@"Server=(localdb)\ProjectModels;Database=RealEstateDb;");
        //}

        public DbSet<Log> Logs { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; } //>
    }
}
