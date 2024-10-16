using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    
    public class RegisterPreliminaryInsuranceViewModel : ObservableObject
    {
        private InsuranceController insuranceController;

        private PrivateCustomer selectedPrivateCustomer;
        public PrivateCustomer SelectedPrivateCustomer
        {
            get => selectedPrivateCustomer;
            set
            {
                selectedPrivateCustomer = value;
                OnPropertyChanged();
            }
        }

        #region Customer
        private PostalCodeCity postalCodeCity = new PostalCodeCity
        {
            City = "Borås",
            PostalCode = "50630"
        };

        private void LoadCustomerData()
        {
            // Tilldela direkt till egenskapen för att trigga OnPropertyChanged.
            SelectedPrivateCustomer = new PrivateCustomer(
                "0706689932",
                "ingalillblommor52@emial.com",
                "Gatuvägen 21",
                postalCodeCity,
                "19521019-1234",
                "Inga-Lill",
                "Bengtsson",
                "0346-58948");
        }
        #endregion

        public RegisterPreliminaryInsuranceViewModel()
        {
            insuranceController = new InsuranceController();
            LoadCustomerData();
        }
    }
}
