namespace Railbite.Application.Common.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public sealed class AppValidationException : AppException
{
    public AppValidationException(string message) : base(message, 400)
    {
    }
}

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Invalid credentials.") : base(message, 401)
    {
    }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message, 409)
    {
    }
}
