using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class CompanyCustomerRepository : Repository<CompanyCustomer>
    {
        public CompanyCustomerRepository(Context context) : base(context) { }
        public IList<CompanyCustomer> GetAll()
        {
            return Context.Set<CompanyCustomer>().
                Include(i => i.Insurances).
                Include(p => p.PostalCodeCity).ToList();
        }
    }
}
