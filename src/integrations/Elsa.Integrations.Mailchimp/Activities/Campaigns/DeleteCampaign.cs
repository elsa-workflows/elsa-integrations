using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;

namespace Elsa.Integrations.Mailchimp.Activities.Campaigns;

/// <summary>
/// Deletes a campaign in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Campaigns",
    "Mailchimp Campaigns",
    "Deletes a campaign in Mailchimp.",
    DisplayName = "Delete Campaign")]
[UsedImplicitly]
public class DeleteCampaign : MailchimpActivity
{
    /// <summary>
    /// The ID of the campaign to delete.
    /// </summary>
    [Input(Description = "The ID of the campaign to delete.")]
    public Input<string> CampaignId { get; set; } = null!;

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
        var campaignId = context.Get(CampaignId)!;
        var client = GetClient(context);

        try
        {
            await client.Campaigns.DeleteAsync(campaignId);
            context.Set(Success, true);
        }
        catch
        {
            context.Set(Success, false);
        }
    }
}