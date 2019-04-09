using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWithOrm
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ISet<Enrollment> CourseEnrollments { get; set; } = new HashSet<Enrollment>();
    }

    class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }

        public virtual ISet<Enrollment> StudentEnrollments { get; set; } = new HashSet<Enrollment>();
    }

    class Enrollment
    {
        [Key, Column(Order = 0)]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Key, Column(Order = 1)]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int Year { get; set; }
        public Semester Semester { get; set; }
        public Grade Grade { get; set; }
    }

    enum Semester
    {
        None, Spring, Fall, Summer, PostPesach, Intersesion, 
    }
    enum Grade
    {
        None, A, B, C, D, F
    }

    public class DataLayer : DbContext
    {
        public DataLayer() { }
        public DataLayer(string dbName) : base(dbName)
        {}
        public DbSet<Student> Students { get; set; }  // db Table
        public DbSet<Course> Courses { get; set; }  // db Table
        public DbSet<Enrollment> Enrollments { get; set; }  // db Table
    }

    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DataLayer>(new DropCreateDatabaseAlways<DataLayer>());
            
            DataLayer dal = new DataLayer();
            dal.Configuration.ProxyCreationEnabled = true;
            dal.Configuration.LazyLoadingEnabled = true;
            Console.WriteLine(dal.Students.Count());
           
            var s = new Student{FirstName = "Moshe", LastName = "Ioffe"};
            
            var course = new Course{CourseName = "Advanced Topics in Garbage Collection", CourseNumber = "GC 507"};


            s.CourseEnrollments.Add( new  Enrollment
            {
                Course = course,
                Student = s,
                Grade = Grade.A
            });

            dal.Students.Add(s);

            dal.SaveChanges();



            Console.ReadLine();
        }

        private static string Reverse(string s)
        {
            return s.Length <= 1 ? s : Reverse(s.Substring(1, s.Length - 1)) + s[0];
        }
    }
}
