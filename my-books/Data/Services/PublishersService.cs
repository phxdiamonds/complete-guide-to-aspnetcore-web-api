using my_books.Data.Models;
using my_books.Data.ViewModel;
using my_books.Exceptions;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;

        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public Publisher AddPublisher(PublisherVM publisher)
        {
            if(StringStartsWithNumber(publisher.Name))throw new PublisherNameException("Publisher name start with number", publisher.Name) ;
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };

            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;
        }

        public PublisherWithBookAndAuthorVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId).Select(n => new PublisherWithBookAndAuthorVM()
            {
                Name = n.Name,
                BookAuthors = n.Books.Select(n => new BookAuthorsVM()
                {
                    BookName = n.Title,
                    BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).ToList()
            }).FirstOrDefault();

            return _publisherData;

        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);

            if(_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The Publisher with Id: {id} does not exist");
            }


        }

        //it goes and find out the publisher data otherwise return null
        public Publisher GetPublisherById(int id)
        {
            return _context.Publishers.FirstOrDefault(n => n.Id == id);
        }

        //creating a new method to check whether the publsher name is starts with number or data before adding to database

        private bool StringStartsWithNumber(string name)
        {
           if( Regex.IsMatch(name, @"^\d")) 
                return true;
            return false;
        }



    }
}
