using Elsa.Integrations.Mailchimp.Services;
using Xunit;

namespace Elsa.Integrations.Mailchimp.Tests.Services;

public class MailchimpClientFactoryTests
{
    [Fact]
    public void GetClient_ShouldReturnClient_WhenValidApiKeyProvided()
    {
        // Arrange
        var factory = new MailchimpClientFactory();
        var apiKey = "test-api-key-us1";

        // Act
        var client = factory.GetClient(apiKey);

        // Assert
        Assert.NotNull(client);
    }

    [Fact]
    public void GetClient_ShouldReturnSameInstance_WhenCalledMultipleTimesWithSameApiKey()
    {
        // Arrange
        var factory = new MailchimpClientFactory();
        var apiKey = "test-api-key-us1";

        // Act
        var client1 = factory.GetClient(apiKey);
        var client2 = factory.GetClient(apiKey);

        // Assert
        Assert.Same(client1, client2);
    }

    [Fact]
    public void GetClient_ShouldReturnDifferentInstances_WhenCalledWithDifferentApiKeys()
    {
        // Arrange
        var factory = new MailchimpClientFactory();
        var apiKey1 = "test-api-key-1-us1";
        var apiKey2 = "test-api-key-2-us1";

        // Act
        var client1 = factory.GetClient(apiKey1);
        var client2 = factory.GetClient(apiKey2);

        // Assert
        Assert.NotSame(client1, client2);
    }
}