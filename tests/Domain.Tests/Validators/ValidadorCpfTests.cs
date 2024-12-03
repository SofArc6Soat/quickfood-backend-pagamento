using Core.Domain.Validacoes;
using Xunit;

namespace Core.Domain.Tests.Validacoes;

public class ValidadorCpfTests
{
    [Theory]
    [InlineData("111.111.111-11", false)] // CPF com dígitos repetidos
    [InlineData("529.982.247-25", true)]  // CPF válido
    [InlineData("52998224725", true)]     // CPF válido sem caracteres especiais
    [InlineData("529.982.247-2", false)]  // CPF com tamanho inválido
    public void Validar_DeveRetornarResultadoEsperado(string cpf, bool resultadoEsperado)
    {
        // Act
        var resultado = ValidadorCpf.Validar(cpf);

        // Assert
        Assert.Equal(resultadoEsperado, resultado);
    }
}

public class DigitoVerificadorTests
{
    [Fact]
    public void CalculaDigito_DeveRetornarDigitoCorreto()
    {
        // Arrange
        var numero = "529982247";
        var digitoVerificador = new DigitoVerificador(numero)
            .ComMultiplicadoresDeAte(2, 11)
            .Substituindo("0", 10, 11);

        // Act
        var primeiroDigito = digitoVerificador.CalculaDigito();
        digitoVerificador.AddDigito(primeiroDigito);
        var segundoDigito = digitoVerificador.CalculaDigito();

        // Assert
        Assert.Equal("2", primeiroDigito);
        Assert.Equal("5", segundoDigito);
    }

    [Fact]
    public void ComMultiplicadoresDeAte_DeveConfigurarMultiplicadoresCorretamente()
    {
        // Arrange
        var numero = "529982247";
        var digitoVerificador = new DigitoVerificador(numero);

        // Act
        digitoVerificador.ComMultiplicadoresDeAte(2, 9);

        // Assert
        // Verificar se os multiplicadores foram configurados corretamente
        // Não há uma maneira direta de verificar os multiplicadores, então este teste é mais para garantir que o método não lance exceções
    }

    [Fact]
    public void Substituindo_DeveConfigurarSubstituicoesCorretamente()
    {
        // Arrange
        var numero = "529982247";
        var digitoVerificador = new DigitoVerificador(numero);

        // Act
        digitoVerificador.Substituindo("0", 10, 11);

        // Assert
        // Verificar se as substituições foram configuradas corretamente
        // Não há uma maneira direta de verificar as substituições, então este teste é mais para garantir que o método não lance exceções
    }
}
