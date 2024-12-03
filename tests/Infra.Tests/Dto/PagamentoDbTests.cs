using Infra.Dto;

namespace Infra.Tests.Dto;

public class PagamentoDbTests
{
    [Fact]
    public void PagamentoDb_DeveInicializarComValoresPadrao()
    {
        // Arrange & Act
        var pagamento = new PagamentoDb();

        // Assert
        Assert.Equal(Guid.Empty, pagamento.PedidoId);
        Assert.Equal(string.Empty, pagamento.Status);
        Assert.Equal(0m, pagamento.Valor);
        Assert.Null(pagamento.QrCodePix);
        Assert.Equal(default(DateTime), pagamento.DataPagamento);
        Assert.Null(pagamento.Pedido);
    }

    [Fact]
    public void PagamentoDb_DevePermitirDefinirValores()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var status = "Pago";
        var valor = 100.50m;
        var qrCodePix = "QRCode123";
        var dataPagamento = DateTime.Now;
        var pedido = new PedidoDb
        {
            Id = Guid.NewGuid(),
            NumeroPedido = 123,
            ValorTotal = 200.75m,
            DataPedido = DateTime.Now
        };

        // Act
        var pagamento = new PagamentoDb
        {
            PedidoId = pedidoId,
            Status = status,
            Valor = valor,
            QrCodePix = qrCodePix,
            DataPagamento = dataPagamento,
            Pedido = pedido
        };

        // Assert
        Assert.Equal(pedidoId, pagamento.PedidoId);
        Assert.Equal(status, pagamento.Status);
        Assert.Equal(valor, pagamento.Valor);
        Assert.Equal(qrCodePix, pagamento.QrCodePix);
        Assert.Equal(dataPagamento, pagamento.DataPagamento);
        Assert.Equal(pedido, pagamento.Pedido);
    }

    [Fact]
    public void PagamentoDb_DeveLancarExcecaoParaValoresInvalidos()
    {
        // Arrange
        var pagamento = new PagamentoDb();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => pagamento.Status = null);
        Assert.Throws<ArgumentOutOfRangeException>(() => pagamento.Valor = -1);
    }
}
