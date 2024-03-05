namespace Orange_Bay.Exceptions;

public class CustomExceptionWithStatusCode : ApplicationException
{
    public int StatusCode { get; private set; }

    public CustomExceptionWithStatusCode(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}