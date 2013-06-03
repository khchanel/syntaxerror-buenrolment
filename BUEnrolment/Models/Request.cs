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
    /// <summary>
    /// Request model
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Unique ID for database
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Request Reason description
        /// </summary>
        [Required]
        [Display(Name = "Reason For Enrolment")]
        public string Description { get; set; }

        /// <summary>
        /// Reference to Subject
        /// </summary>
        [Required]
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// Request Status
        /// </summary>
        public string Status { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public Request()
        {
            this.Subject = new Subject();
        }
    }
}