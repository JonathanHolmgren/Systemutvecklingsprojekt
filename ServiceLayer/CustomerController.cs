using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Models;

namespace ServiceLayer
{
    public class CustomerController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public void AddCustomer(Customer customer)
        {
            try
            {
                PostalCodeCity existingPostalCodeCity =
                    unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(
                        customer.PostalCodeCity.PostalCode
                    );

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

        public IList<PrivateCustomer> GetPrivateCustomerList()
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomers();
        }

        public IList<CompanyCustomer> GetCompanyCustomerList()
        {
            return unitOfWork.CustomerRepository.GetCompanyCustomers();
        }

        public void RemovePrivateCustomer(PrivateCustomer privateCustomer)
        {
            unitOfWork.CustomerRepository.Remove(privateCustomer);
            unitOfWork.SaveChanges();
        }

        public void RemoveCompanyCustomer(CompanyCustomer companyCustomer)
        {
            unitOfWork.CustomerRepository.Remove(companyCustomer);
            unitOfWork.SaveChanges();
        }

        public void RemovePrivateCustomerAndInactiveInsurances(PrivateCustomer privateCustomer)
        {
            List<int> insuranceIds = new List<int>();
            IList<InsuranceSpec> insuranceSpecs = new List<InsuranceSpec>();

            foreach (Insurance insurance in privateCustomer.Insurances)
            {
                insuranceIds.Add(insurance.InsuranceId);
            }

            foreach (int insuranceId in insuranceIds)
            {
                IList<InsuranceSpec> temporaryInsuranceSpecs =
                    unitOfWork.InsuranceSpecRepository.GetAllInsuranceSpecsForInsurance(
                        insuranceId
                    );

                for (int i = 0; i < temporaryInsuranceSpecs.Count; i++)
                {
                    insuranceSpecs.Add(temporaryInsuranceSpecs[i]);
                }
            }

            RemoveAllInsuranceSpecsAndInsuranceTypeAttributes(insuranceSpecs);
            RemoveAllPrivateCustomerInsurances(privateCustomer);
            unitOfWork.CustomerRepository.Remove(privateCustomer);
            unitOfWork.SaveChanges();
        }

        public void RemoveCompanyCustomerAndInactiveInsurances(CompanyCustomer companyCustomer)
        {
            List<int> insuranceIds = new List<int>();
            IList<InsuranceSpec> insuranceSpecs = new List<InsuranceSpec>();

            foreach (Insurance insurance in companyCustomer.Insurances)
            {
                insuranceIds.Add(insurance.InsuranceId);
            }

            foreach (int insuranceId in insuranceIds)
            {
                IList<InsuranceSpec> temporaryInsuranceSpecs =
                    unitOfWork.InsuranceSpecRepository.GetAllInsuranceSpecsForInsurance(
                        insuranceId
                    );

                for (int i = 0; i < temporaryInsuranceSpecs.Count; i++)
                {
                    insuranceSpecs.Add(temporaryInsuranceSpecs[i]);
                }
            }

            RemoveAllInsuranceSpecsAndInsuranceTypeAttributes(insuranceSpecs);
            RemoveAllCompanyCustomerInsurances(companyCustomer);
            unitOfWork.CustomerRepository.Remove(companyCustomer);
            unitOfWork.SaveChanges();
        }

        public void RemoveAllInsuranceSpecsAndInsuranceTypeAttributes(
            IList<InsuranceSpec> insuranceSpecs
        )
        {
            foreach (InsuranceSpec insuranceSpec in insuranceSpecs)
            {
                unitOfWork.InsuranceSpecRepository.Remove(insuranceSpec);
                unitOfWork.InsuranceTypeAttributeRepository.Remove(
                    insuranceSpec.InsuranceTypeAttribute
                );
            }
        }

        public void RemoveAllPrivateCustomerInsurances(PrivateCustomer privateCustomer)
        {
            foreach (Insurance insurance in privateCustomer.Insurances)
            {
                unitOfWork.InsuranceRepository.Remove(insurance);
            }
        }

        public void RemoveAllCompanyCustomerInsurances(CompanyCustomer companyCustomer)
        {
            foreach (Insurance insurance in companyCustomer.Insurances)
            {
                unitOfWork.InsuranceRepository.Remove(insurance);
            }
        }
    }
}
