using DataLayer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Newtonsoft.Json;
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
        public void AddCompanyCustomer(CompanyCustomer companyCustomer)
        {
            try
            {
                PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(companyCustomer.PostalCodeCity.PostalCode);
                if(companyCustomer.OrganisationNumber==unitOfWork.CustomerRepository.GetSpecificCompanyCustomer(companyCustomer.CustomerID).OrganisationNumber)
                {
                    throw new Exception("Organisationsnummer finns redan");
                }

                if (existingPostalCodeCity == null)
                {
                    unitOfWork.PostalCodeCityRepository.Add(companyCustomer.PostalCodeCity);
                }
                else
                {
                    companyCustomer.PostalCodeCity = existingPostalCodeCity;
                }
                unitOfWork.CustomerRepository.Add(companyCustomer);
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
        public string GetCustomerInsuranceTypes(int insuranceId)
        {
            Insurance insurance = unitOfWork.InsuranceRepository.GetInsurance(insuranceId);
            return unitOfWork.InsuranceTypeRepository.GetCustomerInsuranceType(insurance.InsuranceType.InsuranceTypeId);
        }
       
        public double GetCustomerPremie(int customerId) //Calculates the total premie for each customer
        {
            double totalPremie = 0;
            IList<Insurance>customerInsurances=unitOfWork.InsuranceRepository.GetCustomerInsurances(customerId);
            foreach (Insurance insurance in customerInsurances)
            {
                if (insurance.InsuranceStatus == InsuranceStatus.Active)
                {
                    if (insurance.BillingingInterval == BillingInterval.Monthly)
                    {
                        totalPremie += CalculatePremiePerInsurance(insurance);
                        
                    }
                    if (insurance.BillingingInterval == BillingInterval.Quartly)
                    {
                        if ((DateTime.Now.Month - insurance.ExpiryDate.Month) % 3 == 0 &&
                             DateTime.Now >= insurance.ExpiryDate)
                        {
                            totalPremie += CalculatePremiePerInsurance(insurance);
                            
                        }

                    }
                    if (insurance.BillingingInterval == BillingInterval.HalfYear)
                    {
                        
                        if ((DateTime.Now.Month - insurance.ExpiryDate.Month) % 6 == 0 &&
                             DateTime.Now >= insurance.ExpiryDate)
                        {
                            totalPremie += CalculatePremiePerInsurance(insurance);
                            
                        }
                    }
                    if (insurance.BillingingInterval == BillingInterval.Yearly)
                    {
                        if ((DateTime.Now.Month == insurance.ExpiryDate.Month) &&
                             DateTime.Now >= insurance.ExpiryDate)
                        {
                            totalPremie += CalculatePremiePerInsurance(insurance);
                            
                        }
                    }
                }
            }
            return totalPremie;
        }
        public double CalculatePremiePerInsurance(Insurance insurance) //Calculates each individual insurance premie
        {
            
            double premie = 0;
            
            int insuranceTypeAttributeID = unitOfWork.InsuranceTypeAttributeRepository.GetPremieTypeAttributeId(insurance.InsuranceType.InsuranceTypeId);
            
            IList<InsuranceSpec> insuranceSpecs = unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insurance.InsuranceId);
            foreach (InsuranceSpec insuranceSpec in insuranceSpecs)
            {
                
                if (insuranceTypeAttributeID == insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId)
                {
                    premie = double.Parse(insuranceSpec.Value); 
                }

            }
            return premie;
        }
       public void ExportObjectToJson(object objects)
        {
            string jsonResult = JsonConvert.SerializeObject(objects, Formatting.Indented);
            string outputPath = @"C:\JsonTest\CustomerInformation.json";
            File.WriteAllText(outputPath, jsonResult);
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
    }
}
