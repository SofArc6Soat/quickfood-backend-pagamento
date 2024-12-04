using Infra.Dto;

namespace Infra.Tests.Dto;

public class PedidoDbTests
{
    [Fact]
    public void PedidoDb_Properties_Should_SetAndGetValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = 123;
        var valorTotal = 456.78m;
        var dataPedido = DateTime.UtcNow;

        // Act
        var pedido = new PedidoDb
        {
            Id = id,
            NumeroPedido = numeroPedido,
            ValorTotal = valorTotal,
            DataPedido = dataPedido
        };

        // Assert
        Assert.Equal(id, pedido.Id);
        Assert.Equal(numeroPedido, pedido.NumeroPedido);
        Assert.Equal(valorTotal, pedido.ValorTotal);
        Assert.Equal(dataPedido, pedido.DataPedido);
    }
}