using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class ExportInvoiceBasisViewModel : ObservableObject
    {
        InsuranceController insuranceController;
        public ICommand GetPrivateCustomersInsurancesCommand { get; }
        public ICommand GetCompanyCustomersInsurancesCommand { get; }
        public Visibility IsDataGridVisiblePrivate =>
            privateInsurances?.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDataGridVisibleCompany =>
            companyInsurances?.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        private ObservableCollection<Insurance> privateInsurances = null;
        public ObservableCollection<Insurance> PrivateInsurances
        {
            get => privateInsurances;
            set
            {
                privateInsurances = value;
                OnPropertyChanged(nameof(PrivateInsurances));
                OnPropertyChanged(nameof(IsDataGridVisiblePrivate));
            }
        }

        private ObservableCollection<Insurance> companyInsurances = null;
        public ObservableCollection<Insurance> CompanyInsurances
        {
            get => companyInsurances;
            set
            {
                companyInsurances = value;
                OnPropertyChanged(nameof(CompanyInsurances));
                OnPropertyChanged(nameof(IsDataGridVisibleCompany));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ExportInvoiceBasisViewModel()
        {
            insuranceController = new InsuranceController();
            GetPrivateCustomersInsurancesCommand = new RelayCommand(LoadPrivateCustomersInsurances);
            GetCompanyCustomersInsurancesCommand = new RelayCommand(LoadCompanyCustomersInsurances);

            PrivateInsurances = new ObservableCollection<Insurance>();
            CompanyInsurances = new ObservableCollection<Insurance>();
        }

        public void LoadPrivateCustomersInsurances()
        {
            PrivateInsurances.Clear();
            CompanyInsurances.Clear();
            var newInsurances = insuranceController.LoadPrivateCustomersInsurances();
            foreach (var privateInsurance in newInsurances)
            {
                PrivateInsurances.Add(privateInsurance);
                MessageBox.Show($"{privateInsurance.InsuranceId}");
            }
            OnPropertyChanged(nameof(PrivateInsurances));
        }

        public void LoadCompanyCustomersInsurances()
        {
            CompanyInsurances.Clear();
            PrivateInsurances.Clear();
            var tempInsurances = insuranceController.LoadCompanyCustomersInsurances();
            foreach (var companyInsurance in tempInsurances)
            {
                CompanyInsurances.Add(companyInsurance);
                MessageBox.Show($"{companyInsurance.InsuranceId}");
            }
        }
    }
}
