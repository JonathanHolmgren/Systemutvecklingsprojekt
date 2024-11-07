using PresentationLayer.Models;
using ServiceLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Command;
using System.Windows.Input;
using System.Windows;

namespace PresentationLayer.ViewModels
{
    internal class ShowProspectsViewModel : ObservableObject
    {
        #region Initation of objects
        private CustomerController customerController = new CustomerController();

        private string note = string.Empty;
        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(nameof(Note)); 
                IsNoteFilled = true;
            }
        }

        private bool isNoteFilled;
        public bool IsNoteFilled
        {
            get { return isNoteFilled; }
            set {  isNoteFilled = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProspectNote> prospectNotesList = null;
        public ObservableCollection<ProspectNote> ProspectNotesList
        {
            get { return prospectNotesList; }
            set { prospectNotesList = value; OnPropertyChanged(nameof(ProspectNotesList)); }
        }

        private ObservableCollection<PrivateCustomer> privateCustomers;
        public ObservableCollection<PrivateCustomer> PrivateCustomers
        {
            get { return privateCustomers; }
            set
            {
                privateCustomers = value;
                OnPropertyChanged(nameof(PrivateCustomers));
            }
        }

        private ObservableCollection<CompanyCustomer> companyCustomers;
        public ObservableCollection<CompanyCustomer> CompanyCustomers
        {
            get { return companyCustomers; }
            set
            {
                companyCustomers = value;
                OnPropertyChanged(nameof(CompanyCustomers));
            }
        }

        private ObservableCollection<PrivateCustomer> privateProspects;
        public ObservableCollection<PrivateCustomer> PrivateProspects
        {
            get { return privateProspects; }
            set
            {
                privateProspects = value;
                OnPropertyChanged(nameof(PrivateProspects));
            }
        }

        private ObservableCollection<CompanyCustomer> companyProspects;
        public ObservableCollection<CompanyCustomer> CompanyProspects
        {
            get { return companyProspects; }
            set
            {
                companyProspects = value;
                OnPropertyChanged(nameof(CompanyProspects));
            }
        }

        private bool isCompanySelected;
        public bool IsCompanySelected
        {
            get { return isCompanySelected; }
            set
            {
                PrivateProspectSelectedItem = null;
                isCompanySelected = value;
                OnPropertyChanged(nameof(IsCompanySelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateProspectsList();
            }
        }

        private bool isPrivateSelected;
        public bool IsPrivateSelected
        {
            get { return isPrivateSelected; }
            set
            {
                CompanyProspectSelectedItem = null;
                isPrivateSelected = value;
                OnPropertyChanged(nameof(IsPrivateSelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateProspectsList();
            }
        }
        public bool IsPrivateColumnVisible => IsPrivateSelected;
        public bool IsCompanyColumnVisible => IsCompanySelected;


        private PrivateCustomer privateProspectSelectedItem = null;
        public PrivateCustomer PrivateProspectSelectedItem 
        { 
            get { return privateProspectSelectedItem; } 
            set
            {
                privateProspectSelectedItem = value; 
                OnPropertyChanged(nameof(PrivateProspectSelectedItem));
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(PrivateSalesPersonAgentnumber));
                
                if (privateProspectSelectedItem != null)
                {
                    ProspectNotesList = new ObservableCollection<ProspectNote>(privateProspectSelectedItem.ProspectNotes);
                }
                else
                {
                    ProspectNotesList?.Clear();
                }
            }
        }

        private CompanyCustomer companyProspectSelectedItem = null;
        public CompanyCustomer CompanyProspectSelectedItem
        {
            get { return companyProspectSelectedItem; }
            set
            {
                companyProspectSelectedItem = value;
                OnPropertyChanged(nameof(CompanyProspectSelectedItem));
                OnPropertyChanged(nameof(CompanySalesPersonAgentnumber));
                if (companyProspectSelectedItem != null)
                {
                    ProspectNotesList = new ObservableCollection<ProspectNote>(companyProspectSelectedItem.ProspectNotes);
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
                if (PrivateProspectSelectedItem != null)
                {
                    return $"{PrivateProspectSelectedItem.FirstName} {PrivateProspectSelectedItem.LastName}";
                }
                return string.Empty;
            }
        }

        public string PrivateSalesPersonAgentnumber
        {
            get
            {
                if (PrivateProspectSelectedItem != null)
                {
                    return string.Join(", ", PrivateProspectSelectedItem.Insurances.Select(i => i.User.Employee.AgentNumber));
                }
                return string.Empty;
            }
        }
        public string CompanySalesPersonAgentnumber
        {
            get
            {
                if (CompanyProspectSelectedItem != null)
                {
                    return string.Join(", ", CompanyProspectSelectedItem.Insurances.Select(u => u.User.Employee.AgentNumber));
                }
                return string.Empty;
            }
        }

        public ICommand AddPrivateProspectNoteCommand { get; private set; }
        public ICommand AddCompanyProspectNoteCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        #endregion
        #region Constructors
        public ShowProspectsViewModel()
        {
            PrivateCustomers = new ObservableCollection<PrivateCustomer>(customerController.GetPrivateCustomerList());
            CompanyCustomers = new ObservableCollection<CompanyCustomer>(customerController.GetCompanyCustomerList());

            PrivateProspects = new ObservableCollection<PrivateCustomer>(FilterPrivateProspects());
            CompanyProspects = new ObservableCollection<CompanyCustomer>(FilterCompanyProspects());

            AddPrivateProspectNoteCommand = new RelayCommand(AddPrivateProspectNote);
            AddCompanyProspectNoteCommand = new RelayCommand(AddCompanyProspectNote);
            ReturnCommand = new RelayCommand(Return);

            IsCompanySelected = true;
            IsNoteFilled = true;
        }
        #endregion
        #region Methods

        private ObservableCollection<PrivateCustomer> FilterPrivateProspects()
        {
            ObservableCollection<PrivateCustomer> filteredList = new ObservableCollection<PrivateCustomer>();

            if (PrivateCustomers != null) 
            { 
                foreach (var privateCustomer in PrivateCustomers)
                {
                    if (privateCustomer.Insurances != null && privateCustomer.Insurances.Count == 1 && !filteredList.Any(p => p.CustomerID == privateCustomer.CustomerID))
                    {
                        filteredList.Add(privateCustomer);
                    }
                }
            }
            return filteredList; 
        }
     
        private ObservableCollection<CompanyCustomer> FilterCompanyProspects()
        {
            ObservableCollection<CompanyCustomer> filteredList = new ObservableCollection<CompanyCustomer>();

            if (CompanyCustomers != null)
            {
                foreach (var companyCustomer in CompanyCustomers)
                {
                    if (companyCustomer.Insurances != null && companyCustomer.Insurances.Count == 1 && !filteredList.Any(p => p.CustomerID == companyCustomer.CustomerID))
                    {
                        filteredList.Add(companyCustomer);
                    }
                }
            }
            return filteredList; 
        }

        private void UpdateProspectsList()
        {
            if (IsCompanySelected)
            {                
                CompanyProspects = FilterCompanyProspects();
            }
            else if (IsPrivateSelected)
            {                
                PrivateProspects = FilterPrivateProspects();
            }
        }

        //Add a note to the private prospect
        private void AddPrivateProspectNote()
        {
            if (!string.IsNullOrWhiteSpace(Note))
            {
                Insurance insurance = PrivateProspectSelectedItem.Insurances.FirstOrDefault(); //Gör om logiken så inloggad person blir user istället, endast tillfällig lösning
                User user = insurance.User;

                ProspectNote prospectNote = new ProspectNote(Note, DateTime.Now, user, PrivateProspectSelectedItem);
                customerController.AddProspectNote(prospectNote);
                PrivateProspectSelectedItem.ProspectNotes.Add(prospectNote);
                ProspectNotesList.Add(prospectNote);
                Note = string.Empty;
            }
            else
            {
                IsNoteFilled = false;
            }
        }

        //Add a note to the company prospect
        private void AddCompanyProspectNote()
        {
            if (!string.IsNullOrWhiteSpace(Note))
            {
                Insurance insurance = CompanyProspectSelectedItem.Insurances.FirstOrDefault(); //Gör om logiken så inloggad person blir user istället, endast tillfällig lösning
                User user = insurance.User;

                ProspectNote prospectNote = new ProspectNote(Note, DateTime.Now, user, CompanyProspectSelectedItem);
                customerController.AddProspectNote(prospectNote);
                CompanyProspectSelectedItem.ProspectNotes.Add(prospectNote);
                ProspectNotesList.Add(prospectNote);
                Note = string.Empty;
            }
            else
            {
                IsNoteFilled = false;
            }
        }

        //Return to main menu
        private void Return() 
        {
            
        }
        #endregion
    }
}
