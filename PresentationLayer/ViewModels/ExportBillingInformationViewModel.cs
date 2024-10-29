using Microsoft.EntityFrameworkCore.Query.Internal;
using Models;
using Newtonsoft.Json;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;


namespace PresentationLayer.ViewModels
{
    #region Initiation of objects
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
        private ObservableCollection<object> filteredListPrivate;
        public ObservableCollection<object> FilteredListPrivate
        {
            get { return filteredListPrivate; }
            set
            {
                filteredListPrivate = value;
                OnPropertyChanged(nameof(FilteredListPrivate));
            }
        }
        private ObservableCollection<object> filteredListCompany;
        public ObservableCollection<object> FilteredListCompany
        {
            get { return filteredListCompany; }
            set
            {
                filteredListCompany = value;
                OnPropertyChanged(nameof(FilteredListCompany));
            }
        }
        private object selectedCustomer;
        public object SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(selectedCustomer));
            }
        }
        private bool isPrivateCustomerSelected = true; 

        public bool IsPrivateCustomerSelected
        {
            get => isPrivateCustomerSelected;
            set
            {
                isPrivateCustomerSelected = value;
                OnPropertyChanged(nameof(IsPrivateCustomerSelected));
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
                if (value) IsPrivateCustomerSelected = false;
            }
        }
        private string filterText = null;
        public string FilterText
        {
            get { return filterText; }
            set
            {
                if (filterText != value)
                {
                    filterText = value;
                    ApplyFilter(filterText);
                    OnPropertyChanged(nameof(FilterText));

                }

            }
        }
        public ICommand ExportAllToJsonCommand { get; private set; }
        public ICommand ExportSingleToJsonCommand { get; private set; }
        #endregion
        #region Constructor
        public ExportBillingInformationViewModel()
        {
            PrivateCustomers = new ObservableCollection<PrivateCustomer>(customerController.GetAllPrivateCustomers());
            CompanyCustomers = new ObservableCollection<CompanyCustomer>(customerController.GetAllCompanyCustomers());
            ExportAllToJsonCommand = new RelayCommand<object>(execute => ExportAllToJson());
            ExportSingleToJsonCommand = new RelayCommand<object>(execute => ExportSingleToJson());
            
            CompanyCustomerInsuranceInfoToList(CompanyCustomers);
            PrivateCustomerInsurancesInfoToList(PrivateCustomers);
            ApplyFilter(null);
        }
        #endregion
        #region Methods
        private void PrivateCustomerInsurancesInfoToList(IList<PrivateCustomer> privateCustomers) //Converting private customer list to a list och private customer information
        {
            InsuranceInfoPrivateCustomer = new ObservableCollection<object>();

            foreach (PrivateCustomer privateCustomer in PrivateCustomers)
            {
                var customerInfo = CreatePrivateCustomerInfo(privateCustomer);
                
                if(customerController.GetCustomerPremie(privateCustomer.CustomerID)>0)
                {
                    InsuranceInfoPrivateCustomer.Add(customerInfo);
                }
                    
                
            }
        }
        private void CompanyCustomerInsuranceInfoToList(IList<CompanyCustomer> companyCustomers)//Converting company customer list to a list och private customer information
        {
            InsuranceInfoCompanyCustomer = new ObservableCollection<object>();
            foreach (CompanyCustomer companyCustomer in CompanyCustomers)
            {
                
                var companyInfo = CreateCompanyCustomerInfo(companyCustomer);
                

                if (customerController.GetCustomerPremie(companyCustomer.CustomerID) > 0)
                {
                    InsuranceInfoCompanyCustomer.Add(companyInfo);
                }
            }
        }
        
        private void ExportSingleToJson() //Exporting a single selected customer to Json
        {
            string jsonResult=JsonConvert.SerializeObject(SelectedCustomer, Formatting.Indented);
            customerController.ExportObjectToJson(jsonResult);
        }
        private void ExportAllToJson()
        {
            var customerDataToJson = new
            {
                
                PrivateCustomers = InsuranceInfoPrivateCustomer,
                CompanyCustomers = InsuranceInfoCompanyCustomer
            };

            customerController.ExportObjectToJson(customerDataToJson);
        } //Exporting all due customer to Json
        private void ApplyFilter(string filterText) //Apply filter method to chceck what to show 
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                ResetFilteredLists();
            }
            else if (IsCompanyCustomerSelected)
            {
                FilterCompanyCustomers(filterText);
            }
            else if (IsPrivateCustomerSelected)
            {
                FilterPrivateCustomers(filterText);
            }
            OnPropertyChanged(nameof(FilteredListPrivate));
            OnPropertyChanged(nameof(FilteredListCompany));
        }
        private void ResetFilteredLists() //Reseting the list to its original state
        {
            FilteredListCompany = new ObservableCollection<object>(InsuranceInfoCompanyCustomer);
            OnPropertyChanged(nameof(FilteredListCompany));

            FilteredListPrivate = new ObservableCollection<object>(InsuranceInfoPrivateCustomer);
            OnPropertyChanged(nameof(FilteredListPrivate));
        }
        private void FilterCompanyCustomers(string filterText) //Applying the filter text to company customers
        {
            FilteredListCompany.Clear();

            foreach (CompanyCustomer companyCustomer in CompanyCustomers)
            {
                if (IsCompanyCustomerMatch(companyCustomer, filterText))
                {
                    var companyInfo = CreateCompanyCustomerInfo(companyCustomer);
                    if (customerController.GetCustomerPremie(companyCustomer.CustomerID)>0)
                    {
                        FilteredListCompany.Add(companyInfo);
                    }
                }
            }
        }
        private void FilterPrivateCustomers(string filterText) //Applying the filter text to private customers
        {
            FilteredListPrivate.Clear();

            foreach (PrivateCustomer privateCustomer in PrivateCustomers)
            {
                if (IsPrivateCustomerMatch(privateCustomer, filterText))
                {
                    var privateInfo = CreatePrivateCustomerInfo(privateCustomer);
                    if (customerController.GetCustomerPremie(privateCustomer.CustomerID) > 0)
                    {
                        FilteredListPrivate.Add(privateInfo);
                    }
                }
            }
        }
        private bool IsCompanyCustomerMatch(CompanyCustomer companyCustomer, string filterText)
        {
            return companyCustomer.OrganisationNumber.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                   companyCustomer.CompanyName.Contains(filterText, StringComparison.OrdinalIgnoreCase);
        }
        private bool IsPrivateCustomerMatch(PrivateCustomer privateCustomer, string filterText)
        {
            return privateCustomer.FirstName.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                   privateCustomer.LastName.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                   privateCustomer.SSN.Contains(filterText, StringComparison.OrdinalIgnoreCase);
        }
        private object CreatePrivateCustomerInfo(PrivateCustomer privateCustomer) //Creating the private customer and adding the attributes
        {
            double totalPremie = customerController.GetCustomerPremie(privateCustomer.CustomerID);
            ObservableCollection<string> insuranceDetails = GetInsuranceDetails(privateCustomer);

            return new
            {
                FirstName = privateCustomer.FirstName,
                LastName = privateCustomer.LastName,
                SSN = privateCustomer.SSN,
                Address = privateCustomer.StreetAddress,
                PostalCode = privateCustomer.PostalCode,
                City = privateCustomer.City,
                TotalPremium = totalPremie,
                InsuranceSummary = insuranceDetails
            };
        }
        private object CreateCompanyCustomerInfo(CompanyCustomer companyCustomer) //Creating the customer customer and adding the attributes
        {
            double totalPremie = customerController.GetCustomerPremie(companyCustomer.CustomerID);
            ObservableCollection<string> insuranceDetails = GetInsuranceDetails(companyCustomer);

            return new
            {
                CompanyName = companyCustomer.CompanyName,
                OrganisationNumber = companyCustomer.OrganisationNumber,
                ContactPerson = companyCustomer.ContactPersonName,
                Address = companyCustomer.StreetAddress,
                PostalCode = companyCustomer.PostalCode,
                City = companyCustomer.City,
                TotalPremium = totalPremie,
                InsuranceSummary = insuranceDetails
            };
        }


        private ObservableCollection<string> GetInsuranceDetails(Customer customer) //Getting the extra insurances details that is not connected to the customer
        {
            ObservableCollection<string> insuranceDetails = new ObservableCollection<string>();

            foreach (Insurance insurance in customer.Insurances)
            {
                if (customerController.CalculatePremiePerInsurance(insurance) > 0)
                {
                    string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceId);
                    insuranceDetails.Add(insuranceName);
                }
            }

            return insuranceDetails;
        }


    }
    #endregion
}
