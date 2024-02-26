using my_books.Data.Models;
using my_books.Data.ViewModel;
using static my_books.Data.ViewModel.AuthorVM;

namespace my_books.Data.Services
{
    //Inject the AppdbContext file, to add the data to the database

    public class AuthorsService
    {
        private AppDbContext _context;

        public AuthorsService(AppDbContext context)
        {
            _context = context;
        }
        

        public void AddAuthor(AuthorVM author)
        {
            var _author = new Author()
            {
                FullName = author.FullName
            };

            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public AuthorwithBooksVM GetAuthorwithBooks(int authorId)
        {
            var author = _context.Authors.Where(n => n.Id == authorId).Select(n => new AuthorwithBooksVM()
            {
                FullName = n.FullName,
                BookTitles = n.Book_Authors.Select(n => n.Book.Title).ToList()
            }).FirstOrDefault();

            return author;

        }
    }
}
