using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceTypeAttribute
    {
        public int InsuranceTypeAttributeId { get; set; }
        public string InsuranceAttribute { get; set; }

        // Navigation property
        public InsuranceType? InsuranceType { get; set; }

        public InsuranceTypeAttribute() { }

        public InsuranceTypeAttribute(string insuranceAttribute, InsuranceType insuranceType)
        {
            InsuranceAttribute = insuranceAttribute;
            InsuranceType = insuranceType;
        }
    }
}
