using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuranceTypeAttributeRepository:Repository<InsuranceTypeAttribute>
    {
        public InsuranceTypeAttributeRepository(Context context) : base(context) { }
        public int GetPremieTypeAttributeId(int insuranceTypeId)
        {
            var insuranceTypeAttribute = Context.Set<InsuranceTypeAttribute>()
                .Include(iT => iT.InsuranceType)
                .FirstOrDefault(iT =>
                    iT != null &&
                    iT.InsuranceType != null &&
                    iT.InsuranceAttribute == "Månadspremie" &&
                    iT.InsuranceType.InsuranceTypeId == insuranceTypeId);

            return insuranceTypeAttribute?.InsuranceTypeAttributeId ?? 0;
        }
    }
}
