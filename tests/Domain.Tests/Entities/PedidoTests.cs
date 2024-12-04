using Domain.Entities;

namespace Domain.Tests.Entities;

public class PedidoTests
{
    [Fact]
    public void Pedido_Should_SetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = 123;
        var valorTotal = 456.78m;
        var dataPedido = DateTime.UtcNow;

        // Act
        var pedido = new Pedido(id, numeroPedido, valorTotal, dataPedido);

        // Assert
        Assert.Equal(id, pedido.Id);
        Assert.Equal(numeroPedido, pedido.NumeroPedido);
        Assert.Equal(valorTotal, pedido.ValorTotal);
        Assert.Equal(dataPedido, pedido.DataPedido);
    }

    [Fact]
    public void Pedido_Should_ThrowException_When_NumeroPedidoIsInvalid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = -1; // Invalid value
        var valorTotal = 456.78m;
        var dataPedido = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Pedido(id, numeroPedido, valorTotal, dataPedido));
        Assert.Equal("NumeroPedido deve ser maior que zero. (Parameter 'numeroPedido')", exception.Message);
    }

    [Fact]
    public void Pedido_Should_ThrowException_When_ValorTotalIsInvalid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var numeroPedido = 123;
        var valorTotal = -456.78m; // Invalid value
        var dataPedido = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Pedido(id, numeroPedido, valorTotal, dataPedido));
        Assert.Equal("ValorTotal deve ser maior que zero. (Parameter 'valorTotal')", exception.Message);
    }
}
