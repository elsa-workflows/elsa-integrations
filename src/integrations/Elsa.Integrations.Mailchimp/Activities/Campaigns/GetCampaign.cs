using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Campaigns;

/// <summary>
/// Retrieves metadata of a specified campaign from Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Campaigns",
    "Mailchimp Campaigns",
    "Retrieves metadata of a specified campaign from Mailchimp.",
    DisplayName = "Get Campaign")]
[UsedImplicitly]
public class GetCampaign : MailchimpActivity
{
    /// <summary>
    /// The ID of the campaign to retrieve.
    /// </summary>
    [Input(Description = "The ID of the campaign to retrieve.")]
    public Input<string> CampaignId { get; set; } = null!;

    /// <summary>
    /// The retrieved campaign.
    /// </summary>
    [Output(Description = "The retrieved campaign.")]
    public Output<Campaign> RetrievedCampaign { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var campaignId = context.Get(CampaignId)!;
        var client = GetClient(context);

        var campaign = await client.Campaigns.GetAsync(campaignId);
        context.Set(RetrievedCampaign, campaign);
    }
}