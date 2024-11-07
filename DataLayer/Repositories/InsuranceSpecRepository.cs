using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class InsuranceSpecRepository : Repository<InsuranceSpec>
    {
        public InsuranceSpecRepository(Context context)
            : base(context) { }

        public IList<InsuranceSpec> GetInsuranceSpecsByInsuranceId(int insuranceId) //Insurance specs with attributes
        {
            return Context
                .Set<InsuranceSpec>()
                .Where(spec => spec.Insurance.InsuranceId == insuranceId)
                .ToList();
        }

        public IList<InsuranceSpec> GetSpecsForInsurance(int insuranceId) //Insurance specs without attributes
        {
            return Context
                .Set<InsuranceSpec>()
                .Where(insuranceSpec => insuranceSpec.Insurance.InsuranceId == insuranceId)
                .Include(i => i.InsuranceTypeAttribute)
                .ToList();
        }
    }
}
