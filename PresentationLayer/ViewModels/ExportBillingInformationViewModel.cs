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
        public ExportBillingInformationViewModel()
        {
            PrivateCustomers = new ObservableCollection<PrivateCustomer>(customerController.GetAllPrivateCustomers());
            CompanyCustomers = new ObservableCollection<CompanyCustomer>(customerController.GetAllCompanyCustomers());
            
        }
    }    
}
