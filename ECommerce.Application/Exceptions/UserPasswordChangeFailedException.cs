namespace ECommerce.Application.Exceptions;

public sealed class UserPasswordChangeFailedException:Exception
{
    public UserPasswordChangeFailedException():base("Kullanıcı şifresi değiştirilirken beklenmedik bir hatayla karşılaşıldı")
    {
            
    }

    public UserPasswordChangeFailedException(string? message):base(message)
    {
    }

    public UserPasswordChangeFailedException(string? message,Exception? innerException):base(message,innerException)
    {
            
    }
}