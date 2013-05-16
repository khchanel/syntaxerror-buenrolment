using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace EnrolmentTest.Models
{
    public class BUEnrolmentContext :DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasMany(m => m.Prerequisites).WithMany();
        }
    }
}