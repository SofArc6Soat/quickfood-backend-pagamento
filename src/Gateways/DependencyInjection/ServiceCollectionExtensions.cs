using Amazon.SQS;
using Core.Infra.MessageBroker;
using Gateways.Dtos.Events;
using Infra.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Gateways.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayDependencyServices(this IServiceCollection services, string connectionString, Queues queues)
        {
            services.AddScoped<IPagamentoGateway, PagamentoGateway>();
            services.AddScoped<IPedidoGateway, PedidoGateway>();

            services.AddInfraDependencyServices(connectionString);

            services.AddSingleton<ISqsService<PedidoPagoEvent>>(provider => new SqsService<PedidoPagoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoPagoEvent));
            services.AddSingleton<ISqsService<PedidoPendentePagamentoEvent>>(provider => new SqsService<PedidoPendentePagamentoEvent>(provider.GetRequiredService<IAmazonSQS>(), queues.QueuePedidoPendentePagamentoEvent));
        }

        [ExcludeFromCodeCoverage]
        public record Queues
        {
            public string QueuePedidoPagoEvent { get; set; } = string.Empty;
            public string QueuePedidoPendentePagamentoEvent { get; set; } = string.Empty;
        }
    }
}