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
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public virtual List<Subject> EnrolledSubjects { get; set; }
        public virtual List<Request> Requests { get; set; }
        public IDictionary<Subject, Result> CompletedSubjects { get; set; }

        public Student()
        {
            this.Requests = new List<Request>();
            this.EnrolledSubjects = new List<Subject>();
        }

        public void EnrolSubject(Subject subject)
        {
            EnrolledSubjects.Add(subject);
        }

        public List<Subject> GetRequestableSubjects(List<Subject> allSubjects)
        {
            return allSubjects;
        }

        public List<Request> GetStudentRequest(List<Request> allRequests)
        {
            List<Request> filteredRequests = (from Request request in allRequests where (from studentRequest in Requests select studentRequest.Id).Contains(request.Id) select request).ToList();

            return filteredRequests;
        }
    }
}