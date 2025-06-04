using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Campaigns;

/// <summary>
/// Updates an existing campaign in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Campaigns",
    "Mailchimp Campaigns",
    "Updates an existing campaign in Mailchimp.",
    DisplayName = "Update Campaign")]
[UsedImplicitly]
public class UpdateCampaign : MailchimpActivity
{
    /// <summary>
    /// The ID of the campaign to update.
    /// </summary>
    [Input(Description = "The ID of the campaign to update.")]
    public Input<string> CampaignId { get; set; } = null!;

    /// <summary>
    /// The subject line for the campaign.
    /// </summary>
    [Input(Description = "The subject line for the campaign.")]
    public Input<string?> SubjectLine { get; set; } = default!;

    /// <summary>
    /// The title of the campaign.
    /// </summary>
    [Input(Description = "The title of the campaign.")]
    public Input<string?> Title { get; set; } = default!;

    /// <summary>
    /// The from name for the campaign.
    /// </summary>
    [Input(Description = "The from name for the campaign.")]
    public Input<string?> FromName { get; set; } = default!;

    /// <summary>
    /// The reply-to email address for the campaign.
    /// </summary>
    [Input(Description = "The reply-to email address for the campaign.")]
    public Input<string?> ReplyTo { get; set; } = default!;

    /// <summary>
    /// The preview text for the campaign.
    /// </summary>
    [Input(Description = "The preview text for the campaign.")]
    public Input<string?> PreviewText { get; set; } = default!;

    /// <summary>
    /// The updated campaign.
    /// </summary>
    [Output(Description = "The updated campaign.")]
    public Output<Campaign> UpdatedCampaign { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var campaignId = context.Get(CampaignId)!;
        var subjectLine = context.Get(SubjectLine);
        var title = context.Get(Title);
        var fromName = context.Get(FromName);
        var replyTo = context.Get(ReplyTo);
        var previewText = context.Get(PreviewText);

        var client = GetClient(context);

        var campaign = new Campaign
        {
            Id = campaignId,
            Settings = new Setting()
        };

        if (!string.IsNullOrEmpty(subjectLine))
            campaign.Settings.SubjectLine = subjectLine;
        if (!string.IsNullOrEmpty(title))
            campaign.Settings.Title = title;
        if (!string.IsNullOrEmpty(fromName))
            campaign.Settings.FromName = fromName;
        if (!string.IsNullOrEmpty(replyTo))
            campaign.Settings.ReplyTo = replyTo;
        if (!string.IsNullOrEmpty(previewText))
            campaign.Settings.PreviewText = previewText;

        var result = await client.Campaigns.UpdateAsync(campaignId, campaign);
        context.Set(UpdatedCampaign, result);
    }
}