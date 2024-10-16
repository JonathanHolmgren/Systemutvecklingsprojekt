using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    internal class RegisterCompanyCustomerViewModel:ObservableObject
    {
        CustomerController customerController = new CustomerController();
        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; OnPropertyChanged(nameof(CompanyName)); }
        }

        private string organisationNumber;
        public string OrganisationNumber
        {
            get { return organisationNumber; }
            set { organisationNumber = value; OnPropertyChanged(nameof(OrganisationNumber)); }
        }
        private string contactPersonName;
        public string ContactPersonName
        {
            get { return contactPersonName; }
            set { contactPersonName = value; OnPropertyChanged(nameof(ContactPersonName)); }
        }
        private string cellPhoneNumberContactPerson;
        public string CellPhoneNumberContactPerson
        {
            get { return cellPhoneNumberContactPerson; }
            set { cellPhoneNumberContactPerson = value; OnPropertyChanged(nameof(CellPhoneNumberContactPerson)); }
        }
        private string streetAdress;
        public string StreetAdress
        {
            get { return streetAdress; }
            set { streetAdress = value; OnPropertyChanged(nameof(StreetAdress)); }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }
        private string telephoneNumber;
        public string TelephoneNumber
        {
            get { return telephoneNumber; }
            set { telephoneNumber = value; OnPropertyChanged(nameof(TelephoneNumber)); }
        }
        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged(nameof(City)); }
        }
        public ICommand CreateCompanyCustomerCommand { get; private set; }
        public RegisterCompanyCustomerViewModel()
        {
            CreateCompanyCustomerCommand = new RelayCommand<object>(execute => CreateCompanyCustomer());;
        }
        private void CreateCompanyCustomer()
        {
            
            StringBuilder errorMessage = new StringBuilder();

            if (!AreFieldsValid())
            {
                return; 
            }

            if (!IsValidPostalCode(PostalCode))
            {
                errorMessage.AppendLine("Postnummer måste vara 5 siffror..");
            }

            if (!IsValidOrganisationNumber(OrganisationNumber))
            {
                errorMessage.AppendLine("Organisationsnummer måste vara exakt 10 siffror.");
            }

            if (!IsValidEmail(Email))
            {
                errorMessage.AppendLine("Ogiltig e-postadress. E-post måste innehålla '@'.");
            }

            if (!IsValidCellPhoneNumber(CellPhoneNumberContactPerson))
            {
                errorMessage.AppendLine("Mobilnummer får vara 10 siffror.");
            }
            if (!IsValidCompanyPhoneNumber(TelephoneNumber))
            {
                errorMessage.AppendLine("Företags nummer måste vara mellan 5 och 8 siffror.");
            }

            // Error message (ONLY TEMPORARY)
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString(), "Valideringsfel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Creating CompanyCustomer and PostalCodecity objects
            CompanyCustomer companyCustomer = new CompanyCustomer();
            PostalCodeCity postalCodeCity = new PostalCodeCity();

            //Creation of PostalCode
            postalCodeCity.City = CapitalizeFirstLetter(City);
            postalCodeCity.PostalCode = PostalCode;
            //Creation of CompanyCustomer
            companyCustomer.PostalCodeCity = postalCodeCity;
            companyCustomer.CompanyName = CapitalizeFirstLetter(CompanyName);
            companyCustomer.OrganisationNumber = OrganisationNumber;
            companyCustomer.ContactPersonName = CapitalizeFirstLetter(ContactPersonName);
            companyCustomer.CompanyPersonTelephoneNumber = CellPhoneNumberContactPerson;
            companyCustomer.TelephoneNumber = TelephoneNumber;
            companyCustomer.Email = Email;
            companyCustomer.StreetAddress = CapitalizeFirstLetter(StreetAdress);

            AddCompanyCustomer(companyCustomer);
        }

         //Adding the Company customer to the database through the controller        
        private void AddCompanyCustomer(CompanyCustomer companyCustomer)
        {
            try
            {
                customerController.AddCustomer(companyCustomer);
                MessageBox.Show("Kunden har lagts till framgångsrikt!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Hantera fel och visa MessageBox här
                MessageBox.Show(ex.Message, "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //Error handeling
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Dela upp strängen i ord baserat på mellanslag
            var words = input.Split(' ');

            // Kapitalisera första bokstaven i varje ord
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            // Sätt ihop strängen igen med mellanslag mellan orden
            return string.Join(" ", words);
        }
        private bool IsValidOrganisationNumber(string organisationNumber)
        {
            return organisationNumber.Length == 10 && organisationNumber.All(char.IsDigit);
        }
        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.IndexOf("@") > 0 && email.IndexOf("@") < email.Length - 1;
        }
        private bool IsValidCellPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
        }
        private bool IsValidCompanyPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length >= 5 && phoneNumber.Length <= 8 && phoneNumber.All(char.IsDigit);
        }
        private bool IsValidPostalCode(string postalCode)
        {
            return postalCode.Length == 5 && postalCode.All(char.IsDigit);
        }
        private bool AreFieldsValid()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(City))
                errorMessage.AppendLine("Stad får inte vara tom.");

            if (string.IsNullOrWhiteSpace(CompanyName))
                errorMessage.AppendLine("Företagsnamn får inte vara tomt.");

            if (string.IsNullOrWhiteSpace(ContactPersonName))
                errorMessage.AppendLine("Kontaktpersonens namn får inte vara tomt.");

            if (string.IsNullOrWhiteSpace(OrganisationNumber))
                errorMessage.AppendLine("Organisationsnummer får inte vara tomt.");

            if (string.IsNullOrWhiteSpace(Email))
                errorMessage.AppendLine("E-postadress får inte vara tom.");

            if (string.IsNullOrWhiteSpace(cellPhoneNumberContactPerson))
                errorMessage.AppendLine("Mobilnummer får inte vara tomt.");

            if (string.IsNullOrWhiteSpace(TelephoneNumber))
                errorMessage.AppendLine("Telefonnummer får inte vara tomt.");

            if (string.IsNullOrWhiteSpace(StreetAdress))
                errorMessage.AppendLine("Gatuadress får inte vara tom.");

            if (string.IsNullOrWhiteSpace(PostalCode))
                errorMessage.AppendLine("Postkod får inte vara tom.");

            // If any errors return false
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString(), "Valideringsfel", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // All fields are correct
            return true;
        }


    }
}
