using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BUEnrolment.Models;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;

namespace BUEnrolment.Controllers
{
    /// <summary>
    /// Controller for subject related pages
    /// </summary>
    public class SubjectController : Controller
    {
        /// <summary>
        /// database context for entity framework
        /// </summary>
        private BUEnrolmentContext db = new BUEnrolmentContext();

        public SubjectController(BUEnrolmentContext db)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            this.db = db;
        }

        public SubjectController()
        {

        }
        /// <summary>
        /// GET: /Subject/
        /// 
        /// List subject page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Subject> allSubjects = db.Subjects.Where(s => s.Active).ToList();

            // if user is student, filter down the list to only show what they can enroll
            if (Roles.IsUserInRole("Student")) 
            {
                Student student = db.Students.Include(m => m.EnrolledSubjects).FirstOrDefault(m => m.Id == WebSecurity.CurrentUserId);
               
                return View(student.GetEnrollableSubjects(allSubjects));
            }
            return View(allSubjects);
            
        }

        /// <summary>
        /// GET: /Subject/Details/5
        ///
        /// View subject details pages
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        /// <summary>
        /// Student View Enrolled subject list
        /// </summary>
        /// <returns></returns>
        public ActionResult Enrolled()
        {
            Student student = db.Students.Include(m => m.EnrolledSubjects).FirstOrDefault(m => m.Id == WebSecurity.CurrentUserId);

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student.EnrolledSubjects);
        }

        /// <summary>
        /// GET: /Subject/Create
        ///
        /// Create subject form page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            SelectList allSubjects = new SelectList(db.Subjects.Where(s => s.Active), "Id", "Name");
            ViewBag.allSubjects = allSubjects;
            return View();
        }

        /// <summary>
        /// Create subject handler
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="SelectedPrerequisites"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject subject, List<int> SelectedPrerequisites)
        {
            bool subjectInContext = (db.Subjects.FirstOrDefault(s => s.SubjectNumber == subject.SubjectNumber) != null);
            ViewBag.allSubjects = new SelectList(db.Subjects.Where(s => s.Active), "Id", "Name");
            if (SelectedPrerequisites != null && SelectedPrerequisites.Count > 0)
            {
                //Get a list of subject where the id is contained within the list of selectedPrerequisites
                List<Subject> Prerequisites = db.Subjects.Where(m => SelectedPrerequisites.Contains(m.Id)).ToList();
                Prerequisites.ForEach(m => subject.Prerequisites.Add(m));
            }
            if (!subjectInContext)
            {
                subject.Active = true;
                if (ModelState.IsValid)
                {
                    db.Subjects.Add(subject);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The subject already exist in the system");
            }

            return View(subject);
        }

        /// <summary>
        /// Subject edit form page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            else if (subject.Active == false)
            {
                return RedirectToAction("Index");
            }
            List<Subject> NonPrerequisites = db.Subjects.Where(s => s.Active).ToList().Except(subject.Prerequisites).ToList();
            NonPrerequisites.Remove(subject);
            ViewBag.SelectedPrerequisites = new SelectList(subject.Prerequisites, "Id", "Name");
            ViewBag.NonPrerequisites = new SelectList(NonPrerequisites, "Id", "Name");
            return View(subject);
        }

        /// <summary>
        /// Subject edit handler
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="PrerequisiteList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subject subject, List<int> PrerequisiteList, int id = 0)
        {
            List<Subject> SelectedPrerequisites = new List<Subject>();
            
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.Entry(subject).Collection(m => m.Prerequisites).Load();
                subject.Prerequisites.Clear();
                if (PrerequisiteList != null && PrerequisiteList.Count > 0)
                {
                    //Get a list of subject where the id is contained within the list of selectedPrerequisites
                    SelectedPrerequisites = db.Subjects.Where(m => PrerequisiteList.Contains(m.Id)).ToList();
                    SelectedPrerequisites.ForEach(m => subject.Prerequisites.Add(m));
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = (Subject)entry.GetDatabaseValues().ToObject();
                    var clientValues = (Subject)entry.Entity;

                    if (databaseValues.Active == false)
                    {
                        return RedirectToAction("Index");
                    }

                    /* Concurrency checking */
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");

                    if (databaseValues.SubjectNumber != clientValues.SubjectNumber)
                    {
                        ModelState.AddModelError("SubjectNumber", "Current Subject Number: " + databaseValues.SubjectNumber);
                    }

                    if (databaseValues.Name != clientValues.Name)
                    {
                        ModelState.AddModelError("Name", "Current Value: " + databaseValues.Name);
                    }

                    if (databaseValues.MaxEnrolment != clientValues.MaxEnrolment)
                    {
                        ModelState.AddModelError("MaxEnrolment", "Current Max Enrolment: " + databaseValues.Name);
                    }
                    /* end concurrency checking */
                }
            }
            List<Subject> NonPrerequisites = db.Subjects.ToList().Except(SelectedPrerequisites).ToList();
            NonPrerequisites.Remove(db.Subjects.Find(id));
            ViewBag.SelectedPrerequisites = new SelectList(SelectedPrerequisites, "Id", "Name");
            ViewBag.NonPrerequisites = new SelectList(NonPrerequisites, "Id", "Name");
            return View(subject);
        }

        /// <summary>
        /// Subject Delete
        /// </summary>
        /// <param name="concurrencyError"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(bool? concurrencyError, int id = 0)
        {
            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }
            Subject subject = db.Subjects.Find(id);
            return View(subject);
        }

        /// <summary>
        /// Subject delete confirmation
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Subject subject)
        {

            try
            {
                subject.Active = false;
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary { { "concurrencyError", true } });
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}