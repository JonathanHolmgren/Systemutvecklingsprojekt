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

        public List<PrivateCustomer> GetAllPrivateCustomers()
        {
            return Context.Set<PrivateCustomer>().Include(c => c.Insurances).ToList();
        }
    }

}
