using System.Collections.Generic; // Импортируем пространство имен для работы с коллекциями

namespace project // Пространство имен
{
    public class DataCollections // Класс DataCollections
    {
        public List<Person> People { get; set; } = new List<Person>(); // Коллекция для людей (Collection for people)
        public List<Course> Courses { get; set; } = new List<Course>(); // Коллекция для курсов (Collection for courses)
        public List<Assignment> Assignments { get; set; } = new List<Assignment>(); // Коллекция для заданий (Collection for assignments)
        public List<Submission> Submissions { get; set; } = new List<Submission>(); // Коллекция для сдач (Collection for submissions)
    }
}
