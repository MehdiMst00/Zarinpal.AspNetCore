using Microsoft.AspNetCore.Mvc;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.Extensions;
using Zarinpal.AspNetCore.Interfaces;

namespace Zarinpal.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IZarinpalService _zarinpalService;
        private readonly IAdvancedZarinpalService _advancedZarinpalService;

        public HomeController(IZarinpalService zarinpalService, 
            IAdvancedZarinpalService advancedZarinpalService)
        {
            _zarinpalService = zarinpalService;
            _advancedZarinpalService = advancedZarinpalService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestPayment()
        {
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

            //if (result.StatusCode == ZarinpalStatusCode.St100)
            //{
            //    // if you want see status message
            //    var message = result.StatusCode.Value.GetStatusCodeMessage();
            //    // Do Something
            //}

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyPayment()
        {
            // Check 'Status' and 'Authority' query param so zarinpal sent for us
            if (HttpContext.IsValidZarinpalVerifyQueries())
            {
                // If store your price in toman you can use TomanToRial extension
                int toman = 500;
                var verify = new ZarinpalVerifyDTO(toman.TomanToRial(),
                    HttpContext.GetZarinpalAuthorityQuery()!);

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
                    ViewData["RefId"] = response.RefId;
                }

                //if (response.StatusCode == ZarinpalStatusCode.St100)
                //{
                //    // if you want see status message
                //    var message = response.StatusCode.Value.GetStatusCodeMessage();
                //    // Do Something
                //}

                return View(response.IsSuccessStatusCode);
            }

            return View(false);
        }

        [HttpGet]
        public async Task<IActionResult> UnVerifiedPayments()
        {
            var result = await _advancedZarinpalService.UnVerifiedAsync();
            return View(result);
        }
    }
}