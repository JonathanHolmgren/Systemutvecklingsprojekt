using System.Windows;
using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class EditCompanyCustomerViewModel : ObservableObject
{
    #region Initiation of objects
    private CustomerController customerController = new CustomerController();
    private InsuranceController insuranceController = new InsuranceController();
    private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();

    private CompanyCustomer _companyCustomerToEdit;
    public CompanyCustomer CompanyCustomerToEdit
    {
        get { return _companyCustomerToEdit; }
        set
        {
            _companyCustomerToEdit = value;
            OnPropertyChanged();
        }
    }

    private bool isValidated;
    public bool IsValidated
    {
        get { return isValidated; }
        set
        {
            isValidated = value;
            OnPropertyChanged(nameof(IsValidated));
        }
    }

    private ICommand _saveEditedCompanyCustomerCommand = null!;
    public ICommand SaveEditedCompanyCustomerCommand =>
        _saveEditedCompanyCustomerCommand ??= new RelayCommand(SaveEditedCompanyCustomer);

    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify(
                "ChangeView",
                new CompanyCustomerProfileViewModel(CompanyCustomerToEdit)
            );
        });
    #endregion
    #region Constructors
    public EditCompanyCustomerViewModel() { }

    public EditCompanyCustomerViewModel(CompanyCustomer companyCustomerToEdit)
    {
        _companyCustomerToEdit = companyCustomerToEdit;
    }
    #endregion
    #region Methods
    private void SaveEditedCompanyCustomer()
    {
       
        if (!ValidateCompanyCustomer(out string validationErrors))
        {
            MessageBox.Show(validationErrors);
        }

        CompanyCustomerToEdit = CompanyCustomerToEdit;
        CompanyCustomerToEdit.CompanyName = CapitalizeFirstLetter(
            CompanyCustomerToEdit.CompanyName
        );
        CompanyCustomerToEdit.City = CapitalizeFirstLetter(CompanyCustomerToEdit.City);
        CompanyCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
            CompanyCustomerToEdit.StreetAddress
        );

       
        customerController.UpdateCompanyCustomer(CompanyCustomerToEdit);

     
        MessageBox.Show("Ändringar är sparade");
    }

    private bool ValidateCompanyCustomer(out string validationErrors)
    {
        validationErrors = "";

        if (CompanyCustomerToEdit == null)
        {
            validationErrors = "Kundinformation saknas.";
            IsValidated = false;
            return false;
        }

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.CompanyName))
            validationErrors += "Företagsnamn saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.StreetAddress))
            validationErrors += "Gatuadress saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.PostalCode))
            validationErrors += "Postnummer saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.City))
            validationErrors += "Stad saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.TelephoneNumber))
            validationErrors += "Telefonnummer saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.Email))
            validationErrors += "E-postadress saknas.\n";

        if (string.IsNullOrEmpty(CompanyCustomerToEdit.ContactPersonName))
            validationErrors += "Kontaktperson saknas.\n";

        if (
            CompanyCustomerToEdit.PostalCode.Length != 5
            || !CompanyCustomerToEdit.PostalCode.All(char.IsDigit)
        )
            validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

        if (
            CompanyCustomerToEdit.OrganisationNumber.Length != 10
            || !CompanyCustomerToEdit.OrganisationNumber.All(char.IsDigit)
        )
            validationErrors += "Personnumret måste innehålla exakt 12 siffror.\n";

        IsValidated = string.IsNullOrEmpty(validationErrors);
        return IsValidated;
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
    #endregion
}
