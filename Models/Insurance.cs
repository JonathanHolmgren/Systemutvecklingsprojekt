using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Insurance
    {
        public int InsuranceId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public BillingInterval BillingingInterval { get; set; }
        public InsuranceStatus InsuranceStatus { get; set; }
        public string Notes { get; set; }
      
        // Navigation property
        public User User { get; set; }
        public InsuredPerson InsuredPerson { get; set; }
        public Customer Customer {get; set;}
        public InsuranceType InsuranceType {get; set;}
       
        
    }
}
