using System; 
// 6.punkts
namespace project {

    public class Submission {

        public Assignment Assignment { get; set; } // uzdevumu ipašiba
        public Student Student { get; set; } // studenta ipašiba
        public DateTime SubmissionTime { get; set; } // nodevumu ipašiba
        public int Score { get; set; } // atzimes ipasiba
    }
}
