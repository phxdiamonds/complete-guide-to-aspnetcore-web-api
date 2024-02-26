using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Exceptions;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    

    
    public class PublishersController : ControllerBase
    {
        private PublishersService _publishersService;

        public PublishersController(PublishersService publisersService)
      
        {
            _publishersService = publisersService;
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
           
            try
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch(PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher Name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-book-with-authors/{id}")]

        public IActionResult GetPublishersData(int id)
        {
            var _response = _publishersService.GetPublisherData(id);
            return Ok(_response);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            //throw new Exception("This is an Exception that will be handled by middleware");

            var _response = _publishersService.GetPublisherById(id);
            if(_response != null)
            {
                return Ok(_response);
            }
            else
            {
                return NotFound();
            }
        }



        [HttpDelete("delete-publisher-by-id/{id}")]

        public IActionResult DeletePublisherById(int id)
        {
            

            try
            {
 
                _publishersService.DeletePublisherById(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
    }
}
