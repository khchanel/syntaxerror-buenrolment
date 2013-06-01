using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUEnrolment.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace BUEnrolment.Controllers
{
    public class ResultController : Controller
    {
        private BUEnrolmentContext db = new BUEnrolmentContext();

        //
        // GET: /Result/

        public ActionResult Index(int id)
        {
            Subject subject = db.Subjects.Include(s => s.EnrolledStudents).Include(s => s.EnrolledStudents.Select(e => e.CompletedSubject)).FirstOrDefault(s => s.Id == id);
            ViewBag.subject = subject;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(List<Result> results, int Id)
        {
            if (ModelState.IsValid)
            {
                Subject subject = db.Subjects.Include(s => s.EnrolledStudents).FirstOrDefault(s => s.Id == Id);
                for (int i = 0; i < subject.EnrolledStudents.Count; i++)
                {
                    Student enrolledStudent = subject.EnrolledStudents[i];
                    results[i].Subject = subject;
                    enrolledStudent.CompleteSubject(results[i]);
                }
                db.SaveChanges();
                subject.EnrolledStudents = new List<Student>();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(results);
        }

        //
        // GET: /Result/Details/5

        public ActionResult Details()
        {
           /* Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);*/

            Student student = db.Students.Include(s => s.CompletedSubject).Include(s => s.CompletedSubject.Select(c => c.Subject)).FirstOrDefault(s => s.Id == WebSecurity.CurrentUserId);
            return View(student.CompletedSubject);

        }

        //
        // GET: /Result/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Result/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(result);
        }

        //
        // GET: /Result/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(result);
        }

        //
        // GET: /Result/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //
        // POST: /Result/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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