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
            ShowProspectCommand = new RelayCommand(ShowProspect);
            ReturnCommand = new RelayCommand(Return);

        }

        private ObservableCollection<Customer> FilterProspects()
        {
            foreach (var customer in Customers)
            {
                if (customer.Insurances != null && customer.Insurances.Count == 1)
                {
                    Prospects.Add(customer);
                }
            }
            return Prospects;
        }

        private void ShowProspect()
        {

        }
        private void Return()
        {

        }
    }
}
