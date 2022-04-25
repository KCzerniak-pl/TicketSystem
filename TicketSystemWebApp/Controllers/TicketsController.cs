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
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get userID from session.
            Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

            // Retrieving data about selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(userID);

            // Check data about selected user - retrieving count about all ticket or only ticket for selected user.
            int ticetsCount = user.Role.ShowAll ? await _ticketsService.GetTicketsCountAsync() : await _ticketsService.GetTicketsCountAsync(userID);

            if (ticetsCount > 0)
            {
                // Check data about selected user - retrieving data about all ticket or only ticket for selected user.
                TicketViewModel[] tickets = user.Role.ShowAll ? await _ticketsService.GetTicketsAsync((page - 1) * take, take) : await _ticketsService.GetTicketsAsync((page - 1) * take, take, userID);

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
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = string.Format("/ticket/{0}", nameof(New)) });
            }

            // Get userID from session.
            Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

            // Check data about selected user - retrieving count about all ticket or only ticket for selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(userID);

            // Data for drop down list with categories.
            List<CategoryViewModel> categories = await _ticketsService.GetCategoriesAsync();
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
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = string.Format("/ticket/{0}", nameof(Edit)) });
            }

            // Get userID from session.
            Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

            // Check data about selected user - retrieving count about all ticket or only ticket for selected user.
            UserViewModel user = await _accountService.GetUserDataAsync(userID);

            // Retrieving data about selected ticket.
            TicketViewModel ticket = await _ticketsService.GetTicketAsync(ticketID, userID);

            // Data for drop down list with categories.
            List<CategoryViewModel> categories = await _ticketsService.GetCategoriesAsync();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            categories.ForEach(x => selectListItem.Add(new SelectListItem { Value = x.CategoryID.ToString(), Text = x.Name }));

            // Get statuses from "appsettings.json"
            IEnumerable<string> statuses = _configuration.GetSection("Statuses:Guid").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();

            ViewBag.CanAccepted = user.Role.CanAccepted;
            ViewBag.Categories = selectListItem;
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
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get userID from session.
            Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

            if (ModelState.IsValid)
            {
                // Add new ticket (used service from TicketsService).
                await _ticketsService.PostTicketAsync(ticket, userID);
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
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get userID from session.
                Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

                // Add new message for ticket (used service from TicketsService).
                await _ticketsService.PostMessageAsync(message, userID);
            }

            return RedirectToAction(nameof(Edit), new { message.TicketID });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> StatusUpdate(TicketStatusUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get userID from session.
                Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

                // Update status for ticket (used service from TicketsService).
                await _ticketsService.PutTicketStatusAsync(ticket, userID);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> TitleUpdate(TicketTitleUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get userID from session.
                Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

                // Update title for ticket (used service from TicketsService).
                await _ticketsService.PutTicketTitleAsync(ticket, userID);
            }

            return RedirectToAction(nameof(Edit), new { ticket.TicketID });
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateAntiForgeryTokenFailed))] // Filter executed in case of incorrect validation of the security token for form.
        public async Task<IActionResult> CategoryUpdate(TicketCategoryUpdateViewModel ticket)
        {
            // Check autorization for this site.
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (ModelState.IsValid)
            {
                // Get userID from session.
                Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

                // Update category for ticket (used service from TicketsService).
                await _ticketsService.PutTicketCategoryAsync(ticket, userID);
            }

            return RedirectToAction(nameof(Edit), new { ticket.TicketID });
        }

        [HttpGet("ticket/delete/{ticketID}")]
        public async Task<IActionResult> DeleteTicket(Guid ticketID)
        {
            // Check autorization for this site.
            if (!SessionHelper.CheckAuthorization(HttpContext))
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            // Get userID from session.
            Guid userID = SessionHelper.GetObjectFromJson<Guid>(HttpContext, "UserID");

            // Delete ticket (used service from TicketsService).
            await _ticketsService.DeleteTicketAsync(ticketID, userID);

            return RedirectToAction(nameof(Index));
        }
    }
}
