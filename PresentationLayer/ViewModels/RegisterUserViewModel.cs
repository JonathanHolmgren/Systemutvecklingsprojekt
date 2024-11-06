using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Azure.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using ServiceLayer;
using ServiceLayer.services;

namespace PresentationLayer.ViewModels;

public class RegisterUserViewModel : ObservableObject
{
    private readonly UserController userController;
    private EmployeeController employeeController = new EmployeeController();

    private AcronymForPermissionLevel acronymForPermissionLevel = new AcronymForPermissionLevel();
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();


    public RegisterUserViewModel()
    {
        this.userController = new UserController();
        LoadEmployees();
    }

    private Employee employeeSelected ;
    public Employee EmployeeSelected
    {
        get { return employeeSelected; }
        set
        {
            employeeSelected = value;
            OnPropertyChanged(nameof(EmployeeSelected));
            if (employeeSelected != null)
            {
                Users = userController.GetUsers(employeeSelected.AgentNumber);
            }
            else
            {
                Users = Enumerable.Empty<User>(); 
            }
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


    private string newUserName = null!;
    public string NewUserName
    {
        get { return newUserName; }
        set
        {
            newUserName = value;
            OnPropertyChanged();
        }
    }

    private AuthorizationLevel authorizationLevelSelected;
    public AuthorizationLevel AuthorizationLevelSelected
    {
        get { return authorizationLevelSelected; }
        set
        {
            authorizationLevelSelected = value;
            OnPropertyChanged();
            if(EmployeeSelected != null) 
            {
                NewUserName = acronymForPermissionLevel.GenereateAcronym(EmployeeSelected.AgentNumber, AuthorizationLevelSelected);
            }
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
              && !string.IsNullOrWhiteSpace(PasswordInput)
              && AuthorizationLevelSelected != null
        );
    private void LoadEmployees()
    {
       
        var employees = employeeController.GetEmployees();
        foreach (var employee in employees)
        {
            Employees.Add(employee);
        }
    }

}
