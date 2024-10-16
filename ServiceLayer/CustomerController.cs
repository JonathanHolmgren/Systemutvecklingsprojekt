using DataLayer;
using DataLayer.Repositories;
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
                // Kasta vidare undantaget så att det kan hanteras i UI-lagret
                throw new Exception($"Ett fel uppstod vid sparandet av kunden: {ex.Message}");
            }
        }

    }
}
