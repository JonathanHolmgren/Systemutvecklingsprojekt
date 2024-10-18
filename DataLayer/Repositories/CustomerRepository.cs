﻿using Microsoft.EntityFrameworkCore;
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
                .Include(p => p.Insurances).ThenInclude(i => i.User).ThenInclude(u => u.Employee)
                .ToList(); // Returnerar hela objektet direkt, inklusive egenskaper från PrivateCustomer och Customer
        }

        public IList<CompanyCustomer> GetCompanyCustomers()
        {
            return Context.Set<Customer>()
                .OfType<CompanyCustomer>()  // Filtrera endast CompanyCustomer-objekt
                .Include(c => c.PostalCodeCity)  // Inkludera PostalCodeCity
                .Include(c => c.Insurances).ThenInclude(a => a.User).ThenInclude(b => b.Employee)
                .ToList(); // Konvertera till en lista
        }

        public CompanyCustomer GetSpecificCompanyCustomer(int customerId)
        {
            return Context.Set<Customer>()
                  .OfType<CompanyCustomer>()  // Filtrera endast CompanyCustomer-objekt
                  .Include(c => c.PostalCodeCity)  // Inkludera PostalCodeCity
                  .FirstOrDefault(); // Konvertera till en lista
        }
        public PrivateCustomer GetSpecificPrivateCustomer(int customerId)
        {
            return Context.Set<Customer>()
                  .OfType<PrivateCustomer>()  // Filtrera endast CompanyCustomer-objekt
                  .Include(c => c.PostalCodeCity)  // Inkludera PostalCodeCity
                  .FirstOrDefault(); // Konvertera till en lista
        }
    }
}