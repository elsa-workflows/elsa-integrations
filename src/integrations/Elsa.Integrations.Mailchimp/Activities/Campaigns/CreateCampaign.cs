using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Campaigns;

/// <summary>
/// Creates a new campaign in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Campaigns",
    "Mailchimp Campaigns",
    "Creates a new campaign in Mailchimp.",
    DisplayName = "Create Campaign")]
[UsedImplicitly]
public class CreateCampaign : MailchimpActivity
{
    /// <summary>
    /// The type of campaign (regular, plaintext, absplit, rss, variate).
    /// </summary>
    [Input(Description = "The type of campaign (regular, plaintext, absplit, rss, variate).")]
    public Input<string> Type { get; set; } = new("regular");

    /// <summary>
    /// The list ID to send the campaign to.
    /// </summary>
    [Input(Description = "The list ID to send the campaign to.")]
    public Input<string> ListId { get; set; } = null!;

    /// <summary>
    /// The subject line for the campaign.
    /// </summary>
    [Input(Description = "The subject line for the campaign.")]
    public Input<string> SubjectLine { get; set; } = null!;

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
    /// The created campaign.
    /// </summary>
    [Output(Description = "The created campaign.")]
    public Output<Campaign> CreatedCampaign { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var type = context.Get(Type) ?? "regular";
        var listId = context.Get(ListId)!;
        var subjectLine = context.Get(SubjectLine)!;
        var title = context.Get(Title);
        var fromName = context.Get(FromName);
        var replyTo = context.Get(ReplyTo);
        var previewText = context.Get(PreviewText);

        var client = GetClient(context);

        var campaign = new Campaign
        {
            Type = Enum.Parse<CampaignType>(type, true),
            Recipients = new Recipient
            {
                ListId = listId
            },
            Settings = new Setting
            {
                SubjectLine = subjectLine,
                Title = title,
                FromName = fromName,
                ReplyTo = replyTo,
                PreviewText = previewText
            }
        };

        var result = await client.Campaigns.AddAsync(campaign);
        context.Set(CreatedCampaign, result);
    }
}