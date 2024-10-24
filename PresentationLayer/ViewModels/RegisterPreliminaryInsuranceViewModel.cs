using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class RegisterPreliminaryInsuranceViewModel : ObservableObject
    {
        private InsuranceController insuranceController;
        private CustomerController customerController;

        public RegisterPreliminaryInsuranceViewModel()
        {
            insuranceController = new InsuranceController();
            customerController = new CustomerController();
            LoadInsuranceTypeOptions();
            LoadBillingIntervalOptions();
            LoadAddOnOptions1();
            LoadAddOnOptions2();
        }

        #region Users
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
        private string inputSocialSecurityNumber;
        public string InputSocialSecurityNumber
        {
            get { return inputSocialSecurityNumber; }
            set
            {
                inputSocialSecurityNumber = value;
                OnPropertyChanged();
            }
        }

        private PrivateCustomer selectedPrivateCustomer = null!;
        public PrivateCustomer SelectedPrivateCustomer
        {
            get => selectedPrivateCustomer;
            set
            {
                selectedPrivateCustomer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region InsuredPerson
        private string insuredPersonFirstName;
        public string InsuredPersonFirstName
        {
            get { return insuredPersonFirstName; }
            set
            {
                if (insuredPersonFirstName != value)
                {
                    insuredPersonFirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string insuredPersonLastName;
        public string InsuredPersonLastName
        {
            get { return insuredPersonLastName; }
            set
            {
                if (insuredPersonLastName != value)
                {
                    insuredPersonLastName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string insuredPersonSSN;
        public string InsuredPersonSSN
        {
            get { return insuredPersonSSN; }
            set
            {
                if (insuredPersonSSN != value)
                {
                    insuredPersonSSN = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Insurance
        private ObservableCollection<string> availableInsuranceTypes;
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

        private string selectedInsuranceType;
        public string SelectedInsuranceType
        {
            get { return selectedInsuranceType; }
            set
            {
                if (selectedInsuranceType != value)
                {
                    selectedInsuranceType = value;
                    OnPropertyChanged();
                    UpdateBasePriceOptions();
                }
            }
        }

        private DateTime arrivingDate = DateTime.Now;
        public DateTime ArrivingDate
        {
            get { return arrivingDate; }
            set
            {
                if (arrivingDate != value)
                {
                    arrivingDate = value;
                    OnPropertyChanged();
                    UpdateBasePriceOptions();
                }
            }
        }

        private ObservableCollection<string> basePriceOptions;
        public ObservableCollection<string> BasePriceOptions
        {
            get => basePriceOptions;
            set
            {
                if (basePriceOptions != value)
                {
                    basePriceOptions = value;
                    OnPropertyChanged(nameof(BasePriceOptions));
                }
            }
        }

        private string selectedBasePrice;
        public string SelectedBasePrice
        {
            get { return selectedBasePrice; }
            set
            {
                if (selectedBasePrice != value)
                {
                    selectedBasePrice = value;
                    OnPropertyChanged();
                    CalculatePremiePrice();
                }
            }
        }

        private string notes;
        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged();
                }
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
                    CalculatePremiePrice();
                }
            }
        }

        private string totalPremium;
        public string TotalPremium
        {
            get => totalPremium;
            set
            {
                if (totalPremium != value)
                {
                    totalPremium = value;
                    OnPropertyChanged(nameof(TotalPremium));
                }
            }
        }
        #endregion

        #region InsuranceAddOn
        private string selectedAddOnOption1;
        public string SelectedAddOnOption1
        {
            get { return selectedAddOnOption1; }
            set
            {
                if (selectedAddOnOption1 != value)
                {
                    selectedAddOnOption1 = value;
                    OnPropertyChanged();
                    UpdateAddOnBasePrices1();
                }
            }
        }

        private string selectedAddOnOption2;
        public string SelectedAddOnOption2
        {
            get { return selectedAddOnOption2; }
            set
            {
                if (selectedAddOnOption2 != value)
                {
                    selectedAddOnOption2 = value;
                    OnPropertyChanged();
                    UpdateAddOnBasePrice2();
                }
            }
        }

        private ObservableCollection<string> addOnOptions1;
        public ObservableCollection<string> AddOnOptions1
        {
            get => addOnOptions1;
            set
            {
                if (addOnOptions1 != value)
                {
                    addOnOptions1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> addOnOptions2;
        public ObservableCollection<string> AddOnOptions2
        {
            get => addOnOptions2;
            set
            {
                if (addOnOptions2 != value)
                {
                    addOnOptions2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> addOnBasePriceOptions1;
        public ObservableCollection<string> AddOnBasePriceOptions1
        {
            get => addOnBasePriceOptions1;
            set
            {
                if (addOnBasePriceOptions1 != value)
                {
                    addOnBasePriceOptions1 = value;
                    OnPropertyChanged(nameof(AddOnBasePriceOptions1));
                }
            }
        }

        private ObservableCollection<string> addOnBasePriceOptions2;
        public ObservableCollection<string> AddOnBasePriceOptions2
        {
            get => addOnBasePriceOptions2;
            set
            {
                if (addOnBasePriceOptions2 != value)
                {
                    addOnBasePriceOptions2 = value;
                    OnPropertyChanged(nameof(AddOnBasePriceOptions2));
                }
            }
        }

        private string selectedAddOnBasePrice1;
        public string SelectedAddOnBasePrice1
        {
            get { return selectedAddOnBasePrice1; }
            set
            {
                if (selectedAddOnBasePrice1 != value)
                {
                    selectedAddOnBasePrice1 = value;
                    OnPropertyChanged();
                    CalculatePremiePrice();
                }
            }
        }

        private string selectedAddOnBasePrice2;
        public string SelectedAddOnBasePrice2
        {
            get { return selectedAddOnBasePrice2; }
            set
            {
                if (selectedAddOnBasePrice2 != value)
                {
                    selectedAddOnBasePrice2 = value;
                    OnPropertyChanged();
                    CalculatePremiePrice();
                }
            }
        }
        #endregion

        #region Methods
        DateTime cutoffDate = new DateTime(2024, 1, 1);

        private string insuranceType1 = "Sjuk- och olycksförsäkring för barn (t.o.m 17 års åldern)";
        private string insuranceType2 = "Sjuk- och olycksfallsförsäkring för vuxen";
        private string insuranceType3 = "Livförsäkring för vuxen";

        private void UpdateBasePriceOptions()
        {
            if (selectedInsuranceType == insuranceType1 && arrivingDate < cutoffDate)
            {
                BasePriceOptions = new ObservableCollection<string>()
                {
                    "700 000",
                    "900 000",
                    "1 100 000",
                    "1 300 000"
                };
            }
            else if (selectedInsuranceType == insuranceType1 && arrivingDate >= cutoffDate)
            {
                BasePriceOptions = new ObservableCollection<string>()
                {
                    "750 000",
                    "950 000",
                    "1 150 000",
                    "1 350 000"
                };
            }
            if (
                selectedInsuranceType == insuranceType2 && arrivingDate < cutoffDate
                || selectedInsuranceType == insuranceType3 && arrivingDate < cutoffDate
            )
            {
                BasePriceOptions = new ObservableCollection<string>()
                {
                    "300 000",
                    "400 000",
                    "500 000",
                };
            }
            else if (
                selectedInsuranceType == insuranceType2 && arrivingDate >= cutoffDate
                || selectedInsuranceType == insuranceType3 && arrivingDate >= cutoffDate
            )
            {
                BasePriceOptions = new ObservableCollection<string>()
                {
                    "350 000",
                    "450 000",
                    "550 000"
                };
            }
        }

        public void LoadInsuranceTypeOptions()
        {
            AvailableInsuranceTypes = new ObservableCollection<string>
            {
                insuranceType1,
                insuranceType2,
                insuranceType3
            };
        }

        string alt1 = "Inget";
        string AddOn1 = "Invaliditet vid olycksfall";
        string AddOn2 = "Månadsersättning vid långvarig sjukskrivning";

        public void LoadAddOnOptions1()
        {
            AddOnOptions1 = new ObservableCollection<string> { alt1, AddOn1 };
        }

        public void LoadAddOnOptions2()
        {
            AddOnOptions2 = new ObservableCollection<string>() { alt1, AddOn2 };
        }

        public void UpdateAddOnBasePrices1()
        {
            if (selectedAddOnOption1 == AddOn1)
            {
                AddOnBasePriceOptions1 = new ObservableCollection<string>()
                {
                    "100 000",
                    "200 000",
                    "300 000",
                    "400 000",
                    "500 000",
                    "600 000",
                    "700 000",
                    "800 000"
                };
            }
            else
                AddOnBasePriceOptions1 = new ObservableCollection<string>() { };
        }

        public void UpdateAddOnBasePrice2()
        {
            if (selectedAddOnOption2 == AddOn2)
            {
                AddOnBasePriceOptions2 = new ObservableCollection<string>()
                {
                    "500",
                    "1000",
                    "1500",
                    "2000",
                    "2500",
                    "3000",
                    "3500",
                    "4000"
                };
            }
            else
                AddOnBasePriceOptions2 = new ObservableCollection<string>() { };
        }

        public void LoadBillingIntervalOptions()
        {
            BillingIntervals = new ObservableCollection<BillingInterval>(
                (BillingInterval[])Enum.GetValues(typeof(BillingInterval))
            );
        }

        public void CalculatePremiePrice()
        {
            double totalPremie = 0;

            totalPremie += CalculateBasePrice();
            totalPremie += CalculateAddOnPrice1();
            totalPremie += CalculateAddOnPrice2();

            totalPremie = AdjustForBillingInterval(totalPremie);

            TotalPremium = totalPremie.ToString("F2");
        }

        private double CalculateBasePrice()
        {
            if (!string.IsNullOrEmpty(selectedBasePrice))
            {
                double basePrice = double.Parse(selectedBasePrice);
                return basePrice * 0.0005;
            }
            return 0;
        }

        private double CalculateAddOnPrice1()
        {
            if (!string.IsNullOrEmpty(selectedAddOnBasePrice1))
            {
                double addOnPrice1 = double.Parse(selectedAddOnBasePrice1);
                return addOnPrice1 * 0.0003;
            }
            return 0;
        }

        private double CalculateAddOnPrice2()
        {
            if (!string.IsNullOrEmpty(selectedAddOnBasePrice2))
            {
                double addOnPrice2 = double.Parse(selectedAddOnBasePrice2);
                return addOnPrice2 * 0.005;
            }
            return 0;
        }

        private double AdjustForBillingInterval(double totalPremie)
        {
            switch (selectedInterval)
            {
                case BillingInterval.Månad:
                    return totalPremie;
                case BillingInterval.Kvartal:
                    return totalPremie * 3;
                case BillingInterval.Halvår:
                    return totalPremie * 6;
                case BillingInterval.År:
                    return totalPremie * 12;
                default:
                    return totalPremie;
            }
        }

        #endregion

        #region Commands
        private ICommand searchCommand = null!;
        public ICommand SearchCommand =>
            searchCommand ??= searchCommand = new RelayCommand(
                () =>
                {
                    SelectedPrivateCustomer =
                        customerController.GetSpecificPrivateCustomerForInsurance(
                            inputSocialSecurityNumber
                        );
                },
                () => InputSocialSecurityNumber != null
            );

        private ICommand addCommand = null!;
        public ICommand AddCommand =>
            addCommand ??= addCommand = new RelayCommand(
                () =>
                {
                    //Kanske är onödigt att ha denna eftersom att CanAddPreliminaryInsurance gör detta?
                    if (
                        selectedInsuranceType != null
                        && insuredPersonFirstName != null
                        && insuredPersonLastName != null
                        && arrivingDate.ToString() != null
                        && selectedInterval.ToString() != null
                        && selectedPrivateCustomer != null
                        && selectedBasePrice != null
                        && totalPremium != null
                    )
                    {
                        Insurance newInsurance = insuranceController.CreateInsuranceFromInput(
                            1,
                            SelectedInsuranceType,
                            InsuredPersonFirstName,
                            InsuredPersonLastName,
                            InsuredPersonSSN,
                            ArrivingDate,
                            SelectedInterval,
                            Notes,
                            SelectedPrivateCustomer,
                            SelectedBasePrice,
                            TotalPremium,
                            SelectedAddOnBasePrice1,
                            SelectedAddOnBasePrice2,
                            SelectedAddOnOption1,
                            SelectedAddOnOption2
                        );
                    }
                },
                () =>
                    insuranceController.CanAddPrivatePreliminaryInsurance(
                        selectedInsuranceType,
                        insuredPersonFirstName,
                        insuredPersonLastName,
                        insuredPersonSSN,
                        arrivingDate.ToString(),
                        selectedBasePrice,
                        totalPremium,
                        selectedAddOnOption1,
                        selectedAddOnOption2,
                        selectedAddOnBasePrice1,
                        selectedAddOnBasePrice2
                    )
            );
        #endregion
    }
}
