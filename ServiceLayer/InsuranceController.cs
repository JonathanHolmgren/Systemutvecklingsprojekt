using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Insurance> LoadPrivateCustomersInsurances()
        {
            return unitOfWork.InsuranceRepository.LoadPrivateCustomersInsurances();
        }

        public ObservableCollection<Insurance> LoadCompanyCustomersInsurances()
        {
            return unitOfWork.InsuranceRepository.LoadCompanyCustomersInsurances();
        }
    }
}
