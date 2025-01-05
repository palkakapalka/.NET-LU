namespace MD3
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; } // This will be a foreign key linking to Teacher

        public string DisplayInfo => $"{Name}";
    }
}
