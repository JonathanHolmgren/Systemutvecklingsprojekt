using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class PostalCodeCityRepository: Repository<PostalCodeCity>
    {
        public PostalCodeCityRepository(Context context) : base(context) { }
        public PostalCodeCity GetSpecificPostalCode(string postalCode)
        {
            return Context.Set<PostalCodeCity>().SingleOrDefault(p => p.PostalCode.Equals(postalCode));
        }
    }
}
