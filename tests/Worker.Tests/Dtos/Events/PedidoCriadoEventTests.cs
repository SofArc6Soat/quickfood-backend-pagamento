using Worker.Dtos.Events;

namespace Worker.Tests.Dtos.Events;

public class PedidoCriadoEventTests
{
    [Fact]
    public void PedidoCriadoEvent_DeveInicializarComValoresCorretos()
    {
        // Arrange
        var numeroPedido = 123;
        var clienteId = Guid.NewGuid();
        var status = "Criado";
        var valorTotal = 100.50m;
        var dataPedido = DateTime.UtcNow;

        // Act
        var pedidoCriadoEvent = new PedidoCriadoEvent
        {
            NumeroPedido = numeroPedido,
            ClienteId = clienteId,
            Status = status,
            ValorTotal = valorTotal,
            DataPedido = dataPedido
        };

        // Assert
        Assert.Equal(numeroPedido, pedidoCriadoEvent.NumeroPedido);
        Assert.Equal(clienteId, pedidoCriadoEvent.ClienteId);
        Assert.Equal(status, pedidoCriadoEvent.Status);
        Assert.Equal(valorTotal, pedidoCriadoEvent.ValorTotal);
        Assert.Equal(dataPedido, pedidoCriadoEvent.DataPedido);
    }

    [Fact]
    public void PedidoCriadoEvent_DeveTerValoresPadraoCorretos()
    {
        // Act
        var pedidoCriadoEvent = new PedidoCriadoEvent();

        // Assert
        Assert.Equal(0, pedidoCriadoEvent.NumeroPedido);
        Assert.Null(pedidoCriadoEvent.ClienteId);
        Assert.Equal(string.Empty, pedidoCriadoEvent.Status);
        Assert.Equal(0m, pedidoCriadoEvent.ValorTotal);
        Assert.Equal(default(DateTime), pedidoCriadoEvent.DataPedido);
    }

    [Fact]
    public void PedidoCriadoEvent_DevePermitirValoresNulos()
    {
        // Arrange
        var pedidoCriadoEvent = new PedidoCriadoEvent
        {
            ClienteId = null,
            Status = null
        };

        // Assert
        Assert.Null(pedidoCriadoEvent.ClienteId);
        Assert.Null(pedidoCriadoEvent.Status);
    }
}