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
    public class RegisterPreliminaryInsuranceViewModel:ObservableObject
    {
        InsuranceController insuranceController;

        public RegisterPreliminaryInsuranceViewModel()
        {
            insuranceController = new InsuranceController();
            LoadCustomerData();
        }

        private PrivateCustomer privateCustomer;
        public PrivateCustomer PrivateCustomer
        {
            get => PrivateCustomer;
            set 
            {
                privateCustomer = value;
                OnPropertyChanged();
            }
        }

        PostalCodeCity postalCodeCity = new PostalCodeCity
        {
            City = "Borås",
            PostalCode = "50630"
        };

        private void LoadCustomerData()
        {
            PrivateCustomer privateCustomer1 = new PrivateCustomer(
            "0706689932",
            "ingalillblommor52@emial.com",
            "Gatuvägen 21",
            postalCodeCity,
            "19521019-1234",
            "Inga-Lill",
            "Bengtsson"
        );









        }


    }
}
