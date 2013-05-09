using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private IDictionary<Subject, Result> _completedSubjects;
        private List<Subject> _enrolledSubjects;
        private List<Request> _requests;

        
    }
}