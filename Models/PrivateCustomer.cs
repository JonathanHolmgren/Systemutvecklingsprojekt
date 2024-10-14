using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PrivateCustomer:Customer
    {
        public string SSN { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkTelephoneNumber { get; set; }

        public PrivateCustomer(string ssn, string firstName, string lastName, string worktelephoneNumber, string telephoneNumber, string email, string streetAdress, PostalCodeCity postalCodeCity) : base(telephoneNumber, email, streetAdress, postalCodeCity)
        {
            this.SSN = ssn;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.WorkTelephoneNumber = worktelephoneNumber;
        }
    }
}
