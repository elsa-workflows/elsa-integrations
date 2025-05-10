namespace Elsa.Integrations.Telnyx.Exceptions;

public class MissingCallControlIdException(string message, Exception? innerException = null) : TelnyxException(message, innerException);