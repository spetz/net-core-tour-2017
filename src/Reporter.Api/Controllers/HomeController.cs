using Microsoft.AspNetCore.Mvc;

namespace Reporter.Api.Controllers
{
    [Route("")]
    public class HomeController : Controller 
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Welcome to the Reports API!");
        }
    }
}