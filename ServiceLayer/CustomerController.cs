using DataLayer;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public void AddCompanyCustomer(CompanyCustomer companyCustomer)
        {
            try
            {
                PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(companyCustomer.PostalCodeCity.PostalCode);

                if (existingPostalCodeCity == null)
                {
                    unitOfWork.PostalCodeCityRepository.Add(companyCustomer.PostalCodeCity);
                }
                else
                {
                    companyCustomer.PostalCodeCity = existingPostalCodeCity;
                }
                unitOfWork.CompanyCustomerRepository.Add(companyCustomer);
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ett fel uppstod vid sparandet av kunden: {ex.Message}");
            }
        }
        public IList<PrivateCustomer>GetAllPrivateCustomers()
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomers();
        }
        public IList<CompanyCustomer> GetAllCompanyCustomers()
        {
            return unitOfWork.CustomerRepository.GetCompanyCustomers();
        }
        public CompanyCustomer GetSpecificCompanyCustomer(int customerId)
        {
            return unitOfWork.CustomerRepository.GetSpecificCompanyCustomer(customerId);
        }
        public PrivateCustomer GetSpecificPrivateCustomer(int customerId)
        {
            return unitOfWork.CustomerRepository.GetSpecificPrivateCustomer(customerId);
        }
        public string GetCustomerInsuranceTypes(int insuranceTypeId)
        {
            return unitOfWork.InsuranceTypeRepository.GetCustomerInsuranceType(insuranceTypeId);
        }
        public double GetCustomerTotalPremie(int customerId)
        {
            double totalPremie = 0;
            IList<Insurance>customerInsurances=unitOfWork.InsuranceRepository.GetCustomerInsurances(customerId);
            foreach (Insurance insurance in customerInsurances)
            {
                int insuranceTypeAttributeID = unitOfWork.InsuranceTypeAttributeRepository.GetPremieTypeAttributeId(insurance.InsuranceType.InsuranceTypeId);
                IList<InsuranceSpec> insuranceSpecs = unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insurance.InsuranceId);
                foreach (InsuranceSpec insuranceSpec in insuranceSpecs)
                {
                    if (insuranceTypeAttributeID == insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId)
                    {
                        
                        if (double.TryParse(insuranceSpec.Value, out double premiumValue))
                        {
                            totalPremie += premiumValue;
                        }
                    }

                }
            }
            Debug.WriteLine(totalPremie);
            return totalPremie;
        }
        
    }
}
