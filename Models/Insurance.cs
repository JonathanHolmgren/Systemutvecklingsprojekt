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
        public BillingInterval BillingingInterval { get; set; }
        public User User { get; set; }
        public InsuranceStatus InsuranceStatus { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public string Notes { get; set; }
        public Customer Customer { get; set; }
        public InsuredPerson InsuredPerson { get; set; }

        public Insurance() { }

        public Insurance(
            DateTime expiryDate,
            BillingInterval billingInterval,
            User user,
            InsuranceType insuranceType,
            string notes,
            Customer customer,
            InsuredPerson insuredPerson
        ) 
        {
            ExpiryDate = expiryDate;
            BillingingInterval = billingInterval;
            User = user;
            InsuranceStatus = InsuranceStatus.Preliminary;
            InsuranceType = insuranceType;
            Notes = notes;
            Customer = customer;
            InsuredPerson = insuredPerson;

        }
    }
}
