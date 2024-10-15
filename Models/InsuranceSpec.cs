using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceSpec
    {
        public int InsuranceSpecId { get; set; }
        public string Value { get; set; }

        // Navigation property
        public Insurance? Insurance { get; set; }
        public InsuranceTypeAttribute? InsuranceTypeAttribute { get; set; }

        public InsuranceSpec() { }

        public InsuranceSpec(
            string value,
            Insurance insurance,
            InsuranceTypeAttribute insuranceTypeAttribute
        )
        {
            Value = value;
            Insurance = insurance;
            InsuranceTypeAttribute = insuranceTypeAttribute;
        }
    }
}
