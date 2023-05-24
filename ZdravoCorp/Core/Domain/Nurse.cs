namespace ZdravoCorp.Core.Domain
{
    public class Nurse : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Nurse() : base() { }
        public Nurse(int id, string firstName, string lastName, string username, string password, string type) : base(username, password, type)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", FirstName: " + FirstName + ", LastName: " + LastName;
        }
    }
}
