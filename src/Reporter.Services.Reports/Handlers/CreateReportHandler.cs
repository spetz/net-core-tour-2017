using System;
using System.Threading.Tasks;
using RawRabbit;
using Reporter.Messages.Commands;
using Reporter.Messages.Events;
using Reporter.Services.Reports.Models;
using Reporter.Services.Reports.Repositories;

namespace Reporter.Services.Reports.Handlers
{
    public class CreateReportHandler : ICommandHandler<CreateReport>
    {
        private readonly IBusClient _busClient;
        private readonly IReportRepository _repository;

        public CreateReportHandler(IBusClient busClient, IReportRepository repository)
        {
            _busClient = busClient;
            _repository = repository;
        }

        public async Task HandleAsync(CreateReport command)
        {
            if(string.IsNullOrWhiteSpace(command.Text))
            {
                Console.WriteLine($"Received an empty report with id: '{command.Id}'.");
                await _busClient.PublishAsync(new CreateReportRejected(command.Id, "Report can not be empty."));

                return;
            }
            _repository.Reports.Add(new Report
            {
                Id = command.Id,
                Name = command.Name,
                Text = command.Text
            });
            Console.WriteLine($"Created a new report: '{command.Name}' with id: '{command.Id}'.");
            await _busClient.PublishAsync(new ReportCreated(command.Id, command.Name, command.Text));
        }
    }
}