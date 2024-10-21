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

        //public void RemoveInactiveCustomers()
        //{
        //    var inactiveCustomers = unitOfWork.CustomerRepository.GetInActiveCustomersWithInsurances();

        //    foreach (var customer in inactiveCustomers)
        //    {
        //        foreach (var insurance in customer.Insurances)
        //        {
        //            unitOfWork.InsuranceRepository.Remove(insurance);
        //        }

        //        unitOfWork.CustomerRepository.Remove(customer); 
        //    }

        //    unitOfWork.SaveChanges(); 
        //}

        public void RemoveInactiveCustomers()
        {
            var inactiveCustomers = unitOfWork.CustomerRepository.GetInActiveCustomersWithInsurances();

            foreach (var customer in inactiveCustomers)
            {
                foreach (var insurance in customer.Insurances)
                {
                    // Hämta InsuranceSpecs kopplade till varje insurance och ta bort dem
                    var insuranceSpecs = unitOfWork.InsuranceSpecRepository
                        .GetInsuranceSpecsByInsuranceId(insurance.InsuranceId);

                    foreach (var spec in insuranceSpecs)
                    {
                        unitOfWork.InsuranceSpecRepository.Remove(spec);
                    }

                    // Ta bort försäkringen
                    unitOfWork.InsuranceRepository.Remove(insurance);
                }

                // Ta bort kunden
                unitOfWork.CustomerRepository.Remove(customer);
            }

            // Spara ändringar
            unitOfWork.SaveChanges();
        }

    }
}
