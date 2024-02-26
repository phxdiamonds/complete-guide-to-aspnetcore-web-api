using my_books.Data.Models;
using my_books.Data.ViewModel;
using System.ComponentModel;

namespace my_books.Data.Services
{
    public class BooksServices
    {
        //Here we need to inject the AddDbContext file

        //and then configure in start up file

        private readonly AppDbContext _context;
        public BooksServices(AppDbContext context)
        {
            //Here we need to add reference to AddDbContext
            _context = context;
            

        }

        //Here we need to create a method to add books and its void and doent need to return anything

        //the reason im giving BookVm view model is because in model we have propeties like id and dateadded, that user doesnt need to provide
        //so i created a view model for that user doen't need to provide
        public void AddBookwithAuthors(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };

            _context.Books.Add(_book);
            _context.SaveChanges();

            //Adding the relationship between the book and book authors to the Book_Autors table

            foreach(var id in book.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };
                _context.Book_Authors.Add(_book_author);
                _context.SaveChanges();
            }
        }

        //Geting all books

        public List<Book> GetAllBooks()
        {
            var allBooks = _context.Books.ToList();

            return allBooks;
        }

       // public List<Book> GetAllBooks() => _context.Books.ToList();  // we can mention like this also compared to above method

        //Get a book by Id

        public BookWithAuthorsVM GetBookById(int bookId)
        {
            var bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                Publishername = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();

            return bookWithAuthors;
        }

        //Updating a book

        public Book UpdateBookById(int bookId, BookVM book) // here we need two parameters to get book by id and updating only the properties we can
        {
            var updateBook = _context.Books.FirstOrDefault(x=>x.Id == bookId);

            if (updateBook != null)
            {
                updateBook.Title = book.Title;
                updateBook.Description = book.Description;
                updateBook.IsRead = book.IsRead;
                updateBook.DateRead = book.IsRead ? book.DateRead.Value : null;
                updateBook.Rate = book.IsRead ? book.Rate.Value : null;
                updateBook.Genre = book.Genre;
                updateBook.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }

            
            return updateBook;
        }

        //deleting an book by id

        public void DeleteBookById(int bookId)
        {
            var deleteBook = _context.Books.FirstOrDefault(x => x.Id == bookId);
            
            if(deleteBook != null)
            {
                _context.Books.Remove(deleteBook);
                _context.SaveChanges();
            }
        }
    }
}
