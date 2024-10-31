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
    internal class EditPrivateCustomerViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();

        public ICommand SaveEditedPrivateCustomerCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }

        public EditPrivateCustomerViewModel(PrivateCustomer privateCustomer)
        {
            SaveEditedPrivateCustomerCommand = new RelayCommand(SaveEditedPrivateCustomer);
            ReturnCommand = new RelayCommand(ReturnToCustomerProfile);
        }


        private void SaveEditedPrivateCustomer()
        {
            if (ViewedPrivateCustomer != null)
            {
                PrivateCustomerToEdit = ViewedPrivateCustomer;
                if (string.IsNullOrEmpty(PrivateCustomerToEdit.SSN))
                {
                    MessageBox.Show("Personnummer (SSN) saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.FirstName))
                {
                    MessageBox.Show("Förnamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.LastName))
                {
                    MessageBox.Show("Efternamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.StreetAddress))
                {
                    MessageBox.Show("Gatuadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.PostalCode))
                {
                    MessageBox.Show("Postnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.City))
                {
                    MessageBox.Show("Stad saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.TelephoneNumber))
                {
                    MessageBox.Show("Telefonnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(PrivateCustomerToEdit.Email))
                {
                    MessageBox.Show("E-postadress saknas");
                    return;
                }

                //Check that postalcode is 5 letters
                if (
                    PrivateCustomerToEdit.PostalCode.Length != 5
                    || !PrivateCustomerToEdit.PostalCode.All(char.IsDigit)
                )
                {
                    MessageBox.Show("Postnumret måste vara exakt 5 siffror");
                    return;
                }

                //Check that SSN is 10 letters
                if (
                    PrivateCustomerToEdit.SSN.Length != 12
                    || !PrivateCustomerToEdit.SSN.All(char.IsDigit)
                )
                {
                    MessageBox.Show("Personnumret måste innehålla 10 siffror");
                    return;
                }

                PrivateCustomerToEdit.FirstName = CapitalizeFirstLetter(
                    PrivateCustomerToEdit.FirstName
                );
                PrivateCustomerToEdit.LastName = CapitalizeFirstLetter(
                    PrivateCustomerToEdit.LastName
                );
                PrivateCustomerToEdit.City = CapitalizeFirstLetter(PrivateCustomerToEdit.City);
                PrivateCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
                    PrivateCustomerToEdit.StreetAddress
                );

                ViewedPrivateCustomer = PrivateCustomerToEdit;
                customerController.UpdatePrivateCustomer(ViewedPrivateCustomer);
                IsEditPrivatePopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
                FullName = $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";

            }
        }
        private void ReturnToCustomerProfile()
        {
            throw new NotImplementedException();
        }

    }
}
