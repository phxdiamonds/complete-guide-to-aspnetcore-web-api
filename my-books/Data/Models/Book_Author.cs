namespace my_books.Data.Models
{
    public class Book_Author
    {
        //here we need to add and identifier
        public int id { get; set; }


        //Navigation Properties for book and author

        public int BookId { get; set; }

        public Book Book { get; set; }



        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
