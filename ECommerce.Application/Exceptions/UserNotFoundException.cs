namespace ECommerce.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("Kullanıcı sistemde bulunamadı")
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}