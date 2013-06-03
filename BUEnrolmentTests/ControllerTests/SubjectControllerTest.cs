using System;
using System.Web.Mvc;
using BUEnrolment.Controllers;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUEnrolmentTests.ControllerTests
{
    [TestClass]
    public class SubjectControllerTest
    {
        private SubjectController _subjectController;

        [TestInitialize]
        public void Setup()
        {
            _subjectController = new SubjectController(new FakeBUEnrolmentContext());
        }

        [TestMethod]
        public void SubjectIndex()
        {
           ActionResult result =  _subjectController.Index();

        }
    }
}
