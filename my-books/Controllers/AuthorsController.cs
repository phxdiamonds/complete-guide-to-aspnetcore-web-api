using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Data.ViewModel.Authentication;

namespace my_books.Controllers
{
    [Authorize(Roles = UserRoles.Author)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorsService _authorsService;

        public AuthorsController(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AuthorVM author)
        {
            _authorsService.AddAuthor(author);
            return Ok();
        }

        [HttpGet("get-author-with-books/{id}")]

        public IActionResult GetAuthorWithBooks(int id)
        {
           var response = _authorsService.GetAuthorwithBooks(id);
            return Ok(response);
        }
    }
}
