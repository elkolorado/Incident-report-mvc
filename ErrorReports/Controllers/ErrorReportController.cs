using ErrorReports.Areas.Identity.Data;
using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Linq;

namespace ErrorReports.Controllers
{
    public class ErrorReportController : Controller
    {
        private readonly AppDBContext _contex;
        private static IList<ErrorReport> errorReports;
        //private static IList<ErrorReport> errorReports = new List<ErrorReport>
        //{
        //       new ErrorReport { Id = 1, Title = "Błąd w aplikacji", Description = "Aplikacja zawiesza się.", DateReported = DateTime.Now.AddHours(-3), ReporterName = "Jan Kowalski", Status = ErrorStatus.Open, Priority = ErrorPriority.High },
        //       new ErrorReport { Id = 2, Title = "Problem z logowaniem", Description = "Nie można zalogować do systemu.", DateReported = DateTime.Now.AddHours(-31), ReporterName = "Anna Nowak", Status = ErrorStatus.InProgress, Priority = ErrorPriority.Medium },

        //};

        public ErrorReportController(AppDBContext contex) { 
            _contex = contex;
            errorReports = contex.Incidents.ToList();
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

        // GET: ErrorReportController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ErrorReportController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ErrorReport report)
        {
            report.DateReported = DateTime.Now;
            report.Status = ErrorStatus.Open;
            _contex.Incidents.Add(report);
            _contex.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Edit/5
        [Authorize]
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

            _contex.Incidents.Update(existingReport);
            _contex.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: ErrorReportController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View(GetErrorReport(id));
        }

        // POST: ErrorReportController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ErrorReport report)
        {
            _contex.Incidents.Remove(GetErrorReport(id));
            _contex.SaveChanges();
            return RedirectToAction("Index");
        }

        public ErrorReport GetErrorReport(int id){
            return _contex.Incidents.Find(id);
        }
    }
}
