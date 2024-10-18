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
using Microsoft.Win32;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;

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

        private ICommand exportCommand = null!;
        public ICommand ExportCommand => exportCommand ??= new RelayCommand(() =>
        {
            ExportToCsv();
        },()=> SelectedEmployee != null && SelectedYear != null && SelectedMonth != null);

        public CalculateComissionViewModel()
        {
            LoadYears();
            LoadMonths();
            this.comissionRateController = comissionRateController;
            LoadEmployees();
            //ExportCommand = new RelayCommand(ExportToCsv);
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
            if (SelectedEmployee != null && SelectedMonth.HasValue && SelectedYear.HasValue)
            {
                int year = SelectedYear.Value;
                int month = SelectedMonth.Value + 1; 

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                TotalCommission = comissionRateController.CalculateComission(SelectedEmployee, startDate, endDate);
                CommissionPeriod = $"{startDate:yyyy-MM-dd} till {endDate:yyyy-MM-dd}";
            }
        }




        private void ExportToCsv()
        {
            if (SelectedEmployee != null)
            {
                string csvContent = comissionRateController.CreateCsvContent(SelectedEmployee, TotalCommission, CommissionPeriod);

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV file (*.csv)|*.csv",
                    Title = "Spara CSV-fil",
                    FileName = $"{SelectedEmployee.AgentNumber}_{SelectedEmployee.FirstName}_{CommissionPeriod}_commission.csv" 
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, csvContent, Encoding.UTF8);
                }
            }
        }

        private int? selectedYear;
        public int? SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                CalculateCommission();
            }
        }

        private int? selectedMonth;
        public int? SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
                CalculateCommission();
            }
        }

        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<string> Months { get; set; } = new ObservableCollection<string>();

        private void LoadYears()
        {
            for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year; i++)
            {
                Years.Add(i);
            }
        }

        private void LoadMonths()
        {
            Months = new ObservableCollection<string>
            {
                "Januari",
                "Februari",
                "Mars",
                "April",
                "Maj",
                "Juni",
                "Juli",
                "Augusti",
                "September",
                "Oktober",
                "November",
                "December"
            };
        }
    }

}

