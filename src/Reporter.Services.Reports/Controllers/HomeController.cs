using Microsoft.AspNetCore.Mvc;

namespace Reporter.Services.Reports.Controllers
{
    [Route("")]
    public class HomeController : Controller 
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Welcome to the Reports Service!");
        }
    }
}