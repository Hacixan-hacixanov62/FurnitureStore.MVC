
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Register = MVC_FinalProject.Models.Account.Register;

namespace MVC_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ISettingService _settingService;
        public AccountController(IAccountService accountService, ISettingService settingService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

            var response = await _accountService.Login(model);
            var content = await response.Content.ReadAsStringAsync();

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse != null && loginResponse.Success)
            {
                HttpContext.Session.SetString("AuthToken", loginResponse.Token);
                HttpContext.Session.SetString("UserName", loginResponse.UserName ?? "");

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginResponse.UserName ?? ""),
            new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId),
            new Claim("access_token", loginResponse.Token ?? "")
        };

                if (loginResponse.Roles != null)
                {
                    foreach (var role in loginResponse.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }

            return Json(new
            {
                success = false,
                errors = new List<string> { loginResponse?.Error ?? "Invalid login attempt." }
            });
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = errors });
            }

            var responseMessage = await _accountService.Register(request);

            var wrapper = await responseMessage.Content.ReadFromJsonAsync<RegisterResponseWrapper>();
            var content = wrapper?.Value;

            if (content != null && !content.Success)
            {
                return Json(new { success = false, message = content.Message });
            }

            if (responseMessage.IsSuccessStatusCode && content?.Success == true)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = new[] { "An unexpected error occurred." } });
        }


        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var response = await _accountService.ForgetPasswordAsync(email);

            if (response.StatusCode == 400 || response.StatusCode == 404)
                return BadRequest(new { error = response.ResponseMessage });
            return Ok(new { redirectUrl = Url.Action("ForgetPasswordConfirmation", "Account") });
        }


        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new UserPassword
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserPassword userPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userPasswordVM);
            }

            var responseMessage = await _accountService.ResetPasswordAsync(userPasswordVM);

            // Bu hissəni genişləndir:
            if (responseMessage == "Invalid request." ||
                responseMessage == "User not found" ||
                responseMessage == "TokenIsNotValid" ||
                responseMessage == "New password cannot be the same as the old password.")
            {
                ModelState.AddModelError("", responseMessage);
                return View(userPasswordVM);
            }

            TempData["Message"] = responseMessage;
            return RedirectToAction("ResetPasswordConfirmation");
        }


        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("AuthToken");

            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();

            return RedirectToAction("LogoutRedirect");
        }

        [HttpGet]
        public IActionResult LogoutRedirect()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
