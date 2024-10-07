using System; // Импортируем пространство имен для работы с DateTime

namespace project // Пространство имен
{
    public enum Gender // Перечисление для пола
    {
        Man, // Мужчина
        Woman // Женщина
    }

    public abstract class Person // Абстрактный класс Person
    {
        private string _name; // Приватное поле для имени (Private field for name)
        private string _surname; // Приватное поле для фамилии (Private field for surname)

        public string Name // Свойство для имени (Property for name)
        {
            get { return _name; } // Возвращаем имя (Return name)
            set
            {
                if (!string.IsNullOrEmpty(value)) // Проверяем, что значение не пустое (Check if the value is not empty)
                    _name = value; // Устанавливаем имя (Set name)
            }
        }

        public string Surname // Свойство для фамилии (Property for surname)
        {
            get { return _surname; } // Возвращаем фамилию (Return surname)
            set
            {
                if (!string.IsNullOrEmpty(value)) // Проверяем, что значение не пустое (Check if the value is not empty)
                    _surname = value; // Устанавливаем фамилию (Set surname)
            }
        }

        public string FullName => $"{Name} {Surname}"; // Свойство для полного имени (Property for full name)

        public Gender Gender { get; set; } // Свойство для пола (Property for gender)

        public override string ToString() // Переопределяем метод ToString() (Override ToString method)
        {
            return $"Name: {Name}, Surname: {Surname}, FullName: {FullName}, Gender: {Gender}"; // Возвращаем строку с данными (Return a string with data)
        }
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public class Teacher : Person // Класс Teacher, наследующий от Person
    {
        public DateTime ContractDate { get; set; } // Свойство для даты контракта (Property for contract date)

        public override string ToString() // Переопределяем метод ToString() (Override ToString method)
        {
            return base.ToString() + $", ContractDate: {ContractDate}"; // Возвращаем строку с данными (Return a string with data)
        }
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////
    /// </summary>

    public class Student : Person // Класс Student, наследующий от Person
    {
        public string StudentIdNumber { get; set; } // Свойство для номера студента (Property for student ID number)

        public Student(string name, string surname, Gender gender, string studentIdNumber) // Конструктор с параметрами (Constructor with parameters)
        {
            Name = name; // Устанавливаем имя (Set name)
            Surname = surname; // Устанавливаем фамилию (Set surname)
            Gender = gender; // Устанавливаем пол (Set gender)
            StudentIdNumber = studentIdNumber; // Устанавливаем номер студента (Set student ID number)
        }

        public Student() { } // Пустой конструктор (Empty constructor)

        public override string ToString() // Переопределяем метод ToString() (Override ToString method)
        {
            return base.ToString() + $", StudentIdNumber: {StudentIdNumber}"; // Возвращаем строку с данными (Return a string with data)
        }
    }
}
