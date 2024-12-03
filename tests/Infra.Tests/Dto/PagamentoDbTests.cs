using Infra.Dto;

namespace Infra.Tests.Dto;

public class PagamentoDbTests
{
    [Fact]
    public void PagamentoDb_Properties_Should_SetAndGetValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var status = "Pago";
        var valor = 123.45m;
        var qrCodePix = "QRCode123";
        var dataPagamento = DateTime.UtcNow;

        // Act
        var pagamento = new PagamentoDb
        {
            Id = id,
            PedidoId = pedidoId,
            Status = status,
            Valor = valor,
            QrCodePix = qrCodePix,
            DataPagamento = dataPagamento
        };

        // Assert
        Assert.Equal(id, pagamento.Id);
        Assert.Equal(pedidoId, pagamento.PedidoId);
        Assert.Equal(status, pagamento.Status);
        Assert.Equal(valor, pagamento.Valor);
        Assert.Equal(qrCodePix, pagamento.QrCodePix);
        Assert.Equal(dataPagamento, pagamento.DataPagamento);
    }
}