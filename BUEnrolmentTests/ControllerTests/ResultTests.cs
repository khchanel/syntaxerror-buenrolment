using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BUEnrolment.Controllers;
using System.Web.Mvc;
using System.Linq;

namespace BUEnrolmentTests.ControllerTests
{
    /// <summary>
    /// Summary description for ResultTests
    /// </summary>
    [TestClass]
    public class ResultTests
    {
        private Result _result;
        private List<Subject> _subjects;
        private BUEnrolmentContext db;

        public ResultTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestInitialize]
        public void Setup()
        {
            _subjects = new List<Subject>
                {
                    new Subject() {Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000"},
                    new Subject() {Active = true, Name = "Orange", MaxEnrolment = 3, SubjectNumber = "2000"}
                };
            db = new BUEnrolmentContext();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
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
        public void CheckModelDataTypeForResult()
        {
            ResultController resultController = new ResultController(db);
            Subject subject = new Subject() { Active = true, MaxEnrolment = 3, Name = "thisIsTestSubject", SubjectNumber = "123", Description = "" };
            db.Subjects.Add(subject);
            db.SaveChanges();
            ViewResult resultViewResult = (ViewResult)resultController.Create(subject.Id);
            Assert.AreEqual(resultViewResult.ViewData["subject"], subject);
            Subject addedSubject = resultController.db.Subjects.FirstOrDefault(a => a.Name == "thisIsTestSubject");
            Assert.AreEqual(subject.SubjectNumber, addedSubject.SubjectNumber);
        }
    }
}
