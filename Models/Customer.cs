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
        public string StreetAdress { get; set; }
        public PostalCodeCity PostalCodeCity { get; set; }

        public Customer(string telephoneNumber, string email, string streetAdress, PostalCodeCity postalCodeCity)
        {
            this.TelephoneNumber = telephoneNumber;
            this.Email = email;
            this.StreetAdress = streetAdress;
            this.PostalCodeCity = postalCodeCity;
        }
    }
}
