using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using MailChimp.Net.Models;

namespace Elsa.Integrations.Mailchimp.Activities.Lists;

/// <summary>
/// Creates a new list in Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Lists",
    "Mailchimp Lists",
    "Creates a new list in Mailchimp.",
    DisplayName = "Create List")]
[UsedImplicitly]
public class CreateList : MailchimpActivity
{
    /// <summary>
    /// The name of the list.
    /// </summary>
    [Input(Description = "The name of the list.")]
    public Input<string> Name { get; set; } = null!;

    /// <summary>
    /// The contact information for this list.
    /// </summary>
    [Input(Description = "The company name associated with the list.")]
    public Input<string> CompanyName { get; set; } = null!;

    /// <summary>
    /// The contact address for this list.
    /// </summary>
    [Input(Description = "The contact address for this list.")]
    public Input<string> Address1 { get; set; } = null!;

    /// <summary>
    /// The city for the contact address.
    /// </summary>
    [Input(Description = "The city for the contact address.")]
    public Input<string> City { get; set; } = null!;

    /// <summary>
    /// The state for the contact address.
    /// </summary>
    [Input(Description = "The state for the contact address.")]
    public Input<string> State { get; set; } = null!;

    /// <summary>
    /// The zip code for the contact address.
    /// </summary>
    [Input(Description = "The zip code for the contact address.")]
    public Input<string> Zip { get; set; } = null!;

    /// <summary>
    /// The country for the contact address.
    /// </summary>
    [Input(Description = "The country for the contact address.")]
    public Input<string> Country { get; set; } = null!;

    /// <summary>
    /// The permission reminder for the list.
    /// </summary>
    [Input(Description = "The permission reminder for the list.")]
    public Input<string> PermissionReminder { get; set; } = null!;

    /// <summary>
    /// The 'from' name for campaigns sent to this list.
    /// </summary>
    [Input(Description = "The 'from' name for campaigns sent to this list.")]
    public Input<string> FromName { get; set; } = null!;

    /// <summary>
    /// The 'from' email address for campaigns sent to this list.
    /// </summary>
    [Input(Description = "The 'from' email address for campaigns sent to this list.")]
    public Input<string> FromEmail { get; set; } = null!;

    /// <summary>
    /// The language for this list.
    /// </summary>
    [Input(Description = "The language for this list.")]
    public Input<string> Language { get; set; } = new("en");

    /// <summary>
    /// The created list.
    /// </summary>
    [Output(Description = "The created list.")]
    public Output<List> CreatedList { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var name = context.Get(Name)!;
        var companyName = context.Get(CompanyName)!;
        var address1 = context.Get(Address1)!;
        var city = context.Get(City)!;
        var state = context.Get(State)!;
        var zip = context.Get(Zip)!;
        var country = context.Get(Country)!;
        var permissionReminder = context.Get(PermissionReminder)!;
        var fromName = context.Get(FromName)!;
        var fromEmail = context.Get(FromEmail)!;
        var language = context.Get(Language) ?? "en";

        var client = GetClient(context);

        var list = new List
        {
            Name = name,
            Contact = new Contact
            {
                Company = companyName,
                Address1 = address1,
                City = city,
                State = state,
                Zip = zip,
                Country = country
            },
            PermissionReminder = permissionReminder,
            CampaignDefaults = new CampaignDefaults
            {
                FromName = fromName,
                FromEmail = fromEmail,
                Language = language
            }
        };

        var result = await client.Lists.AddAsync(list);
        context.Set(CreatedList, result);
    }
}