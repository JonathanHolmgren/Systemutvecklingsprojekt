using Microsoft.EntityFrameworkCore;
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
            using (var context = new Context()) // Använda using för kontext
            {
                var lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                var lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                // Hämta sålda försäkringar för den senaste månaden
                var insurancesSoldLastMonth = context.Insurances // Använda context direkt
                    .Include(i => i.User) // Inkludera relaterad användare
                    .Where(i => i.User.Employee.AgentNumber == employee.AgentNumber &&
                                i.ExpiryDate >= lastMonthStart &&
                                i.ExpiryDate <= lastMonthEnd)
                    .ToList();

                // Om inga försäkringar har sålts, returnera 0
                if (!insurancesSoldLastMonth.Any())
                {
                    return 0;
                }

                // Beräkna den totala premien
                double totalPremium = 0;

                // Iterera genom försäkringarna för att beräkna den totala premien
                foreach (var insurance in insurancesSoldLastMonth)
                {
                    var insuranceSpecs = context.InsuranceSpecs // Använd context för att hämta InsuranceSpecs
                        .Where(spec => spec.Insurance.InsuranceId == insurance.InsuranceId &&
                                       spec.InsuranceTypeAttribute != null &&
                                       spec.InsuranceTypeAttribute.InsuranceAttribute == "Månadspremie")
                        .ToList();

                    // Summera premien från varje insuranceSpec
                    foreach (var spec in insuranceSpecs)
                    {
                        if (double.TryParse(spec.Value, out double premium))
                        {
                            totalPremium += premium; // Lägg till den giltiga premien
                        }
                    }
                }

                return totalPremium;
            }
        }
    }
}
