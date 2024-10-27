using System; 
// 4.punkts
namespace project {
    public class Course {
        public string Name { get; set; } // kursa varda ipašiba
        public Teacher Teacher { get; set; } // pasniedzeja ipašiba

        public override string ToString() {
            return $"Course Name: {Name}, Teacher: {Teacher?.FullName}";
        }
    }
}
