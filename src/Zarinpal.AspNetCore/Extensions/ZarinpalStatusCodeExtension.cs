using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zarinpal.AspNetCore.Extensions;

public static class ZarinpalStatusCodeExtension
{
    public static string? GetStatusCodeMessage(this ZarinpalStatusCode statusCode)
    {
        var statusEnum = statusCode.GetType().GetMember(statusCode.ToString()).FirstOrDefault();
        if (statusEnum != null)
            return statusEnum.GetCustomAttribute<DisplayAttribute>()?.GetName();

        return "";
    }
}