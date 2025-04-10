namespace Elsa.Integrations.Telnyx.Bookmarks;

/// <summary>
/// A bookmark payload for the call.hangup Telnyx webhook event.
/// </summary>
/// <param name="CallControlId"></param>
public record CallHangupStimulus(string CallControlId);