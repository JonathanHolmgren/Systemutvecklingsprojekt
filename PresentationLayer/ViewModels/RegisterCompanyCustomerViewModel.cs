using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    internal class RegisterCompanyCustomerViewModel:ObservableObject
    {
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(nameof(LastName)); }
        }
        private string socialSecurityNumber;
        public string SocialSecurityNumber
        {
            get { return socialSecurityNumber; }
            set { socialSecurityNumber = value; OnPropertyChanged(nameof(SocialSecurityNumber)); }
        }
        private string cellPhoneNumber;
        public string CellPhoneNumber
        {
            get { return cellPhoneNumber; }
            set { cellPhoneNumber = value; OnPropertyChanged(nameof(CellPhoneNumber)); }
        }
        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; OnPropertyChanged(nameof(EmailAddress)); }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(nameof(Address)); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged(nameof(City)); }
        }
        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
        }
        private string county;
        public string County
        {
            get { return county; }
            set { county = value; OnPropertyChanged(nameof(County)); }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged(nameof(Country)); }
        }
    }
}
