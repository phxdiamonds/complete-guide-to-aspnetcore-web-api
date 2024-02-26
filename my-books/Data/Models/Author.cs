namespace my_books.Data.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        //Navigation properties, one author can have many books we need to add a join model, here we use Book_Author table
        // here the relationhip of the book and author table is stored in Book_Author table

        public List<Book_Author> Book_Authors { get; set; } //List of Book Author




    }
}
