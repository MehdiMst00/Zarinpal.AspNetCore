using Microsoft.AspNetCore.Mvc;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.Interfaces;

namespace Zarinpal.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IZarinpalService _zarinpalService;

        public HomeController(IZarinpalService zarinpalService)
        {
            _zarinpalService = zarinpalService;
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
            if (result.IsSuccessStatusCode)
                return Redirect(result.RedirectUrl);

            return RedirectToAction("Index");
        }
    }
}