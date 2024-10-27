using System; 
// 5. punkts
namespace project {

    public class Assignment {

        public DateTime Deadline { get; set; } // deadline ipašiba 
        public Course Course { get; set; } // kursa ipašiba
        public string Description { get; set; } // apraksta ipašiba

        public override string ToString() {

            return $"Deadline: {Deadline}, Course: {Course?.Name}, Description: {Description}";
        }
    }
}
