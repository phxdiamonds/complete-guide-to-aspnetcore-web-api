using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_books.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public string? Custom  { get; set; }
    }
}
