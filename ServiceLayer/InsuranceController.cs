using System;
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
        InsuranceSpecController insuranceSpecController = new InsuranceSpecController();

        InsuranceTypeAttributeController insuranceTypeAttributeController =
            new InsuranceTypeAttributeController();

        UnitOfWork unitOfWork = new UnitOfWork();

        // public List<Insurance> GetAllPreliminaryInsurances()


        public List<Insurance> GetCompanyCustomerInsurancesByCustomerId(int customerId)
        {
            return unitOfWork.InsuranceRepository.GetCompanyCustomerInsurancesById(customerId);
        }

        public List<Insurance> GetPrivateCustomerInsurancesByCustomerId(int customerId)
        {
            return unitOfWork.InsuranceRepository.GetPrivateCustomerInsurancesById(customerId);
        }

        public void SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.GetInsurance(
                selectedInsurance.InsuranceId
            );

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Active;

                unitOfWork.SaveChanges();
            }
        }

        public void SetInsuranceStatusToInactive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.GetInsurance(
                selectedInsurance.InsuranceId
            );

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Inactive;

                unitOfWork.SaveChanges();
            }
        }

        public InsuredPerson RegisterInsuredPerson(string firstName, string lastName, string ssn)
        {
            return new InsuredPerson(firstName, lastName, ssn);
        }

        public void RemoveInsurance(Insurance selectedInsurance)
        {
            IList<InsuranceSpec> insuranceSpecsToRemove =
                unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(
                    selectedInsurance.InsuranceId
                );

            foreach (InsuranceSpec insuranceSpec in insuranceSpecsToRemove)
            {
                unitOfWork.InsuranceSpecRepository.Remove(insuranceSpec.InsuranceSpecId);
                unitOfWork.InsuranceTypeAttributeRepository.Remove(
                    insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId
                );
            }

            unitOfWork.InsuranceRepository.Remove(selectedInsurance);
            unitOfWork.SaveChanges();
        }

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
            LoggedInUser loggedInUser,
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
            User user = GetUserByID(loggedInUser.UserID);
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
            LoggedInUser loggedInUser,
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
            User user = GetUserByID(loggedInUser.UserID);
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
            LoggedInUser loggedInUser,
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
            User user = GetUserByID(loggedInUser.UserID);
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

        private void AddInsuranceForCompanyCustomer(
            Insurance insurance,
            CompanyCustomer companyCustomer
        )
        {
            insurance.IsPrivateCustomer = false;
            unitOfWork.InsuranceRepository.Add(insurance);
            unitOfWork.Update(companyCustomer);
            unitOfWork.Update(insurance.InsuranceType);
            unitOfWork.SaveChanges();
        }

        private Insurance RegisterPrivatePreliminaryInsurance(
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
            User user = GetUserByID(userId); //denna ska hämta den användaren som är inloggad!
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

        private void CreatePrivateInsuranceSpecifications(
            Insurance newInsurance,
            List<InsuranceTypeAttribute> insuranceTypeAttributesList,
            string selectedBasePrice,
            string totalPremium,
            string selectedAddOnBasePrice1,
            string selectedAddOnBasePrice2,
            DateTime arrivingDate
        )
        {
            foreach (InsuranceTypeAttribute insuranceTypeItem in insuranceTypeAttributesList)
            {
                if (insuranceTypeItem.InsuranceAttribute == "Grundbelopp")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, selectedBasePrice);
                }

                if (insuranceTypeItem.InsuranceAttribute == "Premie")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, totalPremium);
                }

                if (insuranceTypeItem.InsuranceAttribute == "Datum")
                {
                    RegisterInsuranceSpec(
                        insuranceTypeItem,
                        newInsurance,
                        arrivingDate.ToString("yyyy-MM-dd")
                    );
                }

                if (insuranceTypeItem.InsuranceAttribute == "Invaliditet vid olycksfall")
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, selectedAddOnBasePrice1);
                }

                if (
                    insuranceTypeItem.InsuranceAttribute
                    == "Månadsersättning vid långvarig sjukskrivning"
                )
                {
                    RegisterInsuranceSpec(insuranceTypeItem, newInsurance, selectedAddOnBasePrice2);
                }
            }
        }

        public InsuranceType GetInsuranceType(string inputInsuranceType)
        {
            return unitOfWork.InsuranceTypeRepository.GetInsuranceType(inputInsuranceType);
        }

        public InsuranceSpec RegisterInsuranceSpec(
            InsuranceTypeAttribute insuranceTypeAttribute,
            Insurance insurance,
            string value
        )
        {
            InsuranceSpec insuranceSpec = new InsuranceSpec(
                value,
                insurance,
                insuranceTypeAttribute
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

            if (selectedAddOnOption1 != null)
            {
                AddAttribute(selectedAddOnOption1, insuranceType, insuranceTypeAttributesList);
            }

            if (selectedAddOnOption2 != null)
            {
                AddAttribute(selectedAddOnOption2, insuranceType, insuranceTypeAttributesList);
            }

            AddAllInsuranceTypeAttribute(insuranceTypeAttributesList);
            return insuranceTypeAttributesList;
        }

        private void AddAttribute(
            string attributeName,
            InsuranceType insuranceType,
            List<InsuranceTypeAttribute> attributeList
        )
        {
            if (!string.IsNullOrEmpty(attributeName))
            {
                var attribute = new InsuranceTypeAttribute(attributeName, insuranceType);
                attributeList.Add(attribute);
            }
        }

        public void AddInsuranceForPrivateCustomer(
            Insurance insurance,
            PrivateCustomer privateCustomer
        )
        {
            insurance.IsPrivateCustomer = true;
            unitOfWork.InsuranceRepository.Add(insurance);
            // unitOfWork.Update(privateCustomer.ProspectNotes);
            unitOfWork.Update(privateCustomer);
            unitOfWork.Update(insurance.InsuranceType);
            unitOfWork.SaveChanges();
        }

        public void AddInsuranceTypeAttribute(InsuranceTypeAttribute insuranceTypeAttribute)
        {
            unitOfWork.InsuranceTypeAttributeRepository.Add(insuranceTypeAttribute);
            unitOfWork.Update(insuranceTypeAttribute.InsuranceType);
            unitOfWork.SaveChanges();
        }

        public void AddInsuranceSpec(InsuranceSpec insuranceSpec)
        {
            unitOfWork.InsuranceSpecRepository.Add(insuranceSpec);
            unitOfWork.Update(insuranceSpec.Insurance.Customer);
            unitOfWork.Update(insuranceSpec.InsuranceTypeAttribute.InsuranceType);
            unitOfWork.Update(insuranceSpec.InsuranceTypeAttribute);
            unitOfWork.Update(insuranceSpec.Insurance);
            unitOfWork.SaveChanges();
        }

        public void AddAllInsuranceTypeAttribute(
            IList<InsuranceTypeAttribute> insuranceTypeAttribute
        )
        {
            foreach (InsuranceTypeAttribute item in insuranceTypeAttribute)
            {
                AddInsuranceTypeAttribute(item);
            }
        }

        //Ska nog ligga i UserControllern
        private User GetUserByID(int iD)
        {
            return unitOfWork.UserRepository.GetUserByID(iD);
        }

        public bool CanAddPrivatePreliminaryInsurance(
            string selectedInsuranceType,
            string insuredPersonFirstName,
            string insuredPersonLastName,
            string insuredPersonSSN,
            string arrivingDate,
            string selectedBasePrice,
            string totalPremium,
            string selectedAddOnOption1,
            string selectedAddOnOption2,
            string selectedAddOnBasePrice1,
            string selectedAddOnBasePrice2
        )
        {
            bool isValid =
                !string.IsNullOrEmpty(selectedInsuranceType)
                && !string.IsNullOrEmpty(insuredPersonFirstName)
                && !string.IsNullOrEmpty(insuredPersonLastName)
                && !string.IsNullOrEmpty(insuredPersonSSN)
                && !string.IsNullOrEmpty(arrivingDate)
                && !string.IsNullOrEmpty(selectedBasePrice)
                && !string.IsNullOrEmpty(totalPremium);

            if (isValid && selectedAddOnOption1 != "Inget")
            {
                isValid = !string.IsNullOrEmpty(selectedAddOnBasePrice1);
            }

            if (isValid && selectedAddOnOption2 != "Inget")
            {
                isValid = !string.IsNullOrEmpty(selectedAddOnBasePrice2);
            }

            return isValid;
        }

        private void CreateLiabilityInsuranceSpecifications(
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
    }
}
