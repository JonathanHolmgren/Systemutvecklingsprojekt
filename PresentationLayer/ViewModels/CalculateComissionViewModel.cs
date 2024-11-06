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
    internal class CalculateComissionViewModel : ObservableObject
    {
        private ComissionRateController comissionRateController = new ComissionRateController();
        private Employee selectedEmployee;
        private double totalCommission;

        

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
   
            }
        }
        ObservableCollection<Employee> employees;
        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged();
               
            }
        }

        ObservableCollection<Employee> filteredEmployees;
        public ObservableCollection<Employee> FilteredEmployees
        {
            get { return filteredEmployees; }
            set
            {
                filteredEmployees = value;
                OnPropertyChanged();

            }
        }
        private string filterText = null;
        public string FilterText
        {
            get { return filterText; }
            set
            {
                if (filterText != value)
                {
                    filterText = value;
                    ApplyFilter(filterText);
                    OnPropertyChanged(nameof(FilterText));
                }
            }
        }

        public double TotalCommission
        {
            get { return totalCommission; }
            set
            {
                totalCommission = value;
       
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
        private int menuPage = 0;
        public int MenuPage
        {
            get => menuPage;
            set
            {
                menuPage = value;
                OnPropertyChanged(nameof(MenuPage));

            }
        }
        private string errorLabel = string.Empty;
        public string ErrorLabel
        {
            get => errorLabel;
            set
            {
                errorLabel = value;
                OnPropertyChanged();
            }
        }
        private bool _isProvisionVisible;

        public bool IsProvisionVisible
        {
            get { return _isProvisionVisible; }
            set { _isProvisionVisible = value; OnPropertyChanged(); }
        }

        public ICommand CalculateCommisionCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand BackPageCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }

        public CalculateComissionViewModel()
        {
            LoadYears();
            LoadMonths();
            this.comissionRateController = comissionRateController;
            Employees=LoadEmployees();
            ApplyFilter(FilterText);
            ExportCommand = new RelayCommand<object>(execute => ExportToCsv());
            NextPageCommand = new RelayCommand<object>(execute => IncreaseMenuPage());
            BackPageCommand = new RelayCommand<object>(execute => DecreaseMenuPage());
            CalculateCommisionCommand = new RelayCommand<object>(execute => CalculateCommission());
            IsProvisionVisible = false;

            //ExportCommand = new RelayCommand(ExportToCsv);
        }
        private void IncreaseMenuPage()
        {
            if (SelectedEmployee == null)
            {
                ErrorLabel = "Var vänlig och välj en säljare innan du går vidare";
            }
            else if (SelectedEmployee != null)
            {
                MenuPage++;
                ErrorLabel=string.Empty;
            }
        }
        private void DecreaseMenuPage()
        {
            SelectedEmployee = null;
            IsProvisionVisible=false;
            MenuPage--;
        }
        private ObservableCollection<Employee> LoadEmployees()
        {
            ObservableCollection<Employee> salespersons = new ObservableCollection<Employee>();
            var employees = comissionRateController.GetEmployeesWithCommissions();
            foreach (var employee in employees)
            {
                salespersons.Add(employee);
            }
            return salespersons;
        }

        private void CalculateCommission()
        {
            if (SelectedMonth == null && SelectedYear == null)
            {
                ErrorLabel = "Fyll i månad och år för att beräkna provision";
            }
            else if (SelectedMonth == null)
            {
                ErrorLabel = "Fyll i månad för att beräkna provision.";
            }
            int monthNumber = MonthNameToNumber(SelectedMonth);
            if (SelectedEmployee != null &&  monthNumber>0 && SelectedYear.HasValue)
            {
                int year = SelectedYear.Value;
                int month = monthNumber; 

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                TotalCommission = comissionRateController.CalculateComission(SelectedEmployee, startDate, endDate);
                CommissionPeriod = $"{startDate:yyyy-MM-dd} till {endDate:yyyy-MM-dd}";
                IsProvisionVisible = true;
                OnPropertyChanged(nameof(SelectedMonth));
                OnPropertyChanged(nameof(SelectedYear));
                OnPropertyChanged(nameof(TotalCommission));
            }

        }

        private void ApplyFilter(string filterText)
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                FilteredEmployees = new ObservableCollection<Employee>(Employees);
            }
            else
            {
                FilterEmployees(filterText);
            }
                
        }
        private void FilterEmployees(string filterText) //Applying the filter text to company customers
        {
            FilteredEmployees.Clear();

            foreach (Employee employee in Employees)
            {
                if (IsEmployeeMatch(employee, filterText))
                {
                    FilteredEmployees.Add(employee);
                }
            }
        }

        private bool IsEmployeeMatch(Employee employee, string filterText)
        {
            return employee.FirstName.Contains(
                    filterText,
                    StringComparison.OrdinalIgnoreCase
                )
                || employee.LastName.Contains(
                    filterText,
                    StringComparison.OrdinalIgnoreCase
                ) || employee.AgentNumber.Contains(filterText,
                StringComparison.OrdinalIgnoreCase);
        }

        public int MonthNameToNumber(string monthName)
        {

            var months = new List<string>
                    {
                        "Januari", "Februari", "Mars", "April", "Maj", "Juni",
                        "Juli", "Augusti", "September", "Oktober", "November", "December"
                    };

            int monthIndex = months.IndexOf(monthName);

            
            return monthIndex >= 0 ? monthIndex + 1 : -1;
        }


        private void ExportToCsv()
        {
            CalculateCommission();
            if(SelectedMonth==null && SelectedYear==null)
            {
                ErrorLabel = "Fyll i månad och år för att exportera";
            }
            else if (SelectedMonth== null)
            {
                ErrorLabel = "Fyll i månad för att exportera";
            }
            else if (SelectedYear == null)
            {
                ErrorLabel = "Fyll i år för att exportera";
            }
            else if (SelectedEmployee != null)
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

        private int? selectedYear = 2024;
        public int? SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                ErrorLabel=string.Empty;
               
            }
        }

        private string selectedMonth = null;
        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                ErrorLabel = string.Empty;
              
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

