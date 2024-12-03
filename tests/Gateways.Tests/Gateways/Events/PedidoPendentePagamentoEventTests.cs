using Gateways.Dtos.Events;

namespace Gateways.Tests.Gateways.Events;

public class PedidoPendentePagamentoEventTests
{
    [Fact]
    public void PedidoPendentePagamentoEvent_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var pedidoPendentePagamentoEvent = new PedidoPendentePagamentoEvent
        {
            PedidoId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        // Assert
        Assert.Equal("PendentePagamento", pedidoPendentePagamentoEvent.Status);
        Assert.NotEqual(Guid.Empty, pedidoPendentePagamentoEvent.PedidoId);
        Assert.NotEqual(Guid.Empty, pedidoPendentePagamentoEvent.Id);
    }

    [Fact]
    public void PedidoPendentePagamentoEvent_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var id = Guid.NewGuid();
        var status = "OutroStatus";

        // Act
        var pedidoPendentePagamentoEvent = new PedidoPendentePagamentoEvent
        {
            PedidoId = pedidoId,
            Id = id,
            Status = status
        };

        // Assert
        Assert.Equal(pedidoId, pedidoPendentePagamentoEvent.PedidoId);
        Assert.Equal(id, pedidoPendentePagamentoEvent.Id);
        Assert.Equal(status, pedidoPendentePagamentoEvent.Status);
    }
}