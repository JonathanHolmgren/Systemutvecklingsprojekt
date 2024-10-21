using DataLayer;
using DataLayer.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CustomerController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public void AddCustomer(Customer customer)
        {
            try
            {
                PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(customer.PostalCodeCity.PostalCode);

                if (existingPostalCodeCity == null)
                {
                    unitOfWork.PostalCodeCityRepository.Add(customer.PostalCodeCity);
                }
                else
                {
                    customer.PostalCodeCity = existingPostalCodeCity;
                }
                unitOfWork.CustomerRepository.Add(customer);
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ett fel uppstod vid sparandet av kunden: {ex.Message}");
            }
        }
        public PrivateCustomer GetSpecificPrivateCustomer(int customerId)
        {
            PrivateCustomer privateCustomer = unitOfWork.CustomerRepository.GetSpecificPrivateCustomer(customerId);

            return privateCustomer;
        }
        public CompanyCustomer GetSpecificCompanyCustomer(int customerId)
        {
            CompanyCustomer companyCustomer = unitOfWork.CustomerRepository.GetSpecificCompanyCustomer(customerId);

            return companyCustomer;
        }
    }
}
