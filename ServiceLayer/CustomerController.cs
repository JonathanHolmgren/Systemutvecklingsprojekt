using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Newtonsoft.Json;

namespace ServiceLayer
{
    public class CustomerController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public void AddPrivateCustomer(PrivateCustomer privateCustomer)
        {
            try
            {
                unitOfWork.CustomerRepository.Add(privateCustomer);
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Misslyckades att lägga till kund till databas.",
                    ex
                );
            }
        }

        public void UpdateCompanyCustomer(CompanyCustomer updatedCompanyCustomer)
        {
            unitOfWork.Update(updatedCompanyCustomer);
            unitOfWork.SaveChanges();
        }

        public void UpdatePrivateCustomer(PrivateCustomer updatedPrivateCustomer)
        {
            unitOfWork.Update(updatedPrivateCustomer);
            unitOfWork.SaveChanges();
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

        public PrivateCustomer? GetSpecificPrivateCustomerForInsuranceBySSN(string sSN)
        {
            return unitOfWork.CustomerRepository.GetSpecificPrivateCustomerForInsuranceBySSN(sSN);
        }

        public CompanyCustomer? GetSpecificCompanyCustomerForInsuranceByOrgNumber(string orgNr)
        {
            return unitOfWork.CustomerRepository.GetSpecificCompanyCustomerForInsuranceByOrgNumber(
                orgNr
            );
        }

        public void RemoveInactiveCustomers()
        {
            var inactiveCustomers =
                unitOfWork.CustomerRepository.GetInActiveCustomersWithInsurances();

            foreach (var customer in inactiveCustomers)
            {
                foreach (var insurance in customer.Insurances)
                {
                    var insuranceSpecs =
                        unitOfWork.InsuranceSpecRepository.GetInsuranceSpecsByInsuranceId(
                            insurance.InsuranceId
                        );
                    foreach (var spec in insuranceSpecs)
                    {
                        unitOfWork.InsuranceSpecRepository.Remove(spec);
                    }

                    unitOfWork.InsuranceRepository.Remove(insurance);
                }

                unitOfWork.CustomerRepository.Remove(customer);
            }

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
                    unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insuranceId);
                unitOfWork.InsuranceSpecRepository.GetInsuranceSpecsByInsuranceId(insuranceId);

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
                    unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insuranceId);
                unitOfWork.InsuranceSpecRepository.GetInsuranceSpecsByInsuranceId(insuranceId);

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

        public void RemoveAllInsuranceSpecsAndInsuranceTypeAttributes( //Denna jobbar vi med!!!!
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

        public void AddCompanyCustomer(CompanyCustomer companyCustomer)
        {
            try
            {
                var existingCompanyCustomer =
                    unitOfWork.CustomerRepository.GetSpecificCompanyCustomerForInsuranceByOrgNumber(
                        companyCustomer.OrganisationNumber
                    );

                if (existingCompanyCustomer != null)
                {
                    throw new Exception("Organisationsnummer finns redan.");
                }

                unitOfWork.CustomerRepository.Add(companyCustomer);
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ett fel uppstod vid sparandet av kunden: {ex.Message}");
            }
        }

        public IList<PrivateCustomer> GetAllPrivateCustomers()
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomers();
        }

        public IList<CompanyCustomer> GetAllCompanyCustomers()
        {
            return unitOfWork.CustomerRepository.GetCompanyCustomers();
        }

        public string? GetCustomerInsuranceTypes(int insuranceId)
        {
            Insurance insurance = unitOfWork.InsuranceRepository.GetInsurance(insuranceId);
            return unitOfWork.InsuranceTypeRepository.GetCustomerInsuranceType(
                insurance.InsuranceType.InsuranceTypeId
            );
        }

        public double GetCustomerPremie(int customerId) //Calculates the total premie for each customer
        {
            double totalPremie = 0;
            IList<Insurance> customerInsurances =
                unitOfWork.InsuranceRepository.GetCompanyCustomerInsurancesById(customerId);
            foreach (Insurance insurance in customerInsurances)
            {
                if (insurance.InsuranceStatus == InsuranceStatus.Active)
                {
                    if (insurance.BillingInterval == BillingInterval.Månad)
                    {
                        totalPremie += CalculatePremiePerInsurance(insurance);
                    }
                    if (insurance.BillingInterval == BillingInterval.Kvartal)
                    {
                        if (
                            (DateTime.Now.Month - insurance.ExpiryDate.Month) % 3 == 0
                            && DateTime.Now >= insurance.ExpiryDate
                        )
                        {
                            totalPremie += CalculatePremiePerInsurance(insurance);
                        }
                    }
                    if (insurance.BillingInterval == BillingInterval.Halvår)
                    {
                        if (
                            (DateTime.Now.Month - insurance.ExpiryDate.Month) % 6 == 0
                            && DateTime.Now >= insurance.ExpiryDate
                        )
                        {
                            totalPremie += CalculatePremiePerInsurance(insurance);
                        }
                    }
                    if (insurance.BillingInterval == BillingInterval.År)
                    {
                        if (
                            (DateTime.Now.Month == insurance.ExpiryDate.Month)
                            && DateTime.Now >= insurance.ExpiryDate
                        )
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

            int insuranceTypeAttributeID =
                unitOfWork.InsuranceTypeAttributeRepository.GetPremieTypeAttributeId(
                    insurance.InsuranceType.InsuranceTypeId
                );

            IList<InsuranceSpec> insuranceSpecs =
                unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insurance.InsuranceId);
            foreach (InsuranceSpec insuranceSpec in insuranceSpecs)
            {
                if (
                    insuranceTypeAttributeID
                    == insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId
                )
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
            unitOfWork.Update(prospectNote.User);
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

        public PrivateCustomer? GetOnePrivateCustomerBySsn(string ssn)
        {
            return unitOfWork.CustomerRepository.GetPrivateCustomerBySsn(ssn);
        }

        public CompanyCustomer? GetOneCompanyCustomerByOrgNr(string organisationNumber)
        {
            CompanyCustomer companyCustomer =
                unitOfWork.CustomerRepository.GetSpecificCompanyCustomerByOrgNr(organisationNumber);

            return companyCustomer;
        }
    }
}
