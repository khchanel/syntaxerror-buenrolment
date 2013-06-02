using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace BUEnrolment.Models
{
    /// <summary>
    /// Subject model
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Unique ID for database
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Subject number
        /// </summary>
        [Display(Name = "Number")]
        [Required]
        public string SubjectNumber { get; set; }

        /// <summary>
        /// Subject name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Subject description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Maximum enrolment limit for the subject
        /// </summary>
        [Display(Name = "Max Enrolment")]
        [Required]
        public int MaxEnrolment { get; set; }

        /// <summary>
        /// Active status of the subject
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// List of subject prerequisites
        /// </summary>
        public virtual List<Subject> Prerequisites { get; set; }

        /// <summary>
        /// List of Student enrolled to the subject
        /// </summary>
        [Display(Name = "Enrolled Students")]
        public virtual List<Student> EnrolledStudents { get; set; }

        /// <summary>
        /// Timestamp for concurrency purpose
        /// </summary>
        [Timestamp]
        public Byte[] Timestamp { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Subject()
        {
            this.Prerequisites = new List<Subject>();
            this.EnrolledStudents = new List<Student>();
        }


        /// <summary>
        /// Check if max enrolment is reached
        /// </summary>
        /// <returns>true if the subject is full</returns>
        public bool MaxEnrolmentIsReached()
        {
            return EnrolledStudents.Count >= MaxEnrolment;
        }
    }
}