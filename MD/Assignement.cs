using System; // Импортируем пространство имен для работы с DateTime

namespace project // Пространство имен
{
    public class Assignment // Класс Assignment
    {
        public DateTime Deadline { get; set; } // Свойство для срока выполнения (Property for deadline)
        public Course Course { get; set; } // Свойство для курса (Property for course)
        public string Description { get; set; } // Свойство для описания (Property for description)

        public override string ToString() // Переопределяем метод ToString() (Override ToString method)
        {
            return $"Deadline: {Deadline}, Course: {Course?.Name}, Description: {Description}"; // Возвращаем строку с данными (Return a string with data)
        }
    }
}
