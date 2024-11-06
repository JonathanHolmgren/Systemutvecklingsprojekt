using System.Windows.Input;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using PresentationLayer.Views;

namespace PresentationLayer.ViewModels;

public class SearchCustomerProfileViewModel : ObservableObject
{
    private ICommand _searchPrivateCustomerCommand = null!;
    public ICommand SearchPrivateCustomerCommand =>
        _searchPrivateCustomerCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new PrivateCustomerProfileViewModel());
        });

    private ICommand _searchCompanyCustomerCommand = null!;
    public ICommand SearchCompanyCustomerCommand =>
        _searchCompanyCustomerCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new CompanyCustomerProfileViewModel());
        });
}
