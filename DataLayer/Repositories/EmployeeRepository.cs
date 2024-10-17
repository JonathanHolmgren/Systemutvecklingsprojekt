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
            using (var context = new Context()) // Byt ut YourDbContext mot din faktiska DbContext
            {
                var employeesWithCommissions = context.Employees
                    .Include(e => e.Commission) // Inkludera relaterad kommission
                    .ToList();

                return employeesWithCommissions;
            }
        }

    }
}
