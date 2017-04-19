using System;
using System.Threading.Tasks;
using RawRabbit;
using Reporter.Api.Repositories;
using Reporter.Messages.Events;

namespace Reporter.Api.Handlers
{
    public class CreateReportRejectedHandler : IEventHandler<CreateReportRejected>
    {
        private readonly IBusClient _busClient;
        private readonly ILogRepository _repository;

        public CreateReportRejectedHandler(IBusClient busClient, ILogRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(CreateReportRejected command)
        {
            var message = $"There was an error when creating a new report with id: '{command.Id}'. {command.Reason}";
            Console.WriteLine(message);
            _repository.Logs.Add(message);
            await Task.CompletedTask;
        }
    }
}