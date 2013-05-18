using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Request
    {
        public int Id { get; set; }
        public Subject subject { get; set; }
        public string Description { get; set; }
    }
}