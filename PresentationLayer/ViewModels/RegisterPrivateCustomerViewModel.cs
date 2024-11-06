using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Models;
using ServiceLayer;
using PresentationLayer.Command;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using PresentationLayer.Models;
using LiveChartsCore.Kernel;

namespace PresentationLayer.ViewModels
{
    public class RegisterPrivateCustomerViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private CustomerController customerController = new CustomerController();
        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        // Properties med valideringslogik
        private string ssn;
        public string SSN
        {
            get => ssn;
            set
            {
                ssn = value;
                OnPropertyChanged();
                ValidateSSN(); // Validera direkt vid uppdatering
            }
        }

        private string firstname;
        public string FirstName
        {
            get => firstname;
            set
            {
                firstname = value;
                OnPropertyChanged();
                ValidateNotEmpty(nameof(FirstName), firstname, "Förnamn saknas");
            }
        }

        private string lastname;
        public string LastName
        {
            get => lastname;
            set
            {
                lastname = value;
                OnPropertyChanged();
                ValidateNotEmpty(nameof(LastName), lastname, "Efternamn saknas");
            }
        }

        private string streetadress;
        public string StreetAdress
        {
            get => streetadress;
            set
            {
                streetadress = value;
                OnPropertyChanged();
                ValidateNotEmpty(nameof(StreetAdress), streetadress, "Gatuadress saknas");
            }
        }

        private string postalcode;
        public string PostalCode
        {
            get => postalcode;
            set
            {
                postalcode = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
                ValidateNotEmpty(nameof(City), city, "Stad saknas");
            }
        }

        private string telephonenumber;
        public string TelephoneNumber
        {
            get => telephonenumber;
            set
            {
                telephonenumber = value;
                OnPropertyChanged();
                ValidateCellPhoneNumber();
            }
        }

        private string worktelephonenumber = null;
        public string WorkTelephoneNumber
        {
            get => worktelephonenumber;
            set
            {
                worktelephonenumber = value;
                OnPropertyChanged();
                ValidateWorkPhone();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
                ValidateEmail();
            }
        }
        private int menuPage = 0;
        public int MenuPage
        {
            get => menuPage;
            set
            {
                menuPage = value;
                OnPropertyChanged();

            }
        }

        public ICommand AddPrivateCustomerCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand GoBackPageCommand { get; private set; }
        public ICommand ResetReigstrationCommand { get; private set; }
        public RegisterPrivateCustomerViewModel()
        {
            AddPrivateCustomerCommand = new RelayCommand(AddPrivateCustomer);
            NextPageCommand = new RelayCommand<object>(execute => IncreaseMenuPage());
            GoBackPageCommand = new RelayCommand<object>(execute => DecreaseMenuPage());
            ResetReigstrationCommand = new RelayCommand<object>(execute => ResetFields());
        }
        private void IncreaseMenuPage() //Change the menupage in the registering
        {
            if (MenuPage == 0)
            {
                ValidateNotEmpty(nameof(FirstName), firstname, "Förnamn saknas");
                ValidateNotEmpty(nameof(LastName), lastname, "Efternamn saknas");
                ValidateSSN();

                bool hasFirstNameErrors = GetErrors(nameof(FirstName))?.Cast<string>().Any() ?? false;
                bool hasLastNameErrors = GetErrors(nameof(LastName))?.Cast<string>().Any() ?? false;
                bool hasSSNErrors = GetErrors(nameof(SSN))?.Cast<string>().Any() ?? false;

                if (!hasFirstNameErrors && !hasLastNameErrors&&!hasSSNErrors)
                {
                    MenuPage++;
                }
            }
            else if (MenuPage == 1)
            {
                ValidateEmail();
                ValidateCellPhoneNumber();
                ValidateWorkPhone();

                bool hasEmailErrors = GetErrors(nameof(Email))?.Cast<string>().Any() ?? false;
                bool hasCellphoneErrors = GetErrors(nameof(TelephoneNumber))?.Cast<string>().Any() ?? false;
                bool hasWorkPhoneErrors = GetErrors(nameof(WorkTelephoneNumber))?.Cast<string>().Any() ?? false;

                if (!hasCellphoneErrors && !hasEmailErrors && !hasWorkPhoneErrors)
                {
                    MenuPage++;
                }
            }
           
        }
        public void ResetFields()
        {
            // Återställ alla fält till tomma strängar
            SSN = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            StreetAdress = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;
            TelephoneNumber = string.Empty;
            WorkTelephoneNumber = string.Empty;
            Email = string.Empty;

            // Rensa alla valideringsfel genom att ange egenskapsnamnen som strängar
            ClearErrors(nameof(SSN));
            ClearErrors(nameof(FirstName));
            ClearErrors(nameof(LastName));
            ClearErrors(nameof(StreetAdress));
            ClearErrors(nameof(PostalCode));
            ClearErrors(nameof(City));
            ClearErrors(nameof(TelephoneNumber));
            ClearErrors(nameof(WorkTelephoneNumber));
            ClearErrors(nameof(Email));

            // Uppdatera HasErrors så att UI:t känner av ändringen
            OnPropertyChanged(nameof(HasErrors));
            MenuPage = 0;
        }


        private void DecreaseMenuPage()
        {
            MenuPage--;
        }

       
        private void AddPrivateCustomer()
        {
            ValidateAll();
            if (HasErrors)
            {
                MessageBox.Show("Vänligen korrigera alla fel innan du fortsätter.");
                return;
            }

            try
            {
                var privateCustomer = new PrivateCustomer(TelephoneNumber, Email, StreetAdress, PostalCode, City, SSN, FirstName, LastName, WorkTelephoneNumber);
                customerController.AddPrivateCustomer(privateCustomer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel uppstod: {ex.Message}");
            }
            MenuPage++;
        }

     

        #region Valideringslogik

        private void ValidateNotEmpty(string propertyName, string value, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
                AddError(propertyName, errorMessage);
            else
                ClearErrors(propertyName);
        }
        private void ValidateWorkPhone()
        {
            
            if (!IsValidCompanyPhoneNumber(WorkTelephoneNumber))
                AddError(nameof(WorkTelephoneNumber), "Telefonnummer måste vara mellan 5 och 8 siffror.");
            else
                ClearErrors(nameof(WorkTelephoneNumber));

        }
        private void ValidateCellPhoneNumber()
        {
          
            if (string.IsNullOrWhiteSpace(TelephoneNumber) || !IsValidCellPhoneNumber(TelephoneNumber))
                AddError(nameof(TelephoneNumber), "Mobilnummer måste vara exakt 10 siffror.");
            else 
                ClearErrors(nameof(TelephoneNumber));
        }
        private void ValidateSSN()
        {

            var regex = new Regex(@"^\d{8}-\d{4}$");

            if (string.IsNullOrWhiteSpace(SSN) || !regex.IsMatch(SSN))
            {
                AddError(nameof(SSN), "Personnummer måste vara av formatet av 8 siffror - 4 siffor");
               
            }
            else
                ClearErrors(nameof(SSN));
        }

        private void ValidatePostalCode()
        {
            const string errorMessage = "Postnumret måste vara exakt 5 siffror";
            if (PostalCode.Length != 5 || !PostalCode.All(char.IsDigit))
                AddError(nameof(PostalCode), errorMessage);
            else
                ClearErrors(nameof(PostalCode));
        }

        private void ValidateEmail()
        {
            const string errorMessage = "Ogiltig e-postadress";
            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddError(nameof(Email), errorMessage);
            else
                ClearErrors(nameof(Email));
        }
        private void ValidateAll()
        {
            
            ValidateSSN();
            ValidateNotEmpty(nameof(FirstName), FirstName, "Förnamn saknas");
            ValidateNotEmpty(nameof(LastName), LastName, "Efternamn saknas");
            ValidateNotEmpty(nameof(StreetAdress), StreetAdress, "Gatuadress saknas");
            ValidatePostalCode();
            ValidateNotEmpty(nameof(City), City, "Stad saknas");
            ValidateCellPhoneNumber();
            ValidateWorkPhone();
            ValidateEmail();
        }

        private bool IsValidCellPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
            
        }
        private bool IsValidCompanyPhoneNumber(string phoneNumber)
        {
            
            if (phoneNumber == null)
            {
                return true;
            }

            
            return phoneNumber.Length >= 5 && phoneNumber.Length <= 8 && phoneNumber.All(char.IsDigit);
        }

        #endregion

        #region INotifyDataErrorInfo-implementering

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return errors.ContainsKey(propertyName) ? errors[propertyName] : null;
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(errorMessage))
            {
                errors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName) =>
             ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Hjälpmetoder
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        #endregion
    }
}
