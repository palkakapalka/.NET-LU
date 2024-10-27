using System.Collections.Generic;
// 7. punkts
namespace project 
{
    public class DataCollections
    {
        public List<Person> People { get; set; } = new List<Person>(); // cilveku kolekcija
        public List<Course> Courses { get; set; } = new List<Course>(); // kursu kolekcija
        public List<Assignment> Assignments { get; set; } = new List<Assignment>(); // uzdevumu kolekcija
        public List<Submission> Submissions { get; set; } = new List<Submission>(); // nodeevumu kolekcija
    }
}
