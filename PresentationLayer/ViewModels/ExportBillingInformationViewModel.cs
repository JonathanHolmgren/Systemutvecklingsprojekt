using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    internal class ExportBillingInformationViewModel:ObservableObject
    {
        CustomerController customerController = new CustomerController();
        private ObservableCollection<Customer> allCustomers = null;
        public ObservableCollection<Customer> AllCustomers
        {
            get { return allCustomers; }
            set
            {
                allCustomers = value;
                OnPropertyChanged(nameof(AllCustomers));
            }
        }
        public ExportBillingInformationViewModel()
        {
            AllCustomers = new ObservableCollection<Customer>(customerController.GetAll());
        }
        //Separates private and company customers
        private void SeparateCustomers()
        {
            ObservableCollection<PrivateCustomer> privateCustomers = allCustomers;

        }
    }
    
}
