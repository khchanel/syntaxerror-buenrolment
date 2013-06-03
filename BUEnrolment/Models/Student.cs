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
            RemoveOrIncludeWhenSubjectPrerequisitesNotCompleted(ref allSubjects, true);

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
            RemoveOrIncludeWhenSubjectPrerequisitesNotCompleted(ref allSubjects, false);

            return allSubjects;
        }

        /// <summary>
        /// Remove all the students enrolled subjects from the list of enrollable subjects
        /// </summary>
        /// <param name="enrollableSubjects"></param>
        private void RemoveCurrentlyEnrolled(ref List<Subject> enrollableSubjects)
        {
            enrollableSubjects = enrollableSubjects.Except(EnrolledSubjects).ToList();
        }

        /// <summary>
        /// Remove all the students passed subjects from the list of enrollable subjects
        /// </summary>
        /// <param name="enrollableSubjects"></param>
        private void RemovePassed(ref List<Subject> enrollableSubjects)
        {
            foreach (Result completedSubject in CompletedSubject.Where(completedSubject => completedSubject.Mark > 49))
            {
                enrollableSubjects.Remove(completedSubject.Subject);
            }
        }

        /// <summary>
        /// Remove subjects that have reached max enrolment from the list of enrollable subjects
        /// </summary>
        /// <param name="enrollableSubjects"></param>
        private void RemoveMaxEnrolmentReached(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => subject.MaxEnrolmentIsReached()).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        /// <summary>
        /// Remove subjects that the student has failed three times from the list of enrollable subjects
        /// </summary>
        /// <param name="enrollableSubjects"></param>
        private void RemoveFailedThreeTimes(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => CompletedSubject.Count(s => s.Subject == subject && s.Mark < 50) == 3).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        /// <summary>
        /// Remove or Include subjects where the subject prerequisites have not been completed
        /// </summary>
        /// <param name="enrollableSubjects"></param>
        /// <param name="remove">Determines if the subject where the prerequisites have not been completed should be removed or not</param>
        private void RemoveOrIncludeWhenSubjectPrerequisitesNotCompleted(ref List<Subject> enrollableSubjects, bool remove)
        {
            foreach (Subject subject in enrollableSubjects
                .Where(subject => (subject.Prerequisites
                    .Except(CompletedSubject.Where(m => m.Mark >= 50).Where(m => m.Subject.Prerequisites.Count > 0).Select(s => s.Subject)).Any(p => p.Active)) == remove)
                    .ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }
    }
}