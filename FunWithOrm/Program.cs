using System;
using System.Collections.Generic;
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
    }
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class DataLayer : DbContext
    {
        public DataLayer() { }
        public DataLayer(string dbName) : base(dbName)
        {}
        public DbSet<Student> Students { get; set; }  // db Table
        public DbSet<Person> Persons { get; set; }  // db Table
    }

    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DataLayer>(new DropCreateDatabaseIfModelChanges<DataLayer>());

            DataLayer dal = new DataLayer();
            
            Console.WriteLine(dal.Students.Count());
           
            var jb = new Student{FirstName = "Dovie", LastName = "bob"};
            dal.Students.Add(jb);

            dal.SaveChanges();

            foreach (var student in dal.Students.ToList().Where(s => s.LastName == Reverse(s.LastName)))
            {
                Console.WriteLine($"{student.StudentId,10}{student.FirstName,10}{student.LastName,10}");
            }

            Console.ReadLine();
        }

        private static string Reverse(string s)
        {
            return s.Length <= 1 ? s : Reverse(s.Substring(1, s.Length - 1)) + s[0];
        }
    }
}
