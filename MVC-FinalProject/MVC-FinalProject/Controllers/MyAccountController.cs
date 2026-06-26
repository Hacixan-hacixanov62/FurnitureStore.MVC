using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using MVC_FinalProject.ViewModels;
using NuGet.Configuration;

namespace MVC_FinalProject.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public MyAccountController(ISettingService settingService, IAccountService accountService,
                                   IOrderService orderService)
        {
            _settingService = settingService;
            _accountService = accountService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var setting = await _settingService.GetAllAsync();

            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token)) return Unauthorized();
            var userInfo = await _accountService.GetCurrentUserAsync(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var order = await _orderService.GetUserOrdersAsync(userId);

            MyAccountVM model = new MyAccountVM()
            {
                Setting = setting,
                Orders = order,
                UpdateEmail = new UpdateEmail
                {
                    CurrentEmail = userInfo?.Email
                },
                UpdateUsername = new UpdateUsername
                {
                    CurrentUsername = userInfo?.Username
                }
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(UpdateEmail model)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Please login again." });

            if (string.IsNullOrWhiteSpace(model.NewEmail))
            {
                return BadRequest(new { message = "Email is required." });
            }

            var result = await _accountService.UpdateEmailAsync(model, token);

            if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { message = result, redirectToLogin = true });
            }

            return BadRequest(new { message = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(UpdateUsername model)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Please login again." });

            if (string.IsNullOrWhiteSpace(model.NewUsername))
            {
                return BadRequest(new { message = "Username is required." });
            }

            var result = await _accountService.UpdateUsernameAsync(model, token);

            if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { message = result, redirectToLogin = true });
            }

            return BadRequest(new { message = result });
        }

            
    }
}
