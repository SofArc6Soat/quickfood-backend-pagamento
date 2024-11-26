using Core.Domain.Data;
using Infra.Dto;

namespace Infra.Repositories
{
    public interface IPagamentoRepository : IRepositoryGeneric<PagamentoDb>
    {
        Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken);
    }
}
