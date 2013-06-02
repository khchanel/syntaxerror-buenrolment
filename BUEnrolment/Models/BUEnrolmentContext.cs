using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace BUEnrolment.Models
{
    /// <summary>
    /// Database context for EntifyFramework
    /// </summary>
    public class BUEnrolmentContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BUEnrolmentContext()
            : base("BUEnrolmentContext")
        {
            // do nothing
        }


        /// <summary>
        /// Create entity relations
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().
                HasMany(m => m.Prerequisites).
                WithMany()
                .Map(m =>
                    {
                        m.ToTable("SubjectPrerequisite");
                        m.MapLeftKey("SubjectId");
                        m.MapRightKey("PrerequisiteId");
                    });
            modelBuilder.Entity<Student>().HasMany(m => m.Requests);
            modelBuilder.Entity<Student>().HasMany(m => m.CompletedSubject);
            modelBuilder.Entity<Request>().HasRequired(m => m.Subject);
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.EnrolledStudents)
                .WithMany(p => p.EnrolledSubjects)
                .Map(m =>
                {
                    m.ToTable("SubjectStudent");
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("SubjectId");
                });
        }
    }
}