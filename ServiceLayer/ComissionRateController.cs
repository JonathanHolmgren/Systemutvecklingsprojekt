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

        public double CalculateComission(Employee employee, DateTime startDate, DateTime endDate)
        {
            double totalPremium = unitOfWork.InsuranceRepository.GetTotalPremiumForPeriod(employee, startDate, endDate);
            if (totalPremium == 0)
            {
                return 0;
            }

            double commissionRate = employee.Commission?.CommisionRate ?? 0;

            return totalPremium * commissionRate;
        }


        public IList<Employee> GetEmployeesWithCommissions()
        {
            return unitOfWork.EmployeeRepository.GetEmployeesWithCommissions();
        }

        public string CreateCsvContent(Employee employee, double totalCommission, string commissionPeriod)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine($"Förnamn: {employee.FirstName}");
            csvContent.AppendLine($"Efternamn: {employee.LastName}");
            csvContent.AppendLine($"Personnummer: {employee.SSN}");
            csvContent.AppendLine($"Agentnummer: {employee.AgentNumber}");
            csvContent.AppendLine($"Provision för period: {totalCommission} SEK");
            csvContent.AppendLine($"Provisionsperiod: {commissionPeriod}");

            return "\uFEFF" + csvContent.ToString(); 
        }
    }
}
