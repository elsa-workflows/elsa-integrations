using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Lists;

/// <summary>
/// Retrieves metadata of a specified list from Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Lists",
    "Mailchimp Lists",
    "Retrieves metadata of a specified list from Mailchimp.",
    DisplayName = "Get List")]
[UsedImplicitly]
public class GetList : MailchimpActivity
{
    /// <summary>
    /// The ID of the list to retrieve.
    /// </summary>
    [Input(Description = "The ID of the list to retrieve.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// The retrieved list.
    /// </summary>
    [Output(Description = "The retrieved list.")]
    public Output<List> RetrievedList { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var listId = context.Get(ListId)!;
        var client = GetClient(context);

        var list = await client.Lists.GetAsync(listId);
        context.Set(RetrievedList, list);
    }
}