using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoDb : Entity
    {
        public int NumeroPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }

        public PedidoDb()
        {
        }
    }
}
