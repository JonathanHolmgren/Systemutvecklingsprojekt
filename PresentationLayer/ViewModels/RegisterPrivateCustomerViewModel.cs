using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using ServiceLayer;
using PresentationLayer.Command;

namespace PresentationLayer.ViewModels
{
    public class RegisterPrivateCustomerViewModel : INotifyPropertyChanged
    {
        private CustomerController customerController = new CustomerController();

        private string ssn;
        public string SSN 
        { 
            get => ssn;
            set { ssn = value; OnPropertyChanged(nameof(SSN)); } 
        }

        private string firstname;
        public string FirstName
        {
            get => firstname;
            set { firstname = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string lastname;
        public string LastName
        {
            get => lastname;
            set { lastname = value; OnPropertyChanged(nameof(LastName)); }
        }

        private string streetadress;
        public string StreetAdress
        {
            get => streetadress;
            set { streetadress = value; OnPropertyChanged(nameof(StreetAdress)); }
        }

        private string postalcode;
        public string PostalCode
        {
            get => postalcode;
            set { postalcode = value; OnPropertyChanged(nameof(PostalCode)); }
        }

        private string city;
        public string City
        {
            get => city;
            set { city = value; OnPropertyChanged(nameof(City)); }
        }

        private string telephonenumber;
        public string TelephoneNumber
        {
            get => telephonenumber;
            set { telephonenumber = value; OnPropertyChanged(nameof(TelephoneNumber)); }
        }

        private string worktelephonenumber;
        public string WorkTelephoneNumber
        {
            get => worktelephonenumber;
            set { worktelephonenumber = value; OnPropertyChanged(nameof(WorkTelephoneNumber)); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public ICommand AddPrivateCustomerCommand { get; private set; }

        public RegisterPrivateCustomerViewModel()
        {
            AddPrivateCustomerCommand = new RelayCommand(AddPrivateCustomer);
        }

        private void AddPrivateCustomer()
        {
            if (string.IsNullOrEmpty(SSN) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) ||
            string.IsNullOrEmpty(StreetAdress) || string.IsNullOrEmpty(postalcode) || string.IsNullOrEmpty(city) ||
            string.IsNullOrEmpty(TelephoneNumber) || string.IsNullOrEmpty(WorkTelephoneNumber) || string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Var god fyll i alla fält");
            }
            else
            {
                PostalCodeCity postalCodeCity = AddPostalCodeCity(PostalCode, City);
                customerController.RegisterPrivateCustomer(SSN, FirstName, LastName, TelephoneNumber, WorkTelephoneNumber, Email, StreetAdress, postalCodeCity);
                MessageBox.Show("Kunden är tillagd");
            }
        }

        private PostalCodeCity AddPostalCodeCity(string postalCode, string city)
        {
            PostalCodeCity postalcodecity = new PostalCodeCity(postalCode, city);
            return postalcodecity;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
