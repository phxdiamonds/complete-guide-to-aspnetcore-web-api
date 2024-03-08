using System.ComponentModel.DataAnnotations.Schema;

namespace my_books.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRovoked { get; set; } //used to remove old refresh tokens and generate new onew

        public DateTime DateAdded { get; set; }

        public DateTime DateExpire { get; set; }

        //keeping reference to Application user table

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }



    }
}
