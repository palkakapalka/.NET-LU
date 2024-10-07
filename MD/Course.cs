using System; // Импортируем пространство имен для работы с DateTime

namespace project // Пространство имен
{
    public class Course // Класс Course
    {
        public string Name { get; set; } // Свойство для имени курса (Property for course name)
        public Teacher Teacher { get; set; } // Свойство для учителя (Property for teacher)

        public override string ToString() // Переопределяем метод ToString() (Override ToString method)
        {
            return $"Course Name: {Name}, Teacher: {Teacher?.FullName}"; // Возвращаем строку с данными (Return a string with data)
        }
    }
}
