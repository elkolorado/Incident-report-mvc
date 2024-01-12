using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ErrorReports.Controllers
{
    public class ErrorReportController : Controller
    {
        private static IList<ErrorReport> errorReports = new List<ErrorReport>
        {
               new ErrorReport { Id = 1, Title = "Błąd w aplikacji", Description = "Aplikacja zawiesza się.", DateReported = DateTime.Now.AddHours(-3), ReporterName = "Jan Kowalski", Status = ErrorStatus.Open, Priority = ErrorPriority.High },
               new ErrorReport { Id = 2, Title = "Problem z logowaniem", Description = "Nie można zalogować do systemu.", DateReported = DateTime.Now.AddHours(-31), ReporterName = "Anna Nowak", Status = ErrorStatus.InProgress, Priority = ErrorPriority.Medium },

        };

        // GET: ErrorReportController
        [Authorize]
        public ActionResult Index()
        {
            return View(errorReports);
        }

        // GET: ErrorReportController/Details/5
        public ActionResult Details(int id)
        {

            return View(GetErrorReport(id));
        }

        // GET: ErrorReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ErrorReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ErrorReport report)
        {
            report.Id = errorReports.Count + 1;
            report.DateReported = DateTime.Now;
            report.Status = ErrorStatus.Open;
            errorReports.Add(report);
            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ErrorReport updatedReport)
        {
            var existingReport = GetErrorReport(id);
            if (existingReport == null)
            {
                return RedirectToAction("Index");
            }
            existingReport.Title = updatedReport.Title;
            existingReport.Description = updatedReport.Description;
            existingReport.Status = updatedReport.Status;
            existingReport.Priority = updatedReport.Priority;
            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ErrorReport report)
        {
            errorReports.Remove(GetErrorReport(id));
            return RedirectToAction("Index");
        }

        public ErrorReport GetErrorReport(int id){
            return errorReports.FirstOrDefault(x => x.Id == id);
        }
    }
}
