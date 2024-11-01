using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IPrivateCustomer
    {
        public int CustomerID { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? WorkTelephoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
