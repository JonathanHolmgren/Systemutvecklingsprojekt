using System;
using System.Collections.Generic;
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

        public List<Insurance> GetAllPreliminaryInsurances()
        {
            return unitOfWork.InsuranceRepository.GetAllPreliminaryInsurances();
        }

        public void SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            unitOfWork.InsuranceRepository.SetInsuranceStatusToActive(selectedInsurance);
        }
    }
}
