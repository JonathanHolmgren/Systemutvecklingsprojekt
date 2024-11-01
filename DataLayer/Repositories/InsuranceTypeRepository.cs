using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataLayer.Repositories
{
    public class InsuranceTypeRepository : Repository<InsuranceType>
    {
        public InsuranceTypeRepository(Context context)
            : base(context) { }

        public InsuranceType GetInsuranceType(string input)
        {
            return Context.Set<InsuranceType>().FirstOrDefault(x => x.Type == input);
        }

        public string GetCustomerInsuranceType(int insuranceTypeId)
        {
            return Context
                .Set<InsuranceType>()
                .FirstOrDefault(c => c.InsuranceTypeId == insuranceTypeId)
                ?.Type;
        }
    }
}
