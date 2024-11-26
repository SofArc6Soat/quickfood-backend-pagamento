using Domain.Entities;

namespace Gateways
{
    public interface IPagamentoGateway
    {
        Pagamento? ObterPagamentoPorPedido(Guid pedidoId, CancellationToken cancellationToken);
        Task<bool> CadastrarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken);
        Task<bool> NotificarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken);
        string GerarQrCodePixGatewayPagamento(Pagamento pagamento);

        Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken);
    }
}
