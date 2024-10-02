using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceTypeAttribute
    {
        public int InsuranceTypeAttributeID { get; set;}
        public InsuranceType InsuranceTypes { get; set;}
        public string InsuranceAttribute { get; set;}
    }
}
