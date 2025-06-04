using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;

namespace Elsa.Integrations.Mailchimp.Activities.Lists;

/// <summary>
/// Deletes a list in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Lists",
    "Mailchimp Lists",
    "Deletes a list in Mailchimp.",
    DisplayName = "Delete List")]
[UsedImplicitly]
public class DeleteList : MailchimpActivity
{
    /// <summary>
    /// The ID of the list to delete.
    /// </summary>
    [Input(Description = "The ID of the list to delete.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    [Output(Description = "Indicates whether the operation was successful.")]
    public Output<bool> Success { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var listId = context.Get(ListId)!;
        var client = GetClient(context);

        try
        {
            await client.Lists.DeleteAsync(listId);
            context.Set(Success, true);
        }
        catch
        {
            context.Set(Success, false);
        }
    }
}