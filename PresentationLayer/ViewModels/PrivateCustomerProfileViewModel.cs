using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Models;
using Models.InsuranceInformation;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class PrivateCustomerProfileViewModel : ObservableObject
{
    #region Initation of objects
    private CustomerController customerController = new CustomerController();
    private InsuranceController insuranceController = new InsuranceController();
    private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
    private LoggedInUser _user;

    private string searchValue;
    public string SearchValue
    {
        get { return searchValue; }
        set
        {
            searchValue = value;
            OnPropertyChanged(nameof(SearchValue));
        }
    }
    private string note;
    public string Note
    {
        get { return note; }
        set
        {
            note = value;
            OnPropertyChanged(nameof(Note));
        }
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get { return _isAdmin; }
        set
        {
            _isAdmin = value;
            OnPropertyChanged();
        }
    }

    private string fullName;
    public string FullName
    {
        get
        {
            if (ViewedPrivateCustomer != null)
            {
                return $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";
            }
            return string.Empty;
        }
        set
        {
            fullName = value;
            OnPropertyChanged();
        }
    }

    private bool isActiveStatusSelected;
    public bool IsActiveStatusSelected
    {
        get { return isActiveStatusSelected; }
        set
        {
            isActiveStatusSelected = value;
            OnPropertyChanged(nameof(IsActiveStatusSelected));
        }
    }
    private bool isInactiveStatusSelected;
    public bool IsInactiveStatusSelected
    {
        get { return isInactiveStatusSelected; }
        set
        {
            isInactiveStatusSelected = value;
            OnPropertyChanged(nameof(IsInactiveStatusSelected));
        }
    }

    public bool IsPrivateColumnVisible => IsPrivateSelected;

    private ObservableCollection<Insurance> customerInsurances;
    public ObservableCollection<Insurance> CustomerInsurances
    {
        get => customerInsurances;
        set
        {
            customerInsurances = value;
            OnPropertyChanged(nameof(CustomerInsurances));
        }
    }

    private PrivateCustomer privateCustomerToEdit;
    public PrivateCustomer PrivateCustomerToEdit
    {
        get { return privateCustomerToEdit; }
        set
        {
            privateCustomerToEdit = value;
            OnPropertyChanged();
        }
    }

    private PrivateCustomer _viewedPrivateCustomer;
    public PrivateCustomer ViewedPrivateCustomer
    {
        get { return _viewedPrivateCustomer; }
        set
        {
            _viewedPrivateCustomer = value;
            OnPropertyChanged(nameof(ViewedPrivateCustomer));
            OnPropertyChanged(nameof(FullName));

            if (_viewedPrivateCustomer != null)
            {
                ProspectNotesList = new ObservableCollection<ProspectNote>(
                    _viewedPrivateCustomer.ProspectNotes
                );
            }
            else
            {
                ProspectNotesList?.Clear();
            }
        }
    }

    private bool isEditPrivatePopUpOpen;
    public bool IsEditPrivateOpen
    {
        get { return isEditPrivatePopUpOpen; }
        set
        {
            isEditPrivatePopUpOpen = value;
            OnPropertyChanged(nameof(IsEditPrivateOpen));
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

    private Insurance selectedInsurance;
    public Insurance SelectedInsurance
    {
        get => selectedInsurance;
        set
        {
            selectedInsurance = value;
            OnPropertyChanged(nameof(SelectedInsurance));
            ShowAttributesAndSpecs();
        }
    }

    private ObservableCollection<InsuranceSpecAndAttributeInformation> insuranceSpecsAndAttributesInformation;
    public ObservableCollection<InsuranceSpecAndAttributeInformation> InsuranceSpecsAndAttributesInformation
    {
        get => insuranceSpecsAndAttributesInformation;
        set
        {
            insuranceSpecsAndAttributesInformation = value;
            OnPropertyChanged(nameof(InsuranceSpecsAndAttributesInformation));
        }
    }

    private ObservableCollection<ProspectNote> _prospectNotesList = null!;
    public ObservableCollection<ProspectNote> ProspectNotesList
    {
        get { return _prospectNotesList; }
        set
        {
            _prospectNotesList = value;
            OnPropertyChanged(nameof(ProspectNotesList));
        }
    }

    private bool isPrivateSelected;
    public bool IsPrivateSelected
    {
        get { return isPrivateSelected; }
        set
        {
            CustomerInsurances.Clear();
            isPrivateSelected = value;
            OnPropertyChanged(nameof(IsPrivateSelected));
            OnPropertyChanged(nameof(IsPrivateColumnVisible));
            SearchValue = null;
        }
    }

    private bool isRemovePrivateCustomerPopupOpen;
    public bool IsRemovePrivateCustomerPopupOpen
    {
        get { return isRemovePrivateCustomerPopupOpen; }
        set
        {
            isRemovePrivateCustomerPopupOpen = value;
            OnPropertyChanged(nameof(IsRemovePrivateCustomerPopupOpen));
        }
    }
    #endregion
    #region Commands
    public ICommand FindPrivateCustomerCommand { get; private set; }
    public ICommand AddPrivateProspectNoteCommand { get; private set; }

    private ICommand _removePrivateCustomerCommand = null!;
    public ICommand RemovePrivateCustomerCommand =>
        _removePrivateCustomerCommand ??= new RelayCommand(
            RemovePrivateCustomer,
            () => _viewedPrivateCustomer != null
        );

    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new SearchCustomerProfileViewModel(_user));
        });

    private ICommand _saveEditedPrivateCustomerCommand = null!;
    public ICommand SaveEditedPrivateCustomerCommand =>
        _saveEditedPrivateCustomerCommand ??= new RelayCommand(SaveEditedPrivateCustomer);

    private ICommand _onEditPrivateCustomerClickedCommand = null!;
    public ICommand OnEditPrivateCustomerClickedCommand =>
        _onEditPrivateCustomerClickedCommand ??= new RelayCommand(() =>
        {
            PrivateCustomerToEdit = ViewedPrivateCustomer;

            Mediator.Notify(
                "ChangeView",
                new EditPrivateCustomerViewModel(_user, PrivateCustomerToEdit)
            );
        });

    private ICommand _insuranceViewCommand = null!;
    public ICommand InsuranceViewCommand =>
        _insuranceViewCommand ??= new RelayCommand(
            () =>
            {
                Mediator.Notify(
                    "ChangeView",
                    new InsuranceInformationPrivateViewModel(_user, _viewedPrivateCustomer)
                );
            },
            () => _viewedPrivateCustomer != null
        );
    #endregion
    #region Constructors
    public PrivateCustomerProfileViewModel(LoggedInUser user, PrivateCustomer privateCustomerToEdit)
    {
        _user = user;
        ViewedPrivateCustomer = privateCustomerToEdit;
    }

    public PrivateCustomerProfileViewModel(PrivateCustomer privateCustomerToEdit)
    {
        ViewedPrivateCustomer = privateCustomerToEdit;
    }

    public PrivateCustomerProfileViewModel(LoggedInUser user)
    {
        _user = user;
        _isAdmin = CheckIfUSerIsAdmin(user);
        FindPrivateCustomerCommand = new RelayCommand(GetOnePrivateCustomerBySsn);
        AddPrivateProspectNoteCommand = new RelayCommand(AddPrivateProspectNote);

        IsRemovePrivateCustomerPopupOpen = false;
        CustomerInsurances = new ObservableCollection<Insurance>();
        InsuranceSpecsAndAttributesInformation =
            new ObservableCollection<InsuranceSpecAndAttributeInformation>();
    }
    #endregion
    #region Methods
    private void ShowAttributesAndSpecs()
    {
        InsuranceSpecsAndAttributesInformation.Clear();
        if (SelectedInsurance != null)
        {
            IList<InsuranceSpec> insuranceSpecs =
                insuranceSpecController.GetAllInsuranceSpecsForInsurance(
                    selectedInsurance.InsuranceId
                );

            foreach (InsuranceSpec spec in insuranceSpecs)
            {
                InsuranceSpecAndAttributeInformation tempInsuranceSpecAndAttributeInformation =
                    new InsuranceSpecAndAttributeInformation(
                        spec.InsuranceTypeAttribute.InsuranceAttribute,
                        spec.Value
                    );
                InsuranceSpecsAndAttributesInformation.Add(
                    tempInsuranceSpecAndAttributeInformation
                );
            }
        }
    }

    private void UpdatePrivateCustomer()
    {
        ViewedPrivateCustomer = new PrivateCustomer();
        ViewedPrivateCustomer = customerController.GetOnePrivateCustomerBySsn(SearchValue);
    }

    private void UpdateInsuranceList()
    {
        if (ViewedPrivateCustomer != null)
        {
            CustomerInsurances = new ObservableCollection<Insurance>(
                insuranceController.GetCompanyCustomerInsurancesByCustomerId(
                    ViewedPrivateCustomer.CustomerID
                )
            );
        }
    }

    private bool CheckIfUSerIsAdmin(LoggedInUser user)
    {
        return user.AuthorizationLevel == AuthorizationLevel.Admin;
    }

    private void ChangeInsuranceStatus()
    {
        if (SelectedInsurance == null)
            return;
        if (ViewedPrivateCustomer == null)
            return;
        if (IsActiveStatusSelected == true)
        {
            insuranceController.SetInsuranceStatusToActive(SelectedInsurance);
            foreach (var insurance in ViewedPrivateCustomer.Insurances)
            {
                if (insurance.InsuranceId == selectedInsurance.InsuranceId)
                {
                    insurance.InsuranceStatus = InsuranceStatus.Active;
                }
            }
            MessageBox.Show("Status på avtalet är ändrad till aktiv");
            CustomerInsurances.Clear();
            UpdateInsuranceList();
            UpdatePrivateCustomer();
        }
        else if (IsInactiveStatusSelected == true)
        {
            insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
            foreach (var insurance in ViewedPrivateCustomer.Insurances)
            {
                if (insurance.InsuranceId == selectedInsurance.InsuranceId)
                {
                    insurance.InsuranceStatus = InsuranceStatus.Inactive;
                }
            }
            MessageBox.Show("Status på avtalet är ändrad till inaktiv");
            CustomerInsurances.Clear();
            UpdateInsuranceList();
            UpdatePrivateCustomer();
        }
    }

    private void CloseEditCustomer()
    {
        IsValidated = false;
        PrivateCustomerToEdit = null;
    }

    private void SaveEditedPrivateCustomer()
    {
        if (!ValidatePrivateCustomer(out string validationErrors))
        {
            MessageBox.Show(validationErrors);
        }

        PrivateCustomerToEdit = ViewedPrivateCustomer;
        PrivateCustomerToEdit.FirstName = CapitalizeFirstLetter(PrivateCustomerToEdit.FirstName);
        PrivateCustomerToEdit.LastName = CapitalizeFirstLetter(PrivateCustomerToEdit.LastName);
        PrivateCustomerToEdit.City = CapitalizeFirstLetter(PrivateCustomerToEdit.City);
        PrivateCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
            PrivateCustomerToEdit.StreetAddress
        );

        ViewedPrivateCustomer = PrivateCustomerToEdit;
        customerController.UpdatePrivateCustomer(ViewedPrivateCustomer);

        MessageBox.Show("Ändringar är sparade");
        FullName = $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";
    }

    private void GetOnePrivateCustomerBySsn()
    {
        ViewedPrivateCustomer = customerController.GetOnePrivateCustomerBySsn(SearchValue)!;
        if (ViewedPrivateCustomer != null)
        {
            CustomerInsurances.Clear();
            UpdateInsuranceList();
        }
        else
        {
            MessageBox.Show("Ingen kund matchar din sökning");
            CustomerInsurances.Clear();
        }
    }

    //Add a note to the private prospect
    private void AddPrivateProspectNote()
    {
        if (ViewedPrivateCustomer == null)
            return;
        if (!string.IsNullOrWhiteSpace(Note))
        {
            Insurance insurance = ViewedPrivateCustomer.Insurances.FirstOrDefault(); //Gör om logiken så inloggad person blir user istället, endast tillfällig lösning
            User user = insurance.User;

            ProspectNote prospectNote = new ProspectNote(
                Note,
                DateTime.Now,
                user,
                ViewedPrivateCustomer
            );
            customerController.AddProspectNote(prospectNote);
            ViewedPrivateCustomer.ProspectNotes.Add(prospectNote);
            ProspectNotesList.Add(prospectNote);
            MessageBox.Show("Anteckning tillagd");
            Note = string.Empty;
        }
        else
        {
            MessageBox.Show("Var god fyll i utfall");
        }
    }

    private void RemovePrivateCustomer()
    {
        int checker = 0;

        if (ViewedPrivateCustomer == null)
            return;
        UpdatePrivateCustomer();

        switch (ViewedPrivateCustomer.Insurances.Count)
        {
            case < 1:
                customerController.RemovePrivateCustomer(ViewedPrivateCustomer);
                ViewedPrivateCustomer = null!;
                CustomerInsurances = null!;
                MessageBox.Show("Kunden är nu borttagen ur systemet.");
                break;
            case > 0:
            {
                checker += ViewedPrivateCustomer.Insurances.Count(insurance =>
                    insurance.InsuranceStatus == InsuranceStatus.Active
                    || insurance.InsuranceStatus == InsuranceStatus.Preliminary
                );

                switch (checker)
                {
                    case > 0:
                        MessageBox.Show(
                            $"Tyvärr kan du inte ta bort kunden: \"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}\", det finns aktiva eller preliminära försäkringar kopplad till kunden. Hantera dem först."
                        );
                        break;
                    case 0:
                        IsRemovePrivateCustomerPopupOpen = true;
                        break;
                }

                break;
            }
        }
    }

    public void OnContinueClicked()
    {
        IsRemovePrivateCustomerPopupOpen = false;

        if (ViewedPrivateCustomer == null)
            return;
        customerController.RemovePrivateCustomerAndInactiveInsurances(ViewedPrivateCustomer);
        ViewedPrivateCustomer = null;
        CustomerInsurances.Clear();

        MessageBox.Show("Kunden är nu borttagen ur systemet.");
    }

    public void OnCancelClicked()
    {
        IsRemovePrivateCustomerPopupOpen = false;
    }

    private void RemoveChosenInsurance()
    {
        if (ViewedPrivateCustomer != null && SelectedInsurance != null)
        {
            if (SelectedInsurance.InsuranceStatus == InsuranceStatus.Active)
            {
                MessageBox.Show(
                    "Detta är en aktiv försäkring, du måste avsluta den innan du kan ta bort den"
                );
            }
            else
            {
                insuranceController.RemoveInsurance(SelectedInsurance);
                CustomerInsurances.Clear();
                CustomerInsurances = new ObservableCollection<Insurance>(
                    insuranceController.GetCompanyCustomerInsurancesByCustomerId(
                        ViewedPrivateCustomer.CustomerID
                    )
                );
            }
        }
        SelectedInsurance = null;
    }

    //Go to insurances view
    private void GoToAddInsurance()
    {
        Mediator.Notify("ChangeView", new RegisterPreliminaryInsuranceViewModel());
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    private bool ValidatePrivateCustomer(out string validationErrors)
    {
        validationErrors = "";

        if (ViewedPrivateCustomer == null)
        {
            validationErrors = "Kundinformation saknas.";
            IsValidated = false;
            return false;
        }

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.FirstName))
            validationErrors += "Förnamn saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.LastName))
            validationErrors += "Efternamn saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.StreetAddress))
            validationErrors += "Gatuadress saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.PostalCode))
            validationErrors += "Postnummer saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.City))
            validationErrors += "Stad saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.TelephoneNumber))
            validationErrors += "Telefonnummer saknas.\n";

        if (string.IsNullOrEmpty(ViewedPrivateCustomer.Email))
            validationErrors += "E-postadress saknas.\n";

        if (
            ViewedPrivateCustomer.PostalCode.Length != 5
            || !ViewedPrivateCustomer.PostalCode.All(char.IsDigit)
        )
            validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

        if (ViewedPrivateCustomer.SSN.Length != 12 || !ViewedPrivateCustomer.SSN.All(char.IsDigit))
            validationErrors += "Personnumret måste innehålla exakt 12 siffror.\n";

        IsValidated = string.IsNullOrEmpty(validationErrors);
        return IsValidated;
    }
    #endregion
}
