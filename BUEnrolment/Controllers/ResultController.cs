﻿using System;
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
    /// <summary>
    /// Handle Result related pages
    /// </summary>
    public class ResultController : Controller
    {
        /// <summary>
        /// Database context for EntityFramework
        /// </summary>
        public BUEnrolmentContext db = new BUEnrolmentContext();

        public ResultController(ref BUEnrolmentContext db)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            this.db = db;
        }

        public ResultController()
        {

        }


        /// <summary>
        /// GET: /Result/Create
        /// Enter result for subject page
        /// </summary>
        /// <param name="id">subject id</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            Subject subject = db.Subjects.Include(s => s.EnrolledStudents)
                .Include(s => s.EnrolledStudents.Select(e => e.CompletedSubject))
                .FirstOrDefault(s => s.Id == id);
            ViewBag.subject = subject;

            return View();
        }

        /// <summary>
        /// POST: /Result/Create
        /// Handler for Result entry
        /// </summary>
        /// <param name="results">list of entered results</param>
        /// <param name="id">subject id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<Result> results, int id)
        {
            if (ModelState.IsValid)
            {
                Subject subject = db.Subjects.Include(s => s.EnrolledStudents).FirstOrDefault(s => s.Id == id);

                for (int i = 0; i < subject.EnrolledStudents.Count; i++)
                {
                    if (results[i].Mark != null)
                    {
                        results[i].Subject = subject;
                        subject.EnrolledStudents[i].CompleteSubject(results[i]);
                        subject.EnrolledStudents.Remove(subject.EnrolledStudents[i]);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Create");
            }
            else
            {
                Subject subject = db.Subjects.Include(s => s.EnrolledStudents)
                    .Include(s => s.EnrolledStudents.Select(e => e.CompletedSubject))
                    .FirstOrDefault(s => s.Id == id);
                ViewBag.subject = subject;
            }

            return View(results);
        }

        /// <summary>
        /// GET: /Result/Details
        /// 
        /// List result page for the current logged in student
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            Student student = db.Students.Include(s => s.CompletedSubject).Include(s => s.CompletedSubject.Select(c => c.Subject)).FirstOrDefault(s => s.Id == WebSecurity.CurrentUserId);
            return View(student.CompletedSubject);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}