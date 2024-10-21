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

            var insuranceTypes = Context.Set<InsuranceTypeAttribute>()
                .Include(iT=>iT.InsuranceType)
                .ToList();
                                
            int insuranceTypeAttributeId = 0;
            foreach (InsuranceTypeAttribute insuranceType in insuranceTypes)
            {
                if (insuranceType != null && insuranceType.InsuranceAttribute == "Månadspremie"&&insuranceType.InsuranceType.InsuranceTypeId==insuranceTypeId)
                {
                    insuranceTypeAttributeId = insuranceType.InsuranceTypeAttributeId;
                    return insuranceTypeAttributeId;
                }
                
            }
            return 0;
        }
    }
}
