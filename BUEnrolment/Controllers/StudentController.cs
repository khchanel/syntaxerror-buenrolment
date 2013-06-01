using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUEnrolment.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace BUEnrolment.Controllers
{
    public class StudentController : Controller
    {
        private BUEnrolmentContext db = new BUEnrolmentContext();

        //
        // GET: /Student/Enrol/5
        public ActionResult Enrol(int studentId, int subjectId)
        {
            Student student = db.Students.Include(s => s.Requests).FirstOrDefault(s => s.Id == studentId);
            Subject subject = db.Subjects.FirstOrDefault(s => s.Id == subjectId);

            if (!student.FullyEnrolled() && !subject.MaxEnrolmentIsReached())
            {
                db.Entry(student).Collection(s => s.EnrolledSubjects).Load();
                student.EnrolSubject(subject);
                db.SaveChanges();
            }
            else
            {
                if (student.FullyEnrolled())
                {
                    foreach (Request request in student.Requests)
                    {
                        request.Status = "Disapprove";
                    }
                }
            }

            if (Roles.IsUserInRole("Admin"))
            {
                return RedirectToAction("Index", "Request");
            }

            return RedirectToAction("Index", "Subject");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}