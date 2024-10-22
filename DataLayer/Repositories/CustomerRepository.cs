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

        public List<Customer> GetInActiveCustomers()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            return Context.Set<Customer>()
                .Where(c => !c.Insurances.Any(i => i.ExpiryDate > oneYearAgo && i.InsuranceStatus != InsuranceStatus.Active))
                .ToList();
        }

       
        public List<Customer> GetInActiveCustomersWithInsurances()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            // Hämta inaktiva kunder vars alla försäkringar är inaktiva och äldre än ett år
            return Context.Set<Customer>()
                .Where(c => c.Insurances.All(i => i.InsuranceStatus == InsuranceStatus.Inactive && i.ExpiryDate <= oneYearAgo) &&
                            !c.Insurances.Any(i => i.InsuranceStatus != InsuranceStatus.Inactive))
                .Include(c => c.Insurances)
                .ToList();
        }



    }
}
