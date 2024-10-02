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
        public Insurance Insurance { get; set; }
        public InsuranceType InsuranceType { get; set; }
    }
}
