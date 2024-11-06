using DataLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class EmployeeController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public IList<Employee> GetEmployees()
        {
            return unitOfWork.EmployeeRepository.GetEmployees();
        }
    }
}
