using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(Context context) : base(context) { }

        public List<Customer> GetAllCustomers()
        {
            return Context.Set<Customer>().Include(c => c.Insurances).ToList();
        }
    }
}
