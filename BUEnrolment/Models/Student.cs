using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolmentTest.Models
{
    public class Student
    {
        [Key]public Guid Id { get; set; }
        public int StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Subject> EnrolledSubjects { get; set; }
        public IDictionary<Subject, Result> CompletedSubjects { get; set; } 
    }
}