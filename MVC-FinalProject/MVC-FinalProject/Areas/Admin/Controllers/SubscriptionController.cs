using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Subscription;
using MVC_FinalProject.Services;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _subscriptionService.GetAllSubscriptionsAsync();
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscriptionEmail request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email))
            {
                return BadRequest("Email is required.");
            }

            var response = await _subscriptionService.UnsubscribeAsync(request.Email);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = $"User with email '{request.Email}' has been unsubscribed." });
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMsg);
            }
        }

    }
}
