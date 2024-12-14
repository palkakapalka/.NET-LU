using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
// 9.punkts
namespace project
{
    public class DataManager : IDataManager
    {
        private DataCollections _dataCollections = new DataCollections();

        public string Print()
        {
            string output = "People:\n";
            foreach (var person in _dataCollections.People)
            {
                output += person.ToString() + "\n"; // pivienojam informaciju par cilveku
            }

            output += "Courses:\n"; // pivienojam kursus
            foreach (var course in _dataCollections.Courses)
            {
                output += course.ToString() + "\n"; // pivienojam informaciju par kursus
            }

            output += "Assignments:\n";
            foreach (var assignment in _dataCollections.Assignments)
            {
                output += assignment.ToString() + "\n"; // pivienojam uzdevumus
            }

            output += "Submissions:\n";
            foreach (var submission in _dataCollections.Submissions)
            {
                output += $"Uzdevums: {submission.Assignment?.Description}, Name: {submission.Student?.Name} {submission.Student?.Surname},  Score: {submission.Score}\n"; // pievienojam nodevumus
            }

            return output;
        }

        public void Save(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(Print());
            }
        }

        public void Load(string path)
        {
            // ieladejam datus no fails
            if (File.Exists(path)) // parbaudam vai fails eksiste
            {
                string[] lines = File.ReadAllLines(path);
                _dataCollections = new DataCollections(); // Resetojam datu kolekcijas pirms tos ieladet
                foreach (var line in lines)
                {
                    if (line.StartsWith("Name:"))
                    {
                        // Parse person details from the line and add to People list
                        var parts = line.Split(',');
                        if (parts[4].StartsWith(" StudentIdNumber:")) {
                            var person = new Student {
                                StudentIdNumber = parts[4].Split(':')[1].Trim(),
                                Name = parts[0].Split(':')[1].Trim(),
                                Surname = parts[1].Split(':')[1].Trim(),
                                Gender = Enum.Parse<Gender>(parts[3].Split(':')[1].Trim())
                            };
                            _dataCollections.People.Add(person);
                        }else if (DateTime.TryParseExact(parts[4].Split(':')[1].Trim(), "dd.MM.yyyy H", null, System.Globalization.DateTimeStyles.None, out DateTime contractdate))
                        {
                            var person = new Teacher
                            {
                                Name = parts[0].Split(':')[1].Trim(),
                                Surname = parts[1].Split(':')[1].Trim(),
                                Gender = Enum.Parse<Gender>(parts[3].Split(':')[1].Trim()),
                                ContractDate = contractdate
                            };
                            _dataCollections.People.Add(person);
                        }
                        
                        
                    }
                    else if (line.StartsWith("Course Name:"))
                    {
                        // Parse course details from the line and add to Courses list
                        var parts = line.Split(',');
                        var course = new Course
                        {
                            Name = parts[0].Split(':')[1].Trim(),
                            Teacher = new Teacher { Name = parts[1].Split(':')[1].Trim() }
                        };
                        _dataCollections.Courses.Add(course);
                    }
                    else if (line.StartsWith("Deadline:"))
                    {
                        // Parse assignment details from the line and add to Assignments list
                        var parts = line.Split(',');
                        if (DateTime.TryParseExact(parts[0].Split(':')[1].Trim(), "dd.MM.yyyy H", null, System.Globalization.DateTimeStyles.None, out DateTime deadline)) // Изменяем формат даты
                        {
                            var assignment = new Assignment
                            {
                                Deadline = deadline,
                                Description = parts[2].Split(':')[1].Trim(),
                                Course = new Course { Name = parts[1].Split(':')[1].Trim() }
                            };
                            _dataCollections.Assignments.Add(assignment);
                        }
                        else
                        {
                            Console.WriteLine($"Unable to parse date: {parts[0].Split(':')[1].Trim()}");
                        }
                    }
                    else if (line.Contains("Uzdevums:"))
                    {
                        // Parse submission details from the line and add to Submissions list
                        var parts = line.Split(',');
                        var submission = new Submission
                        {
                            Score = int.Parse(parts[2].Split(':')[1].Trim()),
                            Student = new Student { Name = parts[1].Split(':')[1].Trim() },
                            Assignment = new Assignment { Description = parts[0].Split(':')[1].Trim() }
                        };
                        _dataCollections.Submissions.Add(submission);
                    }
                }
            }
        }


        public void CreateTestData()
        {
            // testa dati
            var teacher = new Teacher { Name = "Janis", Surname = "Berziņš", Gender = Gender.Man, ContractDate = DateTime.Now }; // veidojam pasniedzeju
            _dataCollections.People.Add(teacher);

            var student = new Student("Anna", "Priede", Gender.Woman, "AP1234"); // veidojam studentu
            _dataCollections.People.Add(student);

            var course = new Course { Name = "Mathematics", Teacher = teacher }; // viedojam kursu
            _dataCollections.Courses.Add(course);

            var assignment = new Assignment { Deadline = DateTime.Now.AddDays(7), Course = course, Description = "Algebra Homework" }; // veidojam uzdevumu
            _dataCollections.Assignments.Add(assignment);

            var submission = new Submission { Assignment = assignment, Student = student, SubmissionTime = DateTime.Now, Score = 95 }; // veidojam nodevumu
            _dataCollections.Submissions.Add(submission);
        }

        public void Reset()
        {
            _dataCollections = new DataCollections(); // Reset data collection
        }

        /////////////////////////////////////////////////////////  MD2 /////////////////////////////////////////////////////////

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
        public List<Submission> Submissions { get; set; } = new List<Submission>();

        // jauna studenta pievienošanas metode
        public void AddStudent(string name, string surname,string studentIdNumber, Gender gender)
        {
            var student = new Student
            {
                Name = name,
                Surname = surname,
                Gender = gender,
                StudentIdNumber = studentIdNumber
            };
        
            _dataCollections.People.Add(student);
        }

        // jauna uzdevuma pievienošanas metode
        public void AddAssignment(string description, DateTime deadline, string courseName)
        {
            var assignment = new Assignment
            {
                Description = description,
                Deadline = deadline,
                Course = new Course { Name = courseName }
            };
            _dataCollections.Assignments.Add(assignment);

        }

        // jauna nodevuma pievienošanas metode
        public void AddSubmission(string studentName, string assignmentDescription, int score)
        {

            var submission = new Submission 
            {
                Score = score,
                Student = new Student { Name = studentName },
                Assignment = new Assignment { Description = assignmentDescription }

            };
            _dataCollections.Submissions.Add(submission);

        }

        //  Assignment rediģešanas funkcija
        public void UpdateAssignment(string currentDescription, string newDescription, DateTime newDeadline, string newCourseName)
        {
            // meklejam uzdevumu pa aprakstu
            var assignment = _dataCollections.Assignments.Find(a => a.Description == currentDescription);
            if (assignment != null)
            {
                // atjauno ipašibas
                assignment.Description = newDescription;
                assignment.Deadline = newDeadline;
                assignment.Course = new Course { Name = newCourseName };
            }
            else
            {
                Console.WriteLine("Assignment не найден.");
            }
        }

        // Assignment rediģešanas Submission
        public void UpdateSubmission(string studentName, string assignmentDescription, int newScore)
        {
            // Meklekam nodevumu izmatojot studenta vardu un uzdevuma aprakstu
            var submission = _dataCollections.Submissions.Find(s => s.Student.Name == studentName && s.Assignment.Description == assignmentDescription);

            if (submission != null)
            {
                //atjauno ipašibas
                submission.Score = newScore;
            }
            else
            {
                Console.WriteLine("Submission не найден.");
            }
        }

        // Assignment dzēšana
        public void DeleteAssignment(string description)
        {
            var assignment = _dataCollections.Assignments.FirstOrDefault(a => a.Description == description);
            if (assignment != null)
            {
                _dataCollections.Assignments.Remove(assignment);
            }
            else
            {
                Console.WriteLine($"Assignment with description '{description}' not found.");
            }
        }

        // Submission dzēšana
        public void DeleteSubmission(string studentName, string studentSurname, string assignmentDescription)
        {
            var submission = _dataCollections.Submissions.FirstOrDefault(s => s.Student.Name == studentName && s.Student.Surname == studentSurname && s.Assignment.Description == assignmentDescription);

            if (submission != null)
            {
                _dataCollections.Submissions.Remove(submission);
            }
            else
            {
                Console.WriteLine($"Submission by student '{studentName}' for assignment '{assignmentDescription}' not found.");
            }
        }

    }
}
