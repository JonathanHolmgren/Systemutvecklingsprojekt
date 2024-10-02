using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PrivateCustomer:Customer
    {
        public string SSN { get; set; } //Ska vi ta detta som ID?
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkTelephoneNumber { get; set; }
    }
}
