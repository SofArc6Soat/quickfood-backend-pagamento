using Core.Infra.MessageBroker;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worker.Dtos.Events;

namespace Worker.BackgroundServices
{
    public class PedidoCriadoBackgroundService(ISqsService<PedidoCriadoEvent> sqsClient, IServiceScopeFactory serviceScopeFactory, ILogger<PedidoCriadoBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessMessageAsync(await sqsClient.ReceiveMessagesAsync(stoppingToken), stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing messages.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessMessageAsync(PedidoCriadoEvent? message, CancellationToken cancellationToken)
        {
            if (message is not null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

                var pedidoExistente = await pedidoRepository.FindByIdAsync(message.Id, cancellationToken);

                if (pedidoExistente is null)
                {
                    await pedidoRepository.InsertAsync(ConvertMessageToDb(message), cancellationToken);
                    await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
                }
            }
        }

        private static PedidoDb ConvertMessageToDb(PedidoCriadoEvent message) =>
            new()
            {
                Id = message.Id,
                NumeroPedido = message.NumeroPedido,
                ValorTotal = message.ValorTotal,
                DataPedido = message.DataPedido
            };
    }
}
