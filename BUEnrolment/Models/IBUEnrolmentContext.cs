using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace BUEnrolment.Models
{
    /// <summary>
    /// BUEnrolment DB Context interface
    /// </summary>
    public interface IBUEnrolmentContext
    {
        IDbSet<Subject> Subjects { get; set; }
        IDbSet<Student> Students { get; set; }
        IDbSet<Result> Results { get; set; }
        IDbSet<Request> Requests { get; set; }

        int SaveChanges();
    }
}