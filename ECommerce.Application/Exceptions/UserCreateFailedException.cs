using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class UserCreateFailedException:Exception
    {
        public UserCreateFailedException():base("Kullanıcı oluşturulurken beklenmedik bir hatayla karşılaşıldı")
        {
            
        }

        public UserCreateFailedException(string? message):base(message)
        {
        }

        public UserCreateFailedException(string? message,Exception? innerException):base(message,innerException)
        {
            
        }
    }
}
