using Domain.Entities;
using Infra.Dto;

namespace Domain.Tests.TestHelpers;

public static class PedidoFakeDataFactory
{
    private static readonly Guid GuidFixo = Guid.Parse("12345678-1234-1234-1234-1234567890ab");

    public static Pedido CriarPedidoValido() => new(GuidFixo, 123, 100.00m, DateTime.Now);

    public static PedidoDb CriarPedidoDbValido() => new()
    {
        Id = GuidFixo,
        NumeroPedido = 123,
        ValorTotal = 100.00m,
        DataPedido = DateTime.Now
    };

    public static Guid ObterNovoGuid() => GuidFixo;
}