using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Insurance
    {
        public int InsuranceID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime BillingingInterval { get; set; }
        public SalesPerson SalesPerson { get; set; }
        public string Status { get; set; }
        public InsuranceType InsuranceType {get; set;}
        public string Notes { get; set; }
        //public Customer Customer {get; set;}
        public InsuredPerson InsuredPerson { get; set; }
        private static void Premium()
        {

        }

    }
}
