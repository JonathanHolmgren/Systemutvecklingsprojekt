using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceSpec
    {
        public int InsuranceSpecID { get; set; }
        public string Value { get; set; } //Kan behövas konvertera till Int/double
        public Insurance Insurance { get; set; }
        public InsuranceTypeAttribute InsuranceTypeAttribute { get; set; }

    }
}
