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
                    ApplyFilter();
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
            
            GetAllCompanyCustomerInsurances();
            GetAllPrivateCustomerInsurances();
            ApplyFilter();
        }
        #endregion
        #region Methods
        private void GetAllPrivateCustomerInsurances()
        {
            InsuranceInfoPrivateCustomer = new ObservableCollection<object>();

            foreach (PrivateCustomer privateCustomer in PrivateCustomers)
            {
                
                double totalPremie = customerController.GetCustomerTotalPremie(privateCustomer.CustomerID);

                ObservableCollection<string> insuranceDetails = new ObservableCollection<string>();
                foreach (Insurance insurance in privateCustomer.Insurances)
                {
                    string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                    insuranceDetails.Add(insuranceName);
                }

                var customerInfo = new
                {
                    FirstName= privateCustomer.FirstName,
                    LastName= privateCustomer.LastName,
                    SSN = privateCustomer.SSN,
                    Address = privateCustomer.StreetAddress,
                    PostalCode = privateCustomer.PostalCodeCity.PostalCode,
                    City = privateCustomer.PostalCodeCity.City,
                    TotalPremium = totalPremie,
                    InsuranceSummary = insuranceDetails
                };
                InsuranceInfoPrivateCustomer.Add(customerInfo);
            }
        }
        private void GetAllCompanyCustomerInsurances()
        {
            InsuranceInfoCompanyCustomer = new ObservableCollection<object>(); //

            foreach (CompanyCustomer companyCustomer in CompanyCustomers)
            {
                
                double totalPremie = customerController.GetCustomerTotalPremie(companyCustomer.CustomerID);

                
                ObservableCollection<string> insuranceDetails = new ObservableCollection<string>();
                foreach (Insurance insurance in companyCustomer.Insurances)
                {
                    string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                    insuranceDetails.Add(insuranceName);
                }

                
                var companyInfo = new
                {
                    CompanyName = companyCustomer.CompanyName,
                    OrganisationNumber = companyCustomer.OrganisationNumber,
                    ContactPerson = companyCustomer.ContactPersonName,
                    Address = companyCustomer.StreetAddress,
                    PostalCode = companyCustomer.PostalCodeCity.PostalCode,
                    City = companyCustomer.PostalCodeCity.City,
                    TotalPremium = totalPremie,
                    InsuranceSummary = insuranceDetails
                };

               
                InsuranceInfoCompanyCustomer.Add(companyInfo);
            }
        }
        private void ExportSingleToJson()
        {
            string jsonResult=JsonConvert.SerializeObject(SelectedCustomer, Formatting.Indented);
            string outputPath = @"C:\JsonTest\CustomerInformationSingle.json";
            //File.WriteAllText(outputPath, jsonResult);
        }
        private void ExportAllToJson()
        {
            var customerDataToJson = new
            {
                PrivateCustomers = InsuranceInfoPrivateCustomer,
                CompanyCustomers = InsuranceInfoCompanyCustomer
            };
            string jsonResult = JsonConvert.SerializeObject(customerDataToJson, Formatting.Indented);
            string outputPath = @"C:\JsonTest\CustomerInformation.json";
            //File.WriteAllText(outputPath, jsonResult);
        }
        private void ApplyFilter()
        {
            
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                
                    FilteredListCompany = new ObservableCollection<object>(InsuranceInfoCompanyCustomer);
                    OnPropertyChanged(nameof(FilteredListCompany));
                
                    FilteredListPrivate = new ObservableCollection<object>(InsuranceInfoPrivateCustomer);
                    OnPropertyChanged(nameof(FilteredListPrivate));
            }

  

            if (IsCompanyCustomerSelected&&FilterText!=null)
            {
                FilteredListCompany.Clear();
                foreach (CompanyCustomer companyCustomer in CompanyCustomers)
                {
                    if (companyCustomer.OrganisationNumber.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                        companyCustomer.CompanyName.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    {
                        
                        double totalPremie = customerController.GetCustomerTotalPremie(companyCustomer.CustomerID);

                        ObservableCollection<string> insuranceDetails = new ObservableCollection<string>();
                        foreach (Insurance insurance in companyCustomer.Insurances)
                        {
                            string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                            insuranceDetails.Add(insuranceName);
                        }

                        
                        var companyInfo = new
                        {
                            CompanyName = companyCustomer.CompanyName,
                            OrganisationNumber = companyCustomer.OrganisationNumber,
                            ContactPerson = companyCustomer.ContactPersonName,
                            Address = companyCustomer.StreetAddress,
                            PostalCode = companyCustomer.PostalCodeCity.PostalCode,
                            City = companyCustomer.PostalCodeCity.City,
                            TotalPremium = totalPremie,
                            InsuranceSummary = insuranceDetails
                        };

                        
                        FilteredListCompany.Add(companyInfo);
                    }
                }
            }
            else if (IsPrivateCustomerSelected&&FilterText!=null)
            {
                FilteredListPrivate.Clear();
                foreach (PrivateCustomer privateCustomer in PrivateCustomers)
                {
                    if (privateCustomer.FirstName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                        privateCustomer.LastName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                        privateCustomer.SSN.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    {
                        
                        double totalPremie = customerController.GetCustomerTotalPremie(privateCustomer.CustomerID);

                        
                        ObservableCollection<string> insuranceDetails = new ObservableCollection<string>();
                        foreach (Insurance insurance in privateCustomer.Insurances)
                        {
                            string insuranceName = customerController.GetCustomerInsuranceTypes(insurance.InsuranceType.InsuranceTypeId);
                            insuranceDetails.Add(insuranceName);
                        }

                        
                        var privateInfo = new
                        {
                            FirstName = privateCustomer.FirstName,
                            LastName = privateCustomer.LastName,
                            SSN = privateCustomer.SSN,
                            Address = privateCustomer.StreetAddress,
                            PostalCode = privateCustomer.PostalCodeCity.PostalCode,
                            City = privateCustomer.PostalCodeCity.City,
                            TotalPremium = totalPremie,
                            InsuranceSummary = insuranceDetails
                        };

                        
                        FilteredListPrivate.Add(privateInfo);
                    }
                }
            }

            
            OnPropertyChanged(nameof(FilteredListPrivate));
        }
        #endregion

    }
}
