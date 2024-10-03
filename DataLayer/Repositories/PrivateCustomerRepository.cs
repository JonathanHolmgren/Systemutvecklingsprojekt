using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataLayer.Repositories
{
    public class PrivateCustomerRepository : Repository<PrivateCustomer>
    {
        public PrivateCustomerRepository(Context context) : base(context) { }
    }
}
