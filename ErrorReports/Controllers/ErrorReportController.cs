using ErrorReports.Areas.Identity.Data;
using ErrorReports.Authorization;
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
        private readonly IAuthorizationService _authorizationService;
        private UserManager<AppUser> UserManager { get; }

        public ErrorReportController(AppDBContext contex, IAuthorizationService authorizationService, UserManager<AppUser> userManager)
        {
            _contex = contex;
            errorReports = contex.Incidents.ToList();
            _authorizationService = authorizationService;
            UserManager = userManager;

        }

        // GET: ErrorReportController
        [Authorize]
        public ActionResult Index()
        {
            return View(errorReports);
        }

        // GET: ErrorReportController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {

            return View(GetErrorReport(id));
        }

        [BindProperty]
        public ErrorReport ErrorReport { get; set; }

        // GET: ErrorReportController/Create
        [Authorize]
        public async Task<ActionResult> CreateAsync()
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                        User, ErrorReport,
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
        public async Task<ActionResult> CreateAsync(ErrorReport report)
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
        public async Task<ActionResult> EditAsync(int id)
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
        public async Task<ActionResult> EditAsync(int id, ErrorReport updatedReport)
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

        // GET: ErrorReportController/Delete/5
        [Authorize]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, GetErrorReport(id),
                IncidentOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, ErrorReport report)
        {
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User, report,
                IncidentOperations.Update);
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
    }
}
