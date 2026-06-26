using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Services.Interfaces;

public class PromoCodeController : Controller
{
    private readonly IPromoCodeService _promoCodeService;

    public PromoCodeController(IPromoCodeService promoCodeService)
    {
        _promoCodeService = promoCodeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string code)
    {
        var promo = await _promoCodeService.GetByCodeAsync(code);
        if (promo == null) return NotFound();

        return Json(promo);
    }

    //[HttpPost]
    //public async Task<IActionResult> UsePromoCode(string code)
    //{
    //    var success = await _promoCodeService.UsePromoCodeAsync(code);
    //    if (!success)
    //        return BadRequest(new { message = "The promo code could not be used." });

    //    return Ok(new { message = "Discount applied !" });
    //}

    // PromoController.cs içində UsePromoCode metodu
    [HttpPost]
    public async Task<IActionResult> UsePromoCode(string code)
    {
        var promo = await _promoCodeService.GetByCodeAsync(code);

        if (promo == null || !promo.IsActive || (promo.UsageLimit > 0 && promo.UsageCount >= promo.UsageLimit))
            return BadRequest();

        HttpContext.Session.SetString("ActivePromoCode", code); 

        var success = await _promoCodeService.UsePromoCodeAsync(code);

        if (!success)
            return BadRequest();

        return Ok();
    }

}
