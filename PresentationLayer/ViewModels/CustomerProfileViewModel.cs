using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Identity.Client;
using Models;
using Models.InsuranceInformation;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Views;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    internal class CustomerProfileViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();
        private InsuranceController insuranceController = new InsuranceController();
        private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();

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

        private ObservableCollection<ProspectNote> prospectNotesList = null;
        public ObservableCollection<ProspectNote> ProspectNotesList
        {
            get { return prospectNotesList; }
            set
            {
                prospectNotesList = value;
                OnPropertyChanged(nameof(ProspectNotesList));
            }
        }
        private bool isCompanySelected;
        public bool IsCompanySelected
        {
            get { return isCompanySelected; }
            set
            {
                ViewedPrivateCustomer = null;
                CustomerInsurances.Clear();
                isCompanySelected = value;
                OnPropertyChanged(nameof(IsCompanySelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                SearchValue = null;
            }
        }

        private bool isPrivateSelected;
        public bool IsPrivateSelected
        {
            get { return isPrivateSelected; }
            set
            {
                ViewedCompanyCustomer = null;
                CustomerInsurances.Clear();
                isPrivateSelected = value;
                OnPropertyChanged(nameof(IsPrivateSelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                SearchValue = null;
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
        public bool IsCompanyColumnVisible => IsCompanySelected;


        private PrivateCustomer viewedPrivateCustomer;
        public PrivateCustomer ViewedPrivateCustomer
        {
            get { return viewedPrivateCustomer; }
            set
            {
                viewedPrivateCustomer = value;
                OnPropertyChanged(nameof(ViewedPrivateCustomer));
                OnPropertyChanged(nameof(FullName));

                if (viewedPrivateCustomer != null)
                {
                    ProspectNotesList = new ObservableCollection<ProspectNote>(
                        viewedPrivateCustomer.ProspectNotes
                    );
                }
                else
                {
                    ProspectNotesList?.Clear();
                }
            }
        }

        private CompanyCustomer viewedCompanyCustomer;
        public CompanyCustomer ViewedCompanyCustomer
        {
            get { return viewedCompanyCustomer; }
            set
            {
                viewedCompanyCustomer = value;
                OnPropertyChanged();

                if (viewedCompanyCustomer != null)
                {
                    ProspectNotesList = new ObservableCollection<ProspectNote>(
                        viewedCompanyCustomer.ProspectNotes
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
            set { companyCustomerToEdit = value; OnPropertyChanged(nameof(CompanyCustomerToEdit)); }
        }

        private PrivateCustomer privateCustomerToEdit;
        public PrivateCustomer PrivateCustomerToEdit
        {
            get { return privateCustomerToEdit; }
            set { privateCustomerToEdit = value; OnPropertyChanged(nameof(PrivateCustomerToEdit)); }
        }



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

        public ICommand FindPrivateCustomerCommand { get; private set; }
        public ICommand FindCompanyCustomerCommand { get; private set; }
        public ICommand AddPrivateProspectNoteCommand { get; private set; }
        public ICommand AddCompanyProspectNoteCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand GoToInsurancesCommand { get; private set; }
        public ICommand OnEditCompanyCustomerClickedCommand { get; private set; }
        public ICommand OnEditPrivateCustomerClickedCommand { get; private set; }
        public ICommand SaveEditedCompanyCustomerCommand { get; private set; }
        public ICommand SaveEditedPrivateCustomerCommand { get; private set; }
        public ICommand RemoveCompanyCustomerCommand { get; private set; }
        public ICommand RemovePrivateCustomerCommand { get; private set; }
        public ICommand ContinueCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RemoveInsuranceCommand { get; private set; }
        public ICommand ChangeInsuranceStatusCommand { get; private set; }
        public ICommand CloseEditCustomerCommand { get; private set; }
        public ICommand GoToAddInsuranceCommand { get; private set; }

        public ICommand ShowInsuranceHolderCommand => new RelayCommand(() => CurrentView = "CustomerProfile");
        public ICommand ShowInsuredPersonCommand => new RelayCommand(() => CurrentView = "EditCompanyCustomer");
        public ICommand ShowInsuranceDetailsCommand => new RelayCommand(() => CurrentView = "EditPrivateCustomer");


        public CustomerProfileViewModel()
        {
            FindPrivateCustomerCommand = new RelayCommand(FindPrivateCustomer);
            FindCompanyCustomerCommand = new RelayCommand(FindCompanyCustomer);
            AddPrivateProspectNoteCommand = new RelayCommand(AddPrivateProspectNote);
            AddCompanyProspectNoteCommand = new RelayCommand(AddCompanyProspectNote);

            OnEditCompanyCustomerClickedCommand = new RelayCommand(OnEditCompanyCustomerClicked);
            OnEditPrivateCustomerClickedCommand = new RelayCommand(OnEditPrivateCustomerClicked);
            SaveEditedCompanyCustomerCommand = new RelayCommand(SaveEditedCompanyCustomer);
            SaveEditedPrivateCustomerCommand = new RelayCommand(SaveEditedPrivateCustomer);
            RemoveCompanyCustomerCommand = new RelayCommand(RemoveCompanyCustomer);
            RemovePrivateCustomerCommand = new RelayCommand(RemovePrivateCustomer);
            ContinueCommand = new RelayCommand(OnContinueClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
            RemoveInsuranceCommand = new RelayCommand(RemoveChosenInsurance);
            ChangeInsuranceStatusCommand = new RelayCommand(ChangeInsuranceStatus);
            CloseEditCustomerCommand = new RelayCommand(CloseEditCustomer);
            GoToAddInsuranceCommand = new RelayCommand(GoToAddInsurance);






            CustomerInsurances = new ObservableCollection<Insurance>();
            InsuranceSpecsAndAttributesInformation =
                new ObservableCollection<InsuranceSpecAndAttributeInformation>();

            IsCompanySelected = true;
            CurrentView = "CustomerProfile";

            IsRemoveCompanyCustomerPopupOpen = false;
            IsRemovePrivateCustomerPopupOpen = false;
        }

        #region methods

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

        private void UpdateInsuranceList()
        {
            if (ViewedCompanyCustomer != null)
            {
                CustomerInsurances = new ObservableCollection<Insurance>(
                    insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID)
                );
            }
            else if (ViewedPrivateCustomer != null)
            {
                CustomerInsurances = new ObservableCollection<Insurance>(
                    insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID)
                );
            }
        }

        private void UpdateCompanyCustomer() //Kanske ändra till customerid
        {
            ViewedCompanyCustomer = null;
            ViewedCompanyCustomer = customerController.GetSpecificCompanyCustomer(SearchValue);
        }

        private void UpdatePrivateCustomer() //Kanske ändra till customerid
        {
            ViewedPrivateCustomer = null;
            ViewedPrivateCustomer = customerController.GetSpecificPrivateCustomer(SearchValue);
        }

        private void ChangeInsuranceStatus()
        {
            if (SelectedInsurance != null)
            {
                if (ViewedCompanyCustomer != null)
                {
                    if (IsActiveStatusSelected == true)
                    {
                        insuranceController.SetInsuranceStatusToActive(SelectedInsurance);
                        foreach (var insurance in ViewedCompanyCustomer.Insurances)
                        {
                            if (insurance.InsuranceId == selectedInsurance.InsuranceId)
                            {
                                insurance.InsuranceStatus = InsuranceStatus.Active;
                            }
                        }
                        MessageBox.Show("Status på avtalet är ändrad till aktiv");
                        CustomerInsurances.Clear();
                        UpdateInsuranceList();
                        UpdateCompanyCustomer();
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
                    }
                    else if (IsInactiveStatusSelected == true)
                    {
                        insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
                        foreach (var insurance in ViewedCompanyCustomer.Insurances)
                        {
                            if (insurance.InsuranceId == selectedInsurance.InsuranceId)
                            {
                                insurance.InsuranceStatus = InsuranceStatus.Inactive;
                            }
                        }
                        MessageBox.Show("Status på avtalet är ändrad till inaktiv");
                        CustomerInsurances.Clear();
                        UpdateInsuranceList();
                        UpdateCompanyCustomer();
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
                    }
                }
                else if (ViewedPrivateCustomer != null)
                {
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
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
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
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
                    }
                }
            }
        }

        private void OnEditPrivateCustomerClicked()
        {
            if (ViewedPrivateCustomer != null)
            {
                PrivateCustomerToEdit = ViewedPrivateCustomer;
                CurrentView = "EditPrivateCustomer";
            }
        }

        private void OnEditCompanyCustomerClicked()
        {
            if (ViewedCompanyCustomer != null)
            {
                CompanyCustomerToEdit = ViewedCompanyCustomer;
                CurrentView = "EditCompanyCustomer";
            }
        }

        //Cancel the editing of a customer
        private void CloseEditCustomer()
        {
            IsValidated = false;
            PrivateCustomerToEdit = null;
            CompanyCustomerToEdit = null;
            CurrentView = "CustomerProfile";
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
            CompanyCustomerToEdit.CompanyName = CapitalizeFirstLetter(CompanyCustomerToEdit.CompanyName);
            CompanyCustomerToEdit.City = CapitalizeFirstLetter(CompanyCustomerToEdit.City);
            CompanyCustomerToEdit.StreetAddress = CapitalizeFirstLetter(CompanyCustomerToEdit.StreetAddress);

            // Uppdatera och spara
            ViewedCompanyCustomer = CompanyCustomerToEdit;
            customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);

            // Bekräftelsemeddelande
            MessageBox.Show("Ändringar är sparade");
        }

        private void SaveEditedPrivateCustomer()
        {
            // Kontrollera validering och samla felmeddelanden
            if (!ValidatePrivateCustomer(out string validationErrors))
            {
                MessageBox.Show(validationErrors);
                return;
            }

            // Sätt den redigerade kundens data
            PrivateCustomerToEdit = ViewedPrivateCustomer;
            PrivateCustomerToEdit.FirstName = CapitalizeFirstLetter(PrivateCustomerToEdit.FirstName);
            PrivateCustomerToEdit.LastName = CapitalizeFirstLetter(PrivateCustomerToEdit.LastName);
            PrivateCustomerToEdit.City = CapitalizeFirstLetter(PrivateCustomerToEdit.City);
            PrivateCustomerToEdit.StreetAddress = CapitalizeFirstLetter(PrivateCustomerToEdit.StreetAddress);

            // Uppdatera och spara
            ViewedPrivateCustomer = PrivateCustomerToEdit;
            customerController.UpdatePrivateCustomer(ViewedPrivateCustomer);

            // Bekräftelsemeddelande
            MessageBox.Show("Ändringar är sparade");
            FullName = $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";
            
        }

        private void FindCompanyCustomer()
        {
            ViewedCompanyCustomer = customerController.GetSpecificCompanyCustomer(SearchValue);
            if (ViewedCompanyCustomer != null)
            {
                CustomerInsurances.Clear();
                UpdateInsuranceList();
                //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
            }
            else
            {
                MessageBox.Show("Ingen kund matchar din sökning");
                CustomerInsurances.Clear();
            }
        }

        private void FindPrivateCustomer()
        {
            ViewedPrivateCustomer = customerController.GetSpecificPrivateCustomer(SearchValue);
            if (ViewedPrivateCustomer != null)
            {
                CustomerInsurances.Clear();
                UpdateInsuranceList();
                //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
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
            if (ViewedCompanyCustomer != null || ViewedPrivateCustomer != null)
            {
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
        }

        //Add a note to the company prospect
        private void AddCompanyProspectNote()
        {
            if (ViewedCompanyCustomer != null || ViewedPrivateCustomer != null)
            {
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
        }

        private void RemovePrivateCustomer()
        {
            int checker = 0;

            if (ViewedPrivateCustomer != null)
            {
                UpdatePrivateCustomer();

                if (ViewedPrivateCustomer.Insurances.Count < 1)
                {
                    customerController.RemovePrivateCustomer(ViewedPrivateCustomer);
                    ViewedPrivateCustomer = null;
                    CustomerInsurances = null;
                    MessageBox.Show("Kunden är nu borttagen ur systemet.");
                }
                else if (ViewedPrivateCustomer.Insurances.Count > 0)
                {
                    foreach (Insurance insurance in ViewedPrivateCustomer.Insurances)
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
                            $"Tyvärr kan du inte ta bort kunden: \"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}\", det finns aktiva eller preliminära försäkringar kopplad till kunden. Hantera dem först."
                        );
                    }
                    else if (checker == 0)
                    {
                        IsRemovePrivateCustomerPopupOpen = true;
                    }
                    
                }
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
                    CustomerInsurances = null;
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
            IsRemovePrivateCustomerPopupOpen = false;
            IsRemoveCompanyCustomerPopupOpen = false;

            if (ViewedPrivateCustomer != null)
            {
                customerController.RemovePrivateCustomerAndInactiveInsurances(
                    ViewedPrivateCustomer
                );
                ViewedPrivateCustomer = null;
                CustomerInsurances.Clear();

                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
            else if (ViewedCompanyCustomer != null)
            {
                customerController.RemoveCompanyCustomerAndInactiveInsurances(
                    ViewedCompanyCustomer
                );
                ViewedCompanyCustomer = null;
                CustomerInsurances.Clear();

                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
        }

        public void OnCancelClicked()
        {
            IsRemovePrivateCustomerPopupOpen = false;
            IsRemoveCompanyCustomerPopupOpen = false;
        }

        private void RemoveChosenInsurance()
        {
            if (ViewedCompanyCustomer != null && SelectedInsurance != null)
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
                        insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID)
                    );
                }
            }
            else if (ViewedPrivateCustomer != null && SelectedInsurance != null)
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
                        insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID)
                    );
                }
            }
            SelectedInsurance = null;
        }


        //Go to insurances view
        private void GoToAddInsurance()
        {
            throw new NotImplementedException();
        }
        #endregion




        #region Errorhandling
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

            if (ViewedPrivateCustomer.PostalCode.Length != 5 || !ViewedPrivateCustomer.PostalCode.All(char.IsDigit))
                validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

            if (ViewedPrivateCustomer.SSN.Length != 12 || !ViewedPrivateCustomer.SSN.All(char.IsDigit))
                validationErrors += "Personnumret måste innehålla exakt 12 siffror.\n";

            IsValidated = string.IsNullOrEmpty(validationErrors);
            return IsValidated;
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

            if (ViewedCompanyCustomer.PostalCode.Length != 5 || !ViewedCompanyCustomer.PostalCode.All(char.IsDigit))
                validationErrors += "Postnumret måste vara exakt 5 siffror.\n";

            if (ViewedCompanyCustomer.OrganisationNumber.Length != 10 || !ViewedCompanyCustomer.OrganisationNumber.All(char.IsDigit))
                validationErrors += "Personnumret måste innehålla exakt 12 siffror.\n";

            IsValidated = string.IsNullOrEmpty(validationErrors);
            return IsValidated;
        }

        #endregion
    }
}
