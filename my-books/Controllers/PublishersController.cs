﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Data.ViewModel.Authentication;
using my_books.Exceptions;

namespace my_books.Controllers
{
    [Authorize(Roles = UserRoles.Publisher+","+UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    

    
    public class PublishersController : ControllerBase
    {
        private PublishersService _publishersService;

        //inject the ilogger configuration

        private readonly ILogger<PublishersController> _logger;

        public PublishersController(PublishersService publisersService, ILogger<PublishersController> logger)
      
        {
            _publishersService = publisersService;
            _logger = logger;
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
        //public CusotmActionResult GetPublisherById(int id)
        //{
        //    //throw new Exception("This is an Exception that will be handled by middleware");

        //    var _response = _publishersService.GetPublisherById(id);
        //    if(_response != null)
        //    {
        //        // return Ok(_response);

        //        var _responseObj = new CustomActionResultVM()
        //        {
        //            Publisher = _response,
        //        };
        //        return new CusotmActionResult(_responseObj);

        //    }
        //    else
        //    {
        //        //return NotFound();

        //        var _responseObj = new CustomActionResultVM()
        //        {
        //            Exception = new Exception("This is coming from publisher Controller"),
        //        };

        //        return new CusotmActionResult(_responseObj);

        //    }
        //}

        public IActionResult GetPublisherById(int id)
        {
            var _response = _publishersService.GetPublisherById(id);

            if (_response != null)
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

        //Using for Sorting filtering and pagination

        [HttpGet("get-all-publishers")]

        public IActionResult GetAllPublishers(string sortBy,string searchString,int pageNumber)
        {
            // throw new Exception("This is an exception thrown from GetAllPublishers method");
            try
            {
                _logger.LogInformation("This is just a log in GetAllPublishers method");
                var _result = _publishersService.GetAllPublishers(sortBy,searchString,pageNumber);

                return Ok(_result);
            }
            catch (Exception ex)
            {

                return BadRequest("sorry, we could not load the publishers");
            }

        }
        
    }
}
