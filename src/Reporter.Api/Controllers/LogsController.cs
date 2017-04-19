using Microsoft.AspNetCore.Mvc;
using Reporter.Api.Repositories;

namespace Reporter.Api.Controllers
{
    [Route("[controller]")]
    public class LogsController : Controller
    {
        private readonly ILogRepository _repository;

        public LogsController(ILogRepository repository)
        {
           _repository = repository;
        }

        [HttpGet]
        public IActionResult Get() => Json(_repository.Logs);        
    }
}