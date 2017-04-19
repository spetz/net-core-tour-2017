using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RawRabbit;
using RawRabbit.vNext;
using Reporter.Api.Framework;
using Reporter.Api.Handlers;
using Reporter.Api.Repositories;
using Reporter.Messages.Events;

namespace Reporter.Api
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
            services.AddSingleton<ILogRepository, LogRepository>();
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
            services.AddScoped<IEventHandler<ReportCreated>, ReportCreatedHandler>(); 
            services.AddScoped<IEventHandler<CreateReportRejected>, CreateReportRejectedHandler>();              
        }
        
        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();
            var reportCreatedHandler = app.ApplicationServices.GetService<IEventHandler<ReportCreated>>();
            var createReportRejectedHandler = app.ApplicationServices.GetService<IEventHandler<CreateReportRejected>>();
            rabbitMqClient.SubscribeAsync<ReportCreated>(async (msg, context) 
                => await reportCreatedHandler.HandleAsync(msg));  
            rabbitMqClient.SubscribeAsync<CreateReportRejected>(async (msg, context) 
                => await createReportRejectedHandler.HandleAsync(msg));                           
        }
    }
}
