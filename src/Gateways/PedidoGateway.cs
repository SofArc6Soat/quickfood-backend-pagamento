using Domain.Entities;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class PedidoGateway(IPedidoRepository pedidoRepository) : IPedidoGateway
    {
        public async Task<Pedido?> ObterPedidoAsync(Guid id, CancellationToken cancellationToken)
        {
            var pedidoDto = await pedidoRepository.FindByIdAsync(id, cancellationToken);

            return pedidoDto is null ? null : new Pedido(pedidoDto.Id, pedidoDto.NumeroPedido, pedidoDto.ValorTotal, pedidoDto.DataPedido);
        }

        public async Task<bool> AtualizarPedidoAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            var pedidoDto = new PedidoDb
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido
            };

            await pedidoRepository.UpdateAsync(pedidoDto, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
