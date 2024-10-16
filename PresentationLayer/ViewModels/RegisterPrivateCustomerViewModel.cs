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
using System.Text.RegularExpressions;

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
            if (string.IsNullOrEmpty(SSN))
            {
                MessageBox.Show("Personnummer (SSN) saknas");
                return;
            }

            if (string.IsNullOrEmpty(FirstName))
            {
                MessageBox.Show("Förnamn saknas");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                MessageBox.Show("Efternamn saknas");
                return;
            }

            if (string.IsNullOrEmpty(StreetAdress))
            {
                MessageBox.Show("Gatuadress saknas");
                return;
            }

            if (string.IsNullOrEmpty(PostalCode))
            {
                MessageBox.Show("Postnummer saknas");
                return;
            }

            if (string.IsNullOrEmpty(City))
            {
                MessageBox.Show("Stad saknas");
                return;
            }

            if (string.IsNullOrEmpty(TelephoneNumber))
            {
                MessageBox.Show("Telefonnummer saknas");
                return;
            }

            if (string.IsNullOrEmpty(WorkTelephoneNumber))
            {
                MessageBox.Show("Arbetstelefonnummer saknas");
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("E-postadress saknas");
                return;
            }

            //Check that postalcode is 5 letters
            if (PostalCode.Length != 5 || !PostalCode.All(char.IsDigit))
            {
                MessageBox.Show("Postnumret måste vara exakt 5 siffror");
                return;
            }

            //Check that SSN is 10 letters
            if (SSN.Length != 10 || !SSN.All(char.IsDigit))
            {
                MessageBox.Show("Personnumret måste innehålla 10 siffror");
                return;
            }
          
                FirstName = CapitalizeFirstLetter(FirstName);
                LastName = CapitalizeFirstLetter(LastName);
                City = CapitalizeFirstLetter(City);
                StreetAdress = CapitalizeFirstLetter(StreetAdress);

            try
            {
                PostalCodeCity postalCodeCity = AddPostalCodeCity(PostalCode, City);
                PrivateCustomer privateCustomer = new PrivateCustomer(TelephoneNumber, Email, StreetAdress, postalCodeCity, SSN, FirstName, LastName, WorkTelephoneNumber);
                customerController.AddCustomer(privateCustomer);
                MessageBox.Show("Kunden är tillagd");
            }
            catch (Exception ex)
            {
                // Visa felmeddelande om något går fel vid skapandet av objektet eller lagringen
                MessageBox.Show($"Ett fel uppstod: {ex.Message}");
            }

        }
        private PostalCodeCity AddPostalCodeCity(string postalcode, string city)
        {
            PostalCodeCity newpostalCodeCity = new PostalCodeCity(postalcode, city);
            return newpostalCodeCity;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Felhanteringsmetoder
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        #endregion
    }
}
