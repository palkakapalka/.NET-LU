﻿using System;
using System.Collections.Generic;
using System.IO;
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
                output += $"{submission.Assignment?.Description}, Score: {submission.Score}\n"; // pievienojam nodevumus
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
                        var person = new Student
                        {
                            Name = parts[0].Split(':')[1].Trim(),
                            Surname = parts[1].Split(':')[1].Trim(),
                            Gender = Enum.Parse<Gender>(parts[3].Split(':')[1].Trim())
                        };
                        _dataCollections.People.Add(person);
                    }
                    else if (line.StartsWith("Course Name:")) 
                    {
                        // Parse course details from the line and add to Courses list
                        var parts = line.Split(',');
                        var course = new Course
                        {
                            Name = parts[0].Split(':')[1].Trim()
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
                                Description = parts[2].Split(':')[1].Trim()
                            };
                            _dataCollections.Assignments.Add(assignment);
                        }
                        else
                        {
                            Console.WriteLine($"Unable to parse date: {parts[0].Split(':')[1].Trim()}");
                        }
                    }
                    else if (line.Contains("Score:")) //
                    {
                        // Parse submission details from the line and add to Submissions list
                        var parts = line.Split(',');
                        var submission = new Submission
                        {
                            Score = int.Parse(parts[1].Split(':')[1].Trim())
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
    }
}
