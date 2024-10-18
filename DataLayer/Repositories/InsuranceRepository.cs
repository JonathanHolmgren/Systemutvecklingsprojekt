using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context) : base(context) { }
        public IList<Insurance> GetCustomerInsurances(int customerId)
        {
            return Context.Set<Insurance>()
                          .Where(insurance => insurance.Customer.CustomerID == customerId)
                          .Include(i=>i.InsuranceType)
                          .ToList();
        }
    }
}
