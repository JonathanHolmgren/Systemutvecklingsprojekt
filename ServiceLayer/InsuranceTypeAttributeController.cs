using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Models;

namespace ServiceLayer
{
    public class InsuranceTypeAttributeController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public InsuranceTypeAttributeController() { }

        public void RemoveInsuranceTypeAttribute(InsuranceTypeAttribute insuranceTypeAttribute)
        {
            unitOfWork.InsuranceTypeAttributeRepository.Remove(insuranceTypeAttribute);
        }
    }
}
