using Microsoft.AspNetCore.Mvc;
using TicketSystemWebApp.Helpers;
using TicketSystemWebApp.Models;
using TicketSystemWebApp.Services;

namespace TicketSystemWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        // Dependency injection - inverse of control. Required configuration in the Program.cs.
        // Service injection from AccountService. Controller can use the connection via WebApi and does not have to create it himself.
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Check autorization for this site.
            bool authorization = SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization");
            if (authorization)
            {
                return RedirectToAction(nameof(TicketsController.Index), "Tickets");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> Login(LoginViewModel user, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // logging user (used service from AccountService).
                LoginResponseDto loginResponse = await _accountService.LoginAsync(user);

                if (loginResponse.Success)
                {
                    // Save data for logged user in session / cookies.
                    SessionHelper.SetObjectAsJson(HttpContext, "Jwt", loginResponse.Jwt.ToString(), user.RemeberMe);
                    SessionHelper.SetObjectAsJson(HttpContext, "Authorization", loginResponse.Success.ToString(), user.RemeberMe);

                    return RedirectToLocal(returnUrl!);
                }

                ViewBag.Errors = loginResponse.Error;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Delete session / cookies.
            SessionHelper.Logout(HttpContext);

            return RedirectToAction(nameof(Login));
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            // Redirect to local site.
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(TicketsController.Index), "Tickets");
        }
    }
}
