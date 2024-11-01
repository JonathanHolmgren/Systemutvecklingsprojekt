using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PrivateCustomer : Customer
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? WorkTelephoneNumber { get; set; }

        //Constructors
        public PrivateCustomer() { }

        public PrivateCustomer(
            string telephoneNumber,
            string email,
            string streetAddress,
            string postalCode,
            string city,
            string sSN,
            string firstName,
            string lastName,
            string workTelephoneNumber
        )
            : base(telephoneNumber, email, streetAddress, postalCode, city)
        {
            SSN = sSN;
            FirstName = firstName;
            LastName = lastName;
            WorkTelephoneNumber = workTelephoneNumber;
        }

        public PrivateCustomer(
            string telephoneNumber,
            string email,
            string streetAddress,
            string postalCode,
            string city,
            string sSN,
            string firstName,
            string lastName
        )
            : base(telephoneNumber, email, streetAddress, postalCode, city)
        {
            SSN = sSN;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
