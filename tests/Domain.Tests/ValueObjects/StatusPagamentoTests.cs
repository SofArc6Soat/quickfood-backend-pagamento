using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.ValueObjects
{
    public class StatusPagamentoTests
    {
        [Fact]
        public void StatusPagamento_DeveConterOsValoresDefinidos()
        {
            // Act
            var valores = Enum.GetValues(typeof(StatusPagamento)).Cast<StatusPagamento>();

            // Assert
            valores.Should().HaveCount(2)
                .And.Contain(
                [
                    StatusPagamento.Pendente,
                    StatusPagamento.Pago
                ]);
        }

        [Theory]
        [InlineData(StatusPagamento.Pendente, 0)]
        [InlineData(StatusPagamento.Pago, 1)]
        public void StatusPagamento_DeveTerValoresCorretos(StatusPagamento status, int valorEsperado) =>
            // Assert
            ((int)status).Should().Be(valorEsperado);

        [Theory]
        [InlineData(0, StatusPagamento.Pendente)]
        [InlineData(1, StatusPagamento.Pago)]
        public void StatusPagamento_DeveConverterDeInteiroCorretamente(int valor, StatusPagamento statusEsperado)
        {
            // Act
            var status = (StatusPagamento)valor;

            // Assert
            status.Should().Be(statusEsperado);
        }

        [Fact]
        public void StatusPagamento_InvalidoDeveGerarExcecao()
        {
            // Act
            Action act = () => Enum.Parse<StatusPagamento>("Invalido");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Invalido*");
        }
    }
}
