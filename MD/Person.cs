using System; 
// 1.punkts
namespace project 
{
    public enum Gender // Pārskaitam genderus
    {
        Man, 
        Woman 
    }

    public abstract class Person {

        private string _name; 
        private string _surname; 

        public string Name 
        {
            get { return _name; } 
            set
            {
                if (!string.IsNullOrEmpty(value)) // parbaudam vai ir tukš
                    _name = value; 
            }
        }

        public string Surname // uzvarda ipašiba
        {
            get { return _surname; }
            set
            {
                if (!string.IsNullOrEmpty(value)) // parbaudam vai nav tukš
                    _surname = value;
            }
        }

        public string FullName => $"{Name} {Surname}"; // pilna varda ipašiba

        public Gender Gender { get; set; } // gendera ipašiba

        public override string ToString() 
        {
            return $"Name: {Name}, Surname: {Surname}, FullName: {FullName}, Gender: {Gender}"; 
        }
    }

    //2.punkts

    public class Teacher : Person 
    {
        public DateTime ContractDate { get; set; } // kontrakta datums

        public override string ToString() 
        {
            return base.ToString() + $", ContractDate: {ContractDate}"; 
        }
    }

    //3.punkts

    public class Student : Person 
    {
        public string StudentIdNumber { get; set; } // ipašiba studenta apliecibas numuram

        public Student(string name, string surname, Gender gender, string studentIdNumber) 
        {
            Name = name; 
            Surname = surname; 
            Gender = gender; 
            StudentIdNumber = studentIdNumber; 
        }

        public Student() { } // tukš konstruktors

        public override string ToString()
        {
            return base.ToString() + $", StudentIdNumber: {StudentIdNumber}";
        }
    }
}
