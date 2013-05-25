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
        public virtual List<Tuple<Subject, Result>> CompletedSubjects { get; set; }

        public Student()
        {
            Requests = new List<Request>();
            EnrolledSubjects = new List<Subject>();
            CompletedSubjects = new List<Tuple<Subject, Result>>();

        }

        public void EnrolSubject(Subject subject)
        {
            EnrolledSubjects.Add(subject);
        }

        public void CompleteSubject(Subject subject, Result result)
        {
            CompletedSubjects.Add(Tuple.Create(subject, result));
        }

        public List<Subject> GetEnrollableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);
            RemoveMaxEnrolmentReached(ref allSubjects);
            RemoveFailedThreeTimes(ref allSubjects);
            RemovePrerequisitesNotCompleted(ref allSubjects);

            return allSubjects;
        }

        public void RemoveCurrentlyEnrolled(ref List<Subject> enrollableSubjects)
        {
            enrollableSubjects = enrollableSubjects.Except(EnrolledSubjects).ToList();
        }

        public void RemovePassed(ref List<Subject> enrollableSubjects)
        {
            foreach (Tuple<Subject, Result> completedSubject in CompletedSubjects.Where(completedSubject => completedSubject.Item2.Mark > 49))
            {
                enrollableSubjects.Remove(completedSubject.Item1);
            }   
        }

        public void RemoveMaxEnrolmentReached(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => subject.MaxEnrolmentIsReached()).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        public void RemoveFailedThreeTimes(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => CompletedSubjects.Count(s => s.Item1 == subject && s.Item2.Mark < 50) == 3).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        public void RemovePrerequisitesNotCompleted(ref List<Subject> enrollableSubjects)
        {
            foreach (Subject subject in enrollableSubjects.Where(subject => !subject.Prerequisites.Except(CompletedSubjects.Where(m => m.Item2.Mark >= 50).Select(s => s.Item1)).Any()).ToList())
            {
                enrollableSubjects.Remove(subject);
            }
        }

        public List<Subject> GetRequestableSubjects(List<Subject> allSubjects)
        {
            RemoveCurrentlyEnrolled(ref allSubjects);
            RemovePassed(ref allSubjects);

            return allSubjects;
        }

        public bool FullyEnrolled()
        {
            return EnrolledSubjects.Count >= 4;
        }
    }
}