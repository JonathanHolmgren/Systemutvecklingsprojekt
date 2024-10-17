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

        private ObservableCollection<Customer> customers = null;
        
        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private ObservableCollection<Customer> prospects = null;
        public ObservableCollection<Customer> Prospects 
        {
            get { return prospects; } 
            set 
            { 
                prospects = value; 
                OnPropertyChanged(nameof(Prospects)); 
            }
        }
        private ObservableCollection<Customer> privateProspects = null;
        public ObservableCollection<Customer> PrivateProspects
        {
            get { return privateProspects; }
            set
            {
                privateProspects = value;
                OnPropertyChanged(nameof(PrivateProspects));
            }
        }
        private ObservableCollection<Customer> companyProspects = null;
        public ObservableCollection<Customer> CompanyProspects
        {
            get { return companyProspects; }
            set
            {
                companyProspects = value;
                OnPropertyChanged(nameof(CompanyProspects));
            }
        }

        private bool isAllSelected;
        public bool IsAllSelected
        {
            get { return isAllSelected; }
            set
            {
                isAllSelected = value;
                OnPropertyChanged(nameof(IsAllSelected));
                UpdateProspectsList();
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

        private Customer prospectSelectedItem;
        public Customer ProspectSelectedItem 
        { 
            get { return prospectSelectedItem; } 
            set
            { 
                prospectSelectedItem = value; 
                OnPropertyChanged(nameof(ProspectSelectedItem));
            }
        }

        public ICommand ShowProspectCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ShowProspectsViewModel()
        {
            Customers = new ObservableCollection<Customer>(customerController.GetAllCustomers());
            
            Prospects = new ObservableCollection<Customer>();
            Prospects = FilterProspects();
            PrivateProspects = new ObservableCollection<Customer>();
            PrivateProspects = FilterPrivateProspects();
            CompanyProspects = new ObservableCollection<Customer>();
            CompanyProspects = FilterCompanyProspects();

            ShowProspectCommand = new RelayCommand(ShowProspect);
            ReturnCommand = new RelayCommand(Return);
        }

        #region Methods
        private ObservableCollection<Customer> FilterProspects()
        {
            Prospects.Clear();
            foreach (var customer in Customers)
            {
                if (customer.Insurances != null && customer.Insurances.Count == 1)
                {
                    Prospects.Add(customer);
                }
            }
            return Prospects;
        }

        private ObservableCollection<Customer> FilterPrivateProspects()
        {
            PrivateProspects.Clear();
            foreach (var customer in Customers)
            {
                if (customer is PrivateCustomer privateCustomer)
                {
                    if (privateCustomer.Insurances != null && privateCustomer.Insurances.Count == 1)
                    {
                        PrivateProspects.Add(privateCustomer);
                    }
                }
            }
            return PrivateProspects;
        }

        private ObservableCollection<Customer> FilterCompanyProspects()
        {
            CompanyProspects.Clear();
            foreach (var customer in Customers)
            {
                if (customer is CompanyCustomer companyCustomer)
                {
                    if (customer.Insurances != null && customer.Insurances.Count == 1)
                    {
                        CompanyProspects.Add(companyCustomer);
                    }
                }
            }
            return CompanyProspects;
        }
        private void UpdateProspectsList()
        {
            if (IsCompanySelected)
            {                
                Prospects = FilterCompanyProspects();
            }
            else if (IsPrivateSelected)
            {                
                Prospects = FilterPrivateProspects();
            }
            else if (IsAllSelected)
            {                
                Prospects = FilterProspects();
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
