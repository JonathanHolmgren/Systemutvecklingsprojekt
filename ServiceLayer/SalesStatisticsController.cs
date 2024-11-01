using System.Collections.ObjectModel;
using DataLayer;
using Models;
using Models.SalesStatistics;

namespace ServiceLayer;

public class SalesStatisticsController
{
    UnitOfWork unitOfWork = new UnitOfWork();

    // public List<Insurance> GetAllActiveInsurance(Employee employees, int year)
    // {
    //     //return unitOfWork.InsuranceRepository.GetAllActiveInsurance();
    // }

    List<Employee> GetAllEmployees()
    {
        throw new NotImplementedException();
    }

    public SalesReport GetSalesReport(int year)
    {
        SalesReport salesReport = unitOfWork.InsuranceRepository.GetSalesReport(year);
        return salesReport;
    }
}
