using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities
{
    public class PagamentoTests
    {
        [Fact]
        public void CriarPagamento_DeveCriarComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 100.00m;

            // Act
            var pagamento = new Pagamento(pedidoId, valor);

            // Assert
            Assert.Equal(pedidoId, pagamento.PedidoId);
            Assert.Equal(valor, pagamento.Valor);
            Assert.Equal(DateTime.Now.Date, pagamento.DataPagamento.Date);
        }

        [Fact]
        public void AtribuirQrCodePix_DeveAtribuirQrCode()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), 100.00m);
            var qrCodePix = "1234567890";

            // Act
            pagamento.AtribuirQrCodePix(qrCodePix);

            // Assert
            Assert.Equal(qrCodePix, pagamento.QrCodePix);
        }

        [Fact]
        public void AlterarStatusPagamento_DeveAlterarStatus()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), 100.00m);

            // Act
            pagamento.AlterarStatusPagamentoParaPendente();

            // Assert
            Assert.Equal(StatusPagamento.Pendente, pagamento.Status);

            pagamento.AlterarStatusPagamentoParaPago();

            // Assert
            Assert.Equal(StatusPagamento.Pago, pagamento.Status);
        }
    }
}