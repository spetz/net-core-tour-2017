using System;
using System.Threading.Tasks;
using RawRabbit;
using Reporter.Api.Repositories;
using Reporter.Messages.Events;

namespace Reporter.Api.Handlers
{
    public class ReportCreatedHandler : IEventHandler<ReportCreated>
    {
        private readonly IBusClient _busClient;
        private readonly ILogRepository _repository;

        public ReportCreatedHandler(IBusClient busClient, ILogRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(ReportCreated command)
        {
            var message = $"New report was created: '{command.Name}' with id: '{command.Id}'.";
            Console.WriteLine(message);
            _repository.Logs.Add(message);
            await Task.CompletedTask;
        }
    }
}