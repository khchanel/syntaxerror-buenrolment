using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BUEnrolment.Controllers

namespace BUEnrolmentTests.ControllerTests
{
    [TestClass]
    class StudentTests
    {
        private BUEnrolmentContext db;
        private List<Student> _students;

        [TestInitialize]
        public void Setup()
        {
            db = new BUEnrolmentContext();
            _students = new List<Student>();
        }
        [TestMethod]
        public void StudentHasEnrolled()
        {
            StudentController sc = new StudentController(db);
            Student student = new Student();
            Subject subject = new Subject();
            subject.SubjectNumber 
            student.EnrolSubject();
        }

    }
}
