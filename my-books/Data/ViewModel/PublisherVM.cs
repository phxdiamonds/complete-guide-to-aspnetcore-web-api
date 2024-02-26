namespace my_books.Data.ViewModel
{
    public class PublisherVM
    {
        public string Name { get; set; }
    }

    public class PublisherWithBookAndAuthorVM
    {
        public string Name { get; set; }

        public List<BookAuthorsVM> BookAuthors   { get; set; }
    }


    public class BookAuthorsVM
    {
        public string BookName { get; set; }

        public List<string> BookAuthors { get; set; }
    }
}
