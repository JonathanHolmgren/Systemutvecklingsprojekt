using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Identity.Client;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    internal class CustomerProfileViewModel : ObservableObject
    {
        private CustomerController customerController = new CustomerController();
        private InsuranceController insuranceController = new InsuranceController();

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
                isPrivateSelected = value;
                OnPropertyChanged(nameof(IsPrivateSelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                SearchValue = null;
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
                OnPropertyChanged(nameof(ViewedCompanyCustomer));

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

        private Insurance insuranceToRemove;
        public Insurance InsuranceToRemove
        {
            get => insuranceToRemove;
            set
            {
                insuranceToRemove = value;
                OnPropertyChanged(nameof(InsuranceToRemove));
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

        public CustomerProfileViewModel()
        {
            FindPrivateCustomerCommand = new RelayCommand(FindPrivateCustomer);
            FindCompanyCustomerCommand = new RelayCommand(FindCompanyCustomer);
            AddPrivateProspectNoteCommand = new RelayCommand(AddPrivateProspectNote);
            AddCompanyProspectNoteCommand = new RelayCommand(AddCompanyProspectNote);
            GoToInsurancesCommand = new RelayCommand(GoToInsurances);
            ReturnCommand = new RelayCommand(Return);
            OnEditCompanyCustomerClickedCommand = new RelayCommand(OnEditCompanyCustomerClicked);
            OnEditPrivateCustomerClickedCommand = new RelayCommand(OnEditPrivateCustomerClicked);
            SaveEditedCompanyCustomerCommand = new RelayCommand(SaveEditedCompanyCustomer);
            SaveEditedPrivateCustomerCommand = new RelayCommand(SaveEditedPrivateCustomer);
            RemoveCompanyCustomerCommand = new RelayCommand(RemoveCompanyCustomer);
            RemovePrivateCustomerCommand = new RelayCommand(RemovePrivateCustomer);
            ContinueCommand = new RelayCommand(OnContinueClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
            RemoveInsuranceCommand = new RelayCommand(RemoveChosenInsurance);

            CustomerInsurances = new ObservableCollection<Insurance>();

            IsCompanySelected = true;
            IsEditPrivatePopUpOpen = false;
            IsEditCompanyPopUpOpen = false;
            IsRemoveCompanyCustomerPopupOpen = false;
            IsRemovePrivateCustomerPopupOpen = false;
        }

        #region methods
        private void OnEditPrivateCustomerClicked()
        {
            IsEditPrivatePopUpOpen = true;
        }

        private void OnEditCompanyCustomerClicked()
        {
            IsEditCompanyPopUpOpen = true;
        }

        private void SaveEditedCompanyCustomer()
        {
            if (ViewedCompanyCustomer != null)
            {
                customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);
                IsEditCompanyPopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
            }
        }

        private void SaveEditedPrivateCustomer()
        {
            if (ViewedPrivateCustomer != null)
            {
                customerController.UpdatePrivateCustomer(ViewedPrivateCustomer);
                IsEditPrivatePopUpOpen = false;
                MessageBox.Show("Ändringar är sparade");
                FullName = $"{ViewedPrivateCustomer.FirstName} {ViewedPrivateCustomer.LastName}";
            }
        }

        private void FindCompanyCustomer()
        {
            ViewedCompanyCustomer = customerController.GetSpecificCompanyCustomer(SearchValue);

            CustomerInsurances.Clear();
            CustomerInsurances = new ObservableCollection<Insurance>(
                ViewedCompanyCustomer.Insurances
            );
        }

        private void FindPrivateCustomer()
        {
            ViewedPrivateCustomer = customerController.GetSpecificPrivateCustomer(SearchValue);
            CustomerInsurances.Clear();
            CustomerInsurances = new ObservableCollection<Insurance>(
                ViewedPrivateCustomer.Insurances
            );
        }

        //Add a note to the private prospect
        private void AddPrivateProspectNote()
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

        //Add a note to the company prospect
        private void AddCompanyProspectNote()
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

        private void RemovePrivateCustomer()
        {
            int checker = 0;

            if (ViewedPrivateCustomer != null)
            {
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
                    else if (checker == 0)
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
            //if (ViewedPrivateCustomer != null)
            //{
            //    ViewedPrivateCustomer = null;
            //}
            //else if (ViewedCompanyCustomer != null)
            //{
            //    ViewedCompanyCustomer = null;
            //}
        }

        private void RemoveChosenInsurance()
        {
            if (ViewedCompanyCustomer != null)
            {
                insuranceController.RemoveInsurance(InsuranceToRemove);
                CustomerInsurances.Clear();
                CustomerInsurances = new ObservableCollection<Insurance>(
                    ViewedCompanyCustomer.Insurances
                );
            }
            else if (ViewedPrivateCustomer != null)
            {
                insuranceController.RemoveInsurance(InsuranceToRemove);
                CustomerInsurances.Clear();
                CustomerInsurances = new ObservableCollection<Insurance>(
                    ViewedPrivateCustomer.Insurances
                );
            }
            InsuranceToRemove = null;
        }

        //Return to main menu
        private void Return()
        {
            throw new NotImplementedException();
        }

        //Go to insurances view
        private void GoToInsurances()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
