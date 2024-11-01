﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace ServiceLayer
{
    public class InsuranceController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
        InsuranceTypeAttributeController insuranceTypeAttributeController =
            new InsuranceTypeAttributeController();

        public List<Insurance> GetAllPreliminaryInsurances()
          
        #region PrivateCustomer
        #region RegisterInsurance
        public Insurance RegisterPrivatePreliminaryInsurance(
            DateTime expiryDate,
            BillingInterval billingInterval,
            User user,
            InsuranceType insuranceType,
            string notes,
            Customer customer,
            InsuredPerson insuredPerson
        )
        {
            return new Insurance(
                expiryDate,
                billingInterval,
                user,
                insuranceType,
                notes,
                customer,
                insuredPerson
            );
        }

        public Insurance CreatePrivateInsuranceFromInput(
            int userId,
            string selectedInsuranceType,
            string insuredPersonFirstName,
            string insuredPersonLastName,
            string insuredPersonSSN,
            DateTime arrivingDate,
            BillingInterval selectedInterval,
            string notes,
            PrivateCustomer selectedPrivateCustomer,
            string selectedBasePrice,
            string totalPremium,
            string selectedAddOnBasePrice1,
            string selectedAddOnBasePrice2,
            string selectedAddOnOption1,
            string selectedAddOnOption2
        )
        {
            User user = GetUserByID(1); //denna ska hämta den användaren som är inloggad!
            InsuranceType insuranceType = GetInsuranceType(selectedInsuranceType);

            List<InsuranceTypeAttribute> insuranceTypeAttributesList =
                RegisterPrivateInsuranceTypeAttribute(
                    insuranceType,
                    selectedAddOnOption1,
                    selectedAddOnOption2
                );

            InsuredPerson insuredPerson = RegisterInsuredPerson(
                insuredPersonFirstName,
                insuredPersonLastName,
                insuredPersonSSN
            );
            Insurance newInsurance = RegisterPrivatePreliminaryInsurance(
                arrivingDate,
                selectedInterval,
                user,
                insuranceType,
                notes,
                selectedPrivateCustomer,
                insuredPerson
            );
            AddInsuranceForPrivateCustomer(newInsurance, selectedPrivateCustomer);
            CreatePrivateInsuranceSpecifications(
                newInsurance,
                insuranceTypeAttributesList,
                selectedBasePrice,
                totalPremium,
                selectedAddOnBasePrice1,
                selectedAddOnBasePrice2,
                arrivingDate
            );
            return newInsurance;
        }

        public void CreatePrivateInsuranceSpecifications(
            Insurance newInsurance,
            List<InsuranceTypeAttribute> insuranceTypeAttributesList,
            string selectedBasePrice,
            string totalPremium,
            string selectedAddOnBasePrice1,
            string selectedAddOnBasePrice2,
            DateTime arrivingDate
        )
        {
            return unitOfWork.InsuranceRepository.GetAllPreliminaryInsurances();
        }
        public IList<Insurance> GetCustomerInsurances(int customerId)
        {
            return unitOfWork.InsuranceRepository.GetCustomerInsurances(customerId);
        }

        public void SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.GetInsurance(
                selectedInsurance.InsuranceId
            );

            AddInsuranceSpec(insuranceSpec);
            return insuranceSpec;
        }

        private List<InsuranceTypeAttribute> RegisterPrivateInsuranceTypeAttribute(
            InsuranceType insuranceType,
            string selectedAddOnOption1,
            string selectedAddOnOption2
        )
        {
            var insuranceTypeAttributesList = new List<InsuranceTypeAttribute>();

            AddAttribute("Grundbelopp", insuranceType, insuranceTypeAttributesList);
            AddAttribute("Premie", insuranceType, insuranceTypeAttributesList);
            AddAttribute("Datum", insuranceType, insuranceTypeAttributesList);

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Active;

                unitOfWork.SaveChanges();
            }
        }

        public void SetInsuranceStatusToInactive(Insurance selectedInsurance)

        public void AddInsuranceForPrivateCustomer(
            Insurance insurance,
            PrivateCustomer privateCustomer
        )
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.GetInsurance(
                selectedInsurance.InsuranceId
            );

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Inactive;

                unitOfWork.SaveChanges();
            }

        public InsuredPerson RegisterInsuredPerson(string firstName, string lastName, string ssn)
        {
            return new InsuredPerson(firstName, lastName, ssn);
        }

        public void RemoveInsurance(Insurance selectedInsurance)
        {
            IList<InsuranceSpec> insuranceSpecsToRemove = unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(selectedInsurance.InsuranceId);

            foreach (InsuranceSpec insuranceSpec in insuranceSpecsToRemove)
            {
                unitOfWork.InsuranceSpecRepository.Remove(insuranceSpec.InsuranceSpecId);
                unitOfWork.InsuranceTypeAttributeRepository.Remove(insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId);
            }

            unitOfWork.InsuranceRepository.Remove(selectedInsurance);
            unitOfWork.SaveChanges();
        }
        #endregion

        #region CompanyCustomer
        public Insurance RegisterCompanyPreliminaryInsurance(
            DateTime activationDate,
            DateTime expiryDate,
            BillingInterval billingInterval,
            User user,
            Customer customer,
            InsuranceType insuranceType
        )
        {
            return new Insurance(
                activationDate,
                expiryDate,
                billingInterval,
                user,
                customer,
                insuranceType
            );
        }

        public Insurance CreatePropertyInsuranceFromInput(
            User loggedInUser,
            CompanyCustomer selectedCompanyCustomer,
            string selectedInsuranceType,
            string propertyAddress,
            string propertyValue,
            string inventoriesValue,
            string propertyPremie,
            string inventoriesPremie,
            DateTime activationDate,
            DateTime expiryDate,
            BillingInterval selectedInterval,
            string totalPremie
        )
        {
            User user = GetUserByID(1);
            InsuranceType insuranceType = GetInsuranceType(selectedInsuranceType);

            Insurance newInsurance = RegisterCompanyPreliminaryInsurance(
                activationDate,
                expiryDate,
                selectedInterval,
                user,
                selectedCompanyCustomer,
                insuranceType
            );
            AddInsuranceForCompanyCustomer(newInsurance, selectedCompanyCustomer);

            List<InsuranceTypeAttribute> insruanceTypeAttribute =
                RegisterCompanyInsuranceTypeAttribute(insuranceType);

            CreatePropertyInsuranceSpecifications(
                newInsurance,
                insruanceTypeAttribute,
                propertyAddress,
                propertyValue,
                inventoriesValue,
                propertyPremie,
                inventoriesPremie,
                activationDate,
                expiryDate,
                totalPremie
            );
            return newInsurance;
        }

        public Insurance CreateCarInsuranceFromInput(
            User loggedInUser,
            CompanyCustomer selectedCompanyCustomer,
            string selectedInsuranceType,
            string selectedCarInsuranceType,
            string selectedCarDeductible,
            string zone,
            string carPremie,
            DateTime activationDate,
            DateTime expiryDate,
            BillingInterval selectedInterval,
            string totalPremie
        )
        {
            User user = GetUserByID(1);
            InsuranceType insuranceType = GetInsuranceType(selectedInsuranceType);

            Insurance newInsurance = RegisterCompanyPreliminaryInsurance(
                activationDate,
                expiryDate,
                selectedInterval,
                user,
                selectedCompanyCustomer,
                insuranceType
            );
            AddInsuranceForCompanyCustomer(newInsurance, selectedCompanyCustomer);

            List<InsuranceTypeAttribute> insruanceTypeAttribute =
                RegisterCompanyInsuranceTypeAttribute(insuranceType);

            CreateCarInsuranceSpecifications(
                newInsurance,
                insruanceTypeAttribute,
                selectedCarInsuranceType,
                selectedCarDeductible,
                zone,
                carPremie,
                activationDate,
                expiryDate,
                totalPremie
            );
            return newInsurance;
        }

        public Insurance CreateLiabilityInsuranceFromInput(
            User loggedInUser,
            CompanyCustomer selectedCompanyCustomer,
            string selectedInsuranceType,
            string selectedLiabilityAmount,
            string selectedLiabilityDeductible,
            string liabilityPremie,
            DateTime activationDate,
            DateTime expiryDate,
            BillingInterval selectedInterval,
            string totalPremie
        )
        {
            User user = GetUserByID(1);
            InsuranceType insuranceType = GetInsuranceType(selectedInsuranceType);

            Insurance newInsurance = RegisterCompanyPreliminaryInsurance(
                activationDate,
                expiryDate,
                selectedInterval,
                user,
                selectedCompanyCustomer,
                insuranceType
            );
            AddInsuranceForCompanyCustomer(newInsurance, selectedCompanyCustomer);

            List<InsuranceTypeAttribute> insruanceTypeAttribute =
                RegisterCompanyInsuranceTypeAttribute(insuranceType);

            CreateLiabilityInsuranceSpecifications(
                newInsurance,
                insruanceTypeAttribute,
                selectedLiabilityAmount,
                selectedLiabilityDeductible,
                liabilityPremie,
                activationDate,
                expiryDate,
                totalPremie
            );
            return newInsurance;
        }

        private List<InsuranceTypeAttribute> RegisterCompanyInsuranceTypeAttribute(
            InsuranceType insuranceType
        )
        {
            var insuranceTypeAttributesList = new List<InsuranceTypeAttribute>();

            if (insuranceType.Type == "Fastighet och inventarieförsäkring")
            {
                AddAttribute("Fastighetsadress", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Värde fastigheter", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Värde inventarier", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Premie fastighet", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Premie inventarier", insuranceType, insuranceTypeAttributesList);
            }
            else if (insuranceType.Type == "Fordonsförsäkring")
            {
                AddAttribute("Omfattning", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Självrisk", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Zon", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Grundkostnad (månad)", insuranceType, insuranceTypeAttributesList);
            }
            else if (insuranceType.Type == "Ansvarsförsäkring")
            {
                AddAttribute("Belopp", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Självrisk", insuranceType, insuranceTypeAttributesList);
                AddAttribute("Grundkostnad (månad)", insuranceType, insuranceTypeAttributesList);
            }

            AddAttribute("Begynellsedatum", insuranceType, insuranceTypeAttributesList);
            AddAttribute("Förfallodatum", insuranceType, insuranceTypeAttributesList);
            AddAttribute("Total premie", insuranceType, insuranceTypeAttributesList);

            AddAllInsuranceTypeAttribute(insuranceTypeAttributesList);
            return insuranceTypeAttributesList;
        }

        public void CreatePropertyInsuranceSpecifications(
            Insurance newInsurance,
            List<InsuranceTypeAttribute> insuranceTypeAttributesList,
            string propertyAddress,
            string propertyValue,
            string inventoriesValue,
            string propertyPremie,
            string inventoryPremie,
            DateTime activationDate,
            DateTime expiryDate,
            string totaltPremie
        )
        {
            foreach (InsuranceTypeAttribute insuranceTypeItem in insuranceTypeAttributesList)
            {
                if (insuranceTypeItem.InsuranceAttribute == "Fastighetsadress")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, propertyAddress);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Värde fastigheter")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, propertyValue);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Värde inventarier")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, inventoriesValue);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Premie fastighet")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, propertyPremie);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Premie inventarier")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, inventoryPremie);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Begynellsedatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        activationDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Förfallodatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        expiryDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Total premie")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, totaltPremie);
                }
            }
        }

        public void CreateCarInsuranceSpecifications(
            Insurance newInsurance,
            List<InsuranceTypeAttribute> insuranceTypeAttributesList,
            string selectedCarInsuranceType,
            string selectedCarDeductible,
            string zone,
            string carPremie,
            DateTime activationDate,
            DateTime expiryDate,
            string totaltPremie
        )
        {
            foreach (InsuranceTypeAttribute insuranceTypeItem in insuranceTypeAttributesList)
            {
                if (insuranceTypeItem.InsuranceAttribute == "Omfattning")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        selectedCarInsuranceType
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Självrisk")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, selectedCarDeductible);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Zon")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, zone);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Grundkostnad (månad)")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, carPremie);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Begynellsedatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        activationDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Förfallodatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        expiryDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Total premie")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, totaltPremie);
                }
            }
        }

        public void CreateLiabilityInsuranceSpecifications(
            Insurance newInsurance,
            List<InsuranceTypeAttribute> insuranceTypeAttributesList,
            string selectedLiabilityAmount,
            string selectedLiabilityDeductible,
            string liabilityPremie,
            DateTime activationDate,
            DateTime expiryDate,
            string totaltPremie
        )
        {
            foreach (InsuranceTypeAttribute insuranceTypeItem in insuranceTypeAttributesList)
            {
                if (insuranceTypeItem.InsuranceAttribute == "Belopp")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, selectedLiabilityAmount);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Självrisk")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        selectedLiabilityDeductible
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Grundkostnad (månad)")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, liabilityPremie);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Begynellsedatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        activationDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Förfallodatum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        expiryDate.ToString("yyyy-MM-dd")
                    );
                }
                if (insuranceTypeItem.InsuranceAttribute == "Total premie")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, totaltPremie);
                }
            }
        }

        public void AddInsuranceForCompanyCustomer(
            Insurance insurance,
            CompanyCustomer companyCustomer
        )
        {
            unitOfWork.InsuranceRepository.Add(insurance);
            unitOfWork.Update(companyCustomer.PostalCodeCity);
            unitOfWork.Update(companyCustomer);
            unitOfWork.Update(insurance.InsuranceType);
            unitOfWork.SaveChanges();
        }
        #endregion
    }
}