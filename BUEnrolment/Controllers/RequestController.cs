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
            List<Student> StudentList = new List<Student>();
            if (Roles.IsUserInRole("Admin"))
            {
                StudentList = db.Students.Where(s => s.Requests.Any(r => r.Status == "Pending")).Include(s => s.Requests.Select(r => r.Subject)).ToList();
            }
            else if (Roles.IsUserInRole("Student"))
            {
                Student student = db.Students.
                Include(m => m.Requests).
                Include(m => m.Requests.Select(r => r.Subject)).
                FirstOrDefault(s => s.Id == WebSecurity.CurrentUserId);
                StudentList.Add(student);
            }
            ViewBag.Students = StudentList;
            return View();
        }

        //
        // GET: /Request/Create

        public ActionResult Create()
        {
            Student student = db.Students.Include(s => s.EnrolledSubjects).FirstOrDefault();
            ViewBag.RequestableSubjects = new SelectList(student.GetRequestableSubjects(db.Subjects.ToList()), "Id", "Name");
            ViewBag.CurrentStudent = student;
            return View();
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
                Subject subject = db.Subjects.FirstOrDefault(s => s.Id == selectedSubject);

                // user not logged in or not found
                if (currentStudent == null)
                {
                    return RedirectToAction("Index");
                }

                request.Subject = subject;
                request.Status = "Pending";
                db.Entry(currentStudent).State = EntityState.Modified;
                db.Entry(currentStudent).Collection(m => m.Requests).Load();

                // check if the student has already made request for the same subject
                if (currentStudent.Requests.All(r => r.Subject.Id != selectedSubject))
                {
                    currentStudent.Requests.Add(request);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You have previously made a request for this subject already.");
                }
            }

            Student student = db.Students.Include(s => s.EnrolledSubjects).FirstOrDefault();
            ViewBag.RequestableSubjects = new SelectList(student.GetRequestableSubjects(db.Subjects.ToList()), "Id", "Name");
            ViewBag.CurrentStudent = student;

            return View(request);
        }

        public ActionResult Approve(int requestId, int studentId)
        {
            Request request = db.Requests.Include(r => r.Subject).FirstOrDefault(r => r.Id == requestId);
            request.Status = "Approve";
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Enrol", "Student", new { studentId = studentId, subjectId = request.Subject.Id });
        }

        public ActionResult Disapprove(int Id)
        {
            Request request = db.Requests.Include(r => r.Subject).FirstOrDefault(r => r.Id == Id);
            request.Status = "Disapprove";
            db.Entry(request).State = EntityState.Modified;
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