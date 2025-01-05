namespace MD3
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime ContractDate { get; set; }

        public string DisplayInfo => $"{Name} {Surname}, Contract: {ContractDate.ToShortDateString()}";
    }
}

