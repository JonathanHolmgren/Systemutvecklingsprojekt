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
            
            CompanyCustomer companyCustomer = new CompanyCustomer();
            PostalCodeCity postalCodeCity = new PostalCodeCity();

            
            postalCodeCity.City = City;
            postalCodeCity.PostalCode = PostalCode;
            companyCustomer.PostalCodeCity = postalCodeCity;
            companyCustomer.CompanyName = CompanyName;
            companyCustomer.OrganisationNumber = OrganisationNumber;
            companyCustomer.ContactPersonName = ContactPersonName;
            companyCustomer.CompanyPersonTelephoneNumber = cellPhoneNumberContactPerson;
            companyCustomer.TelephoneNumber = TelephoneNumber;
            companyCustomer.Email = Email;
            companyCustomer.StreetAdress = StreetAdress;
            //customerController.AddCustomer(companyCustomer);

            
            string message = $"Företagsnamn: {companyCustomer.CompanyName}\n" +
                             $"Organisationsnummer: {companyCustomer.OrganisationNumber}\n" +
                             $"Kontaktperson: {companyCustomer.ContactPersonName}\n" +
                             $"Mobilnummer Kontaktperson: {companyCustomer.CompanyPersonTelephoneNumber}\n" +
                             $"Telefonnummer: {companyCustomer.TelephoneNumber}\n" +
                             $"E-post: {companyCustomer.Email}\n" +
                             $"Adress: {companyCustomer.StreetAdress}\n" +
                             $"Postkod och Stad: {companyCustomer.PostalCodeCity.PostalCode} {companyCustomer.PostalCodeCity.City}";

            
            MessageBox.Show(message, "Företagsinformation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
