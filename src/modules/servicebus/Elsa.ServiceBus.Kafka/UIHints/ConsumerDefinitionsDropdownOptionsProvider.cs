using System.Reflection;
using Elsa.Workflows.UIHints.Dropdown;
using Open.Linq.AsyncExtensions;

namespace Elsa.ServiceBus.Kafka.UIHints;

public class ConsumerDefinitionsDropdownOptionsProvider(IConsumerDefinitionEnumerator consumerEnumerator) : DropDownOptionsProviderBase
{
    protected override async ValueTask<ICollection<SelectListItem>> GetItemsAsync(PropertyInfo propertyInfo, object? context, CancellationToken cancellationToken)
    {
        var definitions = await consumerEnumerator.EnumerateAsync(cancellationToken).ToList();
        return definitions.Select(x => new SelectListItem(x.Name, x.Id)).ToList();
    }
}