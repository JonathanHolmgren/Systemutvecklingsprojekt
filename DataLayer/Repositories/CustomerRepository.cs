using System;
using System.Collections.Generic;
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

        public List<Customer> GetInActiveCustomersWithInsurances()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            return Context
                .Set<Customer>()
                .Where(c =>
                    c.Insurances.All(i =>
                        i.InsuranceStatus == InsuranceStatus.Inactive && i.ExpiryDate <= oneYearAgo
                    ) && !c.Insurances.Any(i => i.InsuranceStatus != InsuranceStatus.Inactive)
                )
                .Include(c => c.Insurances)
                .ToList();
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

        public IList<CompanyCustomer> GetCompanyCustomers()
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

        public CompanyCustomer GetSpecificCompanyCustomer(string organisationNumber)
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
                .Include(c => c.Insurances)
                //.ThenInclude(a => a.InsuranceType)
                //.ThenInclude(b => b.InsuranceTypeAttributes)
                .FirstOrDefault(c => c.OrganisationNumber == organisationNumber);
        }

        public PrivateCustomer GetSpecificPrivateCustomer(string sSN)
        {
            return Context
                .Set<Customer>()
                .OfType<PrivateCustomer>()
                .Include(p => p.ProspectNotes)
                .ThenInclude(i => i.User)
                .ThenInclude(u => u.Employee)
                .Include(c => c.Insurances)
                .ThenInclude(a => a.User)
                .ThenInclude(b => b.Employee)
                .Include(c => c.Insurances)
                //.ThenInclude(a => a.InsuranceType)
                //.ThenInclude(b => b.InsuranceTypeAttributes)
                .Include(c => c.Insurances)
                .ThenInclude(a => a.InsuredPerson)
                .FirstOrDefault(c => c.SSN == sSN);
        }

        //public void Changepostalcode(int customerId, string postalCode)
        //{
        //    var sql =
        //        $"Update Customer Set PostalCodeCityPostalCode = '{postalCode}' Where CustomerID = {customerId} ";

        //    int rowsAffected = Context.Database.ExecuteSqlRaw(sql);
        //    Console.WriteLine($"This work! {rowsAffected} records were affected.");
        //}
    }
}
