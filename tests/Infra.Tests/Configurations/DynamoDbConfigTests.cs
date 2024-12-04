using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Infra.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Infra.Tests.Configurations;

public class DynamoDbConfigTests
{
    private readonly Mock<IAmazonDynamoDB> _mockDynamoDbClient;
    private readonly Mock<IDynamoDBContext> _mockDynamoDbContext;
    private readonly IServiceCollection _services;

    public DynamoDbConfigTests()
    {
        _mockDynamoDbClient = new Mock<IAmazonDynamoDB>();
        _mockDynamoDbContext = new Mock<IDynamoDBContext>();
        _services = new ServiceCollection();
    }

    [Fact]
    public void Configure_ShouldAddDynamoDbServices()
    {
        // Arrange
        string serviceUrl = "http://localhost:8000";
        string accessKey = "fakeAccessKey";
        string secretKey = "fakeSecretKey";

        // Act
        DynamoDbConfig.Configure(_services, serviceUrl, accessKey, secretKey, _mockDynamoDbClient.Object, _mockDynamoDbContext.Object);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var dynamoDbClient = serviceProvider.GetService<IAmazonDynamoDB>();
        var dynamoDbContext = serviceProvider.GetService<IDynamoDBContext>();

        Assert.NotNull(dynamoDbClient);
        Assert.NotNull(dynamoDbContext);
    }
    [Fact]
    public async Task CreateTableIfNotExists_ShouldCreateTable_WhenTableDoesNotExist()
    {
        // Arrange
        _mockDynamoDbClient.SetupSequence(client => client.DescribeTableAsync(It.Is<string>(t => t == "Pagamentos"), default))
            .ThrowsAsync(new ResourceNotFoundException("Table not found"))
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "CREATING" }
            })
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        _mockDynamoDbClient.SetupSequence(client => client.DescribeTableAsync(It.Is<string>(t => t == "Pedidos"), default))
            .ThrowsAsync(new ResourceNotFoundException("Table not found"))
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "CREATING" }
            })
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        _mockDynamoDbClient.Setup(client => client.CreateTableAsync(It.IsAny<CreateTableRequest>(), default))
            .ReturnsAsync(new CreateTableResponse());

        // Act
        await DynamoDbConfig.CreateTableIfNotExists(_mockDynamoDbClient.Object);

        // Assert
        _mockDynamoDbClient.Verify(client => client.CreateTableAsync(It.Is<CreateTableRequest>(r => r.TableName == "Pagamentos"), default), Times.Once);
        _mockDynamoDbClient.Verify(client => client.CreateTableAsync(It.Is<CreateTableRequest>(r => r.TableName == "Pedidos"), default), Times.Once);
    }


    [Fact]
    public async Task CreateTableIfNotExists_ShouldNotCreateTable_WhenTableExists()
    {
        // Arrange
        _mockDynamoDbClient.Setup(client => client.DescribeTableAsync(It.IsAny<string>(), default))
            .ReturnsAsync(new DescribeTableResponse
            {
                Table = new TableDescription { TableStatus = "ACTIVE" }
            });

        // Act
        await DynamoDbConfig.CreateTableIfNotExists(_mockDynamoDbClient.Object);

        // Assert
        _mockDynamoDbClient.Verify(client => client.CreateTableAsync(It.IsAny<CreateTableRequest>(), default), Times.Never);
    }
}
