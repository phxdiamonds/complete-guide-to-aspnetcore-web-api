using Newtonsoft.Json;

namespace my_books.Data.ViewModel
{
    public class ErrorVM
    {
        public int StatusCode { get; set; } //return status code
        public string Message { get; set; }// return error message

        public string Path { get; set; } //return path of error where it came from

        public override string ToString()//override the to string method to return json string
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
