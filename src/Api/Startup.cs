using Api.Configuration;
using Controllers.DependencyInjection;
using Core.WebApi.Configurations;
using Core.WebApi.DependencyInjection;
using Gateways.DependencyInjection;
using Infra.Context;
using System.Diagnostics.CodeAnalysis;
using Worker.DependencyInjection;
using static Gateways.DependencyInjection.ServiceCollectionExtensions;

namespace Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = EnvironmentConfig.ConfigureEnvironment(services, _configuration);

            var jwtBearerConfigureOptions = new JwtBearerConfigureOptions
            {
                Authority = settings.CognitoSettings.Authority,
                MetadataAddress = settings.CognitoSettings.MetadataAddress
            };

            services.AddApiDefautConfig(jwtBearerConfigureOptions);

            services.AddHealthCheckConfig(settings.ConnectionStrings.DefaultConnection);

            services.AddControllerDependencyServices();

            var queues = new Queues
            {
                QueuePedidoPagoEvent = settings.AwsSqsSettings.QueuePedidoPagoEvent,
                QueuePedidoPendentePagamentoEvent = settings.AwsSqsSettings.QueuePedidoPendentePagamentoEvent
            };

            services.AddGatewayDependencyServices(settings.ConnectionStrings.DefaultConnection, queues);

            var workerQueues = new WorkerQueues
            {
                QueuePedidoCriadoEvent = settings.AwsSqsSettings.QueuePedidoCriadoEvent
            };

            services.AddWorkerDependencyServices(workerQueues);
        }

        public static void Configure(IApplicationBuilder app, ApplicationDbContext context)
        {
            DatabaseMigratorBase.MigrateDatabase(context);

            app.UseApiDefautConfig();
        }
    }
}