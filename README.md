# Zarinpal.AspNetCore

Zarinpal payment gateway for Asp.Net Core
## Installation
1. Download and install package from [NuGet](https://www.nuget.org/packages/Zarinpal.AspNetCore) or [GitHub](https://github.com/MehdiMst00/Zarinpal.AspNetCore)

```
PM> Install-Package Zarinpal.AspNetCore -Version 2.0.0
```

2. Use `AddZarinpal` to add needed services to service container.
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
`Note:` If you bind options from appsettings.json [See sample](https://github.com/MehdiMst00/Zarinpal.AspNetCore/blob/master/samples/Zarinpal.AspNetCore.Sample/appsettings.json)

3. Inject `IZarinpalService` to your controller

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

4. Finish setup :)

## Request Payment
```c#
// Amount For Original Payment Is In Rial 
var request = new ZarinpalRequestDTO(5000, "خرید",
    "https://localhost:7219/Home/VerifyPayment",
    "test@test.com", "09123456789");

var result = await _zarinpalService.RequestAsync(request);

if (result.Data != null)
{
    // You can store or log zarinpal data in database
    string authority = result.Data.Authority;
    int code = result.Data.Code;
    int fee = result.Data.Fee;
    string feeType = result.Data.FeeType;
    string message = result.Data.Message;
}

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
        
        if (response.Data != null)
        {
            // You can store or log zarinpal data in database
            ulong refId = response.Data.RefId;
            int fee = response.Data.Fee;
            string feeType = response.Data.FeeType;
            int code = response.Data.Code;
            string cardHash = response.Data.CardHash;
            string cardPan = response.Data.CardPan;
        }
        
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

## Pay attention
- In sandbox don't need set MerchantId. (`Just a string of 36 characters`)
- In sandbox use amount in `Toman`

## What is `IAdvancedZarinpalService`?
- If you wanna use 'UnVerified' or 'Refund'(Coming soon) method, you must inject `IAdvancedZarinpalService` to service container. (Automatically not injected)
- So let's come back into `Program.cs` and edit it (Or Set it in `appsettings.json` like [sample](https://github.com/MehdiMst00/Zarinpal.AspNetCore/blob/master/samples/Zarinpal.AspNetCore.Sample/appsettings.json)): 
```c#
builder.Services.AddZarinpal(options =>
{
    options.MerchantId = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx";
    options.ZarinpalMode = ZarinpalMode.Original;
    options.UseAdvanced = true;
});
```
- Now we can use `IAdvancedZarinpalService`:
```c#
private readonly IAdvancedZarinpalService _advancedZarinpalService;

public MyController(IAdvancedZarinpalService advancedZarinpalService)
{
    _advancedZarinpalService = advancedZarinpalService;
}

public async Task<IActionResult> UnVerifiedPayments()
{
    var result = await _advancedZarinpalService.UnVerifiedAsync();
    return Ok(result);
}
```

## Extensions maybe you need in your project
```c#
// For verify payment
bool isValidZarinpalVerifyQueries = HttpContext.IsValidZarinpalVerifyQueries();

// Get authority from query params
string authority = HttpContext.GetZarinpalAuthorityQuery();

// Convert toman to rial
int toman = 500;
int rial = toman.TomanToRial();

// Get status message
var message = ZarinpalStatusCode.St100.GetStatusCodeMessage();
```
## Support
For support, [click here](https://github.com/MehdiMst00#-you-can-reach-me-on).

## Give a star ⭐️ !!!
If you liked the project, please give a star :)

## License
[MIT](https://github.com/MehdiMst00/Zarinpal.AspNetCore/blob/master/LICENSE.txt)
