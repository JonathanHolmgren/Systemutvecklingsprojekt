using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.SalesStatistics;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context)
            : base(context) { }

        public List<Insurance> GetCompanyCustomerInsurancesById(int customerId)
        {
            return Context
                .Set<Insurance>()
                .Where(insurance => insurance.Customer.CustomerID == customerId)
                .Include(i => i.InsuranceType)
                .Include(i => i.User) // Jag la till dessa
                .ThenInclude(a => a.Employee) // Ta bort om de krånglar
                .ToList();
        }

        public List<Insurance> GetPrivateCustomerInsurancesById(int customerId)
        {
            return Context
                .Set<Insurance>()
                .Where(insurance => insurance.Customer.CustomerID == customerId)
                .Include(i => i.InsuranceType)
                .Include(i => i.InsuredPerson)
                .Include(i => i.User) // Jag la till dessa
                .ThenInclude(a => a.Employee) // Ta bort om de krånglar
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

        public SalesReport GetSalesReportForInscurance()
        {
            return new SalesReport();
        }

        // public List<Insurance> GetAllActiveInsurance()
        // {
        //     return Context
        //         .Set<Insurance>()
        //         .Where(x => x.InsuranceStatus == InsuranceStatus.Active)
        //         .Include(x => x.User)
        //         .ToList();
        // }

        public SalesReport GetSalesReport(int year)
        {
            var insuranceData = Context
                .Set<Insurance>()
                .Where(i =>
                    i.ActiveDate.Year == year && i.InsuranceStatus == InsuranceStatus.Active
                )
                .Include(i => i.User)
                .Include(i => i.InsuranceType)
                .ToList();

            var salesReport = new SalesReport { Year = year };

            var salesDataGrouped = insuranceData
                .GroupBy(i => new { i.User.UserID, Month = i.ActiveDate.Month })
                .Select(g => new
                {
                    SalesPersonId = g.Key.UserID,
                    Month = g.Key.Month,
                    PrivateSales = g.Where(i => i.IsPrivateCustomer == true).ToList(),
                    CompanySales = g.Where(i => i.IsPrivateCustomer == false).ToList(),
                });

            foreach (var personGroup in salesDataGrouped.GroupBy(g => g.SalesPersonId))
            {
                var salesPersonData = new SalesPersonData
                {
                    SalesPersonName = Context
                        .Set<User>()
                        .Include(u => u.Employee)
                        .Where(u => u.UserID == personGroup.Key)
                        .Select(u => $"{u.Employee.FirstName} {u.Employee.LastName}")
                        .Single(),
                };

                foreach (var monthGroup in personGroup.OrderBy(g => g.Month))
                {
                    if (monthGroup.PrivateSales.Any())
                    {
                        var privateSalesData = new MonthlySalesDataPrivate
                        {
                            ActiveDate = new DateTime(year, monthGroup.Month, 1),
                            ChildrenSales = monthGroup.PrivateSales.Sum(i =>
                                i.InsuranceType.Type
                                == "Sjuk- och olycksförsäkring för barn (t.o.m 17 års åldern)"
                                    ? 1
                                    : 0
                            ),
                            AdultSales = monthGroup.PrivateSales.Sum(i =>
                                i.InsuranceType.Type == "Sjuk- och olycksfallsförsäkring för vuxen"
                                    ? 1
                                    : 0
                            ),
                            LifeSales = monthGroup.PrivateSales.Sum(i =>
                                i.InsuranceType.Type == "Livförsäkring för vuxen" ? 1 : 0
                            ),
                        };
                        salesPersonData.MonthlySalesPrivate.Add(privateSalesData);
                    }

                    if (monthGroup.CompanySales.Any())
                    {
                        var companySalesData = new MonthlySalesDataCompany
                        {
                            ActiveDate = new DateTime(year, monthGroup.Month, 1),
                            PropertyEquipmentInsurance = monthGroup.CompanySales.Sum(i =>
                                i.InsuranceType.Type == "Fastighet och inventarieförsäkring" ? 1 : 0
                            ),
                            VehicleInsurance = monthGroup.CompanySales.Sum(i =>
                                i.InsuranceType.Type == "Fordonsförsäkring" ? 1 : 0
                            ),
                            LiabilityInsurance = monthGroup.CompanySales.Sum(i =>
                                i.InsuranceType.Type == "Ansvarsförsäkring" ? 1 : 0
                            ),
                        };
                        salesPersonData.MonthlySalesCompany.Add(companySalesData);
                    }
                }

                salesReport.Sales.Add(salesPersonData);
            }

            return salesReport;
        }

        public List<Insurance> GetAllPreliminaryInsurances()
        {
            return Context
                .Set<Insurance>()
                .Include(i => i.Customer)
                .Where(p => p.InsuranceStatus == InsuranceStatus.Preliminary)
                .ToList();
        }

        //public Insurance SetInsuranceStatusToActive(Insurance selectedInsurance)
        //{
        //    Insurance insuranceToUpdate = Context
        //        .Set<Insurance>()
        //        .FirstOrDefault(p => p.InsuranceId == selectedInsurance.InsuranceId);

        //    return insuranceToUpdate;
        //}
        //public Insurance SetInsuranceStatusToInactive(Insurance selectedInsurance)
        //{
        //    Insurance insuranceToUpdate = Context
        //        .Set<Insurance>()
        //        .FirstOrDefault(p => p.InsuranceId == selectedInsurance.InsuranceId);

        //    return insuranceToUpdate;
        //}
    }
}
