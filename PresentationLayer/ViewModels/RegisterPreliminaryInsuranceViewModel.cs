using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            //LoadCustomerData();
            LoadInsuranceTypeOptions();
            LoadBillingIntervalOptions();
            LoadAddOnOptions1();
            LoadAddOnOptions2();
            LoadUserData();
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

        private PrivateCustomer selectedPrivateCustomer;
        public PrivateCustomer SelectedPrivateCustomer
        {
            get => selectedPrivateCustomer;
            set
            {
                selectedPrivateCustomer = value;
                OnPropertyChanged();
            }
        }

        //private PostalCodeCity postalCodeCity = new PostalCodeCity
        //{
        //    City = "Borås",
        //    PostalCode = "50630"
        //};

        //private void LoadCustomerData()
        //{
        //    SelectedPrivateCustomer = new PrivateCustomer(
        //        "0706689932",
        //        "ingalillblommor52@emial.com",
        //        "Gatuvägen 21",
        //        postalCodeCity,
        //        "19521019-1234",
        //        "Inga-Lill",
        //        "Bengtsson",
        //        "0346-58948"
        //    );
        //}
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
        public void LoadUserData()
        {
            LoggedInUser = insuranceController.GetUser(1);
        }

        DateTime cutoffDate = new DateTime(2024, 1, 1);

        private string insuranceType1 =
            "Sjuk- och olycksfallsförsäkring för barn (t.o.m. 17 års ålder)";
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
                    SelectedPrivateCustomer = customerController.GetSpecificPrivateCustomer(
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
                    if (true)
                    {
                        CreateInsuranceFromInput();
                        MessageBox.Show("Allhamdulla");
                    }
                },
                () => true
            );

        //private User CreateUser() //Ska bli GetSpecific User där man tar den som är inloggad;
        //{
        //    User user;

        //    Commission commissionRate = new Commission(0.12);
        //    PostalCodeCity postalCodeCity = new PostalCodeCity("60548", "Falkenberg");

        //    Employee employee = new Employee(
        //        "2153",
        //        "19930710-1234",
        //        "Ginda",
        //        "Lonsson",
        //        "Hamngatan 9",
        //        postalCodeCity,
        //        "viktor.nystrom@exempel.se",
        //        "Utesäljare",
        //        "070-789 01 23",
        //        commissionRate
        //    );
        //    user = new User("12345", AuthorizationLevel.SalesPerson, employee);
        //    return user;
        //}

        //Denna behövs inte
        //private InsuranceType CreateInsuranceType()
        //{
        //    InsuranceType insuranceType;
        //    insuranceType = new InsuranceType(selectedInsuranceType);
        //    return insuranceType;
        //}

        private List<InsuranceTypeAttribute> CreateInsuranceTypeAttribute(
            InsuranceType insuranceType
        )
        {
            List<InsuranceTypeAttribute> insuranceTypeAttributesList =
                new List<InsuranceTypeAttribute>();
            InsuranceTypeAttribute basePrice = new InsuranceTypeAttribute(
                "Grundbelopp",
                insuranceType
            );
            InsuranceTypeAttribute premie = new InsuranceTypeAttribute("Premie", insuranceType);
            InsuranceTypeAttribute date = new InsuranceTypeAttribute("Datum", insuranceType);

            insuranceTypeAttributesList.Add(basePrice);
            insuranceTypeAttributesList.Add(premie);
            insuranceTypeAttributesList.Add(date);

            if (selectedAddOnOption1 != null)
            {
                InsuranceTypeAttribute addOn1 = new InsuranceTypeAttribute(
                    selectedAddOnOption1,
                    insuranceType
                );
                insuranceTypeAttributesList.Add(addOn1);
            }
            if (selectedAddOnOption2 != null)
            {
                InsuranceTypeAttribute addOn2 = new InsuranceTypeAttribute(
                    selectedAddOnOption2,
                    insuranceType
                );
                insuranceTypeAttributesList.Add(addOn2);
            }
            AddAllInsuranceTypeAttribute(insuranceTypeAttributesList);
            return insuranceTypeAttributesList;
        }

        public void CreateInsuranceFromInput()
        {
            //User user = CreateUser();
            //MessageBox.Show(user.Employee.PostalCodeCity.PostalCode.ToString());

            InsuranceType insuranceType = insuranceController.GetInsuranceType(
                selectedInsuranceType
            );
            List<InsuranceTypeAttribute> insuranceTypeAttributesList = CreateInsuranceTypeAttribute(
                insuranceType
            );
            InsuredPerson insuredPerson = insuranceController.CreateInsuredPerson(
                insuredPersonFirstName,
                insuredPersonLastName,
                insuredPersonSSN
            );

            Insurance newInsurance;
            newInsurance = new Insurance(
                arrivingDate,
                selectedInterval,
                LoggedInUser,
                insuranceType,
                notes,
                selectedPrivateCustomer,
                insuredPerson
            );

            foreach (InsuranceTypeAttribute insuranceTypeItem in insuranceTypeAttributesList)
            {
                if (insuranceTypeItem.InsuranceAttribute == "Grundbelopp")
                {
                    CreateInsuranceSpec(insuranceTypeItem, newInsurance, selectedBasePrice);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Premie")
                {
                    CreateInsuranceSpec(insuranceTypeItem, newInsurance, totalPremium);
                }
                if (insuranceTypeItem.InsuranceAttribute == "Datum")
                {
                    CreateInsuranceSpec(insuranceTypeItem, newInsurance, arrivingDate.ToString());
                }
                if (insuranceTypeItem.InsuranceAttribute == "Invaliditet vid olycksfall")
                {
                    CreateInsuranceSpec(insuranceTypeItem, newInsurance, selectedAddOnBasePrice1);
                }
                if (
                    insuranceTypeItem.InsuranceAttribute
                    == "Månadsersättning vid långvarig sjukskrivning"
                )
                {
                    CreateInsuranceSpec(insuranceTypeItem, newInsurance, selectedAddOnBasePrice1);
                }
            }
            insuranceController.AddInsurance(newInsurance);
        }

        public void AddAllInsuranceTypeAttribute(
            IList<InsuranceTypeAttribute> insuranceTypeAttribute
        )
        {
            foreach (InsuranceTypeAttribute item in insuranceTypeAttribute)
            {
                insuranceController.AddInsuranceTypeAttribute(item);
            }
        }

        public void CreateInsuranceSpec(
            InsuranceTypeAttribute insuranceTypeAttribute,
            Insurance insurance,
            string value
        )
        {
            InsuranceSpec insuranceSpec = new InsuranceSpec(
                value,
                insurance,
                insuranceTypeAttribute
            );
            insuranceController.AddInsuranceSpec(insuranceSpec);
        }
        #endregion
    }
}
