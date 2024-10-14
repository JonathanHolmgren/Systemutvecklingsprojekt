using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ServiceLayer
{
    public class ComissionRateController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public double CalculateComission(Employee employee)
        {
            double totalPremium = GetTotalPremiumForLastMonth(employee);

            double commissionRate = employee.CommisionRate.CommisionrateRate; 

            return totalPremium * commissionRate;
        }


        public double GetTotalPremiumForLastMonth(Employee employee)
        {
            var lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
            var lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            var insurancesSoldLastMonth = unitOfWork.InsuranceRepository
                .Find(i => i.User.Employee.AgentNumber == employee.AgentNumber &&
                            i.ExpiryDate >= lastMonthStart &&
                            i.ExpiryDate <= lastMonthEnd)
                .ToList();

            double totalPremium = 0;

            foreach (var insurance in insurancesSoldLastMonth)
            {
                var insuranceSpec = unitOfWork.InsuranceSpecRepository
                    .FirstOrDefault(spec => spec.Insurance.InsuranceID == insurance.InsuranceID);

                if (insuranceSpec != null)
                {
                    if (double.TryParse(insuranceSpec.Value, out double premium))
                    {
                        totalPremium += premium;
                    }
                }
            }

            return totalPremium;
        }

        public List<Employee> GetAllEmployees()
        {
            return unitOfWork.EmployeeRepository.Find(e => true).ToList();
        }
    }
}
