using DataLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CustomerController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public void RegisterPrivateCustomer(string ssn, string firstName, string lastName, string telephoneNumber, string workTelephoneNumber, string email, string streetAdress, PostalCodeCity postalCodeCity)
        {
            PrivateCustomer privateCustomer = new PrivateCustomer(ssn, firstName, lastName, workTelephoneNumber, telephoneNumber, email, streetAdress, postalCodeCity);

            unitOfWork.PrivateCustomerRepository.Add(privateCustomer);
            unitOfWork.SaveChanges();
        }
    }
}
