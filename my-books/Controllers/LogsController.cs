using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModel.Authentication;

namespace my_books.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        //Injdec the service

        private LogsService _logsService;

        public LogsController(LogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet("get-all-logs")]
        public IActionResult GetAllLogs()
        {
            try
            {
                var allLogs = _logsService.GetAllLogsFromDb();

                return Ok(allLogs);
            }
            catch (Exception ex)
            {
                return BadRequest("Could Not load logs from Database."+ex.Message);
            }
        }


    }
}
