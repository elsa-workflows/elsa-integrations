using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Members;

/// <summary>
/// Adds or updates a list member in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Members",
    "Mailchimp Members",
    "Adds or updates a list member in Mailchimp.",
    DisplayName = "Add/Update List Member")]
[UsedImplicitly]
public class AddUpdateListMember : MailchimpActivity
{
    /// <summary>
    /// The ID of the list to add/update the member to.
    /// </summary>
    [Input(Description = "The ID of the list to add/update the member to.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// The email address of the member.
    /// </summary>
    [Input(Description = "The email address of the member.")]
    public Input<string> EmailAddress { get; set; } = null!;

    /// <summary>
    /// The subscription status of the member.
    /// </summary>
    [Input(Description = "The subscription status of the member (subscribed, unsubscribed, cleaned, pending).")]
    public Input<string> Status { get; set; } = new("subscribed");

    /// <summary>
    /// The first name of the member.
    /// </summary>
    [Input(Description = "The first name of the member.")]
    public Input<string?> FirstName { get; set; } = default!;

    /// <summary>
    /// The last name of the member.
    /// </summary>
    [Input(Description = "The last name of the member.")]
    public Input<string?> LastName { get; set; } = default!;

    /// <summary>
    /// Additional merge fields for the member.
    /// </summary>
    [Input(Description = "Additional merge fields for the member as JSON.")]
    public Input<string?> MergeFields { get; set; } = default!;

    /// <summary>
    /// The added/updated member.
    /// </summary>
    [Output(Description = "The added/updated member.")]
    public Output<Member> UpdatedMember { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var listId = context.Get(ListId)!;
        var emailAddress = context.Get(EmailAddress)!;
        var status = context.Get(Status) ?? "subscribed";
        var firstName = context.Get(FirstName);
        var lastName = context.Get(LastName);
        var mergeFieldsJson = context.Get(MergeFields);

        var client = GetClient(context);

        var member = new Member
        {
            EmailAddress = emailAddress,
            Status = Enum.Parse<Status>(status, true)
        };

        // Set merge fields
        if (!string.IsNullOrEmpty(firstName))
            member.MergeFields.Add("FNAME", firstName);
        if (!string.IsNullOrEmpty(lastName))
            member.MergeFields.Add("LNAME", lastName);

        // Parse additional merge fields if provided
        if (!string.IsNullOrEmpty(mergeFieldsJson))
        {
            try
            {
                var additionalFields = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(mergeFieldsJson);
                if (additionalFields != null)
                {
                    foreach (var field in additionalFields)
                    {
                        member.MergeFields[field.Key] = field.Value;
                    }
                }
            }
            catch
            {
                // Ignore JSON parsing errors
            }
        }

        var result = await client.Members.AddOrUpdateAsync(listId, member);
        context.Set(UpdatedMember, result);
    }
}