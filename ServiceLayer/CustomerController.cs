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
        public void AddProspectNote(ProspectNote prospectNote)
        {
            unitOfWork.ProspectNoteRepository.Add(prospectNote);
            unitOfWork.SaveChanges();
        }

        public IList<PrivateCustomer> GetPrivateCustomerList()
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomers();
        }
        public IList<CompanyCustomer> GetCompanyCustomerList()
        {
            return unitOfWork.CustomerRepository.GetCompanyCustomers();
        }
        public PrivateCustomer GetSpecificPrivateCustomer(string sSN)
        {
            PrivateCustomer privateCustomer = unitOfWork.CustomerRepository.GetSpecificPrivateCustomer(sSN);

            return privateCustomer;
        }
        public CompanyCustomer GetSpecificCompanyCustomer(string organisationNumber)
        {
            CompanyCustomer companyCustomer = unitOfWork.CustomerRepository.GetSpecificCompanyCustomer(organisationNumber);

            return companyCustomer;
        }
    }
}
