namespace Models
{
    public class Employee
    {
        public string AgentNumber { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public PostalCodeCity PostalCodeCity { get; set; }
        public  IList <User> Users { get; set; }
        public CommisionRate CommisionRate { get; set; }
    }
}
