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
 
       
        public List<Customer> GetInActiveCustomersWithInsurances()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            return Context.Set<Customer>()
                .Where(c => c.Insurances.All(i => i.InsuranceStatus == InsuranceStatus.Inactive && i.ExpiryDate <= oneYearAgo) &&
                            !c.Insurances.Any(i => i.InsuranceStatus != InsuranceStatus.Inactive))
                .Include(c => c.Insurances)
                .ToList();
        }

        public IList<PrivateCustomer> GetPrivateCustomers()
        {
            return Context.Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(p => p.PostalCodeCity)
                .Include(i => i.Insurances)
                .Include(p => p.ProspectNotes).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                .Include(p => p.Insurances).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                .ToList(); 
        }
 

        public PrivateCustomer GetSpecificPrivateCustomerForInsuranceBySSN(string sSN)
        {
            return Context
                .Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(c => c.PostalCodeCity)
                .FirstOrDefault(c => c.SSN == sSN);
        }
 
        public IList<CompanyCustomer> GetCompanyCustomers()
        {
            return Context.Set<Customer>()
                .OfType<CompanyCustomer>()  
                .Include(c => c.PostalCodeCity)  
                .Include(p => p.ProspectNotes).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                .Include(c => c.Insurances).ThenInclude(a => a.User).ThenInclude(b => b.Employee)
                .ToList(); 

        }

        public CompanyCustomer GetSpecificCompanyCustomer(string organisationNumber)
        {

            return Context.Set<Customer>()
                  .OfType<CompanyCustomer>()  
                  .Include(c => c.PostalCodeCity)  
                  .Include(p => p.ProspectNotes).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                  .Include(c => c.Insurances).ThenInclude(a => a.User).ThenInclude(b => b.Employee)
                  .FirstOrDefault(c => c.OrganisationNumber == organisationNumber); 
        }
        public PrivateCustomer GetSpecificPrivateCustomer(string sSN)
        {
            return Context.Set<Customer>()
                  .OfType<PrivateCustomer>()  
                  .Include(c => c.PostalCodeCity)  
                  .Include(p => p.ProspectNotes).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                  .Include(c => c.Insurances).ThenInclude(a => a.User).ThenInclude(b => b.Employee)
                  .FirstOrDefault(c => c.SSN == sSN); 
        }
 
    }

}

