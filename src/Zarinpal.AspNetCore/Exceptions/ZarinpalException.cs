namespace Zarinpal.AspNetCore.Exceptions;

public class ZarinpalException : ApplicationException
{
    public ZarinpalException()
    { }

    public ZarinpalException(string message)
        : base(message)
    { }

    public ZarinpalException(string message, Exception innerException)
        : base(message, innerException)
    { }
}