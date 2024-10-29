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

        InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
        InsuranceTypeAttributeController insuranceTypeAttributeController =
            new InsuranceTypeAttributeController();

        public List<Insurance> GetAllPreliminaryInsurances()
        {
            return unitOfWork.InsuranceRepository.GetAllPreliminaryInsurances();
        }

        public void SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.SetInsuranceStatusToActive(
                selectedInsurance
            );

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Active;

                unitOfWork.SaveChanges();
            }
        }

        public void SetInsuranceStatusToInactive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = unitOfWork.InsuranceRepository.SetInsuranceStatusToActive(
                selectedInsurance
            );

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Inactive;

                unitOfWork.SaveChanges();
            }
        }

        public void RemoveInsurance(Insurance selectedInsurance)
        {
            IList<InsuranceSpec> insuranceSpecs =
                unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(
                    selectedInsurance.InsuranceId
                );

            foreach (InsuranceSpec insuranceSpec in insuranceSpecs)
            {
                unitOfWork.InsuranceSpecRepository.Remove(insuranceSpec);
                unitOfWork.InsuranceTypeAttributeRepository.Remove(
                    insuranceSpec.InsuranceTypeAttribute
                );
            }

            unitOfWork.InsuranceRepository.Remove(selectedInsurance);
            unitOfWork.SaveChanges();
        }
    }
}
