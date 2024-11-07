using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using PresentationLayer.Views;

namespace PresentationLayer.ViewModels;

public class SearchCustomerProfileViewModel : ObservableObject
{
    private LoggedInUser _user;

    public SearchCustomerProfileViewModel() { }

    public SearchCustomerProfileViewModel(LoggedInUser user)
    {
        _user = user;
    }

    private ICommand _searchPrivateCustomerCommand = null!;
    public ICommand SearchPrivateCustomerCommand =>
        _searchPrivateCustomerCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new PrivateCustomerProfileViewModel(_user));
        });

    private ICommand _searchCompanyCustomerCommand = null!;
    public ICommand SearchCompanyCustomerCommand =>
        _searchCompanyCustomerCommand ??= new RelayCommand(() =>
        {
            Mediator.Notify("ChangeView", new CompanyCustomerProfileViewModel(_user));
        });
}
