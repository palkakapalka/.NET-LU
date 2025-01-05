// File: DatabaseManager.cs
using MySqlConnector;
using System;
using System.Collections.Generic; // For Dictionary
using System.Data;
using System.IO; // For File operations

namespace MD3
{
    public class DatabaseManager
    {
        private static string GetConnectionString()
        {
            string path = @"C:\Temp\ConnS.txt";
            if (!File.Exists(path))
                throw new FileNotFoundException("Connection string file not found.");
            return File.ReadAllText(path).Trim();
        }

        private MySqlConnection CreateConnection()
        {
            string connectionString = GetConnectionString();
            return new MySqlConnection(connectionString);
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                // Реализация уведомления пользователя
                throw;
            }
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                throw;
            }
            return dataTable;
        }

        public string TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return "Connection successful!";
                }
            }
            catch (Exception ex)
            {
                return $"Connection failed: {ex.Message}";
            }
        }


        public void ResetDataBase()
        {
            try
            {
                // Insert sample data into the Teacher table
                string insertQuery = @"
                        DROP TABLE Submission;
                        DROP TABLE Assignment;
                        DROP TABLE Student;
                        DROP TABLE Course;
                        DROP TABLE Teacher;

                    -- Create the Teacher table
                    CREATE TABLE Teacher (
                        Id INT PRIMARY KEY AUTO_INCREMENT,
                        Name VARCHAR(50) NOT NULL,         
                        Surname VARCHAR(50) NOT NULL,     
                        Gender CHAR(1) NOT NULL,           
                        ContractDate DATE NOT NULL );       

                    -- Create the Course table
                    CREATE TABLE Course (
                        Id INT PRIMARY KEY AUTO_INCREMENT, 
                        Name VARCHAR(100) NOT NULL,     
                        TeacherId INT NOT NULL,           
                        FOREIGN KEY (TeacherId) REFERENCES Teacher(Id) ON DELETE CASCADE ); 
                    
                    -- Create the Assignment table
                    CREATE TABLE Assignment (
                        Id INT PRIMARY KEY AUTO_INCREMENT,
                        Deadline DATE NOT NULL,           
                        CourseId INT NOT NULL,            
                        Description TEXT NOT NULL,       
                        FOREIGN KEY (CourseId) REFERENCES Course(Id) ON DELETE CASCADE );

                    -- Create the Student table
                    CREATE TABLE Student (
                        Id INT PRIMARY KEY AUTO_INCREMENT, 
                        Name VARCHAR(50) NOT NULL,         
                        Surname VARCHAR(50) NOT NULL,      
                        Gender CHAR(1) NOT NULL,          
                        StudentIdNumber VARCHAR(20) NOT NULL UNIQUE );

                    -- Create the Submission table
                    CREATE TABLE Submission (
                        Id INT PRIMARY KEY AUTO_INCREMENT,   
                        AssignmentId INT NOT NULL,            
                        StudentId INT NOT NULL,              
                        SubmissionTime DATETIME NOT NULL,     
                        Score DECIMAL(5, 2),                  
                        FOREIGN KEY (AssignmentId) REFERENCES Assignment(Id) ON DELETE CASCADE,
                        FOREIGN KEY (StudentId) REFERENCES Student(Id) ON DELETE CASCADE );";

                ExecuteNonQuery(insertQuery);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GenerateAndDisplayTeachers: {ex.Message}");
            }
        }

        public void GenerateTestData()
        {
            try
            {
                // Insert sample data into the Teacher table
                // Generate data for Teacher table
                string teacherQuery = @"
                    INSERT INTO Teacher (Name, Surname, Gender, ContractDate) VALUES
                    ('John', 'Doe', 'M', '2022-01-01'),
                    ('Jane', 'Smith', 'F', '2021-06-15'),
                    ('Alice', 'Brown', 'F', '2020-11-23'),
                    ('Bob', 'White', 'M', '2019-03-12');";
                ExecuteNonQuery(teacherQuery);

                // Generate data for Student table
                string studentQuery = @"
                    INSERT INTO Student (Name, Surname, Gender, StudentIdNumber) VALUES
                    ('Michael', 'Johnson', 'M', 'mj1001'),
                    ('Emily', 'Clark', 'F', 'ec1002'),
                    ('Chris', 'Davis', 'M', 'cd1003'),
                    ('Sophia', 'Garcia', 'F', 'sg004');";
                ExecuteNonQuery(studentQuery);

                // Generate data for Course table
                string courseQuery = @"
                    INSERT INTO Course (Name, TeacherId) VALUES
                    ('Math', 1),
                    ('Physics', 2),
                    ('Chemistry', 3),
                    ('Biology', 4);";
                ExecuteNonQuery(courseQuery);

                // Generate data for Assignment table
                string assignmentQuery = @"
                    INSERT INTO Assignment (Deadline, CourseId, Description) VALUES
                    ('2024-01-15', 1, 'Algebra Homework'),
                    ('2024-02-20', 2, 'Mechanics Project'),
                    ('2024-03-05', 3, 'Organic Chemistry Report'),
                    ('2024-04-10', 4, 'Cell Biology Presentation');";
                ExecuteNonQuery(assignmentQuery);

                // Generate data for Submission table
                string submissionQuery = @"
                    INSERT INTO Submission (AssignmentId, StudentId, SubmissionTime, Score) VALUES
                    (1, 1, '2024-01-10', 95.5),
                    (2, 2, '2024-02-18', 88.0),
                    (3, 3, '2024-03-03', 92.0),
                    (4, 4, '2024-04-08', 85.0);";
                ExecuteNonQuery(submissionQuery);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GenerateAndDisplayTeachers: {ex.Message}");
            }
        }


        ////////////////////////////   Teacher block  ///////////////////// create, read, update, delete
        public void AddTeacher(string name, string surname, string gender, DateTime contractDate)
        {
            try
            {
                string query = "INSERT INTO Teacher (Name, Surname, Gender, ContractDate) VALUES (@Name, @Surname, @Gender, @ContractDate)";
                var parameters = new Dictionary<string, object>
                                    {
                                        { "@Name", name },
                                        { "@Surname", surname },
                                        { "@Gender", gender },
                                        { "@ContractDate", contractDate }
                                    };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Teacher added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add teacher: {ex.Message}");
                throw;
            }
        }

        public void DeleteTeacher(int teacherId)
        {
            try
            {
                string query = "DELETE FROM Teacher WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
                                {
                                    { "@Id", teacherId }
                                };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Teacher deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete teacher: {ex.Message}");
                throw;
            }
        }

        public void UpdateTeacher(int teacherId, string newName, string newSurname, string newGender, DateTime newContractDate)
        {
            try
            {
                string query = @"UPDATE Teacher 
                         SET Name = @Name, 
                             Surname = @Surname, 
                             Gender = @Gender, 
                             ContractDate = @ContractDate 
                         WHERE Id = @Id";

                var parameters = new Dictionary<string, object>
                            {
                                { "@Id", teacherId },
                                { "@Name", newName },
                                { "@Surname", newSurname },
                                { "@Gender", newGender },
                                { "@ContractDate", newContractDate }
                            };

                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Teacher updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update teacher: {ex.Message}");
                throw;
            }
        }


        public string ShowTeacher()
        {
            try
            {
                // Retrieve all data from the Teacher table
                string selectQuery = "SELECT * FROM Teacher;";
                DataTable teachers = ExecuteQuery(selectQuery);

                // Convert data to a string
                string result = "Teachers:\n";
                foreach (DataRow row in teachers.Rows)
                {
                    result += $"Name: {row["Name"]} {row["Surname"]}, Gender: {row["Gender"]}, ContractDate: {row["ContractDate"]}\n";
                }

                return result;
            }
            catch (Exception ex)
            {
                return $"Error in GenerateAndDisplayTeachers: {ex.Message}";
            }
        }

        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            string query = "SELECT Id, Name, Surname, Gender, ContractDate FROM Teacher";

            try
            {
                DataTable dataTable = ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    teachers.Add(new Teacher
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        Surname = row["Surname"].ToString(),
                        Gender = row["Gender"].ToString(),
                        ContractDate = Convert.ToDateTime(row["ContractDate"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teachers: {ex.Message}");
                throw;
            }

            return teachers;
        }


        ////////////////////////////   Student block  /////////////////////
        public void AddStudent(string name, string surname, string gender, string studentIdNumber)
        {
            try
            {
                string query = "INSERT INTO Student (Name, Surname, Gender, StudentIdNumber) VALUES (@Name, @Surname, @Gender, @StudentIdNumber)";
                var parameters = new Dictionary<string, object>
                                    {
                                        { "@Name", name },
                                        { "@Surname", surname },
                                        { "@Gender", gender },
                                        { "@StudentIdNumber", studentIdNumber }
                                    };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add student: {ex.Message}");
                throw;
            }
        }

        public void DeleteStudent(int studentId)
        {
            try
            {
                string query = "DELETE FROM Student WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
                                {
                                    { "@Id", studentId }
                                };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete student: {ex.Message}");
                throw;
            }
        }

        public void UpdateStudent(int studentrId, string newName, string newSurname, string newGender, string newStudentIdNumber)
        {
            try
            {
                string query = @"UPDATE Student 
                         SET Name = @Name, 
                             Surname = @Surname, 
                             Gender = @Gender, 
                             StudentIdNumber = @StudentIdNumber 
                         WHERE Id = @Id";

                var parameters = new Dictionary<string, object>
                            {
                                { "@Id", studentrId },
                                { "@Name", newName },
                                { "@Surname", newSurname },
                                { "@Gender", newGender },
                                { "@StudentIdNumber", newStudentIdNumber }
                            };

                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update student: {ex.Message}");
                throw;
            }
        }


        public string ShowStudent()
        {
            try
            {
                // Retrieve all data from the Teacher table
                string selectQuery = "SELECT * FROM Student;";
                DataTable students = ExecuteQuery(selectQuery);

                // Convert data to a string
                string result = "Students:\n";
                foreach (DataRow row in students.Rows)
                {
                    result += $"Name: {row["Name"]} {row["Surname"]}, Gender: {row["Gender"]}, Student Id: {row["StudentIdNumber"]}\n";
                }

                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public List<Student> GetAllStudent()
        {
            List<Student> student = new List<Student>();
            string query = "SELECT Id, Name, Surname, Gender, StudentIdNumber FROM Student";

            try
            {
                DataTable dataTable = ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    student.Add(new Student
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        Surname = row["Surname"].ToString(),
                        Gender = row["Gender"].ToString(),
                        StudentIdNumber = row["StudentIdNumber"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teachers: {ex.Message}");
                throw;
            }

            return student;
        }
        ////////////////////////////   Course block   /////////////////////

        public void AddCourse(string name, string TeacherId)
        {
            try
            {
                string query = "INSERT INTO Course (Name, TeacherId) VALUES (@Name, @TeacherId)";
                var parameters = new Dictionary<string, object>
                            {
                                { "@Name", name },
                                { "@TeacherId", TeacherId },
                            };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add Course: {ex.Message}");
                throw;
            }
        }

        public void DeleteCourse(int CourseId)
        {
            try
            {
                string query = "DELETE FROM Course WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
                        {
                            { "@Id", CourseId }
                        };
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete Course: {ex.Message}");
                throw;
            }
        }

        public void UpdateCourse(int CourserId, string newName, int newTeacherId)
        {
            try
            {
                string query = @"UPDATE Course 
                 SET Name = @Name, 
                     TeacherId = @TeacherId 
                 WHERE Id = @Id";

                var parameters = new Dictionary<string, object>
                    {
                        { "@Id", CourserId },
                        { "@Name", newName },
                        { "@TeacherId", newTeacherId },
                    };

                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update Course: {ex.Message}");
                throw;
            }
        }


        public string ShowCourse()
        {
            try
            {
                // SQL-запрос для получения данных о курсах вместе с именами учителей
                string selectQuery = @"
            SELECT 
                Course.Id,
                Course.Name AS CourseName,
                Teacher.Name AS TeacherName,
                Teacher.Surname AS TeacherSurname
            FROM 
                Course
            INNER JOIN 
                Teacher ON Course.TeacherId = Teacher.Id;";

                // Выполнение запроса
                DataTable courses = ExecuteQuery(selectQuery);

                // Преобразование данных в строку
                string result = "Courses:\n";
                foreach (DataRow row in courses.Rows)
                {
                    result += $"{row["CourseName"]}, Teacher: {row["TeacherName"]} {row["TeacherSurname"]}\n";
                }

                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public List<Course> GetAllCourse()
        {
            List<Course> Course = new List<Course>();
            string query = "SELECT Id, Name, TeacherId FROM Course";

            try
            {
                DataTable dataTable = ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    Course.Add(new Course
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        TeacherId = Convert.ToInt32(row["TeacherId"]),
 
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teachers: {ex.Message}");
                throw;
            }

            return Course;
        }
        //////////////////////////// Assignment block /////////////////////
        //////////////////////////// Submission block /////////////////////
    }
}
