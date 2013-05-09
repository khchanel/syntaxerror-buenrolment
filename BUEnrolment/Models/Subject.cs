using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Subject
    {
        public Guid SubjectId { get; set; }
        public string SubjectNumber { get; set; }
        public string SubjectName { get; set; }
    }
}