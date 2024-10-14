using System.Windows.Input;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class RegisterUserViewModel : ObservableObject
{
    UserController userController;

    // public RegisterUserViewModel(UserController userController)
    //      {
    //         // this.userController = userController;
    //      }
    
    public RegisterUserViewModel()
    {
        // this.userController = userController;
    }

    private Employee employeeSelected = null!;
    public Employee EmployeeSelected  {
        get { return employeeSelected; }
        set
        {
            employeeSelected = value;
            OnPropertyChanged();
        }
    }
    
    private string passwordInput = null!;
    public string PasswordInput  {
        get { return passwordInput; }
        set
        {
            passwordInput = value;
            OnPropertyChanged();
        }
    }

    private AuthorizationLevel authorizationLevelSelected;
    public AuthorizationLevel AuthorizationLevelSelected  {
        get { return authorizationLevelSelected; }
        set
        {
            authorizationLevelSelected = value;
            OnPropertyChanged();
        }
    }


    
    
    private ICommand generatePasswordCommand = null!;
    
    public ICommand GeneratePasswordCommand => generatePasswordCommand ??= new RelayCommand(() =>
    {
        PasswordInput = "123456";
    });

    private ICommand addUserCommand = null!;

    public ICommand AddUserCommand => addUserCommand ??= new RelayCommand(() =>
    {
        //  userController.CreateUser(EmployeeSelected, PasswordInput, authorizationLevelSelected);
    });

}