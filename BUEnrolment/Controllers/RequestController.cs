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
    public class RequestController : Controller
    {
        private BUEnrolmentContext db = new BUEnrolmentContext();

        //
        // GET: /Request/

        public ActionResult Index()
        {

            List<Request> testthis = db.Requests.ToList();
            return View(db.Requests.Include(m => m.Subject).Where(m => m.Status == "Pending").ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Request request)
        {
            return View(db.Requests.ToList());
        }

        //
        // GET: /Request/Details/5

        public ActionResult Details(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // GET: /Request/Create

        public ActionResult Create()
        {
            Student student = db.Students.FirstOrDefault();
            //ViewBag.RequestableSubjects = new SelectList(student.GetRequestableSubjects(db.Subjects.ToList()), "Id", "Name");
            ViewBag.RequestableSubjects = new SelectList(db.Subjects.ToList(), "Id", "Name");
            return View();
        }

        public ActionResult Approve(int Id)
        {
            Request request = db.Requests.FirstOrDefault(r => r.Id == Id);
            request.Status = "Approve";
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Disapprove(int Id)
        {
            Request request = db.Requests.FirstOrDefault(r => r.Id == Id);
            request.Status = "Disapprove";
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // 
        // POST: /Request/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Request request, int selectedSubject)
        {
            if (ModelState.IsValid)
            {
                Student currentStudent = db.Students.FirstOrDefault(s => s.Id == WebSecurity.CurrentUserId);

                request.Subject = db.Subjects.FirstOrDefault(s => s.Id == selectedSubject);
                request.Status = "Pending";
                db.Entry(currentStudent).State = EntityState.Modified;
                db.Entry(currentStudent).Collection(m => m.Requests).Load();
                currentStudent.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        //
        // GET: /Request/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // POST: /Request/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        //
        // GET: /Request/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // POST: /Request/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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