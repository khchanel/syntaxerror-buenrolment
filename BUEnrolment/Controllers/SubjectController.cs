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

namespace BUEnrolment.Controllers
{
    public class SubjectController : Controller
    {
        private BUEnrolmentContext db = new BUEnrolmentContext();

        //
        // GET: /Subject/

        public ActionResult Index()
        {
            List<Subject> allSubjects = db.Subjects.Where(s => s.Active == true).ToList();
            if (Roles.IsUserInRole("Student")) 
            {
                Student student = db.Students.Include(m => m.EnrolledSubjects).FirstOrDefault(m => m.Id == WebSecurity.CurrentUserId);
               
                return View(student.GetEnrollableSubjects(allSubjects));
            }
            return View(allSubjects);
            
        }

        //
        // GET: /Subject/Details/5

        public ActionResult Details(int id = 0)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        //
        // GET: /Subject/Create

        public ActionResult Create()
        {
            SelectList allSubjects = new SelectList(db.Subjects, "Id", "Name");
            ViewBag.allSubjects = allSubjects;
            return View();
        }

        //
        // POST: /Subject/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject subject, List<int> SelectedPrerequisites)
        {
            ViewBag.allSubjects = new SelectList(db.Subjects, "Id", "Name");
            if (SelectedPrerequisites != null && SelectedPrerequisites.Count > 0)
            {
                //Get a list of subject where the id is contained within the list of selectedPrerequisites
                List<Subject> Prerequisites = db.Subjects.Where(m => SelectedPrerequisites.Contains(m.Id)).ToList();
                Prerequisites.ForEach(m => subject.Prerequisites.Add(m));
            }
            subject.Active = true;
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        //
        // GET: /Subject/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            List<Subject> NonPrerequisites = db.Subjects.ToList().Except(subject.Prerequisites).ToList();
            NonPrerequisites.Remove(subject);
            ViewBag.SelectedPrerequisites = new SelectList(subject.Prerequisites, "Id", "Name");
            ViewBag.NonPrerequisites = new SelectList(NonPrerequisites, "Id", "Name");
            return View(subject);
        }

        //
        // POST: /Subject/Edit/5

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
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Subject> NonPrerequisites = db.Subjects.ToList().Except(SelectedPrerequisites).ToList();
            NonPrerequisites.Remove(db.Subjects.Find(id));
            ViewBag.SelectedPrerequisites = new SelectList(SelectedPrerequisites, "Id", "Name");
            ViewBag.NonPrerequisites = new SelectList(NonPrerequisites, "Id", "Name");
            return View(subject);
        }

        //
        // GET: /Subject/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        //
        // POST: /Subject/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Subject subject)
        {
            subject.Active = false;
            db.Entry(subject).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}