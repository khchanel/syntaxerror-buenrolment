//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BUEnrolment
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subject
    {
        public Subject()
        {
            this.Prerequisites_Subject = new HashSet<Prerequisites_Subject>();
            this.Prerequisites_Subject1 = new HashSet<Prerequisites_Subject>();
            this.Requests = new HashSet<Request>();
            this.Results = new HashSet<Result>();
            this.Student_Subject = new HashSet<Student_Subject>();
        }
    
        public int Id { get; set; }
        public Nullable<int> SubjectNumber { get; set; }
        public string SubjectName { get; set; }
        public Nullable<int> MaxEnrolment { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<Prerequisites_Subject> Prerequisites_Subject { get; set; }
        public virtual ICollection<Prerequisites_Subject> Prerequisites_Subject1 { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<Student_Subject> Student_Subject { get; set; }
    }
}
