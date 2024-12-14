// File: DatabaseManager.cs
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MD3
{
    public class DatabaseManager
    {
        private string _connectionString;

        public DatabaseManager()
        {
            _connectionString = ConnectionStringReader.GetConnectionString();
        }

        // CRUD Operations for Teacher
        public void AddTeacher(Teacher teacher)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("INSERT INTO Teacher (Name, Surname, Gender, ContractDate) VALUES (@Name, @Surname, @Gender, @ContractDate)", connection);
                    command.Parameters.AddWithValue("@Name", teacher.Name);
                    command.Parameters.AddWithValue("@Surname", teacher.Surname);
                    command.Parameters.AddWithValue("@Gender", teacher.Gender);
                    command.Parameters.AddWithValue("@ContractDate", teacher.ContractDate);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                if (ex.Number == -1) // Connection loss check
                {
                    Console.WriteLine("Lost connection to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // CRUD Operations for Course
        public void AddCourse(Course course)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("INSERT INTO Course (Name, TeacherId) VALUES (@Name, @TeacherId)", connection);
                    command.Parameters.AddWithValue("@Name", course.Name);
                    command.Parameters.AddWithValue("@TeacherId", course.TeacherId);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                if (ex.Number == -1) // Connection loss check
                {
                    Console.WriteLine("Lost connection to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // CRUD Operations for Assignment
        public void AddAssignment(Assignment assignment)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("INSERT INTO Assignment (Deadline, CourseId, Description) VALUES (@Deadline, @CourseId, @Description)", connection);
                    command.Parameters.AddWithValue("@Deadline", assignment.Deadline);
                    command.Parameters.AddWithValue("@CourseId", assignment.CourseId);
                    command.Parameters.AddWithValue("@Description", assignment.Description);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                if (ex.Number == -1) // Connection loss check
                {
                    Console.WriteLine("Lost connection to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // CRUD Operations for Student
        public void AddStudent(Student student)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("INSERT INTO Student (Name, Surname, Gender, StudentIdNumber) VALUES (@Name, @Surname, @Gender, @StudentIdNumber)", connection);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Surname", student.Surname);
                    command.Parameters.AddWithValue("@Gender", student.Gender);
                    command.Parameters.AddWithValue("@StudentIdNumber", student.StudentIdNumber);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                if (ex.Number == -1) // Connection loss check
                {
                    Console.WriteLine("Lost connection to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // CRUD Operations for Submission
        public void AddSubmission(Submission submission)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("INSERT INTO Submission (AssignmentId, StudentId, SubmissionTime, Score) VALUES (@AssignmentId, @StudentId, @SubmissionTime, @Score)", connection);
                    command.Parameters.AddWithValue("@AssignmentId", submission.AssignmentId);
                    command.Parameters.AddWithValue("@StudentId", submission.StudentId);
                    command.Parameters.AddWithValue("@SubmissionTime", submission.SubmissionTime);
                    command.Parameters.AddWithValue("@Score", submission.Score);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                if (ex.Number == -1) // Connection loss check
                {
                    Console.WriteLine("Lost connection to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
