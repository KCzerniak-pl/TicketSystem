using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketSystemWebApp.Helpers;
using TicketSystemWebApp.Models;
using TicketSystemWebApp.Services;

namespace TicketSystemWebApp.Controllers
{
    [AutoValidateAntiforgeryToken] // Validation filter for security token in forms.
    public class TicketsController : Controller
    {
        private readonly ITicketsService _ticketsService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        // Dependency injection - inverse of control. Required configuration in the Program.cs.
        // Service injection from TicketsService and AccountService. Controller can use the connection via WebApi and does not have to create it himself.
        // Additionally injection IConfiguration which refers to configuration from appsettings.json file.
        public TicketsController(ITicketsService ticketsService, IAccountService accountService, IConfiguration Configuration)
        {
            _ticketsService = ticketsService;
            _accountService = accountService;
            _configuration = Configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int take = 15)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get JWT from session.
            string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

            // Get userId from JWT.
            Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt!, "UserId");

            // Retrieving data about selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(jwt!);

            // Retrieving count about all ticket or only ticket for selected user.
            int ticetsCount = await _ticketsService.GetTicketsCountAsync(jwt!, user.Role!.ShowAll);

            if (ticetsCount > 0)
            {
                // Retrieving data about all ticket or only ticket for selected user.
                TicketViewModel[] tickets = await _ticketsService.GetTicketsAsync(jwt!, (page - 1) * take, take, user.Role.ShowAll);

                // Retrieving data about all statuses.
                IEnumerable<string[]> statuses = _configuration.GetSection("Statuses").Get<Dictionary<string, string[]>>().Select(p => p.Value);
                
                ViewData["statuses"] = statuses;
                ViewBag.Page = page;
                ViewBag.totalPages = (ticetsCount + take - 1) / take;

                return View(tickets);
            }

            return View(new TicketViewModel[0]);
        }

        [HttpGet("ticket/new")]
        public async Task<IActionResult> New()
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = string.Format("/ticket/{0}", nameof(New)) });
            }

            // Get JWT from session.
            string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

            // Retrieving data about selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(jwt!);

            // Data for drop down list with categories.
            List<CategoryViewModel> categories = await _ticketsService.GetCategoriesAsync(jwt!);
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            categories.ForEach(x => selectListItem.Add(new SelectListItem { Value = x.CategoryId.ToString(), Text = x.Name }));

            ViewBag.Categories = selectListItem;
            ViewBag.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
            ViewBag.UserEmail = user.Email;

            return View();
        }

        [HttpGet("ticket/edit/{ticketId}")]
        public async Task<IActionResult> Edit(Guid ticketId)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = string.Format("/ticket/{0}", nameof(Edit)) });
            }

            // Get JWT from session.
            string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

            // Retrieving data about selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(jwt!);

            // Retrieving data about selected ticket.
            TicketViewModel ticket = await _ticketsService.GetTicketAsync(jwt!, ticketId);

            // Retrieving data about categories.
            List<CategoryViewModel> categories = await _ticketsService.GetCategoriesAsync(jwt!);
            // Data for drop down list with categories.
            List<SelectListItem> selectListItemForCategories = new List<SelectListItem>();
            categories.ForEach(x => selectListItemForCategories.Add(new SelectListItem { Value = x.CategoryId.ToString(), Text = x.Name }));

            // Retrieving data about technicians.
            List<UserViewModel> technicians = await _accountService.GetTechniciansAsync(jwt!);
            // Data for drop down list with technicians.
            List<SelectListItem> selectListItemForTechnicians = new List<SelectListItem>();
            technicians.ForEach(x => selectListItemForTechnicians.Add(new SelectListItem { Value = x.UserId.ToString(), Text = x.FirstName + " " + x.LastName }));

            // Check if ticket has the selected technician.
            if (ticket.TechnicianId is null && !user.Role!.CanAccepted)
            {
                ticket.TechnicianName = "&nbsp";
            }
            else
            {
                ticket.TechnicianName = (ticket.TechnicianId is not null) ? technicians.Where(p => p.UserId == ticket.TechnicianId).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault() : technicians.Select(p => p.FirstName + " " + p.LastName).FirstOrDefault();
            }

            // Retrieving data about all statuses.
            var statuses = _configuration.GetSection("Statuses").Get<Dictionary<string, string[]>>();

            ViewData["statuses"] = statuses;
            ViewBag.CanAccepted = user.Role!.CanAccepted;
            ViewBag.Categories = selectListItemForCategories;
            ViewBag.Technicians = selectListItemForTechnicians;

            return View(ticket);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> TicketNew(TicketNewViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                // Add new ticket (used service from TicketsService).
                await _ticketsService.PostTicketAsync(jwt!, ticket);
            }
            else
            {
                return RedirectToAction(nameof(New));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> MessageNew(MessageNewViewModel message)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                // Add new message for ticket (used service from TicketsService).
                await _ticketsService.PostMessageAsync(jwt!, message);
            }

            return RedirectToAction(nameof(Edit), new { message.TicketId });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> StatusUpdate(TicketStatusUpdateViewModel ticket, Guid? technicianId)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                if (technicianId is null)
                {
                    // Retrieving data about technicians.
                    List<UserViewModel> technicians = await _accountService.GetTechniciansAsync(jwt!);

                    // First technician is default.
                    technicianId = technicians.Select(p => p.UserId).FirstOrDefault();
                }

                // Update status for ticket (used service from TicketsService).
                await _ticketsService.PutTicketStatusAsync(jwt!, ticket, technicianId ?? default);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> TitleUpdate(TicketTitleUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                // Update title for ticket (used service from TicketsService).
                await _ticketsService.PutTicketTitleAsync(jwt!, ticket);
            }

            return RedirectToAction(nameof(Edit), new { ticket.TicketId });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> CategoryUpdate(TicketCategoryUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                // Update category for ticket (used service from TicketsService).
                await _ticketsService.PutTicketCategoryAsync(jwt!, ticket);
            }

            return RedirectToAction(nameof(Edit), new { ticket.TicketId });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> TechnicianUpdate(TicketTechnicianUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get JWT from session.
                string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

                // Update category for ticket (used service from TicketsService).
                await _ticketsService.PutTicketTechnicianAsync(jwt!, ticket);
            }

            return RedirectToAction(nameof(Edit), new { ticket.TicketId });
        }

        [HttpGet("ticket/delete/{ticketId}")]
        public async Task<IActionResult> DeleteTicket(Guid ticketId)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get JWT from session.
            string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

            // Delete ticket (used service from TicketsService).
            await _ticketsService.DeleteTicketAsync(jwt!, ticketId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Exception handler.
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            // Error data.
            var message = exceptionHandlerPathFeature?.Error.Message.Replace("\n", "<br/>") ?? "---";

            return View(new ErrorViewModel { Message = message });
        }
    }
}
