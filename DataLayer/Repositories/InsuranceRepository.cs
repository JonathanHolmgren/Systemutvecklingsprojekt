﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context) : base(context) { }

        public double GetTotalPremiumForLastMonth(Employee employee)
        {
            using (var context = new Context()) 
            {
                var lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                var lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                var insurancesSoldLastMonth = context.Insurances 
                    .Include(i => i.User) 
                    .Where(i => i.User.Employee.AgentNumber == employee.AgentNumber &&
                                i.ExpiryDate >= lastMonthStart &&
                                i.ExpiryDate <= lastMonthEnd)
                    .ToList();

                if (!insurancesSoldLastMonth.Any())
                {
                    return 0;
                }

                double totalPremium = 0;

                foreach (var insurance in insurancesSoldLastMonth)
                {
                    var insuranceSpecs = context.InsuranceSpecs 
                        .Where(spec => spec.Insurance.InsuranceId == insurance.InsuranceId &&
                                       spec.InsuranceTypeAttribute != null &&
                                       spec.InsuranceTypeAttribute.InsuranceAttribute == "Månadspremie")
                        .ToList();

                    foreach (var spec in insuranceSpecs)
                    {
                        if (double.TryParse(spec.Value, out double premium))
                        {
                            totalPremium += premium; 
                        }
                    }
                }

                return totalPremium;
            }
        }

        public double GetTotalPremiumForPeriod(Employee employee, DateTime startDate, DateTime endDate)
        {
            using (var context = new Context())
            {
                var insurancesSoldInPeriod = context.Insurances
                    .Include(i => i.User)
                    .Where(i => i.User.Employee.AgentNumber == employee.AgentNumber &&
                                i.ExpiryDate >= startDate &&
                                i.ExpiryDate <= endDate)
                    .ToList();

                if (!insurancesSoldInPeriod.Any())
                {
                    return 0;
                }

                double totalPremium = 0;

                foreach (var insurance in insurancesSoldInPeriod)
                {
                    var insuranceSpecs = context.InsuranceSpecs
                        .Where(spec => spec.Insurance.InsuranceId == insurance.InsuranceId &&
                                       spec.InsuranceTypeAttribute != null &&
                                       spec.InsuranceTypeAttribute.InsuranceAttribute == "Månadspremie")
                        .ToList();

                    foreach (var spec in insuranceSpecs)
                    {
                        if (double.TryParse(spec.Value, out double premium))
                        {
                            totalPremium += premium;
                        }
                    }
                }

                return totalPremium;
            }
        }

    }
}
