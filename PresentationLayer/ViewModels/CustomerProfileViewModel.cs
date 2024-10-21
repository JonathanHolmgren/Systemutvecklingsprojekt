using PresentationLayer.Models;
using ServiceLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    internal class CustomerProfileViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();

        private string searchValue;
        public string SearchValue
        {
            get { return searchValue; }
            set { searchValue = value; OnPropertyChanged(nameof(SearchValue)); }
        }

        public ICommand SearchPrivateCustomerCommand { get; private set; }
        public ICommand AddPrivateNoteCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public CustomerProfileViewModel()
        {
            
        }


    }
}
