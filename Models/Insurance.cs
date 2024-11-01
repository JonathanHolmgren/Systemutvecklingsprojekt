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
        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public BillingInterval BillingInterval { get; set; }
        public InsuranceStatus InsuranceStatus { get; set; }
        public string? Notes { get; set; }

        // Navigation property
        public User? User { get; set; }
        public InsuredPerson? InsuredPerson { get; set; }
        public Customer Customer { get; set; }
        public InsuranceType InsuranceType { get; set; }

        //Constructors
        public Insurance() { }

        public Insurance(
            DateTime expiryDate,
            BillingInterval billingingInterval,
            InsuranceStatus insuranceStatus,
            User user,
            InsuredPerson insuredPerson,
            Customer customer,
            InsuranceType insuranceType
        )
        {
            ExpiryDate = expiryDate;
            BillingInterval = billingingInterval;
            InsuranceStatus = insuranceStatus;
            User = user;
            InsuredPerson = insuredPerson;
            Customer = customer;
            InsuranceType = insuranceType;
        }

        public Insurance(
            DateTime expiryDate,
            BillingInterval billingInterval,
            InsuranceStatus insuranceStatus,
            string notes,
            User user,
            InsuredPerson insuredPerson,
            Customer customer,
            InsuranceType insuranceType
        )
        {
            ExpiryDate = expiryDate;
            BillingInterval = billingInterval;
            InsuranceStatus = insuranceStatus;
            Notes = notes;
            User = user;
            InsuredPerson = insuredPerson;
            Customer = customer;
            InsuranceType = insuranceType;
        }

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
            BillingInterval = billingInterval;
            User = user;
            InsuranceStatus = InsuranceStatus.Preliminary;
            InsuranceType = insuranceType;
            Notes = notes;
            Customer = customer;
            InsuredPerson = insuredPerson;
        }

        public Insurance(
            DateTime activationDate,
            DateTime expiryDate,
            BillingInterval billingInterval,
            User? user,
            Customer customer,
            InsuranceType insuranceType
        )
        {
            ActivationDate = activationDate;
            ExpiryDate = expiryDate;
            BillingInterval = billingInterval;
            InsuranceStatus = InsuranceStatus.Preliminary;
            User = user;
            Customer = customer;
            InsuranceType = insuranceType;
        }
    }
}
