using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }

        // Navigation property
        public ICollection<Insurance> Insurances { get; set; }
        public PostalCodeCity PostalCodeCity { get; set; }

        //Constructors
        public Customer() { }

        public Customer(
            string telephoneNumber,
            string email,
            string streetName,
            PostalCodeCity postalCodeCity
        )
        {
            TelephoneNumber = telephoneNumber;
            Email = email;
            StreetName = streetName;
            PostalCodeCity = postalCodeCity;
        }
    }
}
