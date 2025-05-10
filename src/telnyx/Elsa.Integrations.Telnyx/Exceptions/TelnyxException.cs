namespace Elsa.Integrations.Telnyx.Exceptions;

public class TelnyxException(string message, Exception? innerException = null) : Exception(message, innerException);