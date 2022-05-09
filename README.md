# Zarinpal.AspNetCore
Zarinpal Payment Gateway For Asp.Net Core
## Installation
1. Download And Install Package From [NuGet](https://www.nuget.org/packages/Zarinpal.AspNetCore) Or GitHub

```
PM> Install-Package Zarinpal.AspNetCore -Version 1.1.2
```

2. Use `AddZarinpal` To Add Needed Services To Service Container.
```c#
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddZarinpal(options =>
{
    builder.Configuration.GetSection("Zarinpal").Bind(options);
});

// Or 

builder.Services.AddZarinpal(options =>
{
    options.MerchantId = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx";
    options.ZarinpalMode = ZarinpalMode.Sandbox;
});
```
`Note:` If you bind options from appsettings.json [See Sample](https://github.com/MehdiMst00/Zarinpal.AspNetCore/blob/master/samples/Zarinpal.AspNetCore.Sample/appsettings.json)

3. Inject `IZarinpalService` To Your Controller

```c#
public class MyController : Controller
{
    private readonly IZarinpalService _zarinpalService;

    public MyController(IZarinpalService zarinpalService)
    {
         _zarinpalService = zarinpalService;
    }
}
```

4. Finish Setup :)

## Request Payment
```c#
// Amount For Original Payment Is In Rial 
var request = new ZarinpalRequestDTO(5000, "خرید",
    "https://localhost:7219/Home/VerifyPayment",
    "test@test.com", "09123456789");

var result = await _zarinpalService.RequestAsync(request);
if (result.IsSuccessStatusCode)
    return Redirect(result.RedirectUrl);
```

## Verify Payment
```c#
[HttpGet]
public async Task<IActionResult> VerifyPayment()
{
    // Check 'Status' and 'Authority' query param so zarinpal sent for us
    if (HttpContext.IsValidZarinpalVerifyQueries())
    {
        // If store your price in toman you can use TomanToRial extension
        int toman = 500;
        var verify = new ZarinpalVerifyDTO(toman.TomanToRial(),
            HttpContext.GetZarinpalAuthorityQuery());

        var response = await _zarinpalService.VerifyAsync(verify);
        if (response.IsSuccessStatusCode)
        {
            // Do Somethings...
            var refId = response.RefId;
            var statusCode = response.StatusCode;
        }

        return View(response.IsSuccessStatusCode);
    }

    return View(false);
}
```

#### Pay Attention: 
- In Sandbox Don't Need Set MerchantId. (`Just A String Of 36 Characters`)
- In Sandbox Use Amount In `Toman`

## Extensions Maybe You Need In Your Project
```c#
// For Verify Payment
bool isValidZarinpalVerifyQueries = HttpContext.IsValidZarinpalVerifyQueries();

// Get Authority From Query Params
string authority = HttpContext.GetZarinpalAuthorityQuery();

// Convert Toman To Rial
int toman = 500;
int rial = toman.TomanToRial();

// Get Status Message
var message = ZarinpalStatusCode.St100.GetStatusCodeMessage();
```
## Support
For support, [click here](https://github.com/MehdiMst00#-you-can-reach-me-on).

## Give a star ⭐️ !!!
If you liked the project, please give a star :)

## License
[MIT](https://choosealicense.com/licenses/mit/)
