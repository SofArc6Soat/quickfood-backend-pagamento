using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Gateways.Tests
{
    public class PagamentoGatewayTests
    {
        private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock;
        private readonly PagamentoGateway _pagamentoGateway;

        public PagamentoGatewayTests()
        {
            _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            _pagamentoGateway = new PagamentoGateway(_pagamentoRepositoryMock.Object);
        }

        [Fact]
        public async Task CadastrarPagamentoAsync_DeveCadastrarComSucesso()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), Guid.NewGuid(), StatusPagamento.Pago, 100.00m, "QRCode", DateTime.Now);

            _pagamentoRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _pagamentoRepositoryMock.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            // Act
            var resultado = await _pagamentoGateway.CadastrarPagamentoAsync(pagamento, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task NotificarPagamentoAsync_DeveNotificarComSucesso()
        {
            // Arrange
            var pagamento = new Pagamento(Guid.NewGuid(), Guid.NewGuid(), StatusPagamento.Pago, 100.00m, "QRCode", DateTime.Now);

            _pagamentoRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _pagamentoRepositoryMock.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            // Act
            var resultado = await _pagamentoGateway.NotificarPagamentoAsync(pagamento, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public void ObterPagamentoPorPedido_DeveRetornarPagamento()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pagamentoDb = new PagamentoDb
            {
                Id = Guid.NewGuid(),
                PedidoId = pedidoId,
                Status = StatusPagamento.Pago.ToString(),
                Valor = 100.00m,
                QrCodePix = "QRCode",
                DataPagamento = DateTime.Now
            };

            _pagamentoRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<PagamentoDb, bool>>>(), It.IsAny<CancellationToken>()))
                .Returns(new List<PagamentoDb> { pagamentoDb }.AsQueryable());

            // Act
            var resultado = _pagamentoGateway.ObterPagamentoPorPedido(pedidoId, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.PedidoId.Should().Be(pedidoId);
        }

        [Fact]
        public async Task ObterPagamentoPorPedidoAsync_DeveRetornarJsonValido()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var jsonResult = "[{\"pedidoId\": \"12345\", \"status\": \"Pago\", \"valor\": 100.00, \"dataPagamento\": \"2024-01-01T00:00:00\"}]";

            _pagamentoRepositoryMock.Setup(repo => repo.ObterPagamentoPorPedidoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jsonResult);

            // Act
            var result = await _pagamentoGateway.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain("pedidoId");
        }
    }
}