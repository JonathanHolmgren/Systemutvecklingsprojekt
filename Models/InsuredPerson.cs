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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }

        public ICollection<Insurance> Insurances { get; set; }

        public InsuredPerson() { }

        public InsuredPerson(string firstName, string lastName, string sSN)
        {
            FirstName = firstName;
            LastName = lastName;
            SSN = sSN;
        }
    }
}
