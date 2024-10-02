using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuranceType
    {
        public int InsuranceTypeID {  get; set; }
        public IList<InsuranceTypeAttribute> InsuranceTypeAttributes { get; set; }
        public string Type { get; set; }    
    }
}
