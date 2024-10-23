using PresentationLayer.Models;
using ServiceLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using PresentationLayer.Command;
using System.Windows;
using Microsoft.Identity.Client;

namespace PresentationLayer.ViewModels
{
    internal class CustomerProfileViewModel : ObservableObject
    {
        
        private CustomerController customerController = new CustomerController();

        private string searchValue;
        public string SearchValue
        {
            get { return searchValue; }
            set { searchValue = value; OnPropertyChanged(nameof(SearchValue)); }
        }
        private string note;
        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(nameof(Note)); }
        }

        private bool isPopUpOpen;
        public bool IsPopUpOpen
        {
            get { return isPopUpOpen; }
            set { isPopUpOpen = value; OnPropertyChanged(nameof(IsPopUpOpen));}
        }

        private ObservableCollection<ProspectNote> prospectNotesList = null;
        public ObservableCollection<ProspectNote> ProspectNotesList
        {
            get { return prospectNotesList; }
            set { prospectNotesList = value; OnPropertyChanged(nameof(ProspectNotesList)); }
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
                    ProspectNotesList = new ObservableCollection<ProspectNote>(viewedPrivateCustomer.ProspectNotes);
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
                    ProspectNotesList = new ObservableCollection<ProspectNote>(viewedCompanyCustomer.ProspectNotes);
                }
                else
                {
                    ProspectNotesList?.Clear();
                }
            }
        }
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
        }

        public ICommand FindPrivateCustomerCommand { get; private set; }
        public ICommand FindCompanyCustomerCommand { get; private set; }
        public ICommand AddPrivateProspectNoteCommand { get; private set; }
        public ICommand AddCompanyProspectNoteCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand GoToInsurancesCommand { get; private set; }
        public ICommand OnEditCustomerClickedCommand { get; private set; }
        public ICommand SaveEditedCompanyCustomerCommand { get; private set; }
        public CustomerProfileViewModel()
        {
            FindPrivateCustomerCommand = new RelayCommand(FindPrivateCustomer);
            FindCompanyCustomerCommand = new RelayCommand(FindCompanyCustomer);
            AddPrivateProspectNoteCommand = new RelayCommand(AddPrivateProspectNote);
            AddCompanyProspectNoteCommand = new RelayCommand(AddCompanyProspectNote);
            GoToInsurancesCommand = new RelayCommand(GoToInsurances);
            ReturnCommand = new RelayCommand(Return);
            OnEditCustomerClickedCommand = new RelayCommand(OnEditCustomerClicked);
            SaveEditedCompanyCustomerCommand = new RelayCommand(SaveEditedCompanyCustomer);

            IsCompanySelected = true;
            IsPopUpOpen = false;
        }

        #region Methods

        private void OnEditCustomerClicked()
        {
            IsPopUpOpen = true;
        }
        private void SaveEditedCompanyCustomer()
        {
            if (ViewedCompanyCustomer != null)
            {
                customerController.UpdateCompanyCustomer(ViewedCompanyCustomer);
                IsPopUpOpen = false;
            }
        }
        private void FindCompanyCustomer()
        {
            ViewedCompanyCustomer = customerController.GetSpecificCompanyCustomer(SearchValue);
        }

        private void FindPrivateCustomer()
        {
            ViewedPrivateCustomer = customerController.GetSpecificPrivateCustomer(SearchValue);
        }

        //Add a note to the private prospect
        private void AddPrivateProspectNote()
        {
            if (!string.IsNullOrWhiteSpace(Note))
            {
                Insurance insurance = ViewedPrivateCustomer.Insurances.FirstOrDefault(); //Gör om logiken så inloggad person blir user istället, endast tillfällig lösning
                User user = insurance.User;

                ProspectNote prospectNote = new ProspectNote(Note, DateTime.Now, user, ViewedPrivateCustomer);
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

                ProspectNote prospectNote = new ProspectNote(Note, DateTime.Now, user, ViewedCompanyCustomer);
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
