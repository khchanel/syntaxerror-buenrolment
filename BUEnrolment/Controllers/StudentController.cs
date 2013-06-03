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
    /// <summary>
    /// Controller for Student related pages
    /// </summary>
    public class StudentController : Controller
    {
        /// <summary>
        /// Database context
        /// </summary>
        private BUEnrolmentContext db = new BUEnrolmentContext();

        public StudentController(BUEnrolmentContext db)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            this.db = db;
        }

        public StudentController()
        {

        }

        /// <summary>
        /// GET: /Student/Enrol/5
        ///
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public ActionResult Enrol(int studentId, int subjectId)
        {
            Student student = db.Students.Include(s => s.Requests).FirstOrDefault(s => s.Id == studentId);
            Subject subject = db.Subjects.FirstOrDefault(s => s.Id == subjectId);

            // check if the enrollment limits are not reached
            if (!student.FullyEnrolled() && !subject.MaxEnrolmentIsReached())
            {
                db.Entry(student).Collection(s => s.EnrolledSubjects).Load();
                student.EnrolSubject(subject);
                db.SaveChanges();
            }
            else
            {
                // if student has reached enrolment limit, disapprove all his requests
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