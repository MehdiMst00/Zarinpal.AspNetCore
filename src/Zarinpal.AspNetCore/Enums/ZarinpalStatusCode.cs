using System.ComponentModel.DataAnnotations;

namespace Zarinpal.AspNetCore.Enums;

public enum ZarinpalStatusCode : int
{
    [Display(Name = "خطای اعتبار سنجی")]
    St9 = -9,

    [Display(Name = "ای پی و يا مرچنت كد پذيرنده صحيح نيست")]
    St10 = -10,

    [Display(Name = "مرچنت کد فعال نیست لطفا با تیم پشتیبانی ما تماس بگیرید")]
    St11 = -11,

    [Display(Name = "تلاش بیش از حد در یک بازه زمانی کوتاه.")]
    St12 = -12,

    [Display(Name = "ترمینال شما به حالت تعلیق در آمده با تیم پشتیبانی تماس بگیرید")]
    St15 = -15,

    [Display(Name = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است.")]
    St16 = -16,

    [Display(Name = "عملیات موفق")]
    St100 = 100,

    [Display(Name = "اجازه دسترسی به تسویه اشتراکی شناور ندارید")]
    St30 = -30,

    [Display(Name = "حساب بانکی تسویه را به پنل اضافه کنید مقادیر وارد شده واسه تسهیم درست نیست")]
    St31 = -31,

    [Display(Name = "Wages is not valid, Total wages(floating) has been overload max amount.")]
    St32 = -32,

    [Display(Name = "درصد های وارد شده درست نیست")]
    St33 = -33,

    [Display(Name = "مبلغ از کل تراکنش بیشتر است")]
    St34 = -34,

    [Display(Name = "تعداد افراد دریافت کننده تسهیم بیش از حد مجاز است")]
    St35 = -35,

    [Display(Name = "Invalid extra params, expire_in is not valid.")]
    St40 = -40,

    [Display(Name = "مبلغ پرداخت شده با مقدار مبلغ در وریفای متفاوت است")]
    St50 = -50,

    [Display(Name = "پرداخت ناموفق")]
    St51 = -51,

    [Display(Name = "خطای غیر منتظره با پشتیبانی تماس بگیرید")]
    St52 = -52,

    [Display(Name = "اتوریتی برای این مرچنت کد نیست")]
    St53 = -53,

    [Display(Name = "اتوریتی نامعتبر است")]
    St54 = -54,

    [Display(Name = "تراکنش وریفای شده")]
    St101 = 101,

    [Display(Name = "عملیات با خطا مواجه شد")]
    St400 = 400,
}