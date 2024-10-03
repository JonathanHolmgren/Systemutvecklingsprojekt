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
    }
}
