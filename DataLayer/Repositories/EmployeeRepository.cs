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
    }
}
