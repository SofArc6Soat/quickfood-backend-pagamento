using Cora.Infra.Repository;
using Dapper;
using Infra.Context;
using Infra.Dto;

namespace Infra.Repositories
{
    public class PagamentoRepository(ApplicationDbContext context) : RepositoryGeneric<PagamentoDb>(context), IPagamentoRepository
    {
        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT pedidoId, status, valor, dataPagamento
                FROM Pagamentos
                WHERE PedidoId = @vPedidoId
                FOR JSON PATH";

            var result = await GetDbConnection().QueryFirstOrDefaultAsync<string>(query, new { vPedidoId = pedidoId });

            return !string.IsNullOrEmpty(result) ? result : "[]";
        }
    }
}