using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EnrolmentTest.Models
{
    public class Subject
    {
        [Key]public Guid Id { get; set; }
        public string SubjectNumber { get; set; }
        public string Name { get; set; }
        public List<Subject> Prerequisites { get; set; }

    }
}