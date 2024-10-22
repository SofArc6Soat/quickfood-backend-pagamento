using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.TestHelpers
{
    public static class PagamentoFakeDataFactory
    {
        public static Pagamento CriarPagamentoValido() => new(Guid.NewGuid(), Guid.NewGuid(), StatusPagamento.Pago, 100.00m, "QRCode", DateTime.Now);
    }
}