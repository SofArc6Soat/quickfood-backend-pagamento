using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;

namespace Domain.Tests.TestHelpers
{
    public static class PagamentoFakeDataFactory
    {
        private static readonly Guid GuidFixo = Guid.Parse("12345678-1234-1234-1234-1234567890ab");

        public static Pagamento CriarPagamentoValido() => new(GuidFixo, Guid.NewGuid(), StatusPagamento.Pago, 100.00m, "QRCode", DateTime.Now);

        public static PagamentoDb CriarPagamentoDbValido() => new()
        {
            Id = GuidFixo,
            PedidoId = Guid.NewGuid(),
            Status = StatusPagamento.Pago.ToString(),
            Valor = 100.00m,
            QrCodePix = "QRCode",
            DataPagamento = DateTime.Now
        };

        public static Guid ObterNovoGuid() => GuidFixo;
    }
}