namespace ECommerce.Application.Exceptions;

public class UserLoginFailedException : Exception
{
    public UserLoginFailedException():base("Kullanıcı sistemde bulunamadı")
    {
    }

    public UserLoginFailedException(string? message):base(message)
    {
    }

    public UserLoginFailedException(string? message,Exception? innerException):base(message,innerException)
    {
    }
}