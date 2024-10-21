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
        public IList<PrivateCustomer> GetPrivateCustomers()
        {
            return Context.Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(p => p.PostalCodeCity)
                .Include(i => i.Insurances)
                .ToList();
        }

        public IList<CompanyCustomer> GetCompanyCustomers()
        {
            return Context.Set<Customer>()
                .OfType<CompanyCustomer>() 
                .Include(c => c.PostalCodeCity)
                .Include(i=>i.Insurances)
                .ToList();
        }

        public CompanyCustomer GetSpecificCompanyCustomer(int customerId)
        {
          return Context.Set<Customer>()
                .OfType<CompanyCustomer>()
                .Include(c => c.PostalCodeCity) 
                .FirstOrDefault(c => c.CustomerID == customerId);
        }
        public PrivateCustomer GetSpecificPrivateCustomer(int customerId)
        {
            return Context.Set<Customer>()
                  .OfType<PrivateCustomer>()  // Filtrera endast CompanyCustomer-objekt
                  .Include(c => c.PostalCodeCity)  // Inkludera PostalCodeCity
                  .FirstOrDefault(c=>c.CustomerID==customerId); // Konvertera till en lista
        }





    }

}
