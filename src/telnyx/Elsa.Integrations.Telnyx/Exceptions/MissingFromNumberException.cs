namespace Elsa.Integrations.Telnyx.Exceptions;

public class MissingFromNumberException(string message, Exception? innerException = null) : TelnyxException(message, innerException);