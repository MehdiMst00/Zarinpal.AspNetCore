using System.ComponentModel.DataAnnotations;

namespace Zarinpal.AspNetCore.DTOs.Common;

public enum ZarinpalStatusResult : int
{
    [Display(Name = "خطای اعتبار سنجی.")]
    St9 = -9,

    [Display(Name = "خطای اعتبار سنجی.")]
    St10 = -10,

    [Display(Name = "مرچنت کد فعال نیست لطفا با تیم پشتیبانی ما تماس بگیرید")]
    St11 = -11,

    [Display(Name = "تلاش بیش از حد در یک بازه زمانی کوتاه.")]
    St12 = -12,

    [Display(Name = "ترمینال شما به حالت تعلیق در آمده با تیم پشتیبانی تماس بگیرید")]
    St15 = -15,

    [Display(Name = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است.")]
    St16 = -16,

    [Display(Name = "ﻫﻴﭻ ﻧﻮﻉ ﻋﻤﻠﻴﺎﺕ ﻣﺎﻟﻲ ﺑﺮﺍﻱ ﺍﻳﻦ ﺗﺮﺍﻛﻨﺶ ﻳﺎﻓﺖ ﻧﺸﺪ.")]
    St21 = -21,

    [Display(Name = "ﺗﺮﺍﻛﻨﺶ ﻧﺎ ﻣﻮﻓﻖ ﻣﻲﺑﺎﺷﺪ")]
    St22 = -22,

    [Display(Name = "درصد های وارد شده درست نیست")]
    St33 = -33,

    [Display(Name = "پرداخت ناموفق")]
    St51 = -51,

    [Display(Name = "اتوریتی نامعتبر است")]
    St54 = -54,

    [Display(Name = "عملیات موفق")]
    St100 = 100,

    [Display(Name = "تراکنش وریفای شده")]
    St101 = 101,

    [Display(Name = "عملیات با خطا مواجه شد")]
    St400 = 400,
}