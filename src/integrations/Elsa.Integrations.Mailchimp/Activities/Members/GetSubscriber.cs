using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Members;

/// <summary>
/// Retrieves metadata of a subscriber by email from Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Members",
    "Mailchimp Members",
    "Retrieves metadata of a subscriber by email from Mailchimp.",
    DisplayName = "Get Subscriber")]
[UsedImplicitly]
public class GetSubscriber : MailchimpActivity
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
    /// The retrieved subscriber.
    /// </summary>
    [Output(Description = "The retrieved subscriber.")]
    public Output<Member> RetrievedSubscriber { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var listId = context.Get(ListId)!;
        var emailAddress = context.Get(EmailAddress)!;
        var client = GetClient(context);

        var member = await client.Members.GetAsync(listId, emailAddress);
        context.Set(RetrievedSubscriber, member);
    }
}