using MySqlConnector;
using System;
using System.Data;

namespace MD3
{
    public class DatabaseManager
    {
        private static string GetConnectionString()
        {
            string path = @"C:\Temp\ConnS.txt"; // ceļš uz Connection string
            if (!File.Exists(path))
                throw new FileNotFoundException("Connection string file not found.");
            return File.ReadAllText(path).Trim();
        }

        private MySqlConnection CreateConnection()
        {
            string connectionString = GetConnectionString();
            return new MySqlConnection(connectionString);
        }

        /// Izpilda SQL vaicājumu bez rezultātu atgriešanas, piemēram, INSERT, UPDATE vai DELETE.
        public void ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open(); // Atver datu bāzes savienojumu
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                // Pievieno parametrus pie SQL komandas
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        command.ExecuteNonQuery(); // Izpilda SQL vaicājumu bez rezultāta atgriešanas
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                // Apstrādā kļūdu un informē lietotāju par problēmu
                throw;
            }
        }



        /// Izpilda SQL vaicājumu un atgriež rezultātu kā datu tabulu (piemēram, SELECT vaicājumi).
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open(); // Atver datu bāzes savienojumu
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                // Pievieno parametrus pie SQL komandas
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            // Aizpilda datu tabulu ar vaicājuma rezultātiem
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                // Apstrādā kļūdu un izmet to tālāk
                throw;
            }
            return dataTable; // Atgriež datu tabulu ar vaicājuma rezultātiem
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


        /// Atiestata datu bāzi, dzēšot un atkārtoti izveidojot visas tabulas ar piemēra datu struktūru.
        public void ResetDataBase()
        {
            try
            {
                // SQL skripts tabulu dzēšanai un atkārtotai izveidei
                string insertQuery = @"
                DROP TABLE Submission;
                DROP TABLE Assignment;
                DROP TABLE Student;
                DROP TABLE Course;
                DROP TABLE Teacher;

            -- Izveido Teacher tabulu
            CREATE TABLE Teacher (
                Id INT PRIMARY KEY AUTO_INCREMENT,
                Name VARCHAR(50) NOT NULL,         
                Surname VARCHAR(50) NOT NULL,     
                Gender CHAR(1) NOT NULL,           
                ContractDate DATE NOT NULL );       

            -- Izveido Course tabulu
            CREATE TABLE Course (
                Id INT PRIMARY KEY AUTO_INCREMENT, 
                Name VARCHAR(100) NOT NULL,     
                TeacherId INT NOT NULL,           
                FOREIGN KEY (TeacherId) REFERENCES Teacher(Id) ON DELETE CASCADE ); 
            
            -- Izveido Assignment tabulu
            CREATE TABLE Assignment (
                Id INT PRIMARY KEY AUTO_INCREMENT,
                Deadline DATE NOT NULL,           
                CourseId INT NOT NULL,            
                Description TEXT NOT NULL,       
                FOREIGN KEY (CourseId) REFERENCES Course(Id) ON DELETE CASCADE );

            -- Izveido Student tabulu
            CREATE TABLE Student (
                Id INT PRIMARY KEY AUTO_INCREMENT, 
                Name VARCHAR(50) NOT NULL,         
                Surname VARCHAR(50) NOT NULL,      
                Gender CHAR(1) NOT NULL,          
                StudentIdNumber VARCHAR(20) NOT NULL UNIQUE );

            -- Izveido Submission tabulu
            CREATE TABLE Submission (
                Id INT PRIMARY KEY AUTO_INCREMENT,   
                AssignmentId INT NOT NULL,            
                StudentId INT NOT NULL,              
                SubmissionTime DATETIME NOT NULL,     
                Score DECIMAL(5, 2),                  
                FOREIGN KEY (AssignmentId) REFERENCES Assignment(Id) ON DELETE CASCADE,
                FOREIGN KEY (StudentId) REFERENCES Student(Id) ON DELETE CASCADE );";

                // Izpilda SQL skriptu
                ExecuteNonQuery(insertQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetDataBase: {ex.Message}"); // Izvada kļūdas ziņojumu
            }
        }


        /// Ģenerē testa datus un ievieto tos datu bāzes tabulās, tostarp Teacher, Student, Course, Assignment un Submission tabulās.
        public void GenerateTestData()
        {
            try
            {
                // Dati Teacher tabulai
                string teacherQuery = @"
            INSERT INTO Teacher (Name, Surname, Gender, ContractDate) VALUES
            ('John', 'Doe', 'M', '2022-01-01'),
            ('Jane', 'Smith', 'F', '2021-06-15'),
            ('Alice', 'Brown', 'F', '2020-11-23'),
            ('Bob', 'White', 'M', '2019-03-12');";
                ExecuteNonQuery(teacherQuery);

                // Dati Student tabulai
                string studentQuery = @"
            INSERT INTO Student (Name, Surname, Gender, StudentIdNumber) VALUES
            ('Michael', 'Johnson', 'M', 'mj1001'),
            ('Emily', 'Clark', 'F', 'ec1002'),
            ('Chris', 'Davis', 'M', 'cd1003'),
            ('Sophia', 'Garcia', 'F', 'sg004');";
                ExecuteNonQuery(studentQuery);

                // Dati Course tabulai
                string courseQuery = @"
            INSERT INTO Course (Name, TeacherId) VALUES
            ('Math', 1),
            ('Physics', 2),
            ('Chemistry', 3),
            ('Biology', 4);";
                ExecuteNonQuery(courseQuery);

                // Dati Assignment tabulai
                string assignmentQuery = @"
            INSERT INTO Assignment (Deadline, CourseId, Description) VALUES
            ('2024-01-15', 1, 'Algebra Homework'),
            ('2024-02-20', 2, 'Mechanics Project'),
            ('2024-03-05', 3, 'Organic Chemistry Report'),
            ('2024-04-10', 4, 'Cell Biology Presentation');";
                ExecuteNonQuery(assignmentQuery);

                // Dati Submission tabulai
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
                Console.WriteLine($"Error in GenerateTestData: {ex.Message}");
            }
        }



        ////////////////////////////   Teacher block  ///////////////////// 
        
        /// Pievieno jaunu skolotāju Teacher tabulai, izmantojot norādītos datus (vārds, uzvārds, dzimums un līguma datums).
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


        /// Dzēš skolotāju no Teacher tabulas, izmantojot norādīto skolotāja ID.
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


        /// Atjaunina skolotāja datus Teacher tabulā, izmantojot norādīto skolotāja ID un jaunos datus (vārdu, uzvārdu, dzimumu un līguma datumu).
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



        /// Atgriež informāciju par visiem skolotājiem Teacher tabulā, formatējot to cilvēklasāmā tekstā.
        public string ShowTeacher()
        {
            try
            {
                // Iegūst visu informāciju no Teacher tabulas
                string selectQuery = "SELECT * FROM Teacher;";
                DataTable teachers = ExecuteQuery(selectQuery);

                // Konvertē iegūtos datus uz lasāmu tekstu
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


        /// Iegūst un atgriež sarakstu ar visiem skolotājiem no Teacher tabulas.
        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            string query = "SELECT Id, Name, Surname, Gender, ContractDate FROM Teacher";

            try
            {
                // Izpilda SQL vaicājumu un iegūst datus
                DataTable dataTable = ExecuteQuery(query);

                // Pārveido iegūtos datus par Teacher objektiem un pievieno tos sarakstam
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
        
        /// Pievieno jaunu studentu Student tabulā, izmantojot norādītos datus.
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

                // Izpilda SQL vaicājumu, lai pievienotu studentu
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add student: {ex.Message}");
                throw;
            }
        }


        /// Dzēš studentu no Student tabulas, izmantojot norādīto studenta Id.
        public void DeleteStudent(int studentId)
        {
            try
            {
                string query = "DELETE FROM Student WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
        {
            { "@Id", studentId }
        };

                // Izpilda SQL vaicājumu, lai dzēstu studentu
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete student: {ex.Message}");
                throw;
            }
        }


        /// Atjaunina studenta informāciju Student tabulā, izmantojot norādīto studenta Id un jaunos datus.
        public void UpdateStudent(int studentId, string newName, string newSurname, string newGender, string newStudentIdNumber)
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
            { "@Id", studentId },
            { "@Name", newName },
            { "@Surname", newSurname },
            { "@Gender", newGender },
            { "@StudentIdNumber", newStudentIdNumber }
        };

                // Izpilda SQL vaicājumu, lai atjauninātu studenta informāciju
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Student updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update student: {ex.Message}");
                throw;
            }
        }



        /// Iegūst visu Student tabulas ierakstu datus un pārveido tos lasāmā teksta formātā.
        public string ShowStudent()
        {
            try
            {
                // SQL vaicājums, lai iegūtu visus Student tabulas datus
                string selectQuery = "SELECT * FROM Student;";
                DataTable students = ExecuteQuery(selectQuery);

                // Datu pārveidošana uz lasāmu teksta formātu
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


        /// Iegūst visu Student tabulas ierakstu sarakstu un atgriež to kā Student objektu sarakstu.
        public List<Student> GetAllStudent()
        {
            List<Student> student = new List<Student>();
            string query = "SELECT Id, Name, Surname, Gender, StudentIdNumber FROM Student";

            try
            {
                // Izpilda SQL vaicājumu un saņem rezultātus DataTable formā
                DataTable dataTable = ExecuteQuery(query);

                // Pārveido katru DataTable rindu par Student objektu un pievieno sarakstam
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
                Console.WriteLine($"Error retrieving students: {ex.Message}");
                throw;
            }

            return student;
        }

        ////////////////////////////   Course block   /////////////////////

        /// Pievieno jaunu kursu ar norādīto nosaukumu un skolotāja ID Course tabulai.
        public void AddCourse(string name, string TeacherId)
        {
            try
            {
                string query = "INSERT INTO Course (Name, TeacherId) VALUES (@Name, @TeacherId)";
                var parameters = new Dictionary<string, object>
        {
            { "@Name", name }, // Kursa nosaukums
            { "@TeacherId", TeacherId }, // Skolotāja ID
        };

                // Izpilda SQL vaicājumu, lai ievietotu jaunu ierakstu tabulā
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add Course: {ex.Message}");
                throw;
            }
        }


        /// Dzēš kursu no Course tabulas, izmantojot norādīto kursa ID.
        public void DeleteCourse(int CourseId)
        {
            try
            {
                string query = "DELETE FROM Course WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
        {
            { "@Id", CourseId } // Kursa ID, kuru nepieciešams dzēst
        };

                // Izpilda SQL vaicājumu, lai dzēstu kursu
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete Course: {ex.Message}");
                throw;
            }
        }


        /// Atjaunina kursa informāciju Course tabulā, izmantojot norādīto kursa ID, jauno nosaukumu un jauno pasniedzēja ID.
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
            { "@Id", CourserId },       // Kursa ID, kuru nepieciešams atjaunināt
            { "@Name", newName },       // Jaunais kursa nosaukums
            { "@TeacherId", newTeacherId } // Jaunais pasniedzēja ID
        };

                // Izpilda SQL vaicājumu, lai atjauninātu kursu
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Course updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update Course: {ex.Message}");
                throw;
            }
        }



        /// Parāda visu kursu un to atbilstošo pasniedzēju informāciju, apvienojot datus no Course un Teacher tabulām.
        public string ShowCourse()
        {
            try
            {
                // SQL vaicājums, lai iegūtu informāciju par kursiem un saistītajiem pasniedzējiem
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

                // Izpilda SQL vaicājumu un saņem rezultātu
                DataTable courses = ExecuteQuery(selectQuery);

                // Sagatavo datu atgriešanu kā teksta formātu
                string result = "Courses:\n";
                foreach (DataRow row in courses.Rows)
                {
                    result += $"{row["CourseName"]}, Teacher: {row["TeacherName"]} {row["TeacherSurname"]}\n";
                }

                return result;
            }
            catch (Exception ex)
            {
                // Atgriež kļūdas ziņojumu, ja vaicājuma izpilde neizdodas
                return $"Error: {ex.Message}";
            }
        }



        /// Iegūst visu kursu sarakstu no datu bāzes, iekļaujot katra kursa ID, nosaukumu un saistītā pasniedzēja ID.
        public List<Course> GetAllCourse()
        {
            List<Course> courses = new List<Course>(); // Inicializē kursu sarakstu
            string query = "SELECT Id, Name, TeacherId FROM Course"; // SQL vaicājums visu kursu iegūšanai

            try
            {
                // Izpilda vaicājumu un saņem rezultātu datu tabulā
                DataTable dataTable = ExecuteQuery(query);

                // Apstrādā katru rindu un pievieno kursu objektus sarakstam
                foreach (DataRow row in dataTable.Rows)
                {
                    courses.Add(new Course
                    {
                        Id = Convert.ToInt32(row["Id"]), // Kursa ID
                        Name = row["Name"].ToString(), // Kursa nosaukums
                        TeacherId = Convert.ToInt32(row["TeacherId"]), // Saistītā pasniedzēja ID
                    });
                }
            }
            catch (Exception ex)
            {
                // Izdrukā kļūdas ziņojumu konsolē un pārmet kļūdu augšup
                Console.WriteLine($"Error retrieving courses: {ex.Message}");
                throw;
            }

            return courses; // Atgriež kursu sarakstu
        }

        //////////////////////////// Assignment block /////////////////////
        /// Pievieno jaunu uzdevumu datu bāzē ar norādīto aprakstu, termiņu un saistītā kursa ID.
        public void AddAssignment(string description, DateTime deadline, int courseId)
        {
            try
            {
                // SQL vaicājums jauna uzdevuma pievienošanai
                string query = "INSERT INTO Assignment (Description, Deadline, CourseId) VALUES (@Description, @Deadline, @CourseId)";
                var parameters = new Dictionary<string, object>
        {
            { "@Description", description }, // Uzdevuma apraksts
            { "@Deadline", deadline }, // Uzdevuma termiņš
            { "@CourseId", courseId } // Saistītā kursa ID
        };

                ExecuteNonQuery(query, parameters); // Izpilda vaicājumu ar parametriem
                Console.WriteLine("Assignment added successfully!"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                // Izdrukā kļūdas ziņojumu konsolē un pārmet kļūdu augšup
                Console.WriteLine($"Failed to add assignment: {ex.Message}");
                throw;
            }
        }


        /// Dzēš uzdevumu no datu bāzes, izmantojot norādīto uzdevuma ID.
        public void DeleteAssignment(int assignmentId)
        {
            try
            {
                // SQL vaicājums uzdevuma dzēšanai pēc ID
                string query = "DELETE FROM Assignment WHERE Id = @Id";
                var parameters = new Dictionary<string, object>
        {
            { "@Id", assignmentId } // Uzdevuma ID dzēšanai
        };

                ExecuteNonQuery(query, parameters); // Izpilda dzēšanas vaicājumu ar parametriem
                Console.WriteLine("Assignment deleted successfully!"); // Ziņojums par veiksmīgu dzēšanu
            }
            catch (Exception ex)
            {
                // Izdrukā kļūdas ziņojumu konsolē un pārmet kļūdu augšup
                Console.WriteLine($"Failed to delete assignment: {ex.Message}");
                throw;
            }
        }


        /// Atjaunina uzdevuma informāciju datu bāzē, izmantojot norādīto uzdevuma ID un jaunās vērtības.
        public void UpdateAssignment(int assignmentId, string newDescription, DateTime newDeadline, int newCourseId)
        {
            try
            {
                // SQL vaicājums uzdevuma informācijas atjaunināšanai
                string query = @"UPDATE Assignment 
                         SET Description = @Description, 
                             Deadline = @Deadline, 
                             CourseId = @CourseId 
                         WHERE Id = @Id";

                var parameters = new Dictionary<string, object>
        {
            { "@Id", assignmentId }, // Uzdevuma ID
            { "@Description", newDescription }, // Jaunais apraksts
            { "@Deadline", newDeadline }, // Jaunais termiņš
            { "@CourseId", newCourseId } // Jaunais kursa ID
        };

                ExecuteNonQuery(query, parameters); // Izpilda atjaunināšanas vaicājumu ar parametriem
                Console.WriteLine("Assignment updated successfully!"); // Ziņojums par veiksmīgu atjaunināšanu
            }
            catch (Exception ex)
            {
                // Izdrukā kļūdas ziņojumu konsolē un pārmet kļūdu augšup
                Console.WriteLine($"Failed to update assignment: {ex.Message}");
                throw;
            }
        }


        /// Parāda visus uzdevumus ar to aprakstiem, termiņiem un kursu nosaukumiem.
        public string ShowAssignments()
        {
            try
            {
                // SQL vaicājums, lai iegūtu uzdevumu datus kopā ar kursu nosaukumiem
                string selectQuery = @"SELECT 
                                Assignment.Id,
                                Assignment.Description,
                                Assignment.Deadline,
                                Course.Name AS CourseName
                             FROM 
                                Assignment
                             INNER JOIN 
                                Course ON Assignment.CourseId = Course.Id";

                DataTable assignments = ExecuteQuery(selectQuery);

                // Konvertē datus uz tekstu
                string result = "Assignments:\n";
                foreach (DataRow row in assignments.Rows)
                {
                    result += $"Description: {row["Description"]}, Deadline: {row["Deadline"]}, Course: {row["CourseName"]}\n";
                }

                return result; // Atgriež formatētu rezultātu
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}"; // Atgriež kļūdas ziņojumu, ja rodas kļūda
            }
        }


        /// Iegūst visus uzdevumus no datubāzes un atgriež to sarakstu.
        public List<Assignment> GetAllAssignments()
        {
            List<Assignment> assignments = new List<Assignment>();
            string query = "SELECT Id, Description, Deadline, CourseId FROM Assignment";

            try
            {
                // Izpilda SQL vaicājumu un iegūst rezultātu tabulu
                DataTable dataTable = ExecuteQuery(query);

                // Iterē cauri visām rindām un izveido Assignment objektus
                foreach (DataRow row in dataTable.Rows)
                {
                    assignments.Add(new Assignment
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Description = row["Description"].ToString(),
                        Deadline = Convert.ToDateTime(row["Deadline"]),
                        CourseId = Convert.ToInt32(row["CourseId"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving assignments: {ex.Message}"); // Paziņo par kļūdu
                throw; // Izmet kļūdu tālāk
            }

            return assignments; // Atgriež izveidoto uzdevumu sarakstu
        }


        /// Parāda konkrētam kursam piederošos uzdevumus, izmantojot kursa ID.
        public string ShowAssignmentsByCourse(int courseId)
        {
            try
            {
                // SQL vaicājums, lai iegūtu uzdevumus pēc kursa ID
                string selectQuery = @"SELECT 
                                Assignment.Id,
                                Assignment.Description,
                                Assignment.Deadline
                              FROM 
                                Assignment
                              WHERE 
                                CourseId = @CourseId";

                // Parametru sagatavošana vaicājumiem
                var parameters = new Dictionary<string, object>
        {
            { "@CourseId", courseId }
        };

                // Izpilda uzdevumu vaicājumu
                DataTable assignments = ExecuteQuery(selectQuery, parameters);

                // Vaicājums, lai iegūtu kursa nosaukumu
                string courseNameQuery = @"SELECT Name FROM Course WHERE Id = @courseId;";
                DataTable nameTable = ExecuteQuery(courseNameQuery, parameters);
                string courseName = nameTable.Rows[0]["Name"].ToString(); // Iegūst kursa nosaukumu

                // Veido rezultātu teksta formātā
                string result = $"Assignments for {courseName}:\n";
                foreach (DataRow row in assignments.Rows)
                {
                    result += $"Description: {row["Description"]}, Deadline: {row["Deadline"]}\n";
                }

                return result; // Atgriež uzdevumu sarakstu kā tekstu
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}"; // Paziņo par kļūdu
            }
        }

        //////////////////////////// Submission block /////////////////////

        /// Pievieno jaunu iesniegumu ar norādītajiem uzdevuma ID, studenta ID, iesniegšanas laiku un rezultātu.
        public void AddSubmission(int assignmentId, int studentId, DateTime submissionTime, decimal score)
        {
            try
            {
                // SQL vaicājums, lai pievienotu iesniegumu datubāzei
                string query = "INSERT INTO Submission (AssignmentId, StudentId, SubmissionTime, Score) VALUES (@AssignmentId, @StudentId, @SubmissionTime, @Score)";

                // Parametru sagatavošana vaicājumam
                var parameters = new Dictionary<string, object>
        {
            { "@AssignmentId", assignmentId }, // Uzdevuma ID
            { "@StudentId", studentId }, // Studenta ID
            { "@SubmissionTime", submissionTime }, // Iesniegšanas laiks
            { "@Score", score } // Rezultāts
        };

                // Izpilda vaicājumu bez atgriezeniskās vērtības
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Submission added successfully!"); // Ziņojums par veiksmīgu pievienošanu
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add submission: {ex.Message}"); // Paziņojums par kļūdu
                throw; // Izmet kļūdu tālākai apstrādei
            }
        }


        /// Dzēš iesniegumu no datubāzes, izmantojot norādīto iesnieguma ID.
        public void DeleteSubmission(int submissionId)
        {
            try
            {
                // SQL vaicājums iesnieguma dzēšanai pēc ID
                string query = "DELETE FROM Submission WHERE Id = @Id";

                // Parametru sagatavošana vaicājumam
                var parameters = new Dictionary<string, object>
        {
            { "@Id", submissionId } // Iesnieguma ID
        };

                // Izpilda vaicājumu bez atgriezeniskās vērtības
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Submission deleted successfully!"); // Ziņojums par veiksmīgu dzēšanu
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete submission: {ex.Message}"); // Paziņojums par kļūdu
                throw; // Izmet kļūdu tālākai apstrādei
            }
        }


        /// Atjaunina iesnieguma informāciju datubāzē, izmantojot norādīto iesnieguma ID un jaunās vērtības.
        public void UpdateSubmission(int submissionId, int newAssignmentId, int newStudentId, DateTime newSubmissionTime, decimal newScore)
        {
            try
            {
                // SQL vaicājums iesnieguma atjaunināšanai
                string query = @"UPDATE Submission 
                         SET AssignmentId = @AssignmentId, 
                             StudentId = @StudentId, 
                             SubmissionTime = @SubmissionTime, 
                             Score = @Score 
                         WHERE Id = @Id";

                // Parametru sagatavošana vaicājumam
                var parameters = new Dictionary<string, object>
        {
            { "@Id", submissionId }, // Esošā iesnieguma ID
            { "@AssignmentId", newAssignmentId }, // Jaunā uzdevuma ID
            { "@StudentId", newStudentId }, // Jaunā studenta ID
            { "@SubmissionTime", newSubmissionTime }, // Jaunais iesniegšanas laiks
            { "@Score", newScore } // Jaunais vērtējums
        };

                // Izpilda vaicājumu bez atgriezeniskās vērtības
                ExecuteNonQuery(query, parameters);
                Console.WriteLine("Submission updated successfully!"); // Ziņojums par veiksmīgu atjaunināšanu
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update submission: {ex.Message}"); // Paziņojums par kļūdu
                throw; // Izmet kļūdu tālākai apstrādei
            }
        }


        /// Parāda visu iesniegumu sarakstu ar kursa, uzdevuma un studenta informāciju.
        public string ShowSubmission()
        {
            try
            {
                // SQL vaicājums iesniegumu datu iegūšanai, ietverot kursa, uzdevuma un studenta informāciju
                string selectQuery = @"
            SELECT 
                Course.Name AS CourseName,
                Assignment.Description AS AssignmentDescription,
                Student.Name AS StudentName,
                Student.Surname AS StudentSurname,
                Submission.SubmissionTime,
                Submission.Score
            FROM 
                Submission
            INNER JOIN 
                Assignment ON Submission.AssignmentId = Assignment.Id
            INNER JOIN 
                Course ON Assignment.CourseId = Course.Id
            INNER JOIN 
                Student ON Submission.StudentId = Student.Id;";

                // Izpilda SQL vaicājumu un iegūst rezultātu kā datu tabulu
                DataTable submissions = ExecuteQuery(selectQuery);

                // Apstrādā un formatē rezultātu
                string result = "Submissions:\n";
                foreach (DataRow row in submissions.Rows)
                {
                    result += $"Course: {row["CourseName"]}, Assignment: {row["AssignmentDescription"]}, " +
                              $"Student: {row["StudentName"]} {row["StudentSurname"]}, Submission Time: {row["SubmissionTime"]}, Score: {row["Score"]}\n";
                }

                return result; // Atgriež formatētu iesniegumu sarakstu
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}"; // Atgriež kļūdas ziņojumu, ja rodas problēma
            }
        }



        /// Iegūst visu iesniegumu sarakstu ar papildinformāciju par uzdevumiem un studentiem.
        public List<Submission> GetAllSubmissions()
        {
            List<Submission> submissions = new List<Submission>();

            // SQL vaicājums iesniegumu datu iegūšanai kopā ar uzdevumu aprakstiem un studentu pilnajiem vārdiem
            string query = @"
SELECT 
    Submission.Id, 
    Submission.AssignmentId, 
    Submission.StudentId, 
    Submission.SubmissionTime, 
    Submission.Score,
    Assignment.Description AS AssignmentDescription,
    CONCAT(Student.Name, ' ', Student.Surname) AS StudentFullName
FROM 
    Submission
LEFT JOIN 
    Assignment ON Submission.AssignmentId = Assignment.Id
LEFT JOIN 
    Student ON Submission.StudentId = Student.Id";

            try
            {
                // Izpilda SQL vaicājumu un iegūst rezultātu kā datu tabulu
                DataTable dataTable = ExecuteQuery(query);

                // Iterē cauri datu rindiņām un pievieno tās sarakstam
                foreach (DataRow row in dataTable.Rows)
                {
                    submissions.Add(new Submission
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        AssignmentId = Convert.ToInt32(row["AssignmentId"]),
                        StudentId = Convert.ToInt32(row["StudentId"]),
                        SubmissionTime = Convert.ToDateTime(row["SubmissionTime"]),
                        Score = Convert.ToDecimal(row["Score"]),
                        // Pārbauda, vai ir pieejama informācija par uzdevumu
                        AssignmentDescription = row.Table.Columns.Contains("AssignmentDescription") && row["AssignmentDescription"] != DBNull.Value
                            ? row["AssignmentDescription"].ToString()
                            : "No Description",
                        // Pārbauda, vai ir pieejama informācija par studentu
                        StudentFullName = row.Table.Columns.Contains("StudentFullName") && row["StudentFullName"] != DBNull.Value
                            ? row["StudentFullName"].ToString()
                            : "Unknown Student"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving submissions: {ex.Message}");
                throw;
            }

            return submissions; // Atgriež iesniegumu sarakstu
        }



    }
}
