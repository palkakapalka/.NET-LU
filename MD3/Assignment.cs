// File: Assignment.cs
namespace MD3
{
    public class Assignment
    {
        public int Id { get; set; }
        public DateTime Deadline { get; set; }
        public int CourseId { get; set; } // Foreign key to Course
        public string Description { get; set; }
    }
}
