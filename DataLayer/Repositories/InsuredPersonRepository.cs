using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InsuredPersonRepository: Repository<InsuredPerson>
    {
        public InsuredPersonRepository(Context context) : base(context) { }
    }
}
