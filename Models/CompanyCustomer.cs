using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompanyCustomer : Customer
    {
        public string OrganisationNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string CompanyPersonTelephoneNumber { get; set; }
        public string CompanyName { get; set; }

        //Constructors
        public CompanyCustomer() { }

        public CompanyCustomer(
            string telephoneNumber,
            string email,
            string streetAddress,
            string postalCode,
            string city,
            string organisationNumber,
            string contactPersonName,
            string companyPersonTelephoneNumber,
            string companyName
        )
            : base(telephoneNumber, email, streetAddress, postalCode, city)
        {
            OrganisationNumber = organisationNumber;
            ContactPersonName = contactPersonName;
            CompanyPersonTelephoneNumber = companyPersonTelephoneNumber;
            CompanyName = companyName;
        }
    }
}
