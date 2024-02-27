using Microsoft.AspNetCore.Mvc;
using my_books.Data.ViewModel;

namespace my_books.ActionResults
{
    public class CusotmActionResult : IActionResult
    {
        private readonly CustomActionResultVM _result;

        public CusotmActionResult(CustomActionResultVM result)
        {
              _result = result;  
        }
        
        public async Task ExecuteResultAsync(ActionContext context)
        {
            //Create an object result

            var objectResult = new ObjectResult(_result.Exception ?? _result.Publisher as object)
            {
                StatusCode = _result.Exception != null ? StatusCodes.Status500InternalServerError : StatusCodes.Status200OK
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
