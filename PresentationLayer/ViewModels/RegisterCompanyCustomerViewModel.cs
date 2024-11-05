using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    internal class RegisterCompanyCustomerViewModel : ObservableObject, INotifyDataErrorInfo
    {
        #region Initiation of ojbects
        CustomerController customerController = new CustomerController();
       
        private string companyName;
        public string CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                OnPropertyChanged(nameof(CompanyName));
                ValidateCompanyName();
            }
        }

        private string organisationNumber;
        public string OrganisationNumber
        {
            get => organisationNumber;
            set
            {
                organisationNumber = value;
                OnPropertyChanged(nameof(OrganisationNumber));
                ValidateOrganisationNumber();
            }
        }

        private string contactPersonName;
        public string ContactPersonName
        {
            get => contactPersonName;
            set
            {
                contactPersonName = value;
                OnPropertyChanged(nameof(ContactPersonName));
                ValidateContactPersonName();
            }
        }

        private string cellPhoneNumberContactPerson;
        public string CellPhoneNumberContactPerson
        {
            get => cellPhoneNumberContactPerson;
            set
            {
                cellPhoneNumberContactPerson = value;
                OnPropertyChanged(nameof(CellPhoneNumberContactPerson));
                ValidateCellPhoneNumber();
            }
        }

        private string streetAdress;
        public string StreetAdress
        {
            get => streetAdress;
            set
            {
                streetAdress = value;
                OnPropertyChanged(nameof(StreetAdress));
                ValidateStreetAddress();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
                ValidateEmail();
            }
        }

        private string telephoneNumber;
        public string TelephoneNumber
        {
            get => telephoneNumber;
            set
            {
                telephoneNumber = value;
                OnPropertyChanged(nameof(TelephoneNumber));
                ValidateTelephoneNumber();
            }
        }

        private string postalCode;
        public string PostalCode
        {
            get => postalCode;
            set
            {
                postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
                ValidatePostalCode();
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
                ValidateCity();
            }
        }
        private int menuPage = 0;
        public int MenuPage
        {
            get => menuPage;
            set
            {
                menuPage = value;
                OnPropertyChanged(nameof(MenuPage));

            }
        }
        public ICommand NextPageCommand { get; private set; }
        public ICommand GoBackPageCommand { get; private set; }
        public ICommand ResetReigstrationCommand { get; private set; }
        
        public ICommand CreateCompanyCustomerCommand { get; private set; }
        #endregion
        #region Constructor
        public RegisterCompanyCustomerViewModel()
        {
            CreateCompanyCustomerCommand = new RelayCommand<object>(execute => CreateCompanyCustomer());
            NextPageCommand = new RelayCommand<object>(execute => IncreaseMenuPage());
            GoBackPageCommand = new RelayCommand<object>(execute => DecreaseMenuPage());
            ResetReigstrationCommand = new RelayCommand<object>(execute => ResetProperties());
        }
        #endregion
        #region Methods
        private readonly Dictionary<string, List<string>> _errors = new();
        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void OnErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        public IEnumerable GetErrors(string propertyName) =>
            _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void ValidateAllProperties()
        {
            ValidateCity();
            ValidateCompanyName();
            ValidateContactPersonName();
            ValidateOrganisationNumber();
            ValidateEmail();
            ValidateCellPhoneNumber();
            ValidateTelephoneNumber();
            ValidateStreetAddress();
            ValidatePostalCode();
        }
        private void ValidateCompanyName()
        {
            ClearErrors(nameof(CompanyName));
            if (string.IsNullOrWhiteSpace(CompanyName))
                AddError(nameof(CompanyName), "Företagsnamn får inte vara tomt.");
        }

        private void ValidateOrganisationNumber()
        {
            ClearErrors(nameof(OrganisationNumber));

            var regex = new Regex(@"^\d{6}-\d{4}$");

            if (string.IsNullOrWhiteSpace(OrganisationNumber) || !regex.IsMatch(OrganisationNumber))
            {
                AddError(nameof(OrganisationNumber), "Organisationsnummer måste vara i formatet '******-****'.");
            }
        }

        private void ValidateContactPersonName()
        {
            ClearErrors(nameof(ContactPersonName));
            if (string.IsNullOrWhiteSpace(ContactPersonName))
                AddError(nameof(ContactPersonName), "Kontaktpersonens namn får inte vara tomt.");
        }

        private void ValidateCellPhoneNumber()
        {
            ClearErrors(nameof(CellPhoneNumberContactPerson));
            if (string.IsNullOrWhiteSpace(CellPhoneNumberContactPerson) || !IsValidCellPhoneNumber(CellPhoneNumberContactPerson))
                AddError(nameof(CellPhoneNumberContactPerson), "Mobilnummer måste vara exakt 10 siffror.");
        }

        private void ValidateStreetAddress()
        {
            ClearErrors(nameof(StreetAdress));
            if (string.IsNullOrWhiteSpace(StreetAdress))
                AddError(nameof(StreetAdress), "Gatuadress får inte vara tom.");
        }

        private void ValidateEmail()
        {
            ClearErrors(nameof(Email));
            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
                AddError(nameof(Email), "Ogiltig e-postadress.");
        }

        private void ValidateTelephoneNumber()
        {
            ClearErrors(nameof(TelephoneNumber));
            if (string.IsNullOrWhiteSpace(TelephoneNumber) || !IsValidCompanyPhoneNumber(TelephoneNumber))
                AddError(nameof(TelephoneNumber), "Telefonnummer måste vara mellan 5 och 8 siffror.");
        }

        private void ValidatePostalCode()
        {
            ClearErrors(nameof(PostalCode));
            if (string.IsNullOrWhiteSpace(PostalCode) || !IsValidPostalCode(PostalCode))
                AddError(nameof(PostalCode), "Postnummer måste vara exakt 5 siffror.");
        }

        private void ValidateCity()
        {
            ClearErrors(nameof(City));
            if (string.IsNullOrWhiteSpace(City))
                AddError(nameof(City), "Stad får inte vara tom.");
        }
        private void IncreaseMenuPage() //Change the menupage in the registering
        {
            if (MenuPage == 0)
            {
                ValidateCompanyName();
                ValidateOrganisationNumber();

                bool hasOrganisationNumberErrors = GetErrors(nameof(OrganisationNumber))?.Cast<string>().Any() ?? false;
                bool hasCompanyNameErrors = GetErrors(nameof(CompanyName))?.Cast<string>().Any() ?? false;

                if (!hasOrganisationNumberErrors && !hasCompanyNameErrors)
                {
                    MenuPage++;
                }
            }
            else if (MenuPage == 1)
            {
                ValidateContactPersonName();
                ValidateCellPhoneNumber();

                bool hasCellphoneErrors = GetErrors(nameof(CellPhoneNumberContactPerson))?.Cast<string>().Any() ?? false;
                bool hasContactNameErrors = GetErrors(nameof(ContactPersonName))?.Cast<string>().Any() ?? false;

                if (!hasCellphoneErrors && !hasContactNameErrors)
                {
                    MenuPage++;
                }
            }
            else if (MenuPage == 2)
            {
                ValidateEmail();
                ValidateTelephoneNumber();

                bool hasEmailErrors = GetErrors(nameof(Email))?.Cast<string>().Any() ?? false;
                bool hasTelephoneErrors = GetErrors(nameof(TelephoneNumber))?.Cast<string>().Any() ?? false;

                if (!hasEmailErrors && !hasTelephoneErrors)
                {
                    MenuPage++;
                }
            }

           
           
        }


        private void DecreaseMenuPage()
        {
            MenuPage--;
        }
        public void ResetProperties()
        {
            CompanyName = string.Empty;
            OrganisationNumber = string.Empty;
            ContactPersonName = string.Empty;
            CellPhoneNumberContactPerson = string.Empty;
            StreetAdress = string.Empty;
            Email = string.Empty;
            TelephoneNumber = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;

            // Rensa eventuella valideringsfel om det behövs
            ClearErrors(nameof(CompanyName));
            ClearErrors(nameof(OrganisationNumber));
            ClearErrors(nameof(ContactPersonName));
            ClearErrors(nameof(CellPhoneNumberContactPerson));
            ClearErrors(nameof(StreetAdress));
            ClearErrors(nameof(Email));
            ClearErrors(nameof(TelephoneNumber));
            ClearErrors(nameof(PostalCode));
            ClearErrors(nameof(City));
            MenuPage=0;
        }

        private void CreateCompanyCustomer()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                MessageBox.Show("Vänligen korrigera felen innan registrering.", "Valideringsfel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //Creating CompanyCustomer
            CompanyCustomer companyCustomer = new CompanyCustomer();

            //Creation of CompanyCustomer
            companyCustomer.PostalCode = PostalCode;
            companyCustomer.City = CapitalizeFirstLetter(City);
            companyCustomer.CompanyName = CapitalizeFirstLetter(CompanyName);
            companyCustomer.OrganisationNumber = OrganisationNumber;
            companyCustomer.ContactPersonName = CapitalizeFirstLetter(ContactPersonName);
            companyCustomer.CompanyPersonTelephoneNumber = CellPhoneNumberContactPerson;
            companyCustomer.TelephoneNumber = TelephoneNumber;
            companyCustomer.Email = Email;
            companyCustomer.StreetAddress = CapitalizeFirstLetter(StreetAdress);

            AddCompanyCustomer(companyCustomer);
            MenuPage++;
        }

        private void AddCompanyCustomer(CompanyCustomer companyCustomer)
        {
            try
            {
                customerController.AddCompanyCustomer(companyCustomer);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
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
            return string.Join(" ", words);
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
        #endregion
    }
}
