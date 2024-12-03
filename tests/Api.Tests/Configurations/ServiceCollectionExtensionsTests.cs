using Amazon.CognitoIdentityProvider;
using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Tests.Configurations;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApiDefautConfig_ShouldConfigureNotificador()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        services.AddApiDefautConfig(jwtBearerOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var notificador = serviceProvider.GetService<INotificador>();
        Assert.NotNull(notificador);
    }

    [Fact]
    public void AddApiDefautConfig_ShouldConfigureJsonOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        services.AddApiDefautConfig(jwtBearerOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var jsonOptions = serviceProvider.GetService<IOptions<JsonOptions>>()?.Value;
        Assert.NotNull(jsonOptions);
        Assert.Equal(JsonIgnoreCondition.WhenWritingNull, jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition);
        Assert.Equal(JsonNamingPolicy.CamelCase, jsonOptions.JsonSerializerOptions.PropertyNamingPolicy);
    }

    [Fact]
    public void AddApiDefautConfig_ShouldConfigureAuthentication()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        services.AddApiDefautConfig(jwtBearerOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var authService = serviceProvider.GetService<IAuthenticationService>();
        Assert.NotNull(authService);
    }

    [Fact]
    public void AddApiDefautConfig_ShouldConfigureAuthorization()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Adiciona o serviço de logging
        services.AddLogging();

        // Act
        services.AddApiDefautConfig(jwtBearerOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var authorizationService = serviceProvider.GetService<IAuthorizationService>();
        Assert.NotNull(authorizationService);
    }

    [Fact]
    public void AddApiDefautConfig_ShouldConfigureCognitoService()
    {
        // Arrange
        var services = new ServiceCollection();
        var jwtBearerOptions = new JwtBearerConfigureOptions
        {
            Authority = "https://example.com",
            MetadataAddress = "https://example.com/.well-known/openid-configuration"
        };

        // Act
        services.AddApiDefautConfig(jwtBearerOptions);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var cognitoService = serviceProvider.GetService<IAmazonCognitoIdentityProvider>();
        Assert.NotNull(cognitoService);
    }

    [Fact]
    public async void UseApiDefautConfig_ShouldConfigureMiddleware()
    {
        // Arrange
        var webHostBuilder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                var jwtBearerOptions = new JwtBearerConfigureOptions
                {
                    Authority = "https://example.com",
                    MetadataAddress = "https://example.com/.well-known/openid-configuration"
                };
                services.AddApiDefautConfig(jwtBearerOptions);
                services.AddHealthChecks(); // Adiciona o serviço de verificações de integridade
            })
            .Configure(app =>
            {
                app.UseApiDefautConfig();
            });

        var testServer = new TestServer(webHostBuilder);
        var client = testServer.CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
