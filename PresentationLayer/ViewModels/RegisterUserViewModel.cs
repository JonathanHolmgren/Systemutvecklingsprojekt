using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
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
    #region Initation of objects
    private readonly UserController userController;
    private EmployeeController employeeController = new EmployeeController();

    private AcronymForPermissionLevel acronymForPermissionLevel = new AcronymForPermissionLevel();
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    private string filterText = string.Empty;
    public string FilterText
    {
        get { return filterText; }
        set
        {
            if (filterText != value)
            {
                filterText = value;
                ApplyFilter(filterText);
                OnPropertyChanged(nameof(FilterText));
            }
        }
    }

    private int menuPage = 0;
    public int MenuPage
    {
        get => menuPage;
        set
        {
            menuPage = value;
            OnPropertyChanged(nameof(MenuPage));

        }
    }
    ObservableCollection<Employee> filteredEmployees;
    public ObservableCollection<Employee> FilteredEmployees
    {
        get { return filteredEmployees; }
        set
        {
            filteredEmployees = value;
            OnPropertyChanged();

        }
    }
    private Employee employeeSelected;
    public Employee EmployeeSelected
    {
        get { return employeeSelected; }
        set
        {
            employeeSelected = value;

            if (employeeSelected != null)
            {
                Users = new ObservableCollection<User>(userController.GetUsers(employeeSelected.AgentNumber));
            }
            else
            {
                Users = new ObservableCollection<User>();
            }
            OnPropertyChanged(nameof(EmployeeSelected));
        }
    }
    private ObservableCollection<User> users = null!;
    public ObservableCollection<User> Users
    {
        get { return users; }
        set
        {
            users = value;
            OnPropertyChanged(nameof(Users));
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
            if (EmployeeSelected != null)
            {
                NewUserName = acronymForPermissionLevel.GenereateAcronym(EmployeeSelected.AgentNumber, AuthorizationLevelSelected);
            }
        }
    }
    private string errorLabel = string.Empty;
    public string ErrorLabel
    {
        get => errorLabel;
        set
        {
            errorLabel = value;
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
            ErrorLabel = string.Empty;
        }
    }
    private string passwordInputControll = null!;
    public string PasswordInputControll
    {
        get { return passwordInputControll; }
        set
        {
            passwordInputControll = value;
            OnPropertyChanged();
            ErrorLabel = string.Empty;
        }
    }

    public ICommand NextPageCommand { get; private set; }
    public ICommand BackPageCommand { get; private set; }
    public ICommand RemoveProfilePageCommand { get; private set; }
    public ICommand AddProfilePageCommand { get; private set; }
    public ICommand BackMainPageCommand { get; private set; }
    public ICommand ChoíceMenuPageCommand { get; private set; }
    public ICommand RemoveUserCommand { get; set; }
    public ICommand AddUserCommand { get; set; }
    #endregion
    #region Constructor
    public RegisterUserViewModel()
    {
        this.userController = new UserController();
       Employees = new ObservableCollection<Employee>(employeeController.GetEmployees());
        ApplyFilter(FilterText);
        NextPageCommand = new RelayCommand<object>(execute => IncreaseMenuPage());
        BackPageCommand = new RelayCommand<object>(execute => DecreaseMenuPage());
        AddUserCommand = new RelayCommand<object>(execute => CreateUser());
        RemoveProfilePageCommand = new RelayCommand<object>(execute => RemoveProfileMenuPage());
        AddProfilePageCommand = new RelayCommand<object>(execute=> AddProfileMenuPage());
        BackMainPageCommand = new RelayCommand<object>(execute => BackMainPage());
        ChoíceMenuPageCommand = new RelayCommand<object>(execute => ChoiceMenuPage());
        RemoveUserCommand = new RelayCommand<object>(execute => RemoveUser());
    
    }
    #endregion
    #region Methods
    private void DecreaseMenuPage()
    {
        EmployeeSelected = null;
        ErrorLabel = string.Empty;
        MenuPage--;
    }
    private void ChoiceMenuPage()
    {

        UserSelected = null;
        NewUserName = string.Empty;
        PasswordInput = string.Empty;
        PasswordInputControll = string.Empty;
        ErrorLabel = string.Empty;
        MenuPage = 1;
    }
    private void BackMainPage()
    {
        EmployeeSelected = null;
        UserSelected = null;
        NewUserName = string.Empty;
        PasswordInput = string.Empty;
        PasswordInputControll = string.Empty;
        ErrorLabel = string.Empty;
        MenuPage = 0;

    }
    private void AddProfileMenuPage()
    {
        MenuPage = 3;
        ErrorLabel = string.Empty;
    }
    private void IncreaseMenuPage()
    {
        if (EmployeeSelected == null)
        {
            ErrorLabel = "Var vänlig och välj en säljare innan du går vidare";
        }
        else if (EmployeeSelected != null)
        {
            MenuPage++;
            ErrorLabel = string.Empty;
        }
    }

    private void RemoveProfileMenuPage()
    {
        if (Users.Count()==0)
        {
            ErrorLabel = "Denna säljare har ingen profil";
        }
        else if (EmployeeSelected != null)
        {
            MenuPage=2;
            ErrorLabel = string.Empty;
        }
    }

   
       
       
    private void RemoveUser()
    {
        if (UserSelected != null)
        {
            userController.RemoveUserById(UserSelected.UserID);
            Users = new ObservableCollection<User>(userController.GetUsers(employeeSelected.AgentNumber));
            MessageBox.Show("Profil borttagen");
        }
        else
        {
            ErrorLabel = "Välj en profil";
            
        }
    }

    private void CreateUser()
    {
        if (PasswordInput!=passwordInputControll)
        {
            ErrorLabel = "Lösenorden måste matcha";
        }
        else if (string.IsNullOrWhiteSpace(PasswordInput))
        {
            ErrorLabel="Var vänlig fyll i lösenord";
        }
      
        else if (AuthorizationLevelSelected==null)
        {
            ErrorLabel = "Var vänlig fyll i roll.";
        }
        else
        {
            try
            {
                userController.CreateUser(
                    PasswordInput,
                    EmployeeSelected,
                    AuthorizationLevelSelected
                );
                MessageBox.Show(
                    $"Ny profil skapad för {EmployeeSelected.FirstName} {EmployeeSelected.LastName} med användarnamn {NewUserName}"
                );
                BackMainPage();
            }
            catch (Exception ex)
            {
                ErrorLabel = ex.Message;
            }
        }

    }
    private void ApplyFilter(string filterText)
    {
        if (string.IsNullOrWhiteSpace(filterText))
        {
            FilteredEmployees = new ObservableCollection<Employee>(Employees);
        }
        else
        {
            FilterEmployees(filterText);
        }

    }
    private void FilterEmployees(string filterText) //Applying the filter text to company customers
    {
        FilteredEmployees.Clear();

        foreach (Employee employee in Employees)
        {
            if (IsEmployeeMatch(employee, filterText))
            {
                FilteredEmployees.Add(employee);
            }
        }
    }
    private bool IsEmployeeMatch(Employee employee, string filterText)
    {
        return employee.FirstName.Contains(
                filterText,
                StringComparison.OrdinalIgnoreCase
            )
            || employee.LastName.Contains(
                filterText,
                StringComparison.OrdinalIgnoreCase
            ) || employee.AgentNumber.Contains(filterText,
            StringComparison.OrdinalIgnoreCase);
    }
    #endregion
}
