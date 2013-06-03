using System.Collections.Generic;
using BUEnrolment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BUEnrolmentTests.ModelTests
{
    [TestClass]
    public class SubjectTests
    {
        private List<Subject> _subjects;
        private List<Student> _students;


        [TestInitialize]
        public void Setup()
        {
            CreateSubjects();
            CreateStudents();
        }

        
        [TestMethod]
        public void SubjectCreation()
        {
            Subject subject = new Subject() {Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000", Description = "i am an apple"};

            Assert.AreEqual(true, subject.Active);
            Assert.AreEqual("Apple", subject.Name);
            Assert.AreEqual("1000", subject.SubjectNumber);
            Assert.AreEqual(3, subject.MaxEnrolment);
            Assert.AreEqual("i am an apple", subject.Description);
            Assert.AreEqual(0, subject.Prerequisites.Count);
            Assert.AreEqual(0, subject.EnrolledStudents.Count);
            Assert.AreEqual(2, _subjects.Find(s => s.Name == "Apple and Orange").Prerequisites.Count);
        }

        [TestMethod]
        public void SubjectMaxEnrollmentNotReached()
        {
            Subject subject = new Subject() { Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000", Description = "i am an apple" };

            Assert.AreEqual(0, subject.EnrolledStudents.Count);
            Assert.IsFalse(subject.MaxEnrolmentIsReached());

            subject.EnrolledStudents.Add(_students[0]);
            Assert.AreEqual(1, subject.EnrolledStudents.Count);
            Assert.IsFalse(subject.MaxEnrolmentIsReached());
        }

        [TestMethod]
        public void SubjectMaxEnrollmentHasReached()
        {
            Subject subject = new Subject() { Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000", Description = "i am an apple" };

            Assert.AreEqual(0, subject.EnrolledStudents.Count);
            Assert.IsFalse(subject.MaxEnrolmentIsReached());

            foreach (Student s in _students.FindAll(s => s.Id <= subject.MaxEnrolment))
            {
                subject.EnrolledStudents.Add(s);
            }

            Assert.AreEqual(3, subject.EnrolledStudents.Count);
            Assert.IsTrue(subject.MaxEnrolmentIsReached());
        }


        private void CreateSubjects()
        {
            // create subjects
            _subjects = new List<Subject>
                {
                    new Subject() {Active = true, Name = "Apple", MaxEnrolment = 3, SubjectNumber = "1000"},
                    new Subject() {Active = true, Name = "Orange", MaxEnrolment = 3, SubjectNumber = "2000"}
                };

            // subject with prereq
            var prereqAppleOrange = new List<Subject>(_subjects);
            _subjects.Add(new Subject() { Active = true, Name = "Apple and Orange", MaxEnrolment = 3, SubjectNumber = "3000", Prerequisites = prereqAppleOrange });

            // more subjects
            _subjects.Add(new Subject() { Active = true, Name = "Banana", MaxEnrolment = 3, SubjectNumber = "4000" });
            _subjects.Add(new Subject() { Active = true, Name = "Lemon", MaxEnrolment = 3, SubjectNumber = "5000" });
            _subjects.Add(new Subject() { Active = true, Name = "Lime", MaxEnrolment = 3, SubjectNumber = "6000" });
        }

        private void CreateStudents()
        {
            // Create students
            _students = new List<Student>
                {
                    new Student() { FullName = "StudentA", Id = 1, Username = "studenta" },
                    new Student() { FullName = "StudentB", Id = 2, Username = "studentb" },
                    new Student() { FullName = "StudentC", Id = 3, Username = "studentc" },
                    new Student() { FullName = "StudentD", Id = 4, Username = "studentd" },
                    new Student() { FullName = "StudentE", Id = 5, Username = "studente" },
                };
        }

    }
}
