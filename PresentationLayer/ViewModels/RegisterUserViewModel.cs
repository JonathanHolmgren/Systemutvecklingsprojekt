using System.Diagnostics;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;

namespace PresentationLayer.ViewModels;

public class RegisterUserViewModel : ObservableObject
{
    private readonly UserController userController;

    public RegisterUserViewModel()
    {
        this.userController = new UserController();
    }

    private Employee employeeSelected = null!;
    public Employee EmployeeSelected
    {
        get { return employeeSelected; }
        set
        {
            employeeSelected = value;
            OnPropertyChanged();
        }
    }
    private IEnumerable<User> users = null!;
    public IEnumerable<User> Users
    {
        get { return users; }
        set
        {
            users = value;
            OnPropertyChanged();
        }
    }

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

    private string employeeInput = null!;
    public string EmployeeInput
    {
        get { return employeeInput; }
        set
        {
            employeeInput = value;
            OnPropertyChanged();
        }
    }

    private ICommand searchEmployee = null!;
    public ICommand SearchEmployee =>
        searchEmployee ??= new RelayCommand(
            () =>
            {
                EmployeeSelected = userController.GetEmployee(employeeInput);
                if (EmployeeSelected != null)
                {
                    Debug.WriteLine(EmployeeSelected);
                }
                else
                {
                    Debug.WriteLine("No employee selected");
                }
            },
            () => !EmployeeInput.IsNullOrEmpty()
        );
    private ICommand searchUser = null!;
    public ICommand SearchUser =>
        searchUser ??= new RelayCommand(() =>
        {
            Users = userController.GetUsers(employeeInput);
        });

    private AuthorizationLevel authorizationLevelSelected;
    public AuthorizationLevel AuthorizationLevelSelected
    {
        get { return authorizationLevelSelected; }
        set
        {
            authorizationLevelSelected = value;
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

    private ICommand removeUser = null!;

    public ICommand RemoveUser =>
        removeUser ??= new RelayCommand(
            () =>
            {
                userController.RemoveUserById(UserSelected.UserID);
            },
            () => UserSelected != null
        );

    private ICommand addUserCommand;

    public ICommand AddUserCommand =>
        addUserCommand ??= new RelayCommand(
            () =>
            {
                userController.CreateUser(
                    PasswordInput,
                    EmployeeSelected,
                    AuthorizationLevelSelected
                );
                EmployeeSelected = null;
                EmployeeInput = null!;
                AuthorizationLevelSelected = AuthorizationLevel.Admin;
            },
            () => EmployeeSelected != null
        );
}
