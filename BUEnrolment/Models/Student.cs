using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BUEnrolment.Models
{
    public class Student
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<Subject> EnrolledSubjects { get; set; }
        public virtual List<Request> Requests { get; set; }
        public IDictionary<Subject, Result> CompletedSubjects { get; set; }

        public Student()
        {
            this.Requests = new List<Request>();
            this.EnrolledSubjects = new List<Subject>();
        }
    }
}