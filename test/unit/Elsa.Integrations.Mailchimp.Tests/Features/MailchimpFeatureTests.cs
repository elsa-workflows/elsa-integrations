using Elsa.Features.Services;
using Elsa.Integrations.Mailchimp.Features;
using Elsa.Integrations.Mailchimp.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Elsa.Integrations.Mailchimp.Tests.Features;

public class MailchimpFeatureTests
{
    [Fact]
    public void Apply_ShouldRegisterMailchimpClientFactory()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockModule = new Mock<IModule>();
        mockModule.Setup(m => m.Services).Returns(services);
        
        var feature = new MailchimpFeature(mockModule.Object);

        // Act
        feature.Apply();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetService<MailchimpClientFactory>();
        Assert.NotNull(factory);
    }

    [Fact]
    public void Apply_ShouldRegisterMailchimpClientFactoryAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockModule = new Mock<IModule>();
        mockModule.Setup(m => m.Services).Returns(services);
        
        var feature = new MailchimpFeature(mockModule.Object);

        // Act
        feature.Apply();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory1 = serviceProvider.GetService<MailchimpClientFactory>();
        var factory2 = serviceProvider.GetService<MailchimpClientFactory>();
        Assert.Same(factory1, factory2);
    }
}