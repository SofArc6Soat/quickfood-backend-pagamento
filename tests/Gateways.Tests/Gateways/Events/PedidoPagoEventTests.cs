using Gateways.Dtos.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateways.Tests.Gateways.Events;

public class PedidoPagoEventTests
{
    [Fact]
    public void PedidoPagoEvent_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "Pago";

        // Act
        var pedidoPagoEvent = new PedidoPagoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        // Assert
        Assert.Equal(id, pedidoPagoEvent.Id);
        Assert.Equal(pedidoId, pedidoPagoEvent.PedidoId);
        Assert.Equal(status, pedidoPagoEvent.Status);
    }

    [Fact]
    public void PedidoPagoEvent_ShouldBeEqual_WhenPropertiesAreSame()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "Pago";

        var pedidoPagoEvent1 = new PedidoPagoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        var pedidoPagoEvent2 = new PedidoPagoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        // Act & Assert
        Assert.Equal(pedidoPagoEvent1, pedidoPagoEvent2);
    }

    [Fact]
    public void PedidoPagoEvent_ShouldNotBeEqual_WhenPropertiesAreDifferent()
    {
        // Arrange
        var pedidoPagoEvent1 = new PedidoPagoEvent
        {
            Id = Guid.NewGuid(),
            PedidoId = Guid.NewGuid(),
            Status = "Pago"
        };

        var pedidoPagoEvent2 = new PedidoPagoEvent
        {
            Id = Guid.NewGuid(),
            PedidoId = Guid.NewGuid(),
            Status = "Pago"
        };

        // Act & Assert
        Assert.NotEqual(pedidoPagoEvent1, pedidoPagoEvent2);
    }

    [Fact]
    public void PedidoPagoEvent_ShouldBeImmutable()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "Pago";

        var pedidoPagoEvent = new PedidoPagoEvent
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status
        };

        // Act
        var newPedidoPagoEvent = pedidoPagoEvent with { Status = "NovoStatus" };

        // Assert
        Assert.Equal(id, pedidoPagoEvent.Id);
        Assert.Equal(pedidoId, pedidoPagoEvent.PedidoId);
        Assert.Equal(status, pedidoPagoEvent.Status);
        Assert.Equal("NovoStatus", newPedidoPagoEvent.Status);
    }
}
