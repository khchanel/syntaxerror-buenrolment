using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BUEnrolment.Models
{
    public class Student
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public virtual List<Subject> EnrolledSubjects { get; set; }
        public virtual List<Request> Requests { get; set; }
        public virtual List<Result> CompletedSubject { get; set; }

        public Student()
        {
            Requests = new List<Request>();
            EnrolledSubjects = new List<Subject>();
            CompletedSubject = new List<Result>();
        }

        public void EnrolSubject(Subject subject)
        {
            EnrolledSubjects.Add(subject);
        }

        public void CompleteSubject(Result result)
        {
            CompletedSubject.Add(result);
        }

        public bool FullyEnrolled()
        {
            return EnrolledSubjects.Count >= 4;
        }

        public List<Subject> GetEnrollableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);
            RemoveMaxEnrolmentReached(ref allSubjects);
            RemoveFailedThreeTimes(ref allSubjects);
            RemovePrerequisitesNotCompleted(ref allSubjects, false);

            return allSubjects;
        }

        public List<Subject> GetRequestableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);
            RemoveMaxEnrolmentReached(ref allSubjects);
            RemovePrerequisitesNotCompleted(ref allSubjects, true);

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
                enrollableSubjects.Remove(completedSubject.subject);
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
            foreach (Subject subject in enrollableSubjects.Where(subject => CompletedSubject.Count(s => s.subject == subject && s.Mark < 50) == 3).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        private void RemovePrerequisitesNotCompleted(ref List<Subject> enrollableSubjects, bool remove)
        {

            foreach (Subject subject in enrollableSubjects
                .Where(subject => (subject.Prerequisites
                    .Except(CompletedSubject.Where(m => m.Mark >= 50).Select(s => s.subject)).Any()) == remove)
                    .ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }
    }
}