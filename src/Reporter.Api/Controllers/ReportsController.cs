using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Reporter.Messages.Commands;

namespace Reporter.Api.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private readonly IBusClient _busClient;

        public ReportsController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateReport command)
        {   
            command.Id = Guid.NewGuid();
            await _busClient.PublishAsync(command);

            return Created($"reports/{command.Id}", new object());
        }
    }
}