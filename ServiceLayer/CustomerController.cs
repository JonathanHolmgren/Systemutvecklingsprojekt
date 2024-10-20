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
        public void AddPrivateCustomer(PrivateCustomer privateCustomer)
        {
            try
            {
                PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(privateCustomer.PostalCodeCity.PostalCode);

                if (existingPostalCodeCity == null)
                {
                    unitOfWork.PrivateCustomerRepository.Add(privateCustomer);
                }
                else
                {
                    privateCustomer.PostalCodeCity = existingPostalCodeCity;
                }
                unitOfWork.PrivateCustomerRepository.Add(privateCustomer);
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ett fel uppstod vid sparandet av kunden: {ex.Message}");
            }
        }
        public void AddPrivateProspectNote(ProspectNote prospectNote, PrivateCustomer privatecustomer)
        {
            
        }

        public IList<PrivateCustomer> GetPrivateCustomerList()
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomers();
        }
        public IList<CompanyCustomer> GetCompanyCustomerList()
        {
            return unitOfWork.CustomerRepository.GetCompanyCustomers();
        }
    }
}
