using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context)
            : base(context) { }

        public IList<Insurance> GetCustomerInsurances(int customerId)
        {
            return Context
                .Set<Insurance>()
                .Where(insurance => insurance.Customer.CustomerID == customerId)
                .Include(i => i.InsuranceType)
                .ToList();
        }

        public Insurance GetInsurance(int insuranceId)
        {
            return Context
                .Set<Insurance>()
                .Include(i => i.InsuranceType)
                .FirstOrDefault(i => i.InsuranceId == insuranceId);
        }

        public double GetTotalPremiumForPeriod(
            Employee employee,
            DateTime startDate,
            DateTime endDate
        )
        {
            using (var context = new Context())
            {
                var insurancesSoldInPeriod = context
                    .Insurances.Include(i => i.User)
                    .Where(i =>
                        i.User.Employee.AgentNumber == employee.AgentNumber
                        && i.ExpiryDate >= startDate
                        && i.ExpiryDate <= endDate
                    )
                    .ToList();

                if (!insurancesSoldInPeriod.Any())
                {
                    return 0;
                }

                double totalPremium = 0;

                foreach (var insurance in insurancesSoldInPeriod)
                {
                    var insuranceSpecs = context
                        .InsuranceSpecs.Where(spec =>
                            spec.Insurance.InsuranceId == insurance.InsuranceId
                            && spec.InsuranceTypeAttribute != null
                            && spec.InsuranceTypeAttribute.InsuranceAttribute == "Månadspremie"
                        )
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

        public List<Insurance> GetAllPreliminaryInsurances()
        {
            return Context
                .Set<Insurance>()
                .Include(i => i.Customer)
                .Where(p => p.InsuranceStatus == InsuranceStatus.Preliminary)
                .ToList();
        }

        public Insurance SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            Insurance insuranceToUpdate = Context
                .Set<Insurance>()
                .FirstOrDefault(p => p.InsuranceId == selectedInsurance.InsuranceId);

            return insuranceToUpdate;
        }
    }
}
