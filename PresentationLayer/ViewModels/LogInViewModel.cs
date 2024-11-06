using System.Threading.Channels;
using System.Windows;
using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class LoginViewModel : ObservableObject, ICloseWindows
{
    LoginUser loginUser = new LoginUser();

    public Action Close { get; set; }
    private IWindowService windowService { get; set; }

    private User userSelected = null!;
    public User UserSelected
    {
        get { return userSelected; }
        set
        {
            userSelected = value;
            OnPropertyChanged();
        }
    }

    private string errorMessage = null!;
    public string ErrorMessage
    {
        get { return errorMessage; }
        set
        {
            errorMessage = value;
            OnPropertyChanged();
        }
    }

    private string userNameInput = null!;
    public string UserNameInput
    {
        get { return userNameInput; }
        set
        {
            userNameInput = value;
            OnPropertyChanged();
        }
    }

    private string passwordInput = null!;
    public string PasswordInput
    {
        get { return passwordInput; }
        set
        {
            passwordInput = value;
            OnPropertyChanged();
        }
    }

    private ICommand loginBtn = null!;
    public ICommand LoginBtn =>
        loginBtn ??= new RelayCommand(() =>
        {
            try
            {
                LoggedInUser loggedInuser = loginUser.ValidateUser(userNameInput, passwordInput);
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(loggedInuser);

                
                windowService.ShowWindow(mainWindowViewModel);
                Close?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        });

    public LoginViewModel()
    {
        windowService = new WindowService();
    }
}
