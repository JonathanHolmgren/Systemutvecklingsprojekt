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
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class RegisterPreliminaryInsuranceCompanyViewModel : ObservableObject
    {
        private readonly InsuranceController _insuranceController;
        private readonly CustomerController _customerController;
        private readonly UserController _userController;

        public RegisterPreliminaryInsuranceCompanyViewModel() { }

        public RegisterPreliminaryInsuranceCompanyViewModel(
            LoggedInUser user,
            CompanyCustomer selectedCompanyCustomer
        )
        {
            LoggedInUser = user;
            SelectedCompanyCustomer = selectedCompanyCustomer;
            _insuranceController = new InsuranceController();
            _customerController = new CustomerController();
            _userController = new UserController();

            LoadInsuranceTypeOptions();
            LoadBillingIntervalOptions();
            CurrentView = "Företagsuppgifter";
        }

        private string _currentView = null!;
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
        private LoggedInUser _loggedInUser = null!;
        public LoggedInUser LoggedInUser
        {
            get { return _loggedInUser; }
            set
            {
                if (_loggedInUser != value)
                {
                    _loggedInUser = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region Customer
        private CompanyCustomer _selectedCompanyCustomer = null!;
        public CompanyCustomer SelectedCompanyCustomer
        {
            get { return _selectedCompanyCustomer; }
            set
            {
                _selectedCompanyCustomer = value;
                OnPropertyChanged();
            }
        }
        private string _inputOrgNumber = null!;
        public string InputOrgNumber
        {
            get { return _inputOrgNumber; }
            set
            {
                _inputOrgNumber = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region InsuranceTypes
        private ObservableCollection<string> _availableInsuranceTypes = null!;
        public ObservableCollection<string> AvailableInsuranceTypes
        {
            get => _availableInsuranceTypes;
            set
            {
                if (_availableInsuranceTypes != value)
                {
                    _availableInsuranceTypes = value;
                    OnPropertyChanged(nameof(AvailableInsuranceTypes));
                }
            }
        }
        private string _selectedInsuranceType = null!;
        public string SelectedInsuranceType
        {
            get => _selectedInsuranceType;
            set
            {
                _selectedInsuranceType = value;
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
        private string _propertyAddress = null!;
        public string PropertyAddress
        {
            get => _propertyAddress;
            set
            {
                _propertyAddress = value;
                OnPropertyChanged();
            }
        }
        private string _propertyValue = null!;
        public string PropertyValue
        {
            get => _propertyValue;
            set
            {
                _propertyValue = value;
                OnPropertyChanged();
                CalculatePropertyPremie();
            }
        }
        private string _propertyPremie = null!;
        public string PropertyPremie
        {
            get => _propertyPremie;
            set
            {
                _propertyPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }
        private string _inventoriesValue = null!;
        public string InventoriesValue
        {
            get => _inventoriesValue;
            set
            {
                _inventoriesValue = value;
                OnPropertyChanged();
                CalcultateInventoriesPremie();
            }
        }
        private string _inventoriesPremie = null!;
        public string InventoriesPremie
        {
            get => _inventoriesPremie;
            set
            {
                _inventoriesPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }
        #endregion
        #region CarInsurance
        private ObservableCollection<string> _availableCarInsuranceTypes = null!;
        public ObservableCollection<string> AvailableCarInsuranceTypes
        {
            get => _availableCarInsuranceTypes;
            set
            {
                if (_availableCarInsuranceTypes != value)
                {
                    _availableCarInsuranceTypes = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCarInsuranceType = null!;
        public string SelectedCarInsuranceType
        {
            get => _selectedCarInsuranceType;
            set
            {
                _selectedCarInsuranceType = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }
        private ObservableCollection<string> _availableCarDeductibles = null!;
        public ObservableCollection<string> AvailableCarDeductibles
        {
            get => _availableCarDeductibles;
            set
            {
                if (_availableCarDeductibles != value)
                {
                    _availableCarDeductibles = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCarDeductible = null!;
        public string SelectedCarDeductible
        {
            get => _selectedCarDeductible;
            set
            {
                _selectedCarDeductible = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }
        private ObservableCollection<string> _availableZones = null!;
        public ObservableCollection<string> AvailableZones
        {
            get => _availableZones;
            set
            {
                if (_availableZones != value)
                {
                    _availableZones = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedZone = null!;
        public string SelectedZone
        {
            get => _selectedZone;
            set
            {
                _selectedZone = value;
                OnPropertyChanged();
                CalculateCarPremie();
            }
        }
        private string _carPremie = null!;
        public string CarPremie
        {
            get => _carPremie;
            set
            {
                _carPremie = value;
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
                { ("Hel", "3000"), 600 },
            };
        #endregion
        #region LiabilityInsurance
        private ObservableCollection<string> _availableLiabilityAmount = null!;
        public ObservableCollection<string> AvailableLiabilityAmount
        {
            get => _availableLiabilityAmount;
            set
            {
                _availableLiabilityAmount = value;
                OnPropertyChanged();
            }
        }
        private string _selectedLiabilityAmount = null!;
        public string SelectedLiabilityAmount
        {
            get => _selectedLiabilityAmount;
            set
            {
                _selectedLiabilityAmount = value;
                OnPropertyChanged();
                CalculateLiabiltyPremie();
            }
        }
        private ObservableCollection<string> _availableLiabilityDeductibles = null!;
        public ObservableCollection<string> AvailableLiabilityDeductibles
        {
            get => _availableLiabilityDeductibles;
            set
            {
                if (_availableLiabilityDeductibles != value)
                {
                    _availableLiabilityDeductibles = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedLiabilityDeductible = null!;
        public string SelectedLiabilityDeductible
        {
            get => _selectedLiabilityDeductible;
            set
            {
                _selectedLiabilityDeductible = value;
                OnPropertyChanged();
                CalculateLiabiltyPremie();
            }
        }
        private string _liabilityPremie = null!;
        public string LiabilityPremie
        {
            get => _liabilityPremie;
            set
            {
                _liabilityPremie = value;
                OnPropertyChanged();
                CalculateTotalPremie();
            }
        }
        #endregion

        #region GenerellForInsurance
        // private DateTime activationDate;
        private DateTime _activationDate = DateTime.Now;
        public DateTime ActivationDate
        {
            get => _activationDate;
            set
            {
                _activationDate = value;
                OnPropertyChanged();
            }
        }

        // private DateTime expiryDate;
        private DateTime _expiryDate = DateTime.Now.AddMonths(1);
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                _expiryDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BillingInterval> BillingIntervals { get; private set; } = null!;
        private BillingInterval _selectedInterval;
        public BillingInterval SelectedInterval
        {
            get => _selectedInterval;
            set
            {
                if (_selectedInterval != value)
                {
                    _selectedInterval = value;
                    OnPropertyChanged();
                    CalculateTotalPremie();
                }
            }
        }
        private string _totalPremie = null!;
        public string TotalPremie
        {
            get => _totalPremie;
            set
            {
                _totalPremie = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        #region CalculateMethods
        public void CalculatePropertyPremie()
        {
            if (_selectedInsuranceType == insuranceType1)
            {
                if (
                    !string.IsNullOrWhiteSpace(_propertyValue)
                    && double.TryParse(_propertyValue, out double value)
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
            if (_selectedInsuranceType == insuranceType1)
            {
                if (
                    !string.IsNullOrWhiteSpace(_inventoriesValue)
                    && double.TryParse(_inventoriesValue, out double value)
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
            if (_selectedInsuranceType == insuranceType2)
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
                    _ => 1.0,
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
            if (_selectedInsuranceType == insuranceType3)
            {
                if (
                    _selectedLiabilityAmount == amount1
                    && _selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "800";
                }
                else if (
                    _selectedLiabilityAmount == amount1
                    && _selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "700";
                }
                else if (
                    _selectedLiabilityAmount == amount1
                    && _selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "600";
                }
                else if (
                    _selectedLiabilityAmount == amount1
                    && _selectedLiabilityDeductible == liabilityDeductibles4
                )
                {
                    LiabilityPremie = "500";
                }
                else if (
                    _selectedLiabilityAmount == amount2
                    && _selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "1300";
                }
                else if (
                    _selectedLiabilityAmount == amount2
                    && _selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "1200";
                }
                else if (
                    _selectedLiabilityAmount == amount2
                    && _selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "1100";
                }
                else if (
                    _selectedLiabilityAmount == amount2
                    && _selectedLiabilityDeductible == liabilityDeductibles4
                )
                {
                    LiabilityPremie = "1000";
                }
                else if (
                    _selectedLiabilityAmount == amount3
                    && _selectedLiabilityDeductible == liabilityDeductibles1
                )
                {
                    LiabilityPremie = "1800";
                }
                else if (
                    _selectedLiabilityAmount == amount3
                    && _selectedLiabilityDeductible == liabilityDeductibles2
                )
                {
                    LiabilityPremie = "1700";
                }
                else if (
                    _selectedLiabilityAmount == amount3
                    && _selectedLiabilityDeductible == liabilityDeductibles3
                )
                {
                    LiabilityPremie = "1600";
                }
                else if (
                    _selectedLiabilityAmount == amount3
                    && _selectedLiabilityDeductible == liabilityDeductibles4
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
            if (_selectedInsuranceType == insuranceType1)
            {
                double.TryParse(InventoriesPremie, out double inventoriesPremie);
                double.TryParse(PropertyPremie, out double propertyPremie);
                totalPremie = inventoriesPremie + propertyPremie;
            }
            else if (_selectedInsuranceType == insuranceType2)
            {
                double.TryParse(_carPremie, out totalPremie);
            }
            else if (_selectedInsuranceType == insuranceType3)
            {
                double.TryParse(_liabilityPremie, out totalPremie);
            }
            else
            {
                totalPremie = 0.0;
            }
            TotalPremie = AdjustForBillingInterval(totalPremie);
        }

        private string AdjustForBillingInterval(double totalPremie)
        {
            switch (_selectedInterval)
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
                insuranceType3,
            };
        }

        string carInsuranceType1 = "Trafik";
        string carInsuranceType2 = "Halv";
        string carInsuranceType3 = "Hel";

        public void LoadCarInsuranceOptions()
        {
            if (_selectedInsuranceType == insuranceType2)
            {
                AvailableCarInsuranceTypes = new ObservableCollection<string>
                {
                    carInsuranceType1,
                    carInsuranceType2,
                    carInsuranceType3,
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
            if (_selectedInsuranceType == insuranceType2)
            {
                AvailableCarDeductibles = new ObservableCollection<string>
                {
                    deductible1,
                    deductible2,
                    deductible3,
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
            if (_selectedInsuranceType == insuranceType2)
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
            if (_selectedInsuranceType == insuranceType3)
            {
                AvailableLiabilityAmount = new ObservableCollection<string>
                {
                    amount1,
                    amount2,
                    amount3,
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
            if (_selectedInsuranceType == insuranceType3)
            {
                AvailableLiabilityDeductibles = new ObservableCollection<string>
                {
                    liabilityDeductibles1,
                    liabilityDeductibles2,
                    liabilityDeductibles3,
                    liabilityDeductibles4,
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
        public ICommand ShowInsuranceDetailsCommand => new RelayCommand(ExecuteShowNextView);

        private ICommand _navigateBackCommand = null!;
        public ICommand NavigateBackCommand =>
            _navigateBackCommand ??= new RelayCommand(() =>
            {
                Mediator.Notify("ChangeView", new SearchCustomerProfileViewModel(_loggedInUser));
            });

        private ICommand searchCommand = null!;
        public ICommand SearchCommand =>
            searchCommand ??= searchCommand = new RelayCommand(
                () =>
                {
                    SelectedCompanyCustomer =
                        _customerController.GetSpecificCompanyCustomerForInsuranceByOrgNumber(
                            _inputOrgNumber
                        );
                },
                () => InputOrgNumber != null
            );
        private ICommand addCommand = null!;
        public ICommand AddCommand =>
            addCommand ??= addCommand = new RelayCommand(
                () =>
                {
                    CompanyCustomer companyCustomer = new CompanyCustomer
                    {
                        CustomerID = SelectedCompanyCustomer.CustomerID,
                        TelephoneNumber = SelectedCompanyCustomer.TelephoneNumber,
                        Email = SelectedCompanyCustomer.Email,
                        StreetAddress = SelectedCompanyCustomer.StreetAddress,
                        PostalCode = SelectedCompanyCustomer.PostalCode,
                        City = SelectedCompanyCustomer.City,
                        OrganisationNumber = SelectedCompanyCustomer.OrganisationNumber,
                        ContactPersonName = SelectedCompanyCustomer.ContactPersonName,
                        CompanyPersonTelephoneNumber =
                            SelectedCompanyCustomer.CompanyPersonTelephoneNumber,
                        CompanyName = SelectedCompanyCustomer.CompanyName,
                    };

                    if (_selectedInsuranceType == insuranceType1)
                    {
                        _insuranceController.CreatePropertyInsuranceFromInput(
                            LoggedInUser,
                            companyCustomer,
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
                    if (_selectedInsuranceType == insuranceType2)
                    {
                        _insuranceController.CreateCarInsuranceFromInput(
                            LoggedInUser,
                            companyCustomer,
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
                    if (_selectedInsuranceType == insuranceType3)
                    {
                        _insuranceController.CreateLiabilityInsuranceFromInput(
                            LoggedInUser,
                            companyCustomer,
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
