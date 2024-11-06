using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(Context context) : base(context) { }

        public List<Employee> GetEmployeesWithCommissions()
        {
            using (var context = new Context()) 
            {
                var employeesWithCommissions = context.Employees
                    .Where(e => e.Role.Contains("Innesäljare") || e.Role.Contains("Utesäljare"))
                    .Include(e => e.Commission) 
                    .ToList();

                return employeesWithCommissions;
            }
        }

        public IList<Employee> GetEmployees()
        {
            return Context.Set<Employee>().ToList();
        }

    }
}
