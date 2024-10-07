using System; // Импортируем пространство имен для работы с DateTime
using System.Collections.Generic; // Импортируем пространство имен для работы с коллекциями
using System.IO; // Импортируем пространство имен для работы с файлами

namespace project // Пространство имен
{
    public class DataManager : IDataManager // Класс DataManager, реализующий IDataManager
    {
        private DataCollections _dataCollections = new DataCollections(); // Коллекция данных (Data collection)

        public string Print() // Реализация метода Print (Implementation of Print method)
        {
            // Строка для хранения вывода (String to store output)
            string output = "People:\n";
            foreach (var person in _dataCollections.People) // Перебираем людей (Iterate through people)
            {
                output += person.ToString() + "\n"; // Добавляем информацию о человеке (Add person info)
            }

            output += "Courses:\n"; // Добавляем курсы (Add courses)
            foreach (var course in _dataCollections.Courses) // Перебираем курсы (Iterate through courses)
            {
                output += course.ToString() + "\n"; // Добавляем информацию о курсе (Add course info)
            }

            output += "Assignments:\n"; // Добавляем задания (Add assignments)
            foreach (var assignment in _dataCollections.Assignments) // Перебираем задания (Iterate through assignments)
            {
                output += assignment.ToString() + "\n"; // Добавляем информацию о задании (Add assignment info)
            }

            output += "Submissions:\n"; // Добавляем сдачи (Add submissions)
            foreach (var submission in _dataCollections.Submissions) // Перебираем сдачи (Iterate through submissions)
            {
                output += $"{submission.Assignment?.Description}, Score: {submission.Score}\n"; // Добавляем информацию о сдаче (Add submission info)
            }

            return output; // Возвращаем вывод (Return output)
        }

        public void Save(string path) // Реализация метода Save (Implementation of Save method)
        {
            using (var writer = new StreamWriter(path)) // Создаем объект StreamWriter (Create StreamWriter object)
            {
                writer.WriteLine(Print()); // Записываем данные в файл (Write data to file)
            }
        }

        public void Load(string path) // Реализация метода Load (Implementation of Load method)
        {
            // Загрузка данных из файла (Loading data from file)
            if (File.Exists(path)) // Проверяем, существует ли файл (Check if the file exists)
            {
                string[] lines = File.ReadAllLines(path); // Читаем все строки из файла (Read all lines from the file)
                // Логика для восстановления коллекции из строк (Logic to restore collection from lines)
                // Это нужно реализовать в зависимости от формата сохранения (This needs to be implemented based on the save format)
            }
        }

        public void CreateTestData() // Реализация метода CreateTestData (Implementation of CreateTestData method)
        {
            // Создаем тестовые данные (Create test data)
            var teacher = new Teacher { Name = "John", Surname = "Doe", Gender = Gender.Man, ContractDate = DateTime.Now }; // Создаем учителя (Create a teacher)
            _dataCollections.People.Add(teacher); // Добавляем учителя в коллекцию (Add teacher to collection)

            var student = new Student("Jane", "Doe", Gender.Woman, "S1234"); // Создаем студента (Create a student)
            _dataCollections.People.Add(student); // Добавляем студента в коллекцию (Add student to collection)

            var course = new Course { Name = "Mathematics", Teacher = teacher }; // Создаем курс (Create a course)
            _dataCollections.Courses.Add(course); // Добавляем курс в коллекцию (Add course to collection)

            var assignment = new Assignment { Deadline = DateTime.Now.AddDays(7), Course = course, Description = "Algebra Homework" }; // Создаем задание (Create an assignment)
            _dataCollections.Assignments.Add(assignment); // Добавляем задание в коллекцию (Add assignment to collection)

            var submission = new Submission { Assignment = assignment, Student = student, SubmissionTime = DateTime.Now, Score = 95 }; // Создаем сдачу (Create a submission)
            _dataCollections.Submissions.Add(submission); // Добавляем сдачу в коллекцию (Add submission to collection)
        }

        public void Reset() // Реализация метода Reset (Implementation of Reset method)
        {
            _dataCollections = new DataCollections(); // Сбрасываем коллекцию данных (Reset data collection)
        }
    }
}
