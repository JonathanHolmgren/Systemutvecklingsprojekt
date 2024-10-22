using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class RemoveCustomerViewModel : ObservableObject
    {
        CustomerController customerController;

        #region Commands

        public ICommand RemoveCustomerFromDataBase { get; private set; }
        public ICommand ContinueCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchPrivateCustomerCommand { get; set; }
        public ICommand SearchCompanyCustomerCommand { get; set; }
        public ICommand UpdatePrivateCustomerList { get; set; }
        public ICommand UpdateCompanyCustomerList { get; set; }
        #endregion

        public bool IsPrivateColumnVisible => IsPrivateSelected;
        public bool IsCompanyColumnVisible => IsCompanySelected;

        private bool isCompanySelected;
        public bool IsCompanySelected
        {
            get { return isCompanySelected; }
            set
            {
                PrivateCustomerSelectedItem = null;
                isCompanySelected = value;
                OnPropertyChanged(nameof(IsCompanySelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateCustomerList();
            }
        }

        private bool isPrivateSelected;
        public bool IsPrivateSelected
        {
            get { return isPrivateSelected; }
            set
            {
                CompanyCustomerSelectedItem = null;
                isPrivateSelected = value;
                OnPropertyChanged(nameof(IsPrivateSelected));
                OnPropertyChanged(nameof(IsCompanyColumnVisible));
                OnPropertyChanged(nameof(IsPrivateColumnVisible));
                UpdateCustomerList();
            }
        }

        private bool isPopUpOpen;
        public bool IsPopupOpen
        {
            get { return isPopUpOpen; }
            set
            {
                isPopUpOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        private PrivateCustomer privateCustomerSelectedItem;
        public PrivateCustomer PrivateCustomerSelectedItem
        {
            get => privateCustomerSelectedItem;
            set
            {
                privateCustomerSelectedItem = value;
                OnPropertyChanged(nameof(PrivateCustomerSelectedItem));
            }
        }

        private CompanyCustomer companyCustomerSelectedItem;
        public CompanyCustomer CompanyCustomerSelectedItem
        {
            get => companyCustomerSelectedItem;
            set
            {
                companyCustomerSelectedItem = value;
                OnPropertyChanged(nameof(CompanyCustomerSelectedItem));
            }
        }

        private string privateCustomerSearchedItem;
        public string PrivateCustomerSearchedItem
        {
            get => privateCustomerSearchedItem;
            set
            {
                privateCustomerSearchedItem = value;
                OnPropertyChanged(nameof(PrivateCustomerSearchedItem));
            }
        }

        private string companyCustomerSearchedItem;
        public string CompanyCustomerSearchedItem
        {
            get => companyCustomerSearchedItem;
            set
            {
                companyCustomerSearchedItem = value;
                OnPropertyChanged(nameof(CompanyCustomerSearchedItem));
            }
        }

        private ObservableCollection<PrivateCustomer> privateCustomerList;
        public ObservableCollection<PrivateCustomer> PrivateCustomerList
        {
            get => privateCustomerList;
            set
            {
                privateCustomerList = value;
                OnPropertyChanged(nameof(PrivateCustomerList));
            }
        }

        private ObservableCollection<CompanyCustomer> companyCustomerList;
        public ObservableCollection<CompanyCustomer> CompanyCustomerList
        {
            get => companyCustomerList;
            set
            {
                companyCustomerList = value;
                OnPropertyChanged(nameof(CompanyCustomerList));
            }
        }

        #region Constructor
        public RemoveCustomerViewModel()
        {
            customerController = new CustomerController();

            RemoveCustomerFromDataBase = new RelayCommand(RemoveCustomer, CanExecuteRemoveCustomer);
            ContinueCommand = new RelayCommand(OnContinueClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
            SearchPrivateCustomerCommand = new RelayCommand(GetSearchedPrivateCustomer);
            SearchCompanyCustomerCommand = new RelayCommand(GetSearchedCompanyCustomer);
            UpdatePrivateCustomerList = new RelayCommand(UpdateCustomerList);
            UpdateCompanyCustomerList = new RelayCommand(UpdateCustomerList);

            IsCompanySelected = true;

            PrivateCustomerList = new ObservableCollection<PrivateCustomer>(
                customerController.GetPrivateCustomerList()
            );
            CompanyCustomerList = new ObservableCollection<CompanyCustomer>(
                customerController.GetCompanyCustomerList()
            );
        }
        #endregion
        #region Methods

        public void UpdateCustomerList()
        {
            if (IsCompanySelected)
            {
                CompanyCustomerList = GetCompanyCustomers();
            }
            else if (IsPrivateSelected)
            {
                PrivateCustomerList = GetPrivateCustomers();
            }
        }

        public ObservableCollection<PrivateCustomer> GetPrivateCustomers()
        {
            ObservableCollection<PrivateCustomer> privateCustomerList =
                new ObservableCollection<PrivateCustomer>(
                    customerController.GetPrivateCustomerList()
                );
            return privateCustomerList;
        }

        public ObservableCollection<CompanyCustomer> GetCompanyCustomers()
        {
            ObservableCollection<CompanyCustomer> companyCustomerList =
                new ObservableCollection<CompanyCustomer>(
                    customerController.GetCompanyCustomerList()
                );
            return companyCustomerList;
        }

        public void GetSearchedPrivateCustomer()
        {
            PrivateCustomer searchedPrivateCustomer = new PrivateCustomer();
            int counter = 0;

            foreach (PrivateCustomer privateCustomer in PrivateCustomerList)
            {
                if (privateCustomer.SSN.Equals(PrivateCustomerSearchedItem))
                {
                    searchedPrivateCustomer = privateCustomer;
                    counter++;
                }
            }
            if (counter == 0)
            {
                MessageBox.Show("Det angivna organisationsnummret finns inte i systemet.");
                CompanyCustomerSearchedItem = null;
            }
            else if (counter > 0)
            {
                PrivateCustomerSearchedItem = null;
                PrivateCustomerList.Clear();
                PrivateCustomerList.Add(searchedPrivateCustomer);
            }
        }

        public void GetSearchedCompanyCustomer()
        {
            CompanyCustomer searchedCompanyCustomer = new CompanyCustomer();
            int counter = 0;

            foreach (CompanyCustomer companyCustomer in CompanyCustomerList)
            {
                if (companyCustomer.OrganisationNumber.Equals(CompanyCustomerSearchedItem))
                {
                    searchedCompanyCustomer = companyCustomer;
                    counter++;
                }
            }

            if (counter == 0)
            {
                MessageBox.Show("Det angivna organisationsnummret finns inte i systemet.");
                CompanyCustomerSearchedItem = null;
            }
            else if (counter > 0)
            {
                CompanyCustomerSearchedItem = null;
                CompanyCustomerList.Clear();
                CompanyCustomerList.Add(searchedCompanyCustomer);
            }
        }

        public void RemoveCustomer()
        {
            int checker = 0;

            if (PrivateCustomerSelectedItem != null)
            {
                if (PrivateCustomerSelectedItem.Insurances.Count < 1)
                {
                    customerController.RemovePrivateCustomer(PrivateCustomerSelectedItem);
                    UpdateCustomerList();
                    MessageBox.Show("Kunden är nu borttagen ur systemet.");
                }
                else if (PrivateCustomerSelectedItem.Insurances.Count > 0)
                {
                    foreach (Insurance insurance in PrivateCustomerSelectedItem.Insurances)
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
                            $"Tyvärr kan du inte ta bort kunden: \"{PrivateCustomerSelectedItem.FirstName} {PrivateCustomerSelectedItem.LastName}\", det finns aktiva eller preliminära försäkringar kopplad till kunden. Hantera dem först."
                        );
                    }
                    else if (checker == 0)
                    {
                        IsPopupOpen = true;
                    }
                }
            }
            else if (CompanyCustomerSelectedItem != null)
            {
                if (CompanyCustomerSelectedItem.Insurances.Count < 1)
                {
                    customerController.RemoveCompanyCustomer(CompanyCustomerSelectedItem);
                    UpdateCustomerList();
                    MessageBox.Show("Kunden är nu borttagen ur systemet.");
                }
                else if (CompanyCustomerSelectedItem.Insurances.Count > 0)
                {
                    foreach (Insurance insurance in CompanyCustomerSelectedItem.Insurances)
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
                            $"Tyvärr kan du inte ta bort kunden: \"{CompanyCustomerSelectedItem.CompanyName}\", det finns aktiva eller preliminära försäkringar kopplad till kunden. Hantera dem först."
                        );
                    }
                    else if (checker == 0)
                    {
                        IsPopupOpen = true;
                    }
                }
            }
        }

        public bool CanExecuteRemoveCustomer()
        {
            if (CompanyCustomerSelectedItem != null || PrivateCustomerSelectedItem != null)
            {
                return true;
            }

            return false;
        }

        public void OnContinueClicked()
        {
            IsPopupOpen = false;

            if (PrivateCustomerSelectedItem != null)
            {
                customerController.RemovePrivateCustomerAndInactiveInsurances(
                    PrivateCustomerSelectedItem
                );
                UpdateCustomerList();
                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
            else if (CompanyCustomerSelectedItem != null)
            {
                customerController.RemoveCompanyCustomerAndInactiveInsurances(
                    CompanyCustomerSelectedItem
                );
                UpdateCustomerList();
                MessageBox.Show("Kunden är nu borttagen ur systemet.");
            }
        }

        public void OnCancelClicked()
        {
            IsPopupOpen = false;
            if (PrivateCustomerSelectedItem != null)
            {
                PrivateCustomerSelectedItem = null;
            }
            else if (CompanyCustomerSelectedItem != null)
            {
                CompanyCustomerSelectedItem = null;
            }
        }

        #endregion
    }
}
