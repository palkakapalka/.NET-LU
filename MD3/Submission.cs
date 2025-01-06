namespace MD3
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; } // Foreign key to Assignment
        public int StudentId { get; set; } // Foreign key to Student
        public DateTime SubmissionTime { get; set; }
        public decimal Score { get; set; }



        // Papildu īpašības attēlošanai
        public string AssignmentDescription { get; set; } // Uzdevuma apraksts
        public string StudentFullName { get; set; } // Studenta vārds un uzvārds

        // DisplayInfo uzdevuma un studenta attēlošanai
        public string DisplayInfo => $"{AssignmentDescription}, {StudentFullName}";

    }
}
