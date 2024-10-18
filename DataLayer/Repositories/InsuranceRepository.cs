using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class InsuranceRepository : Repository<Insurance>
    {
        public InsuranceRepository(Context context)
            : base(context) { }

        public ObservableCollection<Insurance> LoadPrivateCustomersInsurances()
        {
            ObservableCollection<Insurance> returnList = new ObservableCollection<Insurance>();

            List<Insurance> tempList = Context
                .Set<Insurance>()
                .Include(i => i.Customer)
                .Where(i => i.Customer is PrivateCustomer)
                .ToList();

            foreach (Insurance i in tempList)
            {
                returnList.Add(i);
            }

            return returnList;
        }

        public ObservableCollection<Insurance> LoadCompanyCustomersInsurances()
        {
            ObservableCollection<Insurance> returnList = new ObservableCollection<Insurance>();

            List<Insurance> tempList = Context
                .Set<Insurance>()
                .Include(i => i.Customer)
                .Where(i => i.Customer is CompanyCustomer)
                .ToList();

            foreach (Insurance i in tempList)
            {
                returnList.Add(i);
            }

            return returnList;
        }
    }
}
