using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUEnrolment.Models;

namespace BUEnrolment.Controllers
{
    public class SubjectController : Controller
    {
        private BUEnrolmentContext db = new BUEnrolmentContext();

        //
        // GET: /Subject/

        public ActionResult Index()
        {
            return View(db.Subjects.Where(s => s.Active == true));
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

            if (SelectedPrerequisites != null && SelectedPrerequisites.Count > 0)
            {
                //Get a list of subject where the id is contained within the list of selectedPrerequisites
                foreach (Subject prerequisite in db.Subjects.Where(m => SelectedPrerequisites.Contains(m.Id)).ToList())
                subject.Prerequisites.Add(prerequisite);
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
        public ActionResult Edit(Subject subject, List<int> SelectedPrerequisites, int id = 0)
        {
            Subject subjectToUpdate = db.Subjects.Find(id);
            if (SelectedPrerequisites != null && SelectedPrerequisites.Count > 0)
            {
                //Get a list of subject where the id is contained within the list of selectedPrerequisites
                subjectToUpdate.Prerequisites.Clear();
                foreach (Subject prerequisite in db.Subjects.Where(m => SelectedPrerequisites.Contains(m.Id)).ToList())
                {
                    subjectToUpdate.Prerequisites.Add(prerequisite);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(subjectToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Subject> NonPrerequisites = db.Subjects.ToList().Except(subject.Prerequisites).ToList();
            NonPrerequisites.Remove(subject);
            ViewBag.SelectedPrerequisites = new SelectList(subject.Prerequisites, "Id", "Name");
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