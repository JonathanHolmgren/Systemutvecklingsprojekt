﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Models;

namespace ServiceLayer
{
    public class InsuranceSpecController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public InsuranceSpecController() { }
        public IList<InsuranceSpec> GetAllInsuranceSpecsForInsurance(int insuranceId)
        {
            return unitOfWork.InsuranceSpecRepository.GetSpecsForInsurance(insuranceId); 
        }
    }
}
