using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Models;

namespace ServiceLayer
{
    public class InsuranceController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

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

        //public void AddInsurance(Insurance insurance)
        //{
        //    PostalCodeCity existingPostalCodeCity =
        //        unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(
        //            insurance.Customer.PostalCodeCity.PostalCode
        //        );

        //    if (existingPostalCodeCity == null)
        //    {
        //        unitOfWork.PostalCodeCityRepository.Add(insurance.Customer.PostalCodeCity);
        //    }
        //    else
        //    {
        //        insurance.Customer.PostalCodeCity.PostalCode = existingPostalCodeCity.PostalCode;
        //    }

        //    PostalCodeCity existingPostalCodeCityEmployee =
        //       unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(
        //           insurance.User.Employee.PostalCodeCity.PostalCode);

        //    if (existingPostalCodeCityEmployee == null)
        //    {
        //        unitOfWork.PostalCodeCityRepository.Add(insurance.User.Employee.PostalCodeCity);
        //    }
        //    else
        //    {
        //        insurance.User.Employee.PostalCodeCity.PostalCode = existingPostalCodeCityEmployee.PostalCode;
        //    }

        //    insurance.User =
        //     unitOfWork.UserRepository.GetUser(
        //         insurance.User.UserID
        //     );

        //    Debug.WriteLine("UserID" + insurance.User.UserID);
        //    Debug.WriteLine("InsuranceType" + insurance.InsuranceType);

        //    insurance.User.Insurances.Add( insurance );

        //    unitOfWork.Update(insurance.Customer);
        //    unitOfWork.Update(insurance.InsuredPerson);
        //    unitOfWork.Update(insurance.InsuranceType);
        //    unitOfWork.Update(insurance.InsuranceType);

        //    unitOfWork.InsuranceRepository.Add(insurance);
        //    unitOfWork.SaveChanges();
        //}

        public void AddInsurance(Insurance insurance)
        {
            // Kontrollera om PostalCodeCity redan finns för Customer
            PostalCodeCity existingPostalCodeCity = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(
                insurance.Customer.PostalCodeCity.PostalCode);

            if (existingPostalCodeCity == null)
            {
                // Lägg till ny PostalCodeCity om den inte finns
                unitOfWork.PostalCodeCityRepository.Add(insurance.Customer.PostalCodeCity);
            }
            else
            {
                // Använd befintlig PostalCodeCity
                insurance.Customer.PostalCodeCity = existingPostalCodeCity;
            }

            // Kontrollera om PostalCodeCity redan finns för Employee
            PostalCodeCity existingPostalCodeCityEmployee = unitOfWork.PostalCodeCityRepository.GetSpecificPostalCode(
                insurance.User.Employee.PostalCodeCity.PostalCode);

            if (existingPostalCodeCityEmployee == null)
            {
                // Lägg till ny PostalCodeCity om den inte finns
                unitOfWork.PostalCodeCityRepository.Add(insurance.User.Employee.PostalCodeCity);
            }
            else
            {
                // Använd befintlig PostalCodeCity
                insurance.User.Employee.PostalCodeCity = existingPostalCodeCityEmployee;
            }

            // Hämta befintlig användare från databasen
            var existingUser = unitOfWork.UserRepository.GetUser(insurance.User.UserID);
            if (existingUser != null)
            {
                // Uppdatera User-referensen till den befintliga användaren
                insurance.User = existingUser;
            }
            else
            {
                throw new Exception("User not found in database.");
            }

            // Kontrollera att alla obligatoriska fält är satta
            if (insurance.Customer == null ||
                insurance.Customer.PostalCodeCity == null ||
                insurance.User == null ||
                insurance.User.Employee == null ||
                insurance.User.Employee.PostalCodeCity == null)
            {
                throw new Exception("All required entities must be set and have valid data before saving.");
            }

            // Lägg till försäkringen till användarens lista med försäkringar
            insurance.User.Insurances.Add(insurance);

            // Lägg till eller uppdatera Customer och InsuredPerson
            if (insurance.Customer.CustomerID == 0)
            {
                unitOfWork.CustomerRepository.Add(insurance.Customer);
            }
            else
            {
                unitOfWork.Update(insurance.Customer);
            }

            if (insurance.InsuredPerson.InsuredPersonID == 0)
            {
                unitOfWork.InsuredPersonRepository.Add(insurance.InsuredPerson);
            }
            else
            {
                unitOfWork.Update(insurance.InsuredPerson);
            }

            // Lägg till eller uppdatera InsuranceType endast om det behövs
            if (insurance.InsuranceType.InsuranceTypeId == 0)
            {
                unitOfWork.InsuranceTypeRepository.Add(insurance.InsuranceType);
            }
            else
            {
                unitOfWork.Update(insurance.InsuranceType);
            }

            // Lägg till den nya försäkringen i databasen
            unitOfWork.InsuranceRepository.Add(insurance);

            // Spara ändringar
            unitOfWork.SaveChanges();
        }




        public void AddInsuranceTypeAttribute(InsuranceTypeAttribute insuranceTypeAttribute)
        {
            unitOfWork.InsuranceTypeAttributeRepository.Add(insuranceTypeAttribute);
            unitOfWork.SaveChanges();
        }
        public void AddInsuranceSpec(InsuranceSpec insuranceSpec)
        {
            unitOfWork.InsuranceSpecRepository.Add(insuranceSpec);
            unitOfWork.SaveChanges();
        }

        public User GetUser(int iD)
        {
            return unitOfWork.UserRepository.GetUser(iD);
        }

        public InsuranceType GetInsuranceType(string inputInsuranceType)
        {
            return unitOfWork.InsuranceTypeRepository.GetInsuranceType(inputInsuranceType);
        }

        public InsuredPerson CreateInsuredPerson(string firstName, string lastName, string ssn)
        {
            return new InsuredPerson(firstName, lastName, ssn);
        }
    }
}
