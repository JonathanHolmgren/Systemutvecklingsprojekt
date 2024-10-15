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

        public CompanyCustomer()
        {
                
        }
        public CompanyCustomer(string organisationNumber, string contactPersonName,string companyPersonTelephoneNumber, string companyName, string telephoneNumber,string email,string streetAdress,PostalCodeCity postalCodeCity):base (telephoneNumber,email,streetAdress,postalCodeCity)
        {
            this.OrganisationNumber=organisationNumber;
            this.ContactPersonName=contactPersonName;
            this.CompanyPersonTelephoneNumber=companyPersonTelephoneNumber;
            this.CompanyName=companyName;
        }
    }
}
