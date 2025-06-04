using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;

namespace Elsa.Integrations.Mailchimp.Activities.Members;

/// <summary>
/// Archives or permanently deletes a subscriber from Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Members",
    "Mailchimp Members",
    "Archives or permanently deletes a subscriber from Mailchimp.",
    DisplayName = "Delete Subscriber")]
[UsedImplicitly]
public class DeleteSubscriber : MailchimpActivity
{
    /// <summary>
    /// The ID of the list.
    /// </summary>
    [Input(Description = "The ID of the list.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// The email address of the subscriber.
    /// </summary>
    [Input(Description = "The email address of the subscriber.")]
    public Input<string> EmailAddress { get; set; } = null!;

    /// <summary>
    /// Whether to permanently delete the subscriber (true) or just archive (false).
    /// </summary>
    [Input(Description = "Whether to permanently delete the subscriber (true) or just archive (false).")]
    public Input<bool> PermanentDelete { get; set; } = new(false);

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
        var emailAddress = context.Get(EmailAddress)!;
        var permanentDelete = context.Get(PermanentDelete);
        var client = GetClient(context);

        try
        {
            if (permanentDelete)
            {
                await client.Members.DeleteAsync(listId, emailAddress);
            }
            else
            {
                // Archive the member by setting status to unsubscribed
                var member = new MailChimp.Net.Models.Member
                {
                    EmailAddress = emailAddress,
                    Status = MailChimp.Net.Models.Status.Unsubscribed
                };
                await client.Members.AddOrUpdateAsync(listId, member);
            }
            context.Set(Success, true);
        }
        catch
        {
            context.Set(Success, false);
        }
    }
}