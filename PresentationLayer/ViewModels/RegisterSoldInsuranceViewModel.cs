using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class RegisterSoldInsuranceViewModel : ObservableObject
    {
        private readonly InsuranceController insuranceController;
        public ICommand RegisterSoldInsuranceCommand { get; }

        private Insurance _selectedInsuranceRow;
        public Insurance SelectedInsuranceRow
        {
            get => _selectedInsuranceRow;
            set
            {
                _selectedInsuranceRow = value;
                OnPropertyChanged(nameof(SelectedInsuranceRow));
            }
        }

        private ObservableCollection<Insurance> _insurances;
        public ObservableCollection<Insurance> Insurances
        {
            get => _insurances;
            set
            {
                _insurances = value;
                OnPropertyChanged(nameof(Insurances));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructors
        public RegisterSoldInsuranceViewModel()
        {
            insuranceController = new InsuranceController();

            RegisterSoldInsuranceCommand = new RelayCommand(
                SetInsuranceStatusToActive,
                CanExecuteSetInsuranceStatusToActive
            );

            Insurances = LoadListBox();
        }
        #endregion

        #region Methods
        public void SetInsuranceStatusToActive()
        {
            insuranceController.SetInsuranceStatusToActive(SelectedInsuranceRow);
            Insurances.Remove(SelectedInsuranceRow);
        }

        public bool CanExecuteSetInsuranceStatusToActive()
        {
            if (SelectedInsuranceRow != null)
            {
                return true;
            }

            return false;
        }

        public ObservableCollection<Insurance> LoadListBox()
        {
            List<Insurance> insurances;

            List<Insurance> preliminaryInsurances =
                insuranceController.GetAllPreliminaryInsurances();

            return new ObservableCollection<Insurance>(preliminaryInsurances);
        }
        #endregion
    }
}
