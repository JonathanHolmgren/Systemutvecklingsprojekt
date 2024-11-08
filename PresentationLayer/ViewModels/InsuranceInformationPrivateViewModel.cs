using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Models;
using Models.InsuranceInformation;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class InsuranceInformationPrivateViewModel : ObservableObject
{
    private InsuranceController insuranceController = new InsuranceController();
    private InsuranceSpecController insuranceSpecController = new InsuranceSpecController();
    private LoggedInUser _user;

    public InsuranceInformationPrivateViewModel() { }

    public InsuranceInformationPrivateViewModel(LoggedInUser user, PrivateCustomer privateCustomer)
    {
        _user = user;
        _viewedPrivateCustomer = privateCustomer;
        _customerInsurances = new ObservableCollection<Insurance>(
            insuranceController.GetPrivateCustomerInsurancesByCustomerId(privateCustomer.CustomerID)
        );
    }

    private PrivateCustomer _viewedPrivateCustomer = null!;
    public PrivateCustomer ViewedPrivateCustomer
    {
        get { return _viewedPrivateCustomer; }
        set
        {
            _viewedPrivateCustomer = value;
            OnPropertyChanged();
        }
    }

    private bool isActiveStatusSelected;
    public bool IsActiveStatusSelected
    {
        get { return isActiveStatusSelected; }
        set
        {
            isActiveStatusSelected = value;
            OnPropertyChanged();
        }
    }
    private bool isInactiveStatusSelected;
    public bool IsInactiveStatusSelected
    {
        get { return isInactiveStatusSelected; }
        set
        {
            isInactiveStatusSelected = value;
            OnPropertyChanged();
        }
    }

    private ICommand _removeInsuranceCommand = null!;
    public ICommand RemoveInsuranceCommand =>
        _removeInsuranceCommand ??= new RelayCommand(RemoveChosenInsurance);

    private ICommand _changeInsuranceStatusCommand = null!;
    public ICommand ChangeInsuranceStatusCommand =>
        _changeInsuranceStatusCommand ??= new RelayCommand(ChangeInsuranceStatus);

    private ObservableCollection<Insurance> _customerInsurances =
        new ObservableCollection<Insurance>();
    public ObservableCollection<Insurance> CustomerInsurances
    {
        get => _customerInsurances;
        set
        {
            _customerInsurances = value;
            OnPropertyChanged();
        }
    }

    private ICommand _createInsuranceAgreementCommand = null!;
    public ICommand CreateInsuranceAgreementCommand =>
        _createInsuranceAgreementCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify(
                "ChangeView",
                new RegisterPreliminaryInsuranceViewModel(_user, _viewedPrivateCustomer)
            );
        });

    private ICommand _navigateBackCommand = null!;
    public ICommand NavigateBackCommand =>
        _navigateBackCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify(
                "ChangeView",
                new PrivateCustomerProfileViewModel(_user, _viewedPrivateCustomer)
            );
        });

    private ObservableCollection<InsuranceSpecAndAttributeInformation> insuranceSpecsAndAttributesInformation =
        new ObservableCollection<InsuranceSpecAndAttributeInformation>();
    public ObservableCollection<InsuranceSpecAndAttributeInformation> InsuranceSpecsAndAttributesInformation
    {
        get => insuranceSpecsAndAttributesInformation;
        set
        {
            insuranceSpecsAndAttributesInformation = value;
            OnPropertyChanged();
        }
    }

    private Insurance _selectedInsurance;
    public Insurance SelectedInsurance
    {
        get => _selectedInsurance;
        set
        {
            _selectedInsurance = value;
            OnPropertyChanged();
            ShowAttributesAndSpecs();
        }
    }

    private void ShowAttributesAndSpecs()
    {
        InsuranceSpecsAndAttributesInformation.Clear();
        if (SelectedInsurance != null)
        {
            IList<InsuranceSpec> insuranceSpecs =
                insuranceSpecController.GetAllInsuranceSpecsForInsurance(
                    _selectedInsurance.InsuranceId
                );

            foreach (InsuranceSpec spec in insuranceSpecs)
            {
                InsuranceSpecAndAttributeInformation tempInsuranceSpecAndAttributeInformation =
                    new InsuranceSpecAndAttributeInformation(
                        spec.InsuranceTypeAttribute.InsuranceAttribute,
                        spec.Value
                    );
                InsuranceSpecsAndAttributesInformation.Add(
                    tempInsuranceSpecAndAttributeInformation
                );
            }
        }
    }

    private void ChangeInsuranceStatus()
    {
        if (SelectedInsurance == null)
            return;
        if (_viewedPrivateCustomer == null)
            return;
        if (IsActiveStatusSelected == true)
        {
            insuranceController.SetInsuranceStatusToActive(SelectedInsurance);
            foreach (var insurance in _customerInsurances)
            {
                if (insurance.InsuranceId == _selectedInsurance.InsuranceId)
                {
                    insurance.InsuranceStatus = InsuranceStatus.Active;
                }
            }
            MessageBox.Show("Status på avtalet är ändrad till aktiv");
            SelectedInsurance = null;
            IsActiveStatusSelected = false;
        }
        else if (IsInactiveStatusSelected == true)
        {
            insuranceController.SetInsuranceStatusToInactive(SelectedInsurance);
            foreach (var insurance in _customerInsurances)
            {
                if (insurance.InsuranceId == _selectedInsurance.InsuranceId)
                {
                    insurance.InsuranceStatus = InsuranceStatus.Inactive;
                }
            }
            MessageBox.Show("Status på avtalet är ändrad till inaktiv");
            SelectedInsurance = null;
            IsInactiveStatusSelected = false;
        }
    }

    private void RemoveChosenInsurance()
    {
        if (_viewedPrivateCustomer != null && SelectedInsurance != null)
        {
            if (SelectedInsurance.InsuranceStatus == InsuranceStatus.Active)
            {
                MessageBox.Show(
                    "Detta är en aktiv försäkring, du måste avsluta den innan du kan ta bort den"
                );
            }
            else
            {
                insuranceController.RemoveInsurance(SelectedInsurance);
            }
        }
        SelectedInsurance = null;
    }
}
