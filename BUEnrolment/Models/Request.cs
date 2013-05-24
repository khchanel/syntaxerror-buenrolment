using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;
namespace BUEnrolment.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Reason For Enrolment")]
        public string Description { get; set; }
        public virtual Subject Subject { get; set; }
        public string Status { get; set; }

        public Request()
        {
            this.Subject = new Subject();
        }
    }
}