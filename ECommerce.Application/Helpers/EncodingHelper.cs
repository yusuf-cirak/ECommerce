using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ECommerce.Application.Helpers;

public static class EncodingHelper
{
    public static string EncodeUrl(this string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);

        return WebEncoders.Base64UrlEncode(bytes);
    }
    
    public static string DecodeUrl(this string value)
    {
        byte[] bytes= WebEncoders.Base64UrlDecode(value);
        return Encoding.UTF8.GetString(bytes);
    }
}