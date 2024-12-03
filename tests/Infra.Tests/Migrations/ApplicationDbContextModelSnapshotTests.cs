
using Infra.Context;
using Infra.Dto;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infra.Tests.Migrations
{
    public class ApplicationDbContextModelSnapshotTests
    {
        [Fact]
        public void ModelSnapshot_ShouldMatchCurrentModel()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            // Act
            var model = context.Model;

            // Assert
            Assert.NotNull(model);

            // Verifica a entidade PagamentoDb
            var pagamentoEntity = model.FindEntityType(typeof(PagamentoDb));
            Assert.NotNull(pagamentoEntity);
            Assert.Equal("Pagamentos", pagamentoEntity.GetTableName());
            Assert.Equal("dbo", pagamentoEntity.GetSchema());

            // Verifica as propriedades de PagamentoDb
            var pagamentoProperties = new[]
            {
                    nameof(PagamentoDb.Id),
                    nameof(PagamentoDb.DataPagamento),
                    nameof(PagamentoDb.PedidoId),
                    nameof(PagamentoDb.QrCodePix),
                    nameof(PagamentoDb.Status),
                    nameof(PagamentoDb.Valor)
                };

            foreach (var property in pagamentoProperties)
            {
                var prop = pagamentoEntity.FindProperty(property);
                Assert.NotNull(prop);
                Assert.Equal(property, prop.Name);
            }

            // Verifica a entidade PedidoDb
            var pedidoEntity = model.FindEntityType(typeof(PedidoDb));
            Assert.NotNull(pedidoEntity);
            Assert.Equal("Pedidos", pedidoEntity.GetTableName());
            Assert.Equal("dbo", pedidoEntity.GetSchema());

            // Verifica as propriedades de PedidoDb
            var pedidoProperties = new[]
            {
                    nameof(PedidoDb.Id),
                    nameof(PedidoDb.DataPedido),
                    nameof(PedidoDb.NumeroPedido),
                    nameof(PedidoDb.ValorTotal)
                };

            foreach (var property in pedidoProperties)
            {
                var prop = pedidoEntity.FindProperty(property);
                Assert.NotNull(prop);
                Assert.Equal(property, prop.Name);
            }

            // Verifica a relação entre PagamentoDb e PedidoDb
            var foreignKey = pagamentoEntity.GetForeignKeys().SingleOrDefault(fk => fk.PrincipalEntityType == pedidoEntity);
            Assert.NotNull(foreignKey);
            Assert.Equal(nameof(PagamentoDb.PedidoId), foreignKey.Properties.Single().Name);
            Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);

            // Verifica os índices
            var pedidoIdIndex = pagamentoEntity.GetIndexes().SingleOrDefault(idx => idx.Properties.Any(p => p.Name == nameof(PagamentoDb.PedidoId)));
            Assert.NotNull(pedidoIdIndex);
        }
    }
}