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
    internal class EditCompanyCustomerViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();

        public ICommand SaveEditedCompanyCustomerCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }

        public EditCompanyCustomerViewModel()
        {
            SaveEditedCompanyCustomerCommand = new RelayCommand(SaveEditedCompanyCustomer);
            ReturnCommand = new RelayCommand(ReturnToCustomerProfile);
        }

        private void SaveEditedCompanyCustomer()
        {
            if (ViewedCompanyCustomer != null)
            {
                CompanyCustomerToEdit = ViewedCompanyCustomer;

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.OrganisationNumber))
                {
                    MessageBox.Show("Organisationsnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.CompanyName))
                {
                    MessageBox.Show("Företagsnamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.StreetAddress))
                {
                    MessageBox.Show("Gatuadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.PostalCode))
                {
                    MessageBox.Show("Postnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.City))
                {
                    MessageBox.Show("Stad saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.Email))
                {
                    MessageBox.Show("E-postadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.TelephoneNumber))
                {
                    MessageBox.Show("Telefonnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(CompanyCustomerToEdit.ContactPersonName))
                {
                    MessageBox.Show("Kontaktperson saknas");
                    return;
                }

                //Check that postalcode is 5 letters
                if (
                    CompanyCustomerToEdit.PostalCode.Length != 5
                    || !CompanyCustomerToEdit.PostalCode.All(char.IsDigit)
                )
                {
                    MessageBox.Show("Postnumret måste vara exakt 5 siffror");
                    return;
                }

                //Check that Organisationnumber is 10 letters
                if (
                    CompanyCustomerToEdit.OrganisationNumber.Length != 10
                    || !CompanyCustomerToEdit.OrganisationNumber.All(char.IsDigit)
                )
                {
                    MessageBox.Show("Organisationsnummret måste innehålla 10 siffror");
                    return;
                }

                CompanyCustomerToEdit.CompanyName = CapitalizeFirstLetter(
                    CompanyCustomerToEdit.CompanyName
                );
                CompanyCustomerToEdit.ContactPersonName = CapitalizeFirstLetter(
                    CompanyCustomerToEdit.ContactPersonName
                );
                CompanyCustomerToEdit.City = CapitalizeFirstLetter(CompanyCustomerToEdit.City);
                CompanyCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
                    CompanyCustomerToEdit.StreetAddress
                );

                ViewedCompanyCustomer = CompanyCustomerToEdit;
                customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);
                IsEditCompanyPopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
            }
        }


        private void ReturnToCustomerProfile()
        {
            throw new NotImplementedException();
        }
    }
}
