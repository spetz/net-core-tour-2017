using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.vNext;
using Reporter.Messages.Commands;
using Reporter.Services.Reports.Framework;
using Reporter.Services.Reports.Handlers;
using Reporter.Services.Reports.Repositories;

namespace Reporter.Services.Reports
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);
            services.AddSingleton<IReportRepository, ReportRepository>();
            ConfigureRabbitMqServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureRabbitMqSubscriptions(app);
            app.UseMvc();
        }

        private void ConfigureRabbitMqServices(IServiceCollection services)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("rabbitmq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);
            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);

            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);
            services.AddScoped<ICommandHandler<CreateReport>, CreateReportHandler>();               
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();
            var createReportHandler = app.ApplicationServices.GetService<ICommandHandler<CreateReport>>();
            rabbitMqClient.SubscribeAsync<CreateReport>(async (msg, context) 
                => await createReportHandler.HandleAsync(msg));          
        }
    }
}
