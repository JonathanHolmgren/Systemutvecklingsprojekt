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
        public string PostalCode { get; set; }
        public string City { get; set; }

        // Navigation property

        //    public ICollection<User> Users { get; set; } = new List<User>();
        public Commission? Commission { get; set; }

        public Employee() { }

        public Employee(
            string agentNumber,
            string ssn,
            string firstName,
            string lastName,
            string streetName,
            string postalCode,
            string city,
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
            PostalCode = postalCode;
            City = city;
        }

        public Employee(
            string agentNumber,
            string ssn,
            string firstName,
            string lastName,
            string streetName,
            string postalCode,
            string city,
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
            PostalCode = postalCode;
            City = city;
            Commission = commission;
        }
    }
}
