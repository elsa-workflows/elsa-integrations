using System.ComponentModel;
using System.Runtime.CompilerServices;
using Elsa.Extensions;
using Elsa.Integrations.Telnyx.Bookmarks;
using Elsa.Integrations.Telnyx.Helpers;
using Elsa.Integrations.Telnyx.Models;
using Elsa.Integrations.Telnyx.Payloads.Abstractions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Memory;
using JetBrains.Annotations;

namespace Elsa.Integrations.Telnyx.Activities;

/// <summary>
/// Represents a Telnyx webhook event trigger.
/// </summary>
[Activity("Telnyx", "Telnyx", "A Telnyx webhook event that executes when a webhook event is received.", Kind = ActivityKind.Trigger)]
[Browsable(false)]
[UsedImplicitly]
public class WebhookEvent : Activity<Payload>
{
    /// <inheritdoc />
    public WebhookEvent([CallerFilePath] string? source = null, [CallerLineNumber] int? line = null) : base(source, line)
    {
    }

    /// <inheritdoc />
    public WebhookEvent(string eventType, string activityTypeName, Variable<Payload> result, int version = 1, [CallerFilePath] string? source = null, [CallerLineNumber] int? line = null)
        : base(activityTypeName, version, source, line)
    {
        EventType = eventType;
        Result = new(result);
    }

    /// <summary>
    /// The Telnyx webhook event type to listen for.
    /// </summary>
    [Description("The Telnyx webhook event type to listen for")]
    public string EventType { get; set; } = null!;

    /// <inheritdoc />
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        if (context.IsTriggerOfWorkflow())
            await Resume(context);
        else
        {
            var eventType = EventType;
            var payload = new WebhookEventStimulus(eventType);

            context.CreateBookmark(new()
            {
                Stimulus = payload,
                Callback = Resume,
                BookmarkName = Type
            });
        }
    }

    private async ValueTask Resume(ActivityExecutionContext context)
    {
        var input = context.GetWorkflowInput<TelnyxWebhook>(WebhookSerializerOptions.Create());
        context.Set(Result, input.Data.Payload);
        await CompleteAsync(context);
    }
}