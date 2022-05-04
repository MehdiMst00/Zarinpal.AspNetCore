using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.Extensions;
using Zarinpal.AspNetCore.Interfaces;
using Zarinpal.AspNetCore.Models;

namespace Zarinpal.AspNetCore.Test;

[TestClass]
public class ZarinpalServiceTest
{
    private readonly IZarinpalService _zarinpalService;

    public ZarinpalServiceTest()
    {
        var serviceProvider = new ServiceCollection()
            .AddZarinpal(options =>
            {
                options.ZarinpalMode = ZarinpalMode.Original;
                options.MerchantId = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx";
            })
            .BuildServiceProvider();

        _zarinpalService = serviceProvider.GetRequiredService<IZarinpalService>();
    }

    [TestMethod]
    public async Task RequestPaymentTest()
    {
        // Arrange
        var request = new ZarinpalRequestDTO(5000, "خرید",
            "https://localhost:5000/Home/VerifyPayment");

        // Act
        var actual = await _zarinpalService.RequestAsync(request);

        // Assert
        Assert.IsTrue(actual.IsSuccessStatusCode);
    }

    [TestMethod]
    public async Task VerifyAsyncTest()
    {
        // Arrange
        var verify = new ZarinpalVerifyDTO
            (5000, "A00000000000000000000000000000000000");

        // Act
        var actual = await _zarinpalService.VerifyAsync(verify);

        // Assert
        Assert.IsFalse(actual.IsSuccessStatusCode);
    }
}