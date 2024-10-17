using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context)
            : base(context) { }

        public List<Insurance> GetAllPreliminaryInsurances()
        {
            return Context
                .Set<Insurance>()
                .Include(i => i.Customer)
                .Where(p => p.InsuranceStatus == InsuranceStatus.Preliminary)
                .ToList();
        }

        public void SetInsuranceStatusToActive(Insurance selectedInsurance)
        {
            var insuranceToUpdate = Context
                .Set<Insurance>()
                .FirstOrDefault(p => p.InsuranceId == selectedInsurance.InsuranceId);

            if (insuranceToUpdate != null)
            {
                insuranceToUpdate.InsuranceStatus = InsuranceStatus.Active;

                Context.SaveChanges();
            }
        }
    }
}
