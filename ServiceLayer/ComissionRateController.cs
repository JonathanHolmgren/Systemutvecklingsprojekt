using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Diagnostics;
using DataLayer.Repositories;

namespace ServiceLayer
{
    public class ComissionRateController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public double CalculateComission(Employee employee)
        {
            // Hämta den totala premien för den senaste månaden
            double totalPremium = unitOfWork.InsuranceRepository.GetTotalPremiumForLastMonth(employee);
            // Kontrollera om totalPremium är 0 för att undvika att multiplikationen ger 0 utan försäkringar
            if (totalPremium == 0)
            {
                return 0;
            }

            // Kontrollera om kommissionstakt är giltig
            double commissionRate = employee.Commission?.CommisionRate ?? 0; // Sätt till 0 om null

            return totalPremium * commissionRate;
        }

        public List<Employee> GetEmployeesWithCommissions()
        {
            return unitOfWork.EmployeeRepository.GetEmployeesWithCommissions();
        }
    }
}
