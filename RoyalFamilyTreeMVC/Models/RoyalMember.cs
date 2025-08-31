namespace RoyalFamily.Models
{
    public class RoyalMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public bool IsAlive { get; set; } = true;

        public RoyalMember() { }

        public RoyalMember(string name, DateOnly dob, bool isAlive = true)
        {
            Name = name; DateOfBirth = dob; IsAlive = isAlive;
        }

        public override string ToString() =>
            $"{Name} ({DateOfBirth:yyyy-MM-dd}){(IsAlive ? string.Empty : " †")}";
    }
}
