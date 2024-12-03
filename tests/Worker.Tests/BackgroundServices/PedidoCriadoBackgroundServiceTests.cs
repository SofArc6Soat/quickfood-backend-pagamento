using Core.Infra.MessageBroker;
using Infra.Dto;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.BackgroundServices;
using Worker.Dtos.Events;

namespace Worker.Tests.BackgroundServices;

public class PedidoCriadoBackgroundServiceTests
{
    private readonly Mock<ISqsService<PedidoCriadoEvent>> _sqsClientMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<ILogger<PedidoCriadoBackgroundService>> _loggerMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly PedidoCriadoBackgroundService _service;

    public PedidoCriadoBackgroundServiceTests()
    {
        _sqsClientMock = new Mock<ISqsService<PedidoCriadoEvent>>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _loggerMock = new Mock<ILogger<PedidoCriadoBackgroundService>>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();

        _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IPedidoRepository))).Returns(_pedidoRepositoryMock.Object);

        _service = new PedidoCriadoBackgroundService(_sqsClientMock.Object, _serviceScopeFactoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveInserirPedidoQuandoJaExiste()
    {
        // Arrange
        var stoppingTokenSource = new CancellationTokenSource();
        var stoppingToken = stoppingTokenSource.Token;
        var pedidoCriadoEvent = new PedidoCriadoEvent { Id = Guid.NewGuid() };
        var pedidoExistente = new PedidoDb { Id = pedidoCriadoEvent.Id };
        _sqsClientMock.Setup(x => x.ReceiveMessagesAsync(stoppingToken)).ReturnsAsync(pedidoCriadoEvent);
        _pedidoRepositoryMock.Setup(x => x.FindByIdAsync(pedidoCriadoEvent.Id, It.IsAny<CancellationToken>())).ReturnsAsync(pedidoExistente);

        // Act
        var executeTask = _service.StartAsync(stoppingToken);
        await Task.Delay(1000); // Espera um pouco mais para permitir que o método ExecuteAsync seja executado
        stoppingTokenSource.Cancel(); // Cancela o token para parar o serviço
        await executeTask; // Aguarda a conclusão do serviço

        // Assert
        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Never);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLidarComMensagemNula()
    {
        // Arrange
        var stoppingTokenSource = new CancellationTokenSource();
        var stoppingToken = stoppingTokenSource.Token;
        _sqsClientMock.Setup(x => x.ReceiveMessagesAsync(stoppingToken)).ReturnsAsync((PedidoCriadoEvent)null);

        // Act
        var executeTask = _service.StartAsync(stoppingToken);
        await Task.Delay(1000); // Espera um pouco mais para permitir que o método ExecuteAsync seja executado
        stoppingTokenSource.Cancel(); // Cancela o token para parar o serviço
        await executeTask; // Aguarda a conclusão do serviço

        // Assert
        _pedidoRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<PedidoDb>(), It.IsAny<CancellationToken>()), Times.Never);
        _pedidoRepositoryMock.Verify(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLidarComExcecoes()
    {
        // Arrange
        var stoppingTokenSource = new CancellationTokenSource();
        var stoppingToken = stoppingTokenSource.Token;

        _sqsClientMock
            .Setup(x => x.ReceiveMessagesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao receber mensagem"));

        // Act
        var executeTask = _service.StartAsync(stoppingToken);
        await Task.Delay(1000); // Aguarda para permitir que o método ExecuteAsync seja executado
        stoppingTokenSource.Cancel(); // Cancela o token para parar o serviço
        await executeTask; // Aguarda a conclusão do serviço

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("An error occurred while processing messages.")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((o, t) => true)),
            Times.AtLeastOnce);
    }
}