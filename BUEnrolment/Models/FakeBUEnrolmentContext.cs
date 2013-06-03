using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BUEnrolment.Models
{
    public class FakeBUEnrolmentContext : IBUEnrolmentContext
    {
        public IDbSet<Subject> Subjects { get; set; }
        public IDbSet<Student> Students { get; set; }
        public IDbSet<Result> Results { get; set; }
        public IDbSet<Request> Requests { get; set; }

        public FakeBUEnrolmentContext()
        {
            Subjects = new FakeDbSet<Subject>();
            Students = new FakeDbSet<Student>();
            Results = new FakeDbSet<Result>();
            Requests = new FakeDbSet<Request>();
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}