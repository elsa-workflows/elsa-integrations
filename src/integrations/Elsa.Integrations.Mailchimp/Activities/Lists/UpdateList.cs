using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Lists;

/// <summary>
/// Updates an existing list in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Lists",
    "Mailchimp Lists",
    "Updates an existing list in Mailchimp.",
    DisplayName = "Update List")]
[UsedImplicitly]
public class UpdateList : MailchimpActivity
{
    /// <summary>
    /// The ID of the list to update.
    /// </summary>
    [Input(Description = "The ID of the list to update.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// The name of the list.
    /// </summary>
    [Input(Description = "The name of the list.")]
    public Input<string?> Name { get; set; } = default!;

    /// <summary>
    /// The permission reminder for the list.
    /// </summary>
    [Input(Description = "The permission reminder for the list.")]
    public Input<string?> PermissionReminder { get; set; } = default!;

    /// <summary>
    /// The 'from' name for campaigns sent to this list.
    /// </summary>
    [Input(Description = "The 'from' name for campaigns sent to this list.")]
    public Input<string?> FromName { get; set; } = default!;

    /// <summary>
    /// The 'from' email address for campaigns sent to this list.
    /// </summary>
    [Input(Description = "The 'from' email address for campaigns sent to this list.")]
    public Input<string?> FromEmail { get; set; } = default!;

    /// <summary>
    /// The updated list.
    /// </summary>
    [Output(Description = "The updated list.")]
    public Output<List> UpdatedList { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var listId = context.Get(ListId)!;
        var name = context.Get(Name);
        var permissionReminder = context.Get(PermissionReminder);
        var fromName = context.Get(FromName);
        var fromEmail = context.Get(FromEmail);

        var client = GetClient(context);

        var list = new List
        {
            Id = listId
        };

        if (!string.IsNullOrEmpty(name))
            list.Name = name;

        if (!string.IsNullOrEmpty(permissionReminder))
            list.PermissionReminder = permissionReminder;

        if (!string.IsNullOrEmpty(fromName) || !string.IsNullOrEmpty(fromEmail))
        {
            list.CampaignDefaults = new CampaignDefaults();
            if (!string.IsNullOrEmpty(fromName))
                list.CampaignDefaults.FromName = fromName;
            if (!string.IsNullOrEmpty(fromEmail))
                list.CampaignDefaults.FromEmail = fromEmail;
        }

        var result = await client.Lists.UpdateAsync(listId, list);
        context.Set(UpdatedList, result);
    }
}