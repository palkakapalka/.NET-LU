namespace MD3
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; } // Foreign key to Assignment
        public int StudentId { get; set; } // Foreign key to Student
        public DateTime SubmissionTime { get; set; }
        public decimal Score { get; set; }
    }
}
