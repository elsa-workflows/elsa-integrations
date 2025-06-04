# Elsa.Integrations.Mailchimp

This module provides integration with Mailchimp for Elsa Workflows. It enables workflows to interact with Mailchimp, supporting campaign management, list handling, segmentation, and event monitoring.

## Features

- **Campaign Management**: Create, update, delete, and retrieve campaigns
- **List Management**: Create, update, delete, and retrieve lists  
- **Member Management**: Add, update, remove list members and manage subscriptions
- **Segmentation**: Manage static segments and member assignments
- **Event Monitoring**: Watch for list events, subscriber changes, and campaign activities
- **API Integration**: Make arbitrary API calls to Mailchimp's REST API

## Getting Started

### Installation

Add the package reference to your project:

```xml
<PackageReference Include="Elsa.Integrations.Mailchimp" Version="1.0.0" />
```

### Configuration

Register the Mailchimp integration in your service configuration:

```csharp
builder.Services.AddElsa(elsa =>
{
    elsa.UseMailchimp();
});
```

## Authentication

The integration supports authentication via Mailchimp API Keys. You can obtain an API key from your Mailchimp account settings.

## Available Activities

### Lists

| Activity | Description |
|----------|-------------|
| CreateList | Creates a new list |
| GetList | Retrieves metadata of a specified list |
| UpdateList | Updates an existing list |
| DeleteList | Deletes a list |
| SearchLists | Searches for lists |

### Members

| Activity | Description |
|----------|-------------|
| AddUpdateListMember | Adds or updates a list member |
| AddUpdateSubscriber | Adds an email address to a subscriber list |
| EditSubscriber | Edits an existing subscriber |
| DeleteSubscriber | Archives or permanently deletes a subscriber |
| GetSubscriber | Retrieves metadata of a subscriber by email |
| SearchSubscribers | Searches for subscribers |

### Campaigns

| Activity | Description |
|----------|-------------|
| CreateCampaign | Creates a new campaign |
| GetCampaign | Retrieves metadata of a specified campaign |
| UpdateCampaign | Updates an existing campaign |
| DeleteCampaign | Deletes a campaign |
| SearchCampaigns | Searches for campaigns |
| PerformCampaignAction | Performs a campaign action (send, schedule, etc.) |

### Segments

| Activity | Description |
|----------|-------------|
| AddMemberToSegment | Adds a new member to a static segment |
| RemoveMemberFromSegment | Removes a member from the specified static segment |
| ListSegmentMembers | Retrieves a list of all segment members |

### Events and Triggers

| Activity | Description |
|----------|-------------|
| WatchLists | Triggers when a new list is created |
| WatchSubscribers | Triggers when a new subscriber joins a list or is updated |
| WatchCampaigns | Triggers when a new campaign is created or sent |
| WatchUnsubscribes | Triggers when a subscriber unsubscribes from a campaign |

### Other

| Activity | Description |
|----------|-------------|
| MakeAPICall | Performs an arbitrary authorized API call |
| AddEvent | Adds a new event to the member |
| AddRemoveMemberTags | Adds or removes tags from a list member |
| CreateMergeField | Creates a new merge field |
| GetMergeField | Retrieves an existing merge field |
| UpdateMergeField | Updates an existing merge field |
| DeleteMergeField | Deletes an existing merge field |

## Example Usage

Here's an example of adding a subscriber to a Mailchimp list from a workflow:

```csharp
// Create a workflow definition
public class AddSubscriberWorkflow : IWorkflow
{
    public void Build(IWorkflowBuilder builder)
    {
        builder
            .StartWith<AddUpdateListMember>(activity =>
            {
                activity.ApiKey = "your-mailchimp-api-key";
                activity.ListId = "your-list-id";
                activity.EmailAddress = new Input<string>("subscriber@example.com");
                activity.Status = "subscribed";
                activity.FirstName = "John";
                activity.LastName = "Doe";
            })
            .Then<WriteRawOutput>(activity =>
            {
                activity.Content = new JavaScriptValue<object>("return `Added subscriber: ${addUpdateListMember.updatedMember.emailAddress}`;");
            });
    }
}
```

### Creating a Campaign

```csharp
public class CreateMailchimpCampaignWorkflow : IWorkflow
{
    public void Build(IWorkflowBuilder builder)
    {
        builder
            .StartWith<CreateCampaign>(activity =>
            {
                activity.ApiKey = "your-mailchimp-api-key";
                activity.Type = "regular";
                activity.ListId = "your-list-id";
                activity.SubjectLine = "Welcome to our newsletter!";
                activity.Title = "Welcome Campaign";
                activity.FromName = "Your Company";
                activity.ReplyTo = "no-reply@yourcompany.com";
            })
            .Then<WriteRawOutput>(activity =>
            {
                activity.Content = new JavaScriptValue<object>("return `Created campaign: ${createCampaign.createdCampaign.id}`;");
            });
    }
}
```

## Notes

- All activities require a valid Mailchimp API key
- The API key should be stored securely and not hardcoded in workflows
- Some trigger activities require webhook configuration in Mailchimp
- Rate limiting applies based on your Mailchimp plan
- Ensure compliance with email marketing regulations (GDPR, CAN-SPAM, etc.)

## References

- 📖 [Mailchimp API Documentation](https://mailchimp.com/developer/)
- 🛠️ [Mailchimp Developer Resources](https://mailchimp.com/developer/guides/)
- 📚 [MailChimp.Net Library](https://github.com/brandonseydel/MailChimp.Net)