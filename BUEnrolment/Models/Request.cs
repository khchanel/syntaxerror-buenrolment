using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        private Subject _subject;
        public string Description { get; set; }
    }
}