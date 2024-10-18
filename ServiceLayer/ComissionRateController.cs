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
            double totalPremium = unitOfWork.InsuranceRepository.GetTotalPremiumForLastMonth(employee);
            if (totalPremium == 0)
            {
                return 0;
            }

            double commissionRate = employee.Commission?.CommisionRate ?? 0; 

            return totalPremium * commissionRate;
        }

        public List<Employee> GetEmployeesWithCommissions()
        {
            return unitOfWork.EmployeeRepository.GetEmployeesWithCommissions();
        }

        public void ExportToCsv(Employee employee, double totalCommission, string commissionPeriod)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Förnamn;Efternamn;Personnummer;Agentnummer;Provision för period;Provisionsperiod");
            csvContent.AppendLine($"{employee.FirstName};{employee.LastName};{employee.SSN};{employee.AgentNumber};{totalCommission};{commissionPeriod}");

            string filePath = "employee_commission.csv";

            File.WriteAllText(filePath, "\uFEFF" + csvContent.ToString(), Encoding.UTF8);

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }




    }
}
