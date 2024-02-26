namespace my_books.Data.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //here we are creating a navigation property that are the model properties that are used to define the relationship between the models

        public List<Book> Books { get; set; }
    }
}
