using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuranceSpecRepository : Repository<InsuranceSpec>
    {
        public InsuranceSpecRepository(Context context) : base(context) { }

        public List<InsuranceSpec> GetInsuranceSpecsByInsuranceId(int insuranceId)
        {
            return Context.Set<InsuranceSpec>()
                .Where(spec => spec.Insurance.InsuranceId == insuranceId)
                .ToList();

        public IList<InsuranceSpec> GetSpecsForInsurance(int insuranceId)
        {
            return Context.Set<InsuranceSpec>()
                          .Where(insuranceSpec => insuranceSpec.Insurance.InsuranceId == insuranceId)
                          .Include(i=>i.InsuranceTypeAttribute)
                          .ToList();

        }
    }
}
