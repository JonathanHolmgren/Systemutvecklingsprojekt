using System;
using System.Collections.Generic;
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
            InsuredPerson insuredPerson //Måste skapas en metod för att skapa detta objektet. Finns inte med i backlogen just nu. 
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

        public void AddInsurance(Insurance insurance)
        {
            unitOfWork.InsuranceRepository.Add(insurance);
            unitOfWork.Update(insurance.User);
            unitOfWork.Update(insurance.Customer);
            unitOfWork.Update(insurance.InsuredPerson);
            unitOfWork.Update(insurance.InsuranceType);
            unitOfWork.SaveChanges();
        }
    }
}
