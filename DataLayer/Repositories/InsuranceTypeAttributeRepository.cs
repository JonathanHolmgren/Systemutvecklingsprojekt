using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuranceTypeAttributeRepository:Repository<InsuranceTypeAttribute>
    {
        public InsuranceTypeAttributeRepository(Context context) : base(context) { }
    }
}
