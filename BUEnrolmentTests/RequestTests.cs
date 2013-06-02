using System;
using System.Web.Mvc;
using BUEnrolment.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BUEnrolment;
using BUEnrolment.Models;

namespace BUEnrolmentTests
{
    [TestClass]
    public class RequestTests
    {
       

        [TestMethod]
        public void TestActionRequestIndex()
        {
            RequestController requestController = new RequestController();
            ActionResult result = requestController.Index();

            System.Console.WriteLine(result.ToString());
        }
    }
}
