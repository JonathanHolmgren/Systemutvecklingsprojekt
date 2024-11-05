using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataLayer.Repositories;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class RegisterPreliminaryInsuranceCompanyViewModel : ObservableObject
    {
        InsuranceController insuranceController;
        CustomerController customerController;

        public RegisterPreliminaryInsuranceCompanyViewModel()
        {
            insuranceController = new InsuranceController();
            customerController = new CustomerController();

            LoadInsuranceTypeOptions();
            LoadBillingIntervalOptions();
            CurrentView = "Företagsuppgifter";
        }
        private string _currentView;
        public string CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        #region User
        private User loggedInUser;
        public User LoggedInUser
        {
            get { return loggedInUser; }
            set
            {
                if (loggedInUser != value)
                {
                    loggedInUser = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Customer
        private CompanyCustomer selectedCompanyCustomer = null!;
        public CompanyCustomer SelectedCompanyCustomer
        {
            get { return selectedCompanyCustomer; }
            set
            {
                selectedCompanyCustomer = value;
                OnPropertyChanged();
            }
        }

        private string inputOrgNumber = null!;
        public string InputOrgNumber
        {
            get { return inputOrgNumber; }
            set
            {
                inputOrgNumber = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region InsuranceTypes
        private ObservableCollection<string> availableInsuranceTypes = null!;
        public ObservableCollection<string> AvailableInsuranceTypes
        {
            get => availableInsuranceTypes;
            set
            {
                if (availableInsuranceTypes != value)
                {
                    availableInsuranceTypes = value;
                    OnPropertyChanged(nameof(AvailableInsuranceTypes));
                }
            }
        }

        private string selectedInsuranceType = null!;
        public string SelectedInsuranceType
        {
            get => selectedInsuranceType;
            set
            {
                selectedInsuranceType = value;
                CalculatePropertyPremie();
                CalcultateInventoriesPremie();

                OnPropertyChanged();
                LoadCarInsuranceOptions();
                LoadDeductibles();
                LoadZones();
                CalculateCarPremie();

                LoadLiabiltyAmounts();
                LoadLiabiltyDeductibles();
                CalculateLiabiltyPremie();

                CalculateTotalPremie();
            }
        }
        #endregion

        #region PropertyInsurance
        private string propertyAddress = null!;
        public string PropertyAddress
        {
            get => propertyAddress;
            set
            {
                propertyAddress = value;
                OnPropertyChanged();
            }
        }

        private string propertyValue = null!;
        public string PropertyValue
        {
            get => propertyValue;
            set
            {
                propertyValue = value;
                OnPropertyChanged();
                CalculatePropertyPremie();
            }
        }

        private string propertyPremie = null!;
        public string PropertyPremie
        {
            get => propertyPremie;
            set
            {
                propertyPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }

        private string inventoriesValue = null!;
        public string InventoriesValue
        {
            get => inventoriesValue;
            set
            {
                inventoriesValue = value;
                OnPropertyChanged();
                CalcultateInventoriesPremie();
            }
        }

        private string inventoriesPremie = null!;
        public string InventoriesPremie
        {
            get => inventoriesPremie;
            set
            {
                inventoriesPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }
        #endregion

        #region CarInsurance
        private ObservableCollection<string> availableCarInsuranceTypes = null!;
        public ObservableCollection<string> AvailableCarInsuranceTypes
        {
            get => availableCarInsuranceTypes;
            set
            {
                if (availableCarInsuranceTypes != value)
                {
                    availableCarInsuranceTypes = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedCarInsuranceType = null!;
        public string SelectedCarInsuranceType
        {
            get => selectedCarInsuranceType;
            set
            {
                selectedCarInsuranceType = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }

        private ObservableCollection<string> availableCarDeductibles = null!;
        public ObservableCollection<string> AvailableCarDeductibles
        {
            get => availableCarDeductibles;
            set
            {
                if (availableCarDeductibles != value)
                {
                    availableCarDeductibles = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedCarDeductible = null!;
        public string SelectedCarDeductible
        {
            get => selectedCarDeductible;
            set
            {
                selectedCarDeductible = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }

        private ObservableCollection<string> availableZones = null!;
        public ObservableCollection<string> AvailableZones
        {
            get => availableZones;
            set
            {
                if (availableZones != value)
                {
                    availableZones = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedZone = null!;
        public string SelectedZone
        {
            get => selectedZone;
            set
            {
                selectedZone = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }
        private string carPremie = null!;
        public string CarPremie
        {
            get => carPremie;
            set
            {
                carPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }

        private readonly Dictionary<
            (string carInsuranceType, string deductible),
            double
        > basePremiums =
            new()
            {
                { ("Trafik", "1000"), 350 },
                { ("Trafik", "2000"), 300 },
                { ("Trafik", "3000"), 250 },
                { ("Halv", "1000"), 550 },
                { ("Halv", "2000"), 450 },
                { ("Halv", "3000"), 400 },
                { ("Hel", "1000"), 800 },
                { ("Hel", "2000"), 700 },
                { ("Hel", "3000"), 600 }
            };

        #endregion

        #region LiabilityInsurance
        private ObservableCollection<string> availableLiabilityAmount = null!;
        public ObservableCollection<string> AvailableLiabilityAmount
        {
            get => availableLiabilityAmount;
            set
            {
                availableLiabilityAmount = value;
                OnPropertyChanged();
            }
        }

        private string selectedLiabilityAmount = null!;
        public string SelectedLiabilityAmount
        {
            get => selectedLiabilityAmount;
            set
            {
                selectedLiabilityAmount = value;
                OnPropertyChanged();
                CalculateLiabiltyPremie();
            }
        }

        private ObservableCollection<string> availableLiabilityDeductibles = null!;
        public ObservableCollection<string> AvailableLiabilityDeductibles
        {
            get => availableLiabilityDeductibles;
            set
            {
                if (availableLiabilityDeductibles != value)
                {
                    availableLiabilityDeductibles = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedLiabilityDeductible = null!;
        public string SelectedLiabilityDeductible
        {
            get => selectedLiabilityDeductible;
            set
            {
                selectedLiabilityDeductible = value;
                OnPropertyChanged();
                CalculateLiabiltyPremie();
            }
        }

        private string liabilityPremie = null!;
        public string LiabilityPremie
        {
            get => liabilityPremie;
            set
            {
                liabilityPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }

        #endregion

        #region GenerellForInsurance
        private DateTime activationDate = DateTime.Now;
        public DateTime ActivationDate
        {
            get => activationDate;
            set
            {
                activationDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime expiryDate = DateTime.Now.AddMonths(1);
        public DateTime ExpiryDate
        {
            get => expiryDate;
            set
            {
                expiryDate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BillingInterval> BillingIntervals { get; private set; }

        private BillingInterval selectedInterval;
        public BillingInterval SelectedInterval
        {
            get => selectedInterval;
            set
            {
                if (selectedInterval != value)
                {
                    selectedInterval = value;
                    OnPropertyChanged();
                    CalculateTotalPremie();
                }
            }
        }

        private string totalPremie = null!;
        public string TotalPremie
        {
            get => totalPremie;
            set
            {
                totalPremie = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods
        #region CalculateMethods
        public void CalculatePropertyPremie()
        {
            if (selectedInsuranceType == insuranceType1)
            {
                if (
                    !string.IsNullOrWhiteSpace(propertyValue)
                    && double.TryParse(propertyValue, out double value)
                )
                {
                    double premie = value * 0.002;
                    PropertyPremie = premie.ToString("F2");
                }
                else
                {
                    PropertyPremie = "";
                }
            }
        }

        public void CalcultateInventoriesPremie()
        {
            if (selectedInsuranceType == insuranceType1)
            {
                if (
                    !string.IsNullOrWhiteSpace(inventoriesValue)
                    && double.TryParse(inventoriesValue, out double value)
                )
                {
                    double premie = value * 0.002;
                    InventoriesPremie = premie.ToString("F2");
                }
                else
                {
                    InventoriesPremie = "";
                }
            }
        }

        private void CalculateCarPremie()
        {
            if (selectedInsuranceType == insuranceType2)
            {
                if (
                    string.IsNullOrEmpty(SelectedCarInsuranceType)
                    || string.IsNullOrEmpty(SelectedCarDeductible)
                )
                {
                    CarPremie = "";
                    return;
                }

                if (
                    !basePremiums.TryGetValue(
                        (SelectedCarInsuranceType, SelectedCarDeductible),
                        out double basePremium
                    )
                )
                {
                    CarPremie = "";
                    return;
                }

                double zoneMultiplier = SelectedZone switch
                {
                    "1 (högst risk)" => 1.3,
                    "2" => 1.2,
                    "3" => 1.1,
                    _ => 1.0
                };

                double totalPremium = basePremium * zoneMultiplier;
                CarPremie = totalPremium.ToString("F2");
            }
            else
            {
                CarPremie = "";
            }
        }

        public void CalculateLiabiltyPremie()
        {
            if (selectedInsuranceType == insuranceType3)
            {
                if (
                    selectedLiabilityAmount == amount1
                    && selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "800";
                }
                else if (
                    selectedLiabilityAmount == amount1
                    && selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "700";
                }
                else if (
                    selectedLiabilityAmount == amount1
                    && selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "600";
                }
                else if (
                    selectedLiabilityAmount == amount1
                    && selectedLiabilityDeductible == liabilityDeductibles4
                )
                {
                    LiabilityPremie = "500";
                }
                else if (
                    selectedLiabilityAmount == amount2
                    && selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "1300";
                }
                else if (
                    selectedLiabilityAmount == amount2
                    && selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "1200";
                }
                else if (
                    selectedLiabilityAmount == amount2
                    && selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "1100";
                }
                else if (
                    selectedLiabilityAmount == amount2
                    && selectedLiabilityDeductible == liabilityDeductibles4
                )
                {
                    LiabilityPremie = "1000";
                }
                else if (
                    selectedLiabilityAmount == amount3
                    && selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "1800";
                }
                else if (
                    selectedLiabilityAmount == amount3
                    && selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "1700";
                }
                else if (
                    selectedLiabilityAmount == amount3
                    && selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "1600";
                }
                else if (
                    selectedLiabilityAmount == amount3
                    && selectedLiabilityDeductible == liabilityDeductibles4
                )
                {
                    LiabilityPremie = "1500";
                }
            }
            else
            {
                LiabilityPremie = "";
            }
        }

        public void CalculateTotalPremie()
        {
            double totalPremie = 0.0;

            if (selectedInsuranceType == insuranceType1)
            {
                double.TryParse(InventoriesPremie, out double inventoriesPremie);
                double.TryParse(PropertyPremie, out double propertyPremie);
                totalPremie = inventoriesPremie + propertyPremie;
            }
            else if (selectedInsuranceType == insuranceType2)
            {
                double.TryParse(carPremie, out totalPremie);
            }
            else if (selectedInsuranceType == insuranceType3)
            {
                double.TryParse(liabilityPremie, out totalPremie);
            }
            else
            {
                totalPremie = 0.0;
            }

            TotalPremie = AdjustForBillingInterval(totalPremie);
        }

        private string AdjustForBillingInterval(double totalPremie)
        {
            switch (selectedInterval)
            {
                case BillingInterval.Månad:
                    return totalPremie.ToString();
                case BillingInterval.Kvartal:
                    return (totalPremie * 3).ToString();
                case BillingInterval.Halvår:
                    return (totalPremie * 6).ToString();
                case BillingInterval.År:
                    return (totalPremie * 12).ToString();
                default:
                    return totalPremie.ToString();
            }
        }
        #endregion

        #region LoadData
        string insuranceType1 = "Fastighet och inventarieförsäkring";
        string insuranceType2 = "Fordonsförsäkring";
        string insuranceType3 = "Ansvarsförsäkring";

        public void LoadInsuranceTypeOptions()
        {
            AvailableInsuranceTypes = new ObservableCollection<string>
            {
                insuranceType1,
                insuranceType2,
                insuranceType3
            };
        }

        string carInsuranceType1 = "Trafik";
        string carInsuranceType2 = "Halv";
        string carInsuranceType3 = "Hel";

        public void LoadCarInsuranceOptions()
        {
            if (selectedInsuranceType == insuranceType2)
            {
                AvailableCarInsuranceTypes = new ObservableCollection<string>
                {
                    carInsuranceType1,
                    carInsuranceType2,
                    carInsuranceType3
                };
            }
            else
            {
                AvailableCarInsuranceTypes = new ObservableCollection<string> { };
            }
        }

        string deductible1 = "1000";
        string deductible2 = "2000";
        string deductible3 = "3000";

        public void LoadDeductibles()
        {
            if (selectedInsuranceType == insuranceType2)
            {
                AvailableCarDeductibles = new ObservableCollection<string>
                {
                    deductible1,
                    deductible2,
                    deductible3
                };
            }
            else
            {
                AvailableCarDeductibles = new ObservableCollection<string> { };
            }
        }

        string zone1 = "1 (högst risk)";
        string zone2 = "2";
        string zone3 = "3";
        string zone4 = "4 (lägst risk)";

        public void LoadZones()
        {
            if (selectedInsuranceType == insuranceType2)
            {
                AvailableZones = new ObservableCollection<string> { zone1, zone2, zone3, zone4 };
            }
            else
            {
                AvailableZones = new ObservableCollection<string> { };
            }
        }

        string amount1 = "3 000 000";
        string amount2 = "5 000 000";
        string amount3 = "10 000 000";

        public void LoadLiabiltyAmounts()
        {
            if (selectedInsuranceType == insuranceType3)
            {
                AvailableLiabilityAmount = new ObservableCollection<string>
                {
                    amount1,
                    amount2,
                    amount3
                };
            }
            else
            {
                AvailableLiabilityAmount = new ObservableCollection<string> { };
            }
        }

        string liabilityDeductibles1 = "14 325";
        string liabilityDeductibles2 = "28 650";
        string liabilityDeductibles3 = "42 975";
        string liabilityDeductibles4 = "57 300";

        public void LoadLiabiltyDeductibles()
        {
            if (selectedInsuranceType == insuranceType3)
            {
                AvailableLiabilityDeductibles = new ObservableCollection<string>
                {
                    liabilityDeductibles1,
                    liabilityDeductibles2,
                    liabilityDeductibles3,
                    liabilityDeductibles4
                };
            }
            else
            {
                AvailableLiabilityDeductibles = new ObservableCollection<string> { };
            }
        }

        public void LoadBillingIntervalOptions()
        {
            BillingIntervals = new ObservableCollection<BillingInterval>(
                (BillingInterval[])Enum.GetValues(typeof(BillingInterval))
            );
        }
        #endregion
        #endregion

        #region Commands
        public ICommand ShowCompanyCommand =>
           new RelayCommand(() => CurrentView = "Företagsuppgifter");
        public ICommand ShowInsuranceTypeCommand =>
            new RelayCommand(() => CurrentView = "Försäkringstyp");
        public ICommand ShowInsuranceDetailsCommand =>
            new RelayCommand(ExecuteShowNextView);
        private ICommand searchCommand = null!;
        public ICommand SearchCommand =>
            searchCommand ??= searchCommand = new RelayCommand(
                () =>
                {
                    SelectedCompanyCustomer =
                        customerController.GetSpecificCompanyCustomerForInsuranceByOrgNumber(
                            inputOrgNumber
                        );
                },
                () => InputOrgNumber != null
            );

        private ICommand addCommand = null!;
        public ICommand AddCommand =>
            addCommand ??= addCommand = new RelayCommand(
                () =>
                {
                    if (selectedInsuranceType == insuranceType1)
                    {
                        insuranceController.CreatePropertyInsuranceFromInput(
                            LoggedInUser,
                            SelectedCompanyCustomer,
                            SelectedInsuranceType,
                            PropertyAddress,
                            PropertyValue,
                            InventoriesValue,
                            PropertyPremie,
                            InventoriesPremie,
                            ActivationDate,
                            ExpiryDate,
                            SelectedInterval,
                            TotalPremie
                        );
                        CurrentView = "KlarVy";
                    }
                    if (selectedInsuranceType == insuranceType2)
                    {
                        insuranceController.CreateCarInsuranceFromInput(
                            LoggedInUser,
                            SelectedCompanyCustomer,
                            SelectedInsuranceType,
                            SelectedCarInsuranceType,
                            SelectedCarDeductible,
                            SelectedZone,
                            CarPremie,
                            ActivationDate,
                            ExpiryDate,
                            SelectedInterval,
                            TotalPremie
                        );
                        CurrentView = "KlarVy";
                    }
                    if (selectedInsuranceType == insuranceType3)
                    {
                        insuranceController.CreateLiabilityInsuranceFromInput(
                            LoggedInUser,
                            SelectedCompanyCustomer,
                            SelectedInsuranceType,
                            SelectedLiabilityAmount,
                            SelectedLiabilityDeductible,
                            LiabilityPremie,
                            ActivationDate,
                            ExpiryDate,
                            SelectedInterval,
                            TotalPremie
                        );
                        CurrentView = "KlarVy";
                    }
                },
                () => true
            );
        private void ExecuteShowNextView()
        {
            if (SelectedInsuranceType == insuranceType1)
            {
                CurrentView = "FörsäkringsUppgifter1";
            }
            else if (SelectedInsuranceType == insuranceType2)
            {
                CurrentView = "FörsäkringsUppgifter2";
            }
            else if (SelectedInsuranceType == insuranceType3)
            {
                CurrentView = "FörsäkringsUppgifter3";
            }
        }

        #endregion
    }
}
