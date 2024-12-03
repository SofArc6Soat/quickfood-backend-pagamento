using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Entities
{
    public class PagamentoTests
    {
        [Fact]
        public void Pagamento_Should_SetPropertiesCorrectly_WithMinimalConstructor()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 123.45m;

            // Act
            var pagamento = new Pagamento(pedidoId, valor);

            // Assert
            Assert.Equal(pedidoId, pagamento.PedidoId);
            Assert.Equal(valor, pagamento.Valor);
            Assert.Equal(DateTime.Now.Date, pagamento.DataPagamento.Date);
            Assert.NotEqual(Guid.Empty, pagamento.Id); // Id should be set to a new Guid
        }

        [Fact]
        public void Pagamento_Should_SetPropertiesCorrectly_WithFullConstructor()
        {
            // Arrange
            var id = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var status = StatusPagamento.Pendente;
            var valor = 123.45m;
            var qrCodePix = "QRCode123";
            var dataCriacao = DateTime.UtcNow;

            // Act
            var pagamento = new Pagamento(id, pedidoId, status, valor, qrCodePix, dataCriacao);

            // Assert
            Assert.Equal(id, pagamento.Id);
            Assert.Equal(pedidoId, pagamento.PedidoId);
            Assert.Equal(status, pagamento.Status);
            Assert.Equal(valor, pagamento.Valor);
            Assert.Equal(qrCodePix, pagamento.QrCodePix);
            Assert.Equal(dataCriacao, pagamento.DataPagamento);
        }

        [Fact]
        public void AtribuirQrCodePix_Should_SetQrCodePix()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), 123.45m);
            var qrCodePix = "QRCode123";

            // Act
            pagamento.AtribuirQrCodePix(qrCodePix);

            // Assert
            Assert.Equal(qrCodePix, pagamento.QrCodePix);
        }

        [Fact]
        public void AlterarStatusPagamentoParaPendente_Should_SetStatusToPendente()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), 123.45m);

            // Act
            pagamento.AlterarStatusPagamentoParaPendente();

            // Assert
            Assert.Equal(StatusPagamento.Pendente, pagamento.Status);
        }

        [Fact]
        public void AlterarStatusPagamentoParaPago_Should_SetStatusToPago()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), 123.45m);

            // Act
            pagamento.AlterarStatusPagamentoParaPago();

            // Assert
            Assert.Equal(StatusPagamento.Pago, pagamento.Status);
        }
    }
}