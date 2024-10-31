using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Identity.Client;
using Models;
using Models.InsuranceInformation;
using PresentationLayer.Command;
using PresentationLayer.Models;
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
        public bool IsEditPrivatePopUpOpen
        {
            get { return isEditPrivatePopUpOpen; }
            set
            {
                isEditPrivatePopUpOpen = value;
                OnPropertyChanged(nameof(IsEditPrivatePopUpOpen));
            }
        }

        private bool isEditCompanyPopUpOpen;
        public bool IsEditCompanyPopUpOpen
        {
            get { return isEditCompanyPopUpOpen; }
            set
            {
                isEditCompanyPopUpOpen = value;
                OnPropertyChanged(nameof(IsEditCompanyPopUpOpen));
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
        public ICommand CloseEditPopupCommand { get; private set; }
        public ICommand GoToAddInsuranceCommand { get; private set; }
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
            CloseEditPopupCommand = new RelayCommand(CloseEditPopup);
            GoToAddInsuranceCommand = new RelayCommand(GoToAddInsurance);           

            CustomerInsurances = new ObservableCollection<Insurance>();
            InsuranceSpecsAndAttributesInformation = new ObservableCollection<InsuranceSpecAndAttributeInformation>();

            IsCompanySelected = true;           
            IsEditPrivatePopUpOpen = false;
            IsEditCompanyPopUpOpen = false;
            IsRemoveCompanyCustomerPopupOpen = false;
            IsRemovePrivateCustomerPopupOpen = false;
        }


        #region methods

        private void ShowAttributesAndSpecs()
        {
            InsuranceSpecsAndAttributesInformation.Clear();
            if (SelectedInsurance != null)
            {
                IList<InsuranceSpec> insuranceSpecs = insuranceSpecController.GetAllInsuranceSpecsForInsurance(selectedInsurance.InsuranceId);

                foreach (InsuranceSpec spec in insuranceSpecs)
                {
                    InsuranceSpecAndAttributeInformation tempInsuranceSpecAndAttributeInformation = new InsuranceSpecAndAttributeInformation(spec.InsuranceTypeAttribute.InsuranceAttribute, spec.Value);
                    InsuranceSpecsAndAttributesInformation.Add(tempInsuranceSpecAndAttributeInformation);
                }                
            }
        }

        private void UpdateInsuranceList()
        {
            if (ViewedCompanyCustomer != null)
            {
                CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
            }
            else if (ViewedPrivateCustomer != null)
            {
                CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
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
                        MessageBox.Show("Status på avtalet är ändrad till aktiv");
                        CustomerInsurances.Clear();
                        UpdateInsuranceList();
                        UpdateCompanyCustomer();
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
                    }
                    else if (IsInactiveStatusSelected == true)
                    {
                        insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
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
                        MessageBox.Show("Status på avtalet är ändrad till aktiv");
                        CustomerInsurances.Clear();
                        UpdateInsuranceList();
                        UpdatePrivateCustomer();
                        //CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
                    }
                    else if (IsInactiveStatusSelected == true)
                    {
                        insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
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
                IsEditPrivatePopUpOpen = true;
            }
        }

        private void OnEditCompanyCustomerClicked()
        {
            if (ViewedCompanyCustomer != null)
            {
                IsEditCompanyPopUpOpen = true;
            }
        }

        private void SaveEditedCompanyCustomer()
        {
            if (ViewedCompanyCustomer != null)
            {
                if (string.IsNullOrEmpty(ViewedCompanyCustomer.OrganisationNumber))
                {
                    MessageBox.Show("Personnummer (SSN) saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.CompanyName))
                {
                    MessageBox.Show("Förnamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.StreetAddress))
                {
                    MessageBox.Show("Gatuadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.PostalCode))
                {
                    MessageBox.Show("Postnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.City))
                {
                    MessageBox.Show("Stad saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.Email))
                {
                    MessageBox.Show("E-postadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.TelephoneNumber))
                {
                    MessageBox.Show("Telefonnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedCompanyCustomer.ContactPersonName))
                {
                    MessageBox.Show("Kontaktperson saknas");
                    return;
                }

                //Check that postalcode is 5 letters
                if (ViewedCompanyCustomer.PostalCode.Length != 5 || !ViewedCompanyCustomer.PostalCode.All(char.IsDigit))
                {
                    MessageBox.Show("Postnumret måste vara exakt 5 siffror");
                    return;
                }

                //Check that Organisationnumber is 10 letters
                if (ViewedCompanyCustomer.OrganisationNumber.Length != 12 || !ViewedCompanyCustomer.OrganisationNumber.All(char.IsDigit))
                {
                    MessageBox.Show("Personnumret måste innehålla 10 siffror");
                    return;
                }

                ViewedCompanyCustomer.CompanyName = CapitalizeFirstLetter(ViewedCompanyCustomer.CompanyName);
                ViewedCompanyCustomer.ContactPersonName = CapitalizeFirstLetter(ViewedCompanyCustomer.ContactPersonName);
                ViewedCompanyCustomer.City = CapitalizeFirstLetter(ViewedCompanyCustomer.City);
                ViewedCompanyCustomer.StreetAddress = CapitalizeFirstLetter(ViewedCompanyCustomer.StreetAddress);

                customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);
                IsEditCompanyPopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
            }
        }

        private void SaveEditedPrivateCustomer()
        {
            if (ViewedPrivateCustomer != null)
            {
                if (string.IsNullOrEmpty(ViewedPrivateCustomer.SSN))
                {
                    MessageBox.Show("Personnummer (SSN) saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.FirstName))
                {
                    MessageBox.Show("Förnamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.LastName))
                {
                    MessageBox.Show("Efternamn saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.StreetAddress))
                {
                    MessageBox.Show("Gatuadress saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.PostalCode))
                {
                    MessageBox.Show("Postnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.City))
                {
                    MessageBox.Show("Stad saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.TelephoneNumber))
                {
                    MessageBox.Show("Telefonnummer saknas");
                    return;
                }

                if (string.IsNullOrEmpty(ViewedPrivateCustomer.Email))
                {
                    MessageBox.Show("E-postadress saknas");
                    return;
                }

                //Check that postalcode is 5 letters
                if (ViewedPrivateCustomer.PostalCode.Length != 5 || !ViewedPrivateCustomer.PostalCode.All(char.IsDigit))
                {
                    MessageBox.Show("Postnumret måste vara exakt 5 siffror");
                    return;
                }

                //Check that SSN is 10 letters
                if (ViewedPrivateCustomer.SSN.Length != 12 || !ViewedPrivateCustomer.SSN.All(char.IsDigit))
                {
                    MessageBox.Show("Personnumret måste innehålla 10 siffror");
                    return;
                }

                ViewedPrivateCustomer.FirstName = CapitalizeFirstLetter(ViewedPrivateCustomer.FirstName);
                ViewedPrivateCustomer.LastName = CapitalizeFirstLetter(ViewedPrivateCustomer.LastName);
                ViewedPrivateCustomer.City = CapitalizeFirstLetter(ViewedPrivateCustomer.City);
                ViewedPrivateCustomer.StreetAddress = CapitalizeFirstLetter(ViewedPrivateCustomer.StreetAddress);

                customerController.UpdatePrivateCustomer(ViewedPrivateCustomer);
                IsEditPrivatePopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
                FullName = $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";
            }
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
                    //CustomerInsurances.Clear();
                }                
            }
        }

        private void RemoveCompanyCustomer()
        {
            int checker = 0;
            if (ViewedCompanyCustomer != null)
            {
                UpdateCompanyCustomer();

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
                    else if (checker == 0)
                    {
                        IsRemoveCompanyCustomerPopupOpen = true;
                    }
                    //CustomerInsurances.Clear();
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

                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
            else if (ViewedCompanyCustomer != null)
            {
                customerController.RemoveCompanyCustomerAndInactiveInsurances(
                    ViewedCompanyCustomer
                );
                ViewedCompanyCustomer = null;

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
                    MessageBox.Show("Detta är en aktiv försäkring, du måste avsluta den innan du kan ta bort den");
                }
                else
                { 
                insuranceController.RemoveInsurance(SelectedInsurance);
                CustomerInsurances.Clear();
                CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedCompanyCustomer.CustomerID));
                }
            }

            else if (ViewedPrivateCustomer != null && SelectedInsurance != null)
            {
                if (SelectedInsurance.InsuranceStatus == InsuranceStatus.Active)
                {
                    MessageBox.Show("Detta är en aktiv försäkring, du måste avsluta den innan du kan ta bort den");
                }
                else
                {
                    insuranceController.RemoveInsurance(SelectedInsurance);
                    CustomerInsurances.Clear();
                    CustomerInsurances = new ObservableCollection<Insurance>(insuranceController.GetCustomerInsurances(ViewedPrivateCustomer.CustomerID));
                }
            }
            SelectedInsurance = null;
        }
        //Cancel the editing of a customer
        private void CloseEditPopup()
        {
            IsEditCompanyPopUpOpen = false;
            IsEditPrivatePopUpOpen = false;
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
        #endregion
    }
}
