using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.InsuranceInformation
{
    public class InsuranceSpecAndAttributeInformation
    {
        public string InsuranceAttribute { get; set; }
        public string Value { get; set; }

        public InsuranceSpecAndAttributeInformation(string insuranceAttribute, string value)
        {
            InsuranceAttribute = insuranceAttribute;
            Value = value;
        }
    }
}
