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

public class CompanyCustomerProfileViewModel : ObservableObject
{
    #region Initiation of objects
    private CustomerController customerController = new CustomerController();
    private InsuranceController insuranceController = new InsuranceController();
    private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
    private UserController userController = new UserController();
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

    private bool _isEditCompanyOpen;
    public bool IsEditCompanyOpen
    {
        get { return _isEditCompanyOpen; }
        set
        {
            _isEditCompanyOpen = value;
            OnPropertyChanged(nameof(IsEditCompanyOpen));
        }
    }

    private bool _isRemoveCompanyCustomerPopupOpen;
    public bool IsRemoveCompanyCustomerPopupOpen
    {
        get { return _isRemoveCompanyCustomerPopupOpen; }
        set
        {
            _isRemoveCompanyCustomerPopupOpen = value;
            OnPropertyChanged(nameof(IsRemoveCompanyCustomerPopupOpen));
        }
    }

    private bool _isActiveStatusSelected;
    public bool IsActiveStatusSelected
    {
        get { return _isActiveStatusSelected; }
        set
        {
            _isActiveStatusSelected = value;
            OnPropertyChanged(nameof(IsActiveStatusSelected));
        }
    }
    private bool _isInactiveStatusSelected;
    public bool IsInactiveStatusSelected
    {
        get { return _isInactiveStatusSelected; }
        set
        {
            _isInactiveStatusSelected = value;
            OnPropertyChanged(nameof(IsInactiveStatusSelected));
        }
    }

    private bool _isCompanySelected;
    public bool IsCompanySelected
    {
        get { return _isCompanySelected; }
        set
        {
            _isCompanySelected = value;
            OnPropertyChanged(nameof(IsCompanySelected));
            SearchValue = string.Empty;
        }
    }

    public bool IsCompanyColumnVisible => IsCompanySelected;

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

    private CompanyCustomer _viewedCompanyCustomer;
    public CompanyCustomer ViewedCompanyCustomer
    {
        get { return _viewedCompanyCustomer; }
        set
        {
            _viewedCompanyCustomer = value;
            OnPropertyChanged();

            if (_viewedCompanyCustomer?.ProspectNotes != null)
            {
                ProspectNotesList = new ObservableCollection<ProspectNote>(
                    _viewedCompanyCustomer.ProspectNotes
                );
            }
            else
            {
                ProspectNotesList?.Clear();
            }
        }
    }
    private CompanyCustomer _companyCustomerToEdit;
    public CompanyCustomer CompanyCustomerToEdit
    {
        get { return _companyCustomerToEdit; }
        set
        {
            _companyCustomerToEdit = value;
            OnPropertyChanged(nameof(CompanyCustomerToEdit));
        }
    }

    private ObservableCollection<InsuranceSpecAndAttributeInformation> _insuranceSpecsAndAttributesInformation;
    public ObservableCollection<InsuranceSpecAndAttributeInformation> InsuranceSpecsAndAttributesInformation
    {
        get => _insuranceSpecsAndAttributesInformation;
        set
        {
            _insuranceSpecsAndAttributesInformation = value;
            OnPropertyChanged(nameof(InsuranceSpecsAndAttributesInformation));
        }
    }

    private string _currentView;
    public string CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    private bool _isValidated;
    public bool IsValidated
    {
        get { return _isValidated; }
        set
        {
            _isValidated = value;
            OnPropertyChanged(nameof(IsValidated));
        }
    }
    #endregion
    #region Commands
    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new SearchCustomerProfileViewModel(_user));
        });

    private ICommand _onEditCompanyCustomerClickedCommand = null!;
    public ICommand OnEditCompanyCustomerClickedCommand =>
        _onEditCompanyCustomerClickedCommand ??= new RelayCommand(() =>
        {
            CompanyCustomerToEdit = ViewedCompanyCustomer;
            Mediator.Notify(
                "ChangeView",
                new EditCompanyCustomerViewModel(_user, CompanyCustomerToEdit)
            );
        });

    private ICommand _insuranceViewCommand = null!;
    public ICommand InsuranceViewCommand =>
        _insuranceViewCommand ??= new RelayCommand(
            () =>
            {
                Mediator.Notify(
                    "ChangeView",
                    new InsuranceInformationCompanyViewModel(_user, _viewedCompanyCustomer)
                );
            },
            () => _viewedCompanyCustomer != null
        );

    private ICommand _removeCompanyCustomerCommand = null!;
    public ICommand RemoveCompanyCustomerCommand =>
        _removeCompanyCustomerCommand ??= new RelayCommand(
            RemoveCompanyCustomer,
            () => _viewedCompanyCustomer != null
        );

    public ICommand FindCompanyCustomerCommand { get; private set; }
    public ICommand AddCompanyProspectNoteCommand { get; private set; }
    public ICommand ReturnCommand { get; private set; }
    public ICommand GoToInsurancesCommand { get; private set; }
    public ICommand ContinueCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand RemoveInsuranceCommand { get; private set; }
    public ICommand ChangeInsuranceStatusCommand { get; private set; }
    public ICommand CloseEditCustomerCommand { get; private set; }
    public ICommand GoToAddInsuranceCommand { get; private set; }

    public ICommand ShowInsuranceHolderCommand =>
        new RelayCommand(() => CurrentView = "CustomerProfile");
    public ICommand ShowInsuredPersonCommand =>
        new RelayCommand(() => CurrentView = "EditCompanyCustomer");
    public ICommand ShowInsuranceDetailsCommand =>
        new RelayCommand(() => CurrentView = "EditPrivateCustomer");
    #endregion
    #region Constructors
    public CompanyCustomerProfileViewModel(LoggedInUser user, CompanyCustomer companyCustomer)
    {
        _user = user;
        ViewedCompanyCustomer = companyCustomer;
    }

    public CompanyCustomerProfileViewModel(CompanyCustomer companyCustomer)
    {
        ViewedCompanyCustomer = companyCustomer;
    }

    public CompanyCustomerProfileViewModel(LoggedInUser user)
    {
        _user = user;
        _isAdmin = CheckIfUSerIsAdmin(user);
        FindCompanyCustomerCommand = new RelayCommand(GetOneCompanyCustomerByOrgNr);
        AddCompanyProspectNoteCommand = new RelayCommand(AddCompanyProspectNote);

        ContinueCommand = new RelayCommand(OnContinueClicked);
        CancelCommand = new RelayCommand(OnCancelClicked);
        // RemoveInsuranceCommand = new RelayCommand(RemoveChosenInsurance);
        // ChangeInsuranceStatusCommand = new RelayCommand(ChangeInsuranceStatus);
        CloseEditCustomerCommand = new RelayCommand(CloseEditCustomer);
        GoToAddInsuranceCommand = new RelayCommand(GoToAddInsurance);

        // CustomerInsurances = new ObservableCollection<Insurance>();
        InsuranceSpecsAndAttributesInformation =
            new ObservableCollection<InsuranceSpecAndAttributeInformation>();

        IsCompanySelected = true;
        IsRemoveCompanyCustomerPopupOpen = false;
    }
    #endregion
    #region Methods
    private void UpdateCompanyCustomer()
    {
        ViewedCompanyCustomer = new CompanyCustomer();
        ViewedCompanyCustomer = customerController.GetOneCompanyCustomerByOrgNr(SearchValue);
    }

    private void CloseEditCustomer()
    {
        IsValidated = false;
        CompanyCustomerToEdit = null!;
    }

    private bool CheckIfUSerIsAdmin(LoggedInUser user)
    {
        return user.AuthorizationLevel == AuthorizationLevel.Admin;
    }

    private void SaveEditedCompanyCustomer()
    {
        if (!ValidateCompanyCustomer(out string validationErrors))
        {
            MessageBox.Show(validationErrors);
            return;
        }

        CompanyCustomerToEdit = ViewedCompanyCustomer;
        CompanyCustomerToEdit.CompanyName = CapitalizeFirstLetter(
            CompanyCustomerToEdit.CompanyName
        );
        CompanyCustomerToEdit.City = CapitalizeFirstLetter(CompanyCustomerToEdit.City);
        CompanyCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
            CompanyCustomerToEdit.StreetAddress
        );

        ViewedCompanyCustomer = CompanyCustomerToEdit;
        customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);
        MessageBox.Show("Ändringar är sparade");
    }

    private void GetOneCompanyCustomerByOrgNr()
    {
        ViewedCompanyCustomer = customerController.GetOneCompanyCustomerByOrgNr(SearchValue);
        if (ViewedCompanyCustomer == null)
        {
            MessageBox.Show("Ingen kund matchar din sökning");
        }
    }

    private void AddCompanyProspectNote()
    {
        if (ViewedCompanyCustomer == null)
            return;
        if (!string.IsNullOrWhiteSpace(Note))
        {
            User user = userController.GetUser(_user.UserID);

            ProspectNote prospectNote = new ProspectNote(
                Note,
                DateTime.Now,
                user,
                ViewedCompanyCustomer
            );
            customerController.AddProspectNote(prospectNote);
            ViewedCompanyCustomer.ProspectNotes.Add(prospectNote);
            ProspectNotesList.Add(prospectNote);
            MessageBox.Show("Anteckning tillagd");
            Note = string.Empty;
        }
        else
        {
            MessageBox.Show("Var god fyll i utfall");
        }
    }

    private void RemoveCompanyCustomer()
    {
        int checker = 0;

        if (ViewedCompanyCustomer != null)
        {
            if (ViewedCompanyCustomer.Insurances.Count < 1)
            {
                customerController.RemoveCompanyCustomer(ViewedCompanyCustomer);
                ViewedCompanyCustomer = new CompanyCustomer();

                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
            else if (ViewedCompanyCustomer.Insurances.Count > 0)
            {
                foreach (Insurance insurance in ViewedCompanyCustomer.Insurances)
                {
                    if (
                        insurance.InsuranceStatus == InsuranceStatus.Active
                        || insurance.InsuranceStatus == InsuranceStatus.Preliminary
                    )
                    {
                        checker++;
                    }
                }

                if (checker > 0)
                {
                    MessageBox.Show(
                        $"Tyvärr kan du inte ta bort kunden: \"{ViewedCompanyCustomer.CompanyName}\", det finns aktiva eller preliminära försäkringar kopplad till kunden. Hantera dem först."
                    );
                }
                else
                {
                    IsRemoveCompanyCustomerPopupOpen = true;
                }
            }
        }
    }

    private void OnContinueClicked()
    {
        IsRemoveCompanyCustomerPopupOpen = false;

        if (ViewedCompanyCustomer == null)
            return;
        customerController.RemoveCompanyCustomerAndInactiveInsurances(ViewedCompanyCustomer);
        ViewedCompanyCustomer = null;
        // CustomerInsurances.Clear();

        MessageBox.Show("Kunden är nu borttagen ur systemet.");
    }

    private void OnCancelClicked()
    {
        IsRemoveCompanyCustomerPopupOpen = false;
    }

    private void GoToAddInsurance()
    {
        Mediator.Notify("ChangeView", new RegisterPreliminaryInsuranceViewModel());
    }
    #endregion
    #region Errorhandling
    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

    private bool ValidateCompanyCustomer(out string validationErrors)
    {
        validationErrors = "";

        if (ViewedCompanyCustomer == null)
        {
            validationErrors = "Kundinformation saknas.";
            IsValidated = false;
            return false;
        }

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.CompanyName))
            validationErrors += "Företagsnamn saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.StreetAddress))
            validationErrors += "Gatuadress saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.PostalCode))
            validationErrors += "Postnummer saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.City))
            validationErrors += "Stad saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.TelephoneNumber))
            validationErrors += "Telefonnummer saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.Email))
            validationErrors += "E-postadress saknas.\n";

        if (string.IsNullOrEmpty(ViewedCompanyCustomer.ContactPersonName))
            validationErrors += "Kontaktperson saknas.\n";

        if (
            ViewedCompanyCustomer.PostalCode.Length != 5
            || !ViewedCompanyCustomer.PostalCode.All(char.IsDigit)
        )
            validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

        if (
            ViewedCompanyCustomer.OrganisationNumber.Length != 10
            || !ViewedCompanyCustomer.OrganisationNumber.All(char.IsDigit)
        )
            validationErrors += "Personnumret måste innehålla exakt 12 siffror.\n";

        IsValidated = string.IsNullOrEmpty(validationErrors);
        return IsValidated;
    }
    #endregion
}
