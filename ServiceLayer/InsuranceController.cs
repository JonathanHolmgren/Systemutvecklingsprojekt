using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Models;

namespace ServiceLayer
{
    public class InsuranceController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        #region PrivateCustomer
        #region RegisterInsurance
        public Insurance RegisterPreliminaryInsurance(
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

        public Insurance CreateInsuranceFromInput(
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
                RegisterInsuranceTypeAttribute(
                    insuranceType,
                    selectedAddOnOption1,
                    selectedAddOnOption2
                );

            InsuredPerson insuredPerson = RegisterInsuredPerson(
                insuredPersonFirstName,
                insuredPersonLastName,
                insuredPersonSSN
            );
            Insurance newInsurance = RegisterPreliminaryInsurance(
                arrivingDate,
                selectedInterval,
                user,
                insuranceType,
                notes,
                selectedPrivateCustomer,
                insuredPerson
            );
            AddInsurance(newInsurance, selectedPrivateCustomer);
            CreateInsuranceSpecifications(
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

        public void CreateInsuranceSpecifications(
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

        private List<InsuranceTypeAttribute> RegisterInsuranceTypeAttribute(
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

        public void AddInsurance(Insurance insurance, PrivateCustomer privateCustomer)
        {
            unitOfWork.InsuranceRepository.Add(insurance);
            unitOfWork.Update(privateCustomer.PostalCodeCity);
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
            unitOfWork.Update(insuranceSpec.Insurance.Customer.PostalCodeCity);
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
        public User GetUserByID(int iD)
        {
            return unitOfWork.UserRepository.GetUserByID(iD);
        }

        //Kan ligga i insuredPersonController om vi vill ha en sån
        public InsuredPerson RegisterInsuredPerson(string firstName, string lastName, string ssn)
        {
            return new InsuredPerson(firstName, lastName, ssn);
        }

        #endregion

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
        #endregion
    }
}
