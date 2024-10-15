using DataLayer;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CustomerController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public void AddCustomer(Customer customer)
        {
            PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(customer.PostalCodeCity.PostalCode);
            if (existingPostalCodeCity == null )
            {
                unitOfWork.PostalCodeCityRepository.Add(customer.PostalCodeCity);
            }
            else 
            {
                customer.PostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(customer.PostalCodeCity.PostalCode);
            }
            unitOfWork.CustomerRepository.Add(customer);
            unitOfWork.SaveChanges();
        }
        
    }
}
