using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace BUEnrolment.Models
{
    public class BUEnrolmentContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Result> Results { get; set; }

        public BUEnrolmentContext()
            : base("BUEnrolmentContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasMany(m => m.Prerequisites).WithMany();
            modelBuilder.Entity<Student>().HasMany(m => m.Requests);
            modelBuilder.Entity<Request>().HasRequired(m => m.Subject);
            modelBuilder.Entity<Request>().HasRequired(m => m.Student);
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

        public DbSet<Request> Requests { get; set; }
    }
}