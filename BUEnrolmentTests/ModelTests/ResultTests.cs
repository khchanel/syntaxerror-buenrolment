using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUEnrolmentTests.ModelTests
{
    /// <summary>
    /// Summary description for ResultTests
    /// </summary>
    [TestClass]
    public class ResultTests
    {
        private Result _result;
        private List<Subject> _subjects; 


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
        public void ResultCreation()
        {
            _result = new Result() { Id = 1, Mark = 60.5, Subject = _subjects[0]};

            Assert.AreEqual(1, _result.Id);
            Assert.AreEqual(60.5, _result.Mark);
            Assert.AreEqual("Apple", _result.Subject.Name);
        }

        [TestMethod]
        public void ResultGradeConversionAppSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            Assert.AreEqual("85", appSettings["HighDistinction"]);
            Assert.AreEqual("75", appSettings["Distinction"]);
            Assert.AreEqual("65", appSettings["Credit"]);
            Assert.AreEqual("50", appSettings["Pass"]);
            Assert.AreEqual("0", appSettings["Fail"]);
        }

        [TestMethod]
        public void ResultGradeConversionFail()
        {
            var result = new Result() { Id = 1, Mark = 30, Subject = _subjects[0] };
            Assert.AreEqual(Result.ResultGrade.Fail, result.Grade);
        }

        [TestMethod]
        public void ResultGradeConversionPass()
        {
            var result = new Result() { Id = 1, Mark = 50, Subject = _subjects[0] };
            Assert.AreEqual(Result.ResultGrade.Pass, result.Grade);
        }

        [TestMethod]
        public void ResultGradeConversionCredit()
        {
            var result = new Result() { Id = 1, Mark = 65, Subject = _subjects[0] };
            Assert.AreEqual(Result.ResultGrade.Credit, result.Grade);
        }

        [TestMethod]
        public void ResultGradeConversionDistinction()
        {
            var result = new Result() { Id = 1, Mark = 75, Subject = _subjects[0] };
            Assert.AreEqual(Result.ResultGrade.Distinction, result.Grade);
        }

        [TestMethod]
        public void ResultGradeConversionHighDistinction()
        {
            var result = new Result() { Id = 1, Mark = 85, Subject = _subjects[0] };
            Assert.AreEqual(Result.ResultGrade.HighDistinction, result.Grade);
        }
    }
}
