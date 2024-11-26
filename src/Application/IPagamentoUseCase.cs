namespace UseCases
{
    public interface IPagamentoUseCase
    {
        Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken);
        Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken);
        Task<bool> NotificarPagamentoAsync(Guid pedidoId, CancellationToken cancellationToken);
    }
}