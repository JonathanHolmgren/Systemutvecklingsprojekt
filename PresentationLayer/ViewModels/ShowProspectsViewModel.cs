using PresentationLayer.Models;
using ServiceLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Command;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    internal class ShowProspectsViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();

        private string prospectNote;
        public string ProspectNote
        {
            get { return prospectNote; }
            set { prospectNote = value; OnPropertyChanged(nameof(ProspectNote)); }
        }

        private ObservableCollection<PrivateCustomer> privateCustomers = null;
        public ObservableCollection<PrivateCustomer> PrivateCustomers
        {
            get { return privateCustomers; }
            set
            {
                privateCustomers = value;
                OnPropertyChanged(nameof(PrivateCustomers));
            }
        }

        private ObservableCollection<CompanyCustomer> companyCustomers = null;
        public ObservableCollection<CompanyCustomer> CompanyCustomers
        {
            get { return companyCustomers; }
            set
            {
                companyCustomers = value;
                OnPropertyChanged(nameof(CompanyCustomers));
            }
        }

        private ObservableCollection<PrivateCustomer> privateProspects;
        public ObservableCollection<PrivateCustomer> PrivateProspects
        {
            get { return privateProspects; }
            set
            {
                privateProspects = value;
                OnPropertyChanged(nameof(PrivateProspects));
            }
        }

        private ObservableCollection<CompanyCustomer> companyProspects;
        public ObservableCollection<CompanyCustomer> CompanyProspects
        {
            get { return companyProspects; }
            set
            {
                companyProspects = value;
                OnPropertyChanged(nameof(CompanyProspects));
            }
        }

        private bool isCompanySelected;
        public bool IsCompanySelected
        {
            get { return isCompanySelected; }
            set
            {
                isCompanySelected = value;
                OnPropertyChanged(nameof(IsCompanySelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateProspectsList();
            }
        }

        private bool isPrivateSelected;
        public bool IsPrivateSelected
        {
            get { return isPrivateSelected; }
            set
            {
                isPrivateSelected = value;
                OnPropertyChanged(nameof(IsPrivateSelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateProspectsList();
            }
        }
        public bool IsPrivateColumnVisible => IsPrivateSelected;
        public bool IsCompanyColumnVisible => IsCompanySelected;

        
        private PrivateCustomer privateProspectSelectedItem;
        public PrivateCustomer PrivateProspectSelectedItem 
        { 
            get { return privateProspectSelectedItem; } 
            set
            { 
                privateProspectSelectedItem = value; 
                OnPropertyChanged(nameof(PrivateProspectSelectedItem));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PrivateSalesPersonAgentnumber));
            }
        }

        private CompanyCustomer companyProspectSelectedItem;
        public CompanyCustomer CompanyProspectSelectedItem
        {
            get { return companyProspectSelectedItem; }
            set
            {
                companyProspectSelectedItem = value;
                OnPropertyChanged(nameof(CompanyProspectSelectedItem));
                OnPropertyChanged(nameof(CompanySalesPersonAgentnumber));
            }
        }

        public string FullName
        {
            get
            {
                if (PrivateProspectSelectedItem != null)
                {
                    return $"{PrivateProspectSelectedItem.FirstName} {PrivateProspectSelectedItem.LastName}";
                }
                return string.Empty;
            }
        }

        public string PrivateSalesPersonAgentnumber
        {
            get
            {
                if (PrivateProspectSelectedItem != null)
                {
                    return string.Join(", ", PrivateProspectSelectedItem.Insurances.Select(i => i.User.Employee.AgentNumber));
                }
                return string.Empty;
            }
        }
        public string CompanySalesPersonAgentnumber
        {
            get
            {
                if (CompanyProspectSelectedItem != null)
                {
                    return string.Join(", ", CompanyProspectSelectedItem.Insurances.Select(u => u.User.Employee.AgentNumber));
                }
                return string.Empty;
            }
        }

        public ICommand ShowProspectCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }

        public ShowProspectsViewModel()
        {
            PrivateCustomers = new ObservableCollection<PrivateCustomer>(customerController.GetPrivateCustomerList());
            CompanyCustomers = new ObservableCollection<CompanyCustomer>(customerController.GetCompanyCustomerList());

            PrivateProspects = new ObservableCollection<PrivateCustomer>(FilterPrivateProspects());
            CompanyProspects = new ObservableCollection<CompanyCustomer>(FilterCompanyProspects());

            ShowProspectCommand = new RelayCommand(ShowProspect);
            ReturnCommand = new RelayCommand(Return);

            IsPrivateSelected = true;
        }



        #region Methods

        private ObservableCollection<PrivateCustomer> FilterPrivateProspects()
        {
            ObservableCollection<PrivateCustomer> filteredList = new ObservableCollection<PrivateCustomer>(customerController.GetPrivateCustomerList());

            if (PrivateCustomers != null) 
            { 
                foreach (var privateCustomer in PrivateCustomers)
                {
                    if (privateCustomer.Insurances != null && privateCustomer.Insurances.Count == 1 && !filteredList.Any(p => p.CustomerID == privateCustomer.CustomerID))
                    {
                        filteredList.Add(privateCustomer);
                    }
                }
            }

            return filteredList; 
        }
     
        private ObservableCollection<CompanyCustomer> FilterCompanyProspects()
        {
            ObservableCollection<CompanyCustomer> filteredList = new ObservableCollection<CompanyCustomer>(customerController.GetCompanyCustomerList());

            if (CompanyCustomers != null)
            {
                foreach (var companyCustomer in CompanyCustomers)
                {
                    if (companyCustomer.Insurances != null && companyCustomer.Insurances.Count == 1 && !filteredList.Any(p => p.CustomerID == companyCustomer.CustomerID))
                    {
                        filteredList.Add(companyCustomer);
                    }
                }
            }
            return filteredList; 
        }

        private void UpdateProspectsList()
        {
            if (IsCompanySelected)
            {                
                CompanyProspects = FilterCompanyProspects();
            }
            else if (IsPrivateSelected)
            {                
                PrivateProspects = FilterPrivateProspects();
            }
        }

        //Show a more detailed view of the prospect
        private void ShowProspect()
        {

        }

        //Return to main menu
        private void Return() 
        {
            
        }
        #endregion
    }
}
