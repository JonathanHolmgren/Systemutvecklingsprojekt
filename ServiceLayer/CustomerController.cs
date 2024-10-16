using DataLayer;
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

        public void AddCustomer(CompanyCustomer customer)
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
    }


}
