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

    private bool isEditCompanyOpen;
    public bool IsEditCompanyOpen
    {
        get { return isEditCompanyOpen; }
        set
        {
            isEditCompanyOpen = value;
            OnPropertyChanged(nameof(IsEditCompanyOpen));
        }
    }

    private bool isRemoveCompanyCustomerPopupOpen;
    public bool IsRemoveCompanyCustomerPopupOpen
    {
        get { return isRemoveCompanyCustomerPopupOpen; }
        set
        {
            isRemoveCompanyCustomerPopupOpen = value;
            OnPropertyChanged(nameof(IsRemoveCompanyCustomerPopupOpen));
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

    private bool isCompanySelected;
    public bool IsCompanySelected
    {
        get { return isCompanySelected; }
        set
        {
            // CustomerInsurances.Clear();
            isCompanySelected = value;
            OnPropertyChanged(nameof(IsCompanySelected));
            SearchValue = null;
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

            if (_viewedCompanyCustomer != null)
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
    private CompanyCustomer companyCustomerToEdit;
    public CompanyCustomer CompanyCustomerToEdit
    {
        get { return companyCustomerToEdit; }
        set
        {
            companyCustomerToEdit = value;
            OnPropertyChanged(nameof(CompanyCustomerToEdit));
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

    private string currentView;
    public string CurrentView
    {
        get => currentView;
        set
        {
            currentView = value;
            OnPropertyChanged(nameof(CurrentView));
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
    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new SearchCustomerProfileViewModel());
        });

    private ICommand _onEditCompanyCustomerClickedCommand = null!;
    public ICommand OnEditCompanyCustomerClickedCommand =>
        _onEditCompanyCustomerClickedCommand ??= new RelayCommand(() =>
        {
            CompanyCustomerToEdit = ViewedCompanyCustomer;
            Mediator.Notify("ChangeView", new EditCompanyCustomerViewModel(CompanyCustomerToEdit));
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

    public CompanyCustomerProfileViewModel(CompanyCustomer companyCustomer)
    {
        ViewedCompanyCustomer = companyCustomer;
    }

    public CompanyCustomerProfileViewModel(LoggedInUser user)
    {
        _user = user;
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

    #region methods



    // private void UpdateInsuranceList()
    // {
    //     if (ViewedCompanyCustomer == null)
    //         return;
    //      CustomerInsurances = new ObservableCollection<Insurance>(
    //         insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID)
    //     );
    // }

    private void UpdateCompanyCustomer() //Kanske ändra till customerid
    {
        ViewedCompanyCustomer = null;
        ViewedCompanyCustomer = customerController.GetOneCompanyCustomerByOrgNr(SearchValue);
    }

    // private void ChangeInsuranceStatus()
    // {
    //     if (SelectedInsurance == null)
    //         return;
    //     if (ViewedCompanyCustomer == null)
    //         return;
    //     if (IsActiveStatusSelected == true)
    //     {
    //         insuranceController.SetInsuranceStatusToActive(SelectedInsurance);
    //         foreach (var insurance in ViewedCompanyCustomer.Insurances)
    //         {
    //             if (insurance.InsuranceId == selectedInsurance.InsuranceId)
    //             {
    //                 insurance.InsuranceStatus = InsuranceStatus.Active;
    //             }
    //         }
    //         MessageBox.Show("Status på avtalet är ändrad till aktiv");
    //         CustomerInsurances.Clear();
    //         UpdateInsuranceList();
    //         UpdateCompanyCustomer();
    //     }
    //     else if (IsInactiveStatusSelected == true)
    //     {
    //         insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
    //         foreach (var insurance in ViewedCompanyCustomer.Insurances)
    //         {
    //             if (insurance.InsuranceId == selectedInsurance.InsuranceId)
    //             {
    //                 insurance.InsuranceStatus = InsuranceStatus.Inactive;
    //             }
    //         }
    //         MessageBox.Show("Status på avtalet är ändrad till inaktiv");
    //         CustomerInsurances.Clear();
    //         UpdateInsuranceList();
    //         UpdateCompanyCustomer();
    //     }
    // }

    // private void OnEditCompanyCustomerClicked()
    // {
    //     if (ViewedCompanyCustomer != null)
    //     {
    //         CompanyCustomerToEdit = ViewedCompanyCustomer;
    //         CurrentView = "EditCompanyCustomer";
    //     }
    // }

    //Cancel the editing of a customer
    private void CloseEditCustomer()
    {
        IsValidated = false;
        CompanyCustomerToEdit = null!;
    }

    private void SaveEditedCompanyCustomer()
    {
        // Kontrollera validering och samla felmeddelanden
        if (!ValidateCompanyCustomer(out string validationErrors))
        {
            MessageBox.Show(validationErrors);
            return;
        }

        // Sätt den redigerade kundens data
        CompanyCustomerToEdit = ViewedCompanyCustomer;
        CompanyCustomerToEdit.CompanyName = CapitalizeFirstLetter(
            CompanyCustomerToEdit.CompanyName
        );
        CompanyCustomerToEdit.City = CapitalizeFirstLetter(CompanyCustomerToEdit.City);
        CompanyCustomerToEdit.StreetAddress = CapitalizeFirstLetter(
            CompanyCustomerToEdit.StreetAddress
        );

        // Uppdatera och spara
        ViewedCompanyCustomer = CompanyCustomerToEdit;
        customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);

        // Bekräftelsemeddelande
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

    //Add a note to the company prospect
    private void AddCompanyProspectNote()
    {
        if (ViewedCompanyCustomer == null)
            return;
        if (!string.IsNullOrWhiteSpace(Note))
        {
            Insurance insurance = ViewedCompanyCustomer.Insurances.FirstOrDefault(); //Gör om logiken så inloggad person blir user istället, endast tillfällig lösning
            User user = insurance.User;

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
                ViewedCompanyCustomer = null;
                // CustomerInsurances = null;
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

    public void OnContinueClicked()
    {
        IsRemoveCompanyCustomerPopupOpen = false;

        if (ViewedCompanyCustomer == null)
            return;
        customerController.RemoveCompanyCustomerAndInactiveInsurances(ViewedCompanyCustomer);
        ViewedCompanyCustomer = null;
        // CustomerInsurances.Clear();

        MessageBox.Show("Kunden är nu borttagen ur systemet.");
    }

    public void OnCancelClicked()
    {
        IsRemoveCompanyCustomerPopupOpen = false;
    }

    // private void RemoveChosenInsurance()
    // {
    //     if (ViewedCompanyCustomer != null && SelectedInsurance != null)
    //     {
    //         if (SelectedInsurance.InsuranceStatus == InsuranceStatus.Active)
    //         {
    //             MessageBox.Show(
    //                 "Detta är en aktiv försäkring, du måste avsluta den innan du kan ta bort den"
    //             );
    //         }
    //         else
    //         {
    //             insuranceController.RemoveInsurance(SelectedInsurance);
    //             CustomerInsurances.Clear();
    //             CustomerInsurances = new ObservableCollection<Insurance>(
    //                 insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID)
    //             );
    //         }
    //     }
    //     SelectedInsurance = null;
    // }

    //Go to insurances view
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
