using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceType
    {
        public int InsuranceTypeId { get; set; }
        public string Type { get; set; }

        // Navigation property
        public ICollection<InsuranceTypeAttribute> InsuranceTypeAttributes { get; set; }
        public ICollection<Insurance> Insurances { get; set; }

        public InsuranceType() { }

        public InsuranceType(string type)
        {
            Type = type;
        }
    }
}
