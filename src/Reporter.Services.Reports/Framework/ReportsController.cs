using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Reporter.Services.Reports.Repositories;

namespace Reporter.Services.Reports.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IReportRepository _repository;

        public ReportsController(IBusClient busClient, IReportRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get() 
            => Json(_repository.Reports.Select(x => new {x.Id, x.Name}));
        
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var report = _repository.Reports.SingleOrDefault(x => x.Id == id);
            if(report == null)
            {
                return NotFound();
            }

            return Json(report);
        }
    }
}