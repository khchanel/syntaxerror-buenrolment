using System;
using System.Text;
using System.Collections.Generic;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUEnrolmentTests.ModelTests
{
    /// <summary>
    /// Summary description for StudentTests
    /// </summary>
    [TestClass]
    public class StudentTests
    {
        private List<Subject> _subjects;
        private Student _student;
 
        public StudentTests()
        {
            // do nothing
        }

        /// <summary>
        /// on Intialization, 6 subjects will be added to _subjects
        /// Apple and Orange subjects has prereq of Apple, Orange subjects
        /// 
        /// an instance of Student is created and stored in _student
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // create subjects
            _subjects = new List<Subject>
                {
                    new Subject() {Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000"},
                    new Subject() {Active = true, Name = "Orange", MaxEnrolment = 3, SubjectNumber = "2000"}
                };

            // subject with prereq
            var prereqAppleOrange = new List<Subject>(_subjects);
            _subjects.Add(new Subject() { Active = true, Name = "Apple and Orange", MaxEnrolment = 3, SubjectNumber = "3000", Prerequisites = prereqAppleOrange });

            // more subjects
            _subjects.Add(new Subject() {Active = true, Name = "Banana", MaxEnrolment = 3, SubjectNumber = "4000"});
            _subjects.Add(new Subject() {Active = true, Name = "Lemon", MaxEnrolment = 3, SubjectNumber = "5000"});
            _subjects.Add(new Subject() { Active = true, Name = "Lime", MaxEnrolment = 3, SubjectNumber = "6000" });

            // create student
            _student = new Student() { FullName = "StudentA", Id = 1, Username = "student" };
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void StudentSubjectCreation()
        {
            // check count match
            Assert.AreEqual(6, _subjects.Count);
            Assert.AreEqual(2, _subjects.Find(s => s.SubjectNumber == "3000").Prerequisites.Count);

            // extract some subjects for verification
            Subject appleOrangeSubject = _subjects.Find(s => s.SubjectNumber == "3000");
            Subject appleSubject = appleOrangeSubject.Prerequisites[0];
            Subject orangeSubject = appleOrangeSubject.Prerequisites[1];

            // verify subjct details
            Assert.AreEqual("Apple and Orange", appleOrangeSubject.Name);
            Assert.AreEqual("Apple", appleSubject.Name);
            Assert.AreEqual("Orange", orangeSubject.Name);
        }

        [TestMethod]
        public void StudentCreation()
        {
            Assert.IsNotNull(_student);
            Assert.IsNotNull(_student.EnrolledSubjects);
            Assert.IsNotNull(_student.CompletedSubject);
            Assert.IsNotNull(_student.Requests);

            Assert.AreEqual("StudentA", _student.FullName);
            Assert.AreEqual("student", _student.Username);
            Assert.AreEqual(1, _student.Id);
        }

        [TestMethod]
        public void StudentEnrollLimitNotReached()
        {
            Assert.AreEqual(0, _student.EnrolledSubjects.Count);

            // enroll some subjects
            _student.EnrolSubject(_subjects.Find(s => s.SubjectNumber == "1000"));
            _student.EnrolSubject(_subjects.Find(s => s.SubjectNumber == "2000"));

            Assert.AreEqual(2, _student.EnrolledSubjects.Count);
            Assert.IsFalse(_student.FullyEnrolled());
        }

        [TestMethod]
        public void StudentEnrollLimitHasReached()
        {
            Assert.AreEqual(0, _student.EnrolledSubjects.Count);

            foreach (var s in _subjects.FindAll(s => s.SubjectNumber != "3000"))
            {
                _student.EnrolSubject(s);
            }

            Assert.AreEqual(5, _student.EnrolledSubjects.Count);
            Assert.IsTrue(_student.FullyEnrolled());
        }

        [TestMethod]
        public void StudentEnrollSubject()
        {
            Assert.AreEqual(0, _student.EnrolledSubjects.Count);

            // enroll a subject
            _student.EnrolSubject(_subjects.Find(s => s.SubjectNumber == "1000"));

            Assert.AreEqual(1, _student.EnrolledSubjects.Count);
            Assert.AreEqual("Apple", _student.EnrolledSubjects[0].Name);
        }

        [TestMethod]
        public void StudentGetEnrollableSubjects()
        {
            List<Subject> enrollableSubjects = _student.GetEnrollableSubjects(_subjects);

            // should have everything except Apple and Orange subjects
            Assert.AreEqual(5, enrollableSubjects.Count);
            Assert.IsNull(enrollableSubjects.Find(s => s.Name == "Apple and Orange"));
        }

        [TestMethod]
        public void StudentGetRequestableSubjects()
        {
            List<Subject> requestableSubjects = _student.GetRequestableSubjects(_subjects);

            Assert.AreEqual(1, requestableSubjects.Count);
            Assert.AreEqual("Apple and Orange", requestableSubjects[0].Name);
        }

        [TestMethod]
        public void StudentCompleteSubject()
        {
            Assert.AreEqual(0, _student.CompletedSubject.Count);

            Result appleResult = new Result() { Id = 1, Mark = 60.5, Subject = _subjects.Find(s => s.Name == "Apple")};
            _student.CompleteSubject(appleResult);

            Assert.AreEqual(1, _student.CompletedSubject.Count);
            Assert.AreEqual(60.5, _student.CompletedSubject.Find(r => r.Id == 1).Mark);
        }
    }
}
