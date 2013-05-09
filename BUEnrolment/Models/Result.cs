using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Result
    {
        public int Mark { get; set; }
        public string Grade { get; set; }

        public string GetGrade(int mark)
        {
            throw new NotImplementedException();
        }
    }
}