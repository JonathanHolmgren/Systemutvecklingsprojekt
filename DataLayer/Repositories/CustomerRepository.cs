using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(Context context)
            : base(context) { }

        public IList<PrivateCustomer> GetInActiveCustomersWithInsurances()
        {
            try
            {
                DateTime oneYearAgo = DateTime.Now.AddYears(-1);

                var customers = Context
                    .Set<PrivateCustomer>()
                    .Where(c =>
                        c.Insurances.All(i =>
                            i.InsuranceStatus == InsuranceStatus.Inactive
                            && i.ExpiryDate <= oneYearAgo
                        )
                    )
                    .Include(c => c.Insurances)
                    .ToList();

                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return new List<PrivateCustomer>();
            }
        }

        public IList<PrivateCustomer> GetPrivateCustomers()
        {
            return Context
                .Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(p => p.ProspectNotes)
                .ThenInclude(i => i.User)
                .ThenInclude(u => u.Employee)
                .Include(p => p.Insurances)
                .ThenInclude(i => i.User)
                .ThenInclude(u => u.Employee)
                .ToList();
        }

        public PrivateCustomer? GetSpecificPrivateCustomerForInsuranceBySSN(string sSN)
        {
            return Context
                .Set<Customer>()
                .OfType<PrivateCustomer>()
                .FirstOrDefault(c => c.SSN == sSN);
        }

        public CompanyCustomer? GetSpecificCompanyCustomerForInsuranceByOrgNumber(string org)
        {
            return Context
                .Set<Customer>()
                .OfType<CompanyCustomer>()
                .FirstOrDefault(c => c.OrganisationNumber == org);
        }

        public IList<CompanyCustomer> GetCompanyCustomers() //Loading all company customer with all attributes
        {
            return Context
                .Set<Customer>()
                .OfType<CompanyCustomer>()
                .Include(p => p.ProspectNotes)
                .ThenInclude(i => i.User)
                .ThenInclude(u => u.Employee)
                .Include(c => c.Insurances)
                .ThenInclude(a => a.User)
                .ThenInclude(b => b.Employee)
                .ToList();
        }

        public CompanyCustomer? GetSpecificCompanyCustomerByOrgNr(string organisationNumber)
        {
            if (string.IsNullOrEmpty(organisationNumber))
            {
                return null!;
            }
            return Context
                .Set<Customer>()
                .OfType<CompanyCustomer>()
                .Include(p => p.ProspectNotes)
                .Include(p => p.Insurances)
                .FirstOrDefault(c => c.OrganisationNumber == organisationNumber);
        }

        public PrivateCustomer? GetPrivateCustomerBySsn(string ssn)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return null!;
            }
            return Context
                .Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(p => p.ProspectNotes)
                .Include(p => p.Insurances)
                .FirstOrDefault(c => c.SSN == ssn);
        }
    }
}
