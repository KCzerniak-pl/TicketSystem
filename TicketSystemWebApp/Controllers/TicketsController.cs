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

            // Get userID from JWT.
            Guid userID = Jwt.GetObjectFromJwt<Guid>(jwt!, "UserID");

            // Retrieving data about selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(jwt!);

            // Retrieving count about all ticket or only ticket for selected user.
            int ticetsCount = await _ticketsService.GetTicketsCountAsync(jwt!, user.Role.ShowAll);

            if (ticetsCount > 0)
            {
                // Retrieving data about all ticket or only ticket for selected user.
                TicketViewModel[] tickets = await _ticketsService.GetTicketsAsync(jwt!, (page - 1) * take, take, user.Role.ShowAll);

                // Retrieving data about all statuses.
                IEnumerable<string> statuses = _configuration.GetSection("Statuses:Guid").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();

                ViewBag.StatusNew = statuses.FirstOrDefault();
                ViewBag.StatusAccept = statuses.Skip(1).FirstOrDefault();
                ViewBag.StatusDiscard = statuses.Skip(2).FirstOrDefault();
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
            categories.ForEach(x => selectListItem.Add(new SelectListItem { Value = x.CategoryID.ToString(), Text = x.Name }));

            ViewBag.Categories = selectListItem;
            ViewBag.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
            ViewBag.UserEmail = user.Email;

            return View();
        }

        [HttpGet("ticket/edit/{ticketID}")]
        public async Task<IActionResult> Edit(Guid ticketID)
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
            TicketViewModel ticket = await _ticketsService.GetTicketAsync(jwt!, ticketID);

            // Retrieving data about categories.
            List<CategoryViewModel> categories = await _ticketsService.GetCategoriesAsync(jwt!);
            // Data for drop down list with categories.
            List<SelectListItem> selectListItemForCategories = new List<SelectListItem>();
            categories.ForEach(x => selectListItemForCategories.Add(new SelectListItem { Value = x.CategoryID.ToString(), Text = x.Name }));

            // Retrieving data about technicians.
            List<UserViewModel> technicians = await _accountService.GetTechniciansAsync(jwt!);
            // Data for drop down list with technicians.
            List<SelectListItem> selectListItemForTechnicians = new List<SelectListItem>();
            technicians.ForEach(x => selectListItemForTechnicians.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }));

            // Check if ticket has the selected technician.
            if (ticket.TechnicianID is null && !user.Role.CanAccepted)
            {
                ticket.TechnicianName = "&nbsp";
            }
            else
            {
                ticket.TechnicianName = (ticket.TechnicianID is not null) ? technicians.Where(p => p.UserID == ticket.TechnicianID).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault() : technicians.Select(p => p.FirstName + " " + p.LastName).FirstOrDefault();
            }

            // Get statuses from "appsettings.json".
            IEnumerable<string> statuses = _configuration.GetSection("Statuses:Guid").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();

            ViewBag.CanAccepted = user.Role.CanAccepted;
            ViewBag.Categories = selectListItemForCategories;
            ViewBag.Technicians = selectListItemForTechnicians;
            ViewBag.StatusNew = statuses.FirstOrDefault();
            ViewBag.StatusAccept = statuses.Skip(1).Take(1).FirstOrDefault();
            ViewBag.StatusDiscard = statuses.Skip(2).Take(1).FirstOrDefault();

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

            return RedirectToAction(nameof(Edit), new { message.TicketID });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> StatusUpdate(TicketStatusUpdateViewModel ticket, Guid? technicianID)
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

                if (technicianID is null)
                {
                    // Retrieving data about technicians.
                    List<UserViewModel> technicians = await _accountService.GetTechniciansAsync(jwt!);

                    // First technician is default.
                    technicianID = technicians.Select(p => p.UserID).FirstOrDefault();
                }

                // Update status for ticket (used service from TicketsService).
                await _ticketsService.PutTicketStatusAsync(jwt!, ticket, technicianID ?? default);
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

            return RedirectToAction(nameof(Edit), new { ticket.TicketID });
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

            return RedirectToAction(nameof(Edit), new { ticket.TicketID });
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

            return RedirectToAction(nameof(Edit), new { ticket.TicketID });
        }

        [HttpGet("ticket/delete/{ticketID}")]
        public async Task<IActionResult> DeleteTicket(Guid ticketID)
        {
            // Check autorization for this site.
            if (!SessionHelper.GetObjectFromJson<bool>(HttpContext, "Authorization"))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get JWT from session.
            string? jwt = SessionHelper.GetObjectFromJson<string>(HttpContext, "Jwt");

            // Delete ticket (used service from TicketsService).
            await _ticketsService.DeleteTicketAsync(jwt!, ticketID);

            return RedirectToAction(nameof(Index));
        }
    }
}
