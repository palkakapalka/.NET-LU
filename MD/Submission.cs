using System; // Импортируем пространство имен для работы с DateTime

namespace project // Пространство имен
{
    public class Submission // Класс Submission
    {
        public Assignment Assignment { get; set; } // Свойство для задания (Property for assignment)
        public Student Student { get; set; } // Свойство для студента (Property for student)
        public DateTime SubmissionTime { get; set; } // Свойство для времени сдачи (Property for submission time)
        public int Score { get; set; } // Свойство для оценки (Property for score)
    }
}
