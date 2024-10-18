using Models;
using PresentationLayer.Models;
using ServiceLayer;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PresentationLayer.ViewModels
{
    internal class ExportBillingInformationViewModel:ObservableObject
    {
        CustomerController customerController = new CustomerController();
        private ObservableCollection<PrivateCustomer> privateCustomer = null;
        public ObservableCollection<PrivateCustomer> PrivateCustomers
        {
            get { return privateCustomer; }
            set
            {
                privateCustomer = value;
                OnPropertyChanged(nameof(PrivateCustomers));
            }
        }
        private ObservableCollection<CompanyCustomer>companyCustomer  = null;
        public ObservableCollection<CompanyCustomer> CompanyCustomers
        {
            get { return companyCustomer; }
            set
            {
                companyCustomer = value;
                OnPropertyChanged(nameof(CompanyCustomers));
            }
        }
        private ObservableCollection<object> insuranceInfoPrivateCustomer = null;
        public ObservableCollection<object> InsuranceInfoPrivateCustomer
        {
            get { return insuranceInfoPrivateCustomer; }
            set
            {
                insuranceInfoPrivateCustomer = value;
                OnPropertyChanged(nameof(InsuranceInfoPrivateCustomer));
            }
        }
        private ObservableCollection<object> insuranceInfoCompanyCustomer = null;
        public ObservableCollection<object> InsuranceInfoCompanyCustomer
        {
            get { return insuranceInfoCompanyCustomer; }
            set
            {
                insuranceInfoCompanyCustomer = value;
                OnPropertyChanged(nameof(InsuranceInfoCompanyCustomer));
            }
        }
        private double premie;
        public double Premie
        {
            get { return premie; }
            set { premie = value; OnPropertyChanged(nameof(Premie)); }
        }
        
        private bool isPrivateCustomerSelected = true; // Default to private customers

        public bool IsPrivateCustomerSelected
        {
            get => isPrivateCustomerSelected;
            set
            {
                isPrivateCustomerSelected = value;
                OnPropertyChanged(nameof(IsPrivateCustomerSelected));
                // Set the other property to false when this one is set to true
                if (value) IsCompanyCustomerSelected = false;
            }
        }

        private bool isCompanyCustomerSelected;

        public bool IsCompanyCustomerSelected
        {
            get => isCompanyCustomerSelected;
            set
            {
                isCompanyCustomerSelected = value;
                OnPropertyChanged(nameof(IsCompanyCustomerSelected));
                // Set the other property to false when this one is set to true
                if (value) IsPrivateCustomerSelected = false;
            }
        }

        public ExportBillingInformationViewModel()
        {
            PrivateCustomers = new ObservableCollection<PrivateCustomer>(customerController.GetAllPrivateCustomers());
            CompanyCustomers = new ObservableCollection<CompanyCustomer>(customerController.GetAllCompanyCustomers());
            
            GetAllCompanyCustomerInsurances();
            GetAllPrivateCustomerInsurances();
        }


        //private void GetAllPrivateCustomerInsúrances()
        //{
        //    //foreach (PrivateCustomer privateCustomer in PrivateCustomers)
        //    //{
        //    //    Premie = customerController.GetCustomerTotalPremie(privateCustomer.CustomerID);
        //    //    InsuranceSpec insuranceSpecTot = new InsuranceSpec();
        //    //    insuranceSpecTot.Value = Premie.ToString();
        //    //    foreach (Insurance eachInsurance in privateCustomer.Insurances)
        //    //    {
        //    //        insuranceSpecTot.Insurance = eachInsurance; 

        //    //        break;
        //    //    }
        //    //    InsuranceInfoPrivateCustomer.Add(insuranceSpecTot);
        //    //    Debug.WriteLine(insuranceSpecTot.Insurance.InsuranceId);
        //    //}


        //}
        private void GetAllPrivateCustomerInsurances()
        {
            InsuranceInfoPrivateCustomer = new ObservableCollection<object>(); // Use object to hold anonymous types

            foreach (PrivateCustomer privateCustomer in PrivateCustomers)
            {
                
                double totalPremie = customerController.GetCustomerTotalPremie(privateCustomer.CustomerID);

                List<string> insuranceDetails = new List<string>();

                foreach (Insurance insurance in privateCustomer.Insurances)
                {
                    string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                    insuranceDetails.Add(insuranceName);
                }

                var customerInfo = new
                {
                    FullName = $"{privateCustomer.FirstName} {privateCustomer.LastName}",
                    SSN = privateCustomer.SSN,
                    Address = privateCustomer.StreetAddress,
                    PostalCode = privateCustomer.PostalCodeCity.PostalCode,
                    City = privateCustomer.PostalCodeCity.City,
                    TotalPremium = totalPremie,
                    InsuranceSummary = string.Join(", ", insuranceDetails)
                };
                InsuranceInfoPrivateCustomer.Add(customerInfo);
            }
        }
        private void GetAllCompanyCustomerInsurances()
        {
            InsuranceInfoCompanyCustomer = new ObservableCollection<object>(); // Use object to hold anonymous types

            foreach (CompanyCustomer companyCustomer in CompanyCustomers)
            {
                // Calculate total premium for the company's insurances
                double totalPremie = customerController.GetCustomerTotalPremie(companyCustomer.CustomerID);

                // Create a list to hold insurance information
                List<string> insuranceDetails = new List<string>();

                foreach (Insurance insurance in companyCustomer.Insurances)
                {
                    string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                    insuranceDetails.Add(insuranceName);
                }

                // Build an anonymous type to hold company info and insurance details
                var companyInfo = new
                {
                    CompanyName = companyCustomer.CompanyName,
                    OrganisationNumber = companyCustomer.OrganisationNumber,
                    ContactPerson = companyCustomer.ContactPersonName,
                    Address = companyCustomer.StreetAddress,
                    PostalCode = companyCustomer.PostalCodeCity.PostalCode,
                    City = companyCustomer.PostalCodeCity.City,
                    TotalPremium = totalPremie,
                    InsuranceSummary = string.Join(", ", insuranceDetails) // Join insurance names into a single string
                };

                // Add to the collection
                InsuranceInfoCompanyCustomer.Add(companyInfo);
            }
        }




    }
}
