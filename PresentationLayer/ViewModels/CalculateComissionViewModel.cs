using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PresentationLayer.Command;
using ServiceLayer;
using Models;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;

namespace PresentationLayer.ViewModels
{
    public class CalculateComissionViewModel : ObservableObject
    {
        private ComissionRateController comissionRateController = new ComissionRateController();
        private Employee selectedEmployee;
        private double totalCommission;

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                CalculateCommission(); 
            }
        }

        
        public double TotalCommission
        {
            get { return totalCommission; }
            set
            {
                totalCommission = value;
                OnPropertyChanged(nameof(TotalCommission));
            }
        }

        private string commissionPeriod;
        public string CommissionPeriod
        {
            get { return commissionPeriod; }
            set
            {
                commissionPeriod = value;
                OnPropertyChanged(nameof(CommissionPeriod));
            }
        }


        public ICommand ExportCommand { get; }

        public CalculateComissionViewModel()
        {
            this.comissionRateController = comissionRateController;
            LoadEmployees();
            ExportCommand = new RelayCommand(ExportToCsv);
        }

        private void LoadEmployees()
        {
            var employees = comissionRateController.GetEmployeesWithCommissions();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private void CalculateCommission()
        {
            if (SelectedEmployee != null)
            {
                TotalCommission = comissionRateController.CalculateComission(SelectedEmployee);

                var lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                var lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                CommissionPeriod = $"{lastMonthStart.ToString("yyyy-MM-dd")} till {lastMonthEnd.ToString("yyyy-MM-dd")}";
            }
        }

        private void ExportToCsv()
        {
            if (SelectedEmployee != null)
            {
                comissionRateController.ExportToCsv(SelectedEmployee, TotalCommission, CommissionPeriod);
            }
        }
    }

}

