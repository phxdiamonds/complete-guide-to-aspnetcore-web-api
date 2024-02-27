using my_books.Data.Models;

namespace my_books.Data.ViewModel
{
    public class CustomActionResultVM
    {
        public Exception Exception { get; set; } // to return some exception

        public Publisher Publisher { get; set; }// to return some data
    }
}
