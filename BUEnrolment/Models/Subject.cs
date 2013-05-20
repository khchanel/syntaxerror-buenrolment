using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace BUEnrolment.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string SubjectNumber { get; set; }
        public string Name { get; set; }
        public int MaxEnrolment { get; set; }
        public bool Active { get; set; }

        public virtual List<Subject> Prerequisites { get; set; }
        public virtual List<Student> EnrolledStudents { get; set; }

        public Subject()
        {
            this.Prerequisites = new List<Subject>();
            this.EnrolledStudents = new List<Student>();
        }
    }
}