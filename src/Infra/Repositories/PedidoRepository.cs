using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;

namespace Infra.Repositories
{
    public class PedidoRepository(ApplicationDbContext context) : RepositoryGeneric<PedidoDb>(context), IPedidoRepository
    {

    }
}
