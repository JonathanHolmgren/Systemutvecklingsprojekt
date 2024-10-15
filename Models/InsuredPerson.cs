using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InsuredPerson
    {
        public int InsuredPersonID { get; set; }
        public string InsuredName { get; set; }
        public string SSN { get; set; }

        public ICollection<Insurance> Insurances { get; set; }

        //Constructors
        public InsuredPerson() { }

        public InsuredPerson(string insuredName, string sSN)
        {
            InsuredName = insuredName;
            SSN = sSN;
        }
    }
}
