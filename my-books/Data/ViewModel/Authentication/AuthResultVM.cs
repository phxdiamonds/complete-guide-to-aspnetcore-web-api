namespace my_books.Data.ViewModel.Authentication
{
    public class AuthResultVM
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
