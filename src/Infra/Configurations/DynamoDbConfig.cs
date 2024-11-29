using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Configurations
{
    public static class DynamoDbConfig
    {
        public static void Configure(IServiceCollection services, string serviceUrl, string accessKey, string secretKey)
        {
            var clientDynamo = ConfigDynamoDb(services, serviceUrl, accessKey, secretKey);
            CreateTableIfNotExists(clientDynamo).Wait();
        }

        private static AmazonDynamoDBClient ConfigDynamoDb(IServiceCollection services, string serviceUrl, string accessKey, string secretKey)
        {
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = serviceUrl
            };

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var amazonDynamoDbClient = new AmazonDynamoDBClient(credentials, config);

            services.AddSingleton<IAmazonDynamoDB>(amazonDynamoDbClient);

            var context = new DynamoDBContext(amazonDynamoDbClient);
            services.AddSingleton<IDynamoDBContext>(context);

            return amazonDynamoDbClient;
        }

        private static async Task CreateTableIfNotExists(IAmazonDynamoDB client)
        {
            var listTable = new List<string>()
            {
                "Pagamentos",
                "Pedidos"
            };

            foreach (var tableName in listTable)
            {
                try
                {
                    await client.DescribeTableAsync(tableName);
                }
                catch (ResourceNotFoundException)
                {
                    var createTableRequest = new CreateTableRequest
                    {
                        TableName = tableName,
                        AttributeDefinitions =
                        [
                            new("Id", ScalarAttributeType.S)
                        ],
                        KeySchema =
                        [
                            new("Id", KeyType.HASH)
                        ],
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5,
                            WriteCapacityUnits = 5
                        }
                    };

                    await client.CreateTableAsync(createTableRequest);

                    var tableStatus = "CREATING";
                    while (tableStatus == "CREATING")
                    {
                        await Task.Delay(1000);
                        var response = await client.DescribeTableAsync(tableName);
                        tableStatus = response.Table.TableStatus;
                    }
                }
            }
        }
    }
}
