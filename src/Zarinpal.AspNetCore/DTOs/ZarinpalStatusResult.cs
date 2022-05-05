using System.ComponentModel.DataAnnotations;

namespace Zarinpal.AspNetCore.DTOs;

public enum ZarinpalStatusResult : int
{
    [Display(Name = "اطلاعات ارسالی ناقص است.")]
    St1 = -1,

    [Display(Name = "ﺑﺎ ﺗﻮﺟﻪ ﺑﻪ ﻣﺤﺪﻭﺩﻳﺖ ﻫﺎﻱ ﺷﺎﭘﺮﻙ ﺍﻣﻜﺎﻥ ﭘﺮﺩﺍﺧﺖ ﺑﺎ ﺭﻗﻢ ﺩﺭﺧﻮﺍﺳﺖ ﺷﺪﻩ ﻣﻴﺴﺮ ﻧﻤﻲ ﺑﺎﺷﺪ.")]
    St3 = -3,

    [Display(Name = "ﺩﺭﺧﻮﺍﺳﺖ ﻣﻮﺭﺩ ﻧﻈﺮ ﻳﺎﻓﺖ ﻧﺸﺪ.")]
    St11 = -11,

    [Display(Name = "ﻫﻴﭻ ﻧﻮﻉ ﻋﻤﻠﻴﺎﺕ ﻣﺎﻟﻲ ﺑﺮﺍﻱ ﺍﻳﻦ ﺗﺮﺍﻛﻨﺶ ﻳﺎﻓﺖ ﻧﺸﺪ.")]
    St21 = -21,

    [Display(Name = "ﺗﺮﺍﻛﻨﺶ ﻧﺎ ﻣﻮﻓﻖ ﻣﻲﺑﺎﺷﺪ")]
    St22 = -22,

    [Display(Name = "ﺭﻗﻢ ﺗﺮﺍﻛﻨﺶ ﺑﺎ ﺭﻗﻢ ﭘﺮﺩﺍﺧﺖ ﺷﺪﻩ ﻣﻄﺎﺑﻘﺖ ﻧﺪﺍﺭﺩ")]
    St33 = -33,

    [Display(Name = "ﻋﻤﻠﻴﺎﺕ ﺑﺎ ﻣﻮﻓﻘﻴﺖ ﺍﻧﺠﺎﻡ ﮔﺮﺩﻳﺪﻩ ﺍﺳﺖ")]
    St100 = 100,

    [Display(Name = " انجام شده است. ﻋﻤﻠﻴﺎﺕ ﭘﺮﺩﺍﺧﺖ ﻣﻮﻓﻖ ﺑﻮﺩﻩ ﻭ ﻗﺒﻼ VerifyPayment")]
    St101 = 101,
}