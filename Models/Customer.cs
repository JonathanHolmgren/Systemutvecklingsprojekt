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
        public string Streetname { get; set; }
        public PostalCodeCity PostalCodeCity { get; set; }
    }
}
