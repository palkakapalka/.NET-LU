using System;
using System.Collections.Generic;

namespace project
{
    // Класс School
    // Class School
    public class School
    {
        // Kolekcija ar studentiem
        // Collection of students
        public List<Student> Students { get; set; } = new List<Student>();

        // Kolekcija ar skolotājiem
        // Collection of teachers
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();

        // Kolekcija ar kursiem
        // Collection of courses
        public List<Course> Courses { get; set; } = new List<Course>();

        // Kolekcija ar uzdevumiem
        // Collection of assignments
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();

        // Kolekcija ar iesniegumiem
        // Collection of submissions
        public List<Submission> Submissions { get; set; } = new List<Submission>();

        // Metode, lai pievienotu studentu
        // Method to add a student
        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        // Metode, lai pievienotu skolotāju
        // Method to add a teacher
        public void AddTeacher(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        // Metode, lai pievienotu kursu
        // Method to add a course
        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        // Metode, lai pievienotu uzdevumu
        // Method to add an assignment
        public void AddAssignment(Assignment assignment)
        {
            Assignments.Add(assignment);
        }

        // Metode, lai pievienotu iesniegumu
        // Method to add a submission
        public void AddSubmission(Submission submission)
        {
            Submissions.Add(submission);
        }

        // Metode, lai attēlotu visus studentus
        // Method to display all students
        public void DisplayStudents()
        {
            foreach (var student in Students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        // Metode, lai attēlotu visus skolotājus
        // Method to display all teachers
        public void DisplayTeachers()
        {
            foreach (var teacher in Teachers)
            {
                Console.WriteLine(teacher.ToString());
            }
        }

        // Metode, lai attēlotu visus kursus
        // Method to display all courses
        public void DisplayCourses()
        {
            foreach (var course in Courses)
            {
                Console.WriteLine(course.ToString());
            }
        }

        // Metode, lai attēlotu visus uzdevumus
        // Method to display all assignments
        public void DisplayAssignments()
        {
            foreach (var assignment in Assignments)
            {
                Console.WriteLine(assignment.ToString());
            }
        }

        // Metode, lai attēlotu visus iesniegumus
        // Method to display all submissions
        public void DisplaySubmissions()
        {
            foreach (var submission in Submissions)
            {
                Console.WriteLine(submission.ToString());
            }
        }
    }
}
