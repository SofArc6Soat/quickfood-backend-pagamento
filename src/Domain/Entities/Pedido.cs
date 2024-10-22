using Core.Domain.Entities;

namespace Domain.Entities
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int NumeroPedido { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataPedido { get; private set; }

        public Pedido(Guid id, int numeroPedido, decimal valorTotal, DateTime dataCricacao)
        {
            Id = id;
            NumeroPedido = numeroPedido;
            ValorTotal = valorTotal;
            DataPedido = dataCricacao;
        }
    }
}
