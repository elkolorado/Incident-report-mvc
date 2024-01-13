using ErrorReports.Areas.Identity.Data;
using ErrorReports.Authorization;
using ErrorReports.Migrations;
using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Composition;
using System.Linq;

namespace ErrorReports.Controllers
{
    public class ErrorReportController : Controller
    {
        private readonly AppDBContext _contex;
        private static IList<ErrorReport> errorReports;
        private static IList<HelpDeskAssignment> assignedReports;

        private readonly IAuthorizationService _authorizationService;
        private UserManager<AppUser> UserManager { get; set; }

        public ErrorReportController(AppDBContext contex, IAuthorizationService authorizationService, UserManager<AppUser> userManager)
        {
            _contex = contex;
            errorReports = contex.Incidents.ToList();
            assignedReports = contex.HelpDeskAssignments.ToList();
            _authorizationService = authorizationService;
            UserManager = userManager;

        }

        // GET: ErrorReportController
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var authorizedReports = new List<ErrorReport>();

            foreach (var errorReport in errorReports)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(
                    User, GetErrorReport(errorReport.Id),
                    IncidentOperations.Read);

                if (isAuthorized.Succeeded)
                {
                    authorizedReports.Add(errorReport);
                }
            }
            return View(authorizedReports);
        }

        // GET: ErrorReportController/Assigned
        [Authorize]
        public async Task<ActionResult> Assigned()
        {
            var userId = UserManager.GetUserId(User);
            IList<ErrorReport> assignedToUser = errorReports
        .Where(report => assignedReports.Any(a => a.HelpDeskUserId == userId && a.ErrorReportId == report.Id))
        .ToList();

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                        User, new ErrorReport(),
                                                        IncidentOperations.Assign);

            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View(assignedToUser);
            
        }




        // GET: ErrorReportController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            ErrorReport report = GetErrorReport(id);
            report.Comments = _contex.ErrorComments.Where(a => a.ErrorReportId == report.Id).ToList();
            var user = await UserManager.FindByIdAsync(report.ReporterName);
            if (user != null)
            {
                ViewData["user"] = user.Email;

            }
            bool assigned = assignedReports.Any(a => a.ErrorReportId == id);
            bool isOwnHelpDesk = assignedReports.Any(a => a.HelpDeskUserId == UserManager.GetUserId(User));
            ViewData["assigned"] = assigned;
            ViewData["owner"] = isOwnHelpDesk;
            return View(report);
        }


        // GET: ErrorReportController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            ErrorReport errorReport = new ErrorReport();
            errorReport.ReporterName = UserManager.GetUserId(User);
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                        User, errorReport,
                                                        IncidentOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: ErrorReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ErrorReport report)
        {
            report.DateReported = DateTime.Now;
            report.Status = ErrorStatus.Open;
            report.ReporterName = UserManager.GetUserId(User);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, report,
                IncidentOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }

            _contex.Incidents.Add(report);
            _contex.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, GetErrorReport(id),
                IncidentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ErrorReport updatedReport)
        {
            var existingReport = GetErrorReport(id);
            if (existingReport == null)
            {
                return RedirectToAction("Index");
            }
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, existingReport,
                IncidentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }

            existingReport.Title = updatedReport.Title;
            existingReport.Description = updatedReport.Description;
            existingReport.Status = updatedReport.Status;
            existingReport.Priority = updatedReport.Priority;

            _contex.Incidents.Update(existingReport);
            _contex.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Accept(int id)
        {
            var errorReport = GetErrorReport(id);
            if (errorReport == null)
            {
                return RedirectToAction("Index");
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, errorReport,
                IncidentOperations.Accept);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }

            var helpDeskUser = await UserManager.GetUserAsync(User);
            if (helpDeskUser == null)
            {
                return RedirectToAction("Index");
            }

            var assignment = new HelpDeskAssignment
            {
                ErrorReportId = id,
                HelpDeskUserId = helpDeskUser.Id,
                AssignedDate = DateTime.Now
            };

            _contex.HelpDeskAssignments.Add(assignment);
            _contex.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, GetErrorReport(id),
                IncidentOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }
            var user = await UserManager.FindByIdAsync(GetErrorReport(id).ReporterName);

            if (user != null)
            {
                ViewData["user"] = user.Email;
            }
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ErrorReport report)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, GetErrorReport(id),
                IncidentOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }
            _contex.Incidents.Remove(GetErrorReport(id));
            _contex.SaveChanges();
            return RedirectToAction("Index");
        }

        public ErrorReport GetErrorReport(int id)
        {
            return _contex.Incidents.Find(id);
        }
        // POST: ErrorReportController/AddComment/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(int id, string comment)
        {
            var errorReport = GetErrorReport(id);
            if (errorReport == null)
            {
                return RedirectToAction("Index");
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, errorReport,
                IncidentOperations.Comment);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }

            var errorComment = new ErrorComment
            {
                ErrorReportId = id,
                Content = comment,
                CommentDate = DateTime.Now,
                UserId = UserManager.GetUserId(User)
            };

            _contex.ErrorComments.Add(errorComment);
            _contex.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }
    }
}
