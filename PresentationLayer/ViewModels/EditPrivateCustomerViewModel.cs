using System.Windows;
using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class EditPrivateCustomerViewModel : ObservableObject
{
    #region Initation of objects
    private CustomerController customerController = new CustomerController();
    private InsuranceController insuranceController = new InsuranceController();
    private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
    private LoggedInUser _user;

    private PrivateCustomer _privateCustomerToEdit;
    public PrivateCustomer PrivateCustomerToEdit
    {
        get { return _privateCustomerToEdit; }
        set
        {
            _privateCustomerToEdit = value;
            OnPropertyChanged();
        }
    }

    private string fullName;
    public string FullName
    {
        get
        {
            if (PrivateCustomerToEdit != null)
            {
                return $"{PrivateCustomerToEdit.FirstName} {PrivateCustomerToEdit.LastName}";
            }
            return string.Empty;
        }
        set
        {
            fullName = value;
            OnPropertyChanged(nameof(PrivateCustomerToEdit));
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

    private ICommand _saveEditedPrivateCustomerCommand = null!;
    public ICommand SaveEditedPrivateCustomerCommand =>
        _saveEditedPrivateCustomerCommand ??= new RelayCommand(SaveEditedPrivateCustomer);

    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify(
                "ChangeView",
                new PrivateCustomerProfileViewModel(_user, PrivateCustomerToEdit)
            );
        });
    #endregion
    #region Constructors
    public EditPrivateCustomerViewModel() { }

    public EditPrivateCustomerViewModel(LoggedInUser user, PrivateCustomer privateCustomerToEdit)
    {
        _user = user;
        _privateCustomerToEdit = privateCustomerToEdit;
    }
    #endregion
    #region Methods
    private void SaveEditedPrivateCustomer()
    {
        if (!ValidatePrivateCustomer(out string validationErrors))
        {
            MessageBox.Show(validationErrors);
        }

        PrivateCustomerToEdit.FirstName = CapitalizeFirstLetter(PrivateCustomerToEdit.FirstName);
        PrivateCustomerToEdit.LastName = CapitalizeFirstLetter(PrivateCustomerToEdit.LastName);
        PrivateCustomerToEdit.City = CapitalizeFirstLetter(PrivateCustomerToEdit.City);
        PrivateCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
            PrivateCustomerToEdit.StreetAddress
        );

        customerController.UpdatePrivateCustomer(PrivateCustomerToEdit);

        MessageBox.Show("Ändringar är sparade");
        FullName = $"{PrivateCustomerToEdit.FirstName} {PrivateCustomerToEdit.LastName}";
    }

    private bool ValidatePrivateCustomer(out string validationErrors)
    {
        validationErrors = "";

        if (PrivateCustomerToEdit == null)
        {
            validationErrors = "Kundinformation saknas.";
            IsValidated = false;
            return false;
        }

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.FirstName))
            validationErrors += "Förnamn saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.LastName))
            validationErrors += "Efternamn saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.StreetAddress))
            validationErrors += "Gatuadress saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.PostalCode))
            validationErrors += "Postnummer saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.City))
            validationErrors += "Stad saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.TelephoneNumber))
            validationErrors += "Telefonnummer saknas.\n";

        if (string.IsNullOrEmpty(PrivateCustomerToEdit.Email))
            validationErrors += "E-postadress saknas.\n";

        if (
            PrivateCustomerToEdit.PostalCode.Length != 5
            || !PrivateCustomerToEdit.PostalCode.All(char.IsDigit)
        )
            validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

        if (PrivateCustomerToEdit.SSN.Length != 12 || !PrivateCustomerToEdit.SSN.All(char.IsDigit))
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
