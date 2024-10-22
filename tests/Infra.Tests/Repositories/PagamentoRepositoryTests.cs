using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories
{
    public class PagamentoRepositoryTests
    {
        [Fact]
        public async Task DeveObterPagamentoPorPedidoComSucesso()
        {
            // Arrange
            var mockRepository = new Mock<IPagamentoRepository>();
            var pedidoId = Guid.NewGuid();
            var jsonResult = "[{\"pedidoId\": \"12345\", \"status\": \"Pago\", \"valor\": 100.00, \"dataPagamento\": \"2024-01-01T00:00:00\"}]";

            mockRepository.Setup(repo => repo.ObterPagamentoPorPedidoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jsonResult);

            // Act
            var result = await mockRepository.Object.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("pedidoId", result);
        }
    }
}