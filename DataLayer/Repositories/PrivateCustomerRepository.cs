using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class PrivateCustomerRepository : Repository<PrivateCustomer>
    {
        public PrivateCustomerRepository(Context context) : base(context) { }
        public IList<PrivateCustomer>GetAll()
        {
            return Context.Set<PrivateCustomer>().
                Include(i => i.Insurances).
                Include(p => p.PostalCodeCity).ToList();
        }
    }
}
