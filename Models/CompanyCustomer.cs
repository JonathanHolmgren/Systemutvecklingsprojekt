using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompanyCustomer:Customer
    {
        public string OrganisationNumber { get; set; } 
        public string ContactPersonName { get; set; }
        public string CompanyPersonTelephoneNumber { get; set; }
        public string CompanyName { get; set; }
    }
}
