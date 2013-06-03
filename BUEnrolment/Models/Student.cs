using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BUEnrolment.Models
{
    /// <summary>
    /// Student model
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Unique ID for database
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Student username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Student full name
        /// </summary>
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        /// <summary>
        /// List of Enrolled Subjects
        /// </summary>
        public virtual List<Subject> EnrolledSubjects { get; set; }

        /// <summary>
        /// List of requests owned by the student
        /// </summary>
        public virtual List<Request> Requests { get; set; }

        /// <summary>
        /// List of Result of completed subject by the student
        /// </summary>
        public virtual List<Result> CompletedSubject { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public Student()
        {
            Requests = new List<Request>();
            EnrolledSubjects = new List<Subject>();
            CompletedSubject = new List<Result>();
        }

        /// <summary>
        /// Add a subject to student's enrolledSubject list
        /// </summary>
        /// <param name="subject"></param>
        public void EnrolSubject(Subject subject)
        {
            EnrolledSubjects.Add(subject);
        }

        /// <summary>
        /// Add result for a completed subject of the student
        /// </summary>
        /// <param name="result"></param>
        public void CompleteSubject(Result result)
        {
            CompletedSubject.Add(result);
        }


        /// <summary>
        /// Check if the student reached max enrolment
        /// </summary>
        /// <returns>true if enrolment limit is met</returns>
        public bool FullyEnrolled()
        {
            return EnrolledSubjects.Count >= 4;
        }

        /// <summary>
        /// Get a List of subjects that the student can enroll
        /// </summary>
        /// <param name="allSubjects"></param>
        /// <returns></returns>
        public List<Subject> GetEnrollableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);
            RemoveMaxEnrolmentReached(ref allSubjects);
            RemoveFailedThreeTimes(ref allSubjects);
            RemovePrerequisitesNotCompleted(ref allSubjects, true);

            return allSubjects;
        }

        /// <summary>
        /// Get a list of subject that the student can make request to enroll
        /// </summary>
        /// <param name="allSubjects"></param>
        /// <returns></returns>
        public List<Subject> GetRequestableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);
            RemoveMaxEnrolmentReached(ref allSubjects);
            RemovePrerequisitesNotCompleted(ref allSubjects, false);

            return allSubjects;
        }


        private void RemoveCurrentlyEnrolled(ref List<Subject> enrollableSubjects)
        {
            enrollableSubjects = enrollableSubjects.Except(EnrolledSubjects).ToList();
        }

        private void RemovePassed(ref List<Subject> enrollableSubjects)
        {
            foreach (Result completedSubject in CompletedSubject.Where(completedSubject => completedSubject.Mark > 49))
            {
                enrollableSubjects.Remove(completedSubject.Subject);
            }
        }

        private void RemoveMaxEnrolmentReached(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => subject.MaxEnrolmentIsReached()).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        private void RemoveFailedThreeTimes(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => CompletedSubject.Count(s => s.Subject == subject && s.Mark < 50) == 3).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        private void RemovePrerequisitesNotCompleted(ref List<Subject> enrollableSubjects, bool remove)
        {

            foreach (Subject subject in enrollableSubjects
                .Where(subject => (subject.Prerequisites
                    .Except(CompletedSubject.Where(m => m.Mark >= 50).Where(m => m.Subject.Prerequisites.Count > 0).Select(s => s.Subject)).Any()) == remove)
                    .ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }
    }
}