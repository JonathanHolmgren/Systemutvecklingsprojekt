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
            IList<InsuranceSpec> insuranceSpecsToRemove = unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(selectedInsurance.InsuranceId);

            foreach (InsuranceSpec insuranceSpec in insuranceSpecsToRemove)
            {
                unitOfWork.InsuranceSpecRepository.Remove(insuranceSpec.InsuranceSpecId);
                //unitOfWork.Remove(unitOfWork.InsuranceTypeAttributeRepository.FirstOrDefault(a => a.InsuranceTypeAttributeId = insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeID));
                unitOfWork.InsuranceTypeAttributeRepository.Remove(insuranceSpec.InsuranceTypeAttribute.InsuranceTypeAttributeId);
            }
            //unitOfWork.InsuranceSpecRepository.Remove(9);

            //unitOfWork.Update(unitOfWork.InsuranceTypeRepository.FirstOrDefault(m => m.InsuranceTypeId == selectedInsurance.InsuranceType.InsuranceTypeId));

            //unitOfWork.Update(unitOfWork.InsuranceTypeRepository.FirstOrDefault(m => m.InsuranceTypeId == 3));

            

            //unitOfWork.InsuranceTypeAttributeRepository.Remove(9);



            unitOfWork.InsuranceRepository.Remove(selectedInsurance);
            unitOfWork.SaveChanges();
        }
    }
}
