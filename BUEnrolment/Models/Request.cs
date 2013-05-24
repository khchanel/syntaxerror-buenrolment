﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
namespace BUEnrolment.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Subject Number")]
        public string SubjectNumber { get; set; }
        [Required]
        [Display(Name = "Reason For Enrolment")]
        public string Description { get; set; }
    }
}