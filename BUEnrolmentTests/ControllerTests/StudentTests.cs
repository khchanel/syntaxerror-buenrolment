using System;
using System.Collections.Generic;
using System.Linq;
using BUEnrolment.Controllers;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUEnrolmentTests.ControllerTests
{
    [TestClass]
    public class StudentTests
    {

        private BUEnrolmentContext db;
        private List<Student> _students;
        private StudentController _sc;

        [TestInitialize]
        public void Setup()
        {
            db = new BUEnrolmentContext();
            _students = new List<Student>();
            _sc = new StudentController(ref db);
        }

        [TestMethod]
        public void StudentHasEnrolled()
        {
            Student student = new Student { Username = "11012592" };
            db.Students.Add(student);
            Subject subject = new Subject { SubjectNumber = "1", Name = "SubjectOne", MaxEnrolment = 2 };
            db.Subjects.Add(subject);
            db.SaveChanges();

            Student dbStudent = db.Students.FirstOrDefault(s => s.Username == "11012592");
            Subject dbSubject = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "1");

            _sc.Enrol(dbStudent.Id, dbSubject.Id);

            Assert.AreEqual(subject.Id, dbStudent.EnrolledSubjects.FirstOrDefault().Id);
        }

        [TestMethod]
        public void StudentCannotEnrolWhenFullyEnrolled()
        {
            Student student = new Student { Username = "11012592" };
            db.Students.Add(student);

            Subject subject1 = new Subject { SubjectNumber = "1", Name = "SubjectOne", MaxEnrolment = 2 };
            Subject subject2 = new Subject { SubjectNumber = "2", Name = "SubjectTwo", MaxEnrolment = 2 };
            Subject subject3 = new Subject { SubjectNumber = "3", Name = "SubjectThree", MaxEnrolment = 2 };
            Subject subject4 = new Subject { SubjectNumber = "4", Name = "SubjectFour", MaxEnrolment = 2 };
            Subject subject5 = new Subject { SubjectNumber = "5", Name = "SubjectFive", MaxEnrolment = 2 };

            db.Subjects.Add(subject1);
            db.Subjects.Add(subject2);
            db.Subjects.Add(subject3);
            db.Subjects.Add(subject4);
            db.Subjects.Add(subject5);

            db.SaveChanges();

            Student dbStudent = db.Students.FirstOrDefault(s => s.Username == "11012592");

            Subject dbSubject1 = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "1");
            Subject dbSubject2 = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "2");
            Subject dbSubject3 = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "3");
            Subject dbSubject4 = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "4");
            Subject dbSubject5 = db.Subjects.FirstOrDefault(s => s.SubjectNumber == "5");

            _sc.Enrol(dbStudent.Id, dbSubject1.Id);
            _sc.Enrol(dbStudent.Id, dbSubject2.Id);
            _sc.Enrol(dbStudent.Id, dbSubject3.Id);
            _sc.Enrol(dbStudent.Id, dbSubject4.Id);
            _sc.Enrol(dbStudent.Id, dbSubject5.Id);

            Assert.AreEqual(4, db.Students.FirstOrDefault().EnrolledSubjects.Count);
        }

        [TestMethod]
        public void StudentCannotEnrolWhenMaxEnrolmentReached()
        {
            
        }
    }


}
