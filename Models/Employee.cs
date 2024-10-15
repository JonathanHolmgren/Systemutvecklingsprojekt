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

        // Navigation property
        public PostalCodeCity PostalCodeCity { get; set; }

        //    public ICollection<User> Users { get; set; } = new List<User>();
        public Commission? Commission { get; set; }

        public Employee() { }

        public Employee(
            string agentNumber,
            string ssn,
            string firstName,
            string lastName,
            string streetName,
            PostalCodeCity postalCodeCity,
            string email,
            string role,
            string phoneNumber
        )
        {
            AgentNumber = agentNumber;
            SSN = ssn;
            FirstName = firstName;
            LastName = lastName;
            StreetName = streetName;
            Email = email;
            Role = role;
            PhoneNumber = phoneNumber;
            PostalCodeCity = postalCodeCity;
        }

        public Employee(
            string agentNumber,
            string ssn,
            string firstName,
            string lastName,
            string streetName,
            PostalCodeCity postalCodeCity,
            string email,
            string role,
            string phoneNumber,
            Commission commission
        )
        {
            AgentNumber = agentNumber;
            SSN = ssn;
            FirstName = firstName;
            LastName = lastName;
            StreetName = streetName;
            Email = email;
            Role = role;
            PhoneNumber = phoneNumber;
            PostalCodeCity = postalCodeCity;
            Commission = commission;
        }
    }
}
