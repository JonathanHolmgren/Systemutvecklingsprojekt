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
    private readonly UserController _userController;
    private readonly EmployeeController _employeeController = new EmployeeController();

    private readonly AcronymForPermissionLevel _acronymForPermissionLevel =
        new AcronymForPermissionLevel();
    public ObservableCollection<Employee> Employees { get; set; } =
        new ObservableCollection<Employee>();
    private string _filterText = string.Empty;
    public string FilterText
    {
        get { return _filterText; }
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                ApplyFilter(_filterText);
                OnPropertyChanged(nameof(FilterText));
            }
        }
    }

    private int _menuPage = 0;
    public int MenuPage
    {
        get => _menuPage;
        set
        {
            _menuPage = value;
            OnPropertyChanged(nameof(MenuPage));
        }
    }
    ObservableCollection<Employee> _filteredEmployees;
    public ObservableCollection<Employee> FilteredEmployees
    {
        get { return _filteredEmployees; }
        set
        {
            _filteredEmployees = value;
            OnPropertyChanged();
        }
    }
    private Employee _employeeSelected;
    public Employee EmployeeSelected
    {
        get { return _employeeSelected; }
        set
        {
            _employeeSelected = value;

            if (_employeeSelected != null)
            {
                Users = new ObservableCollection<User>(
                    _userController.GetUsers(_employeeSelected.AgentNumber)
                );
            }
            else
            {
                Users = new ObservableCollection<User>();
            }
            OnPropertyChanged(nameof(EmployeeSelected));
        }
    }
    private ObservableCollection<User> _users = null!;
    public ObservableCollection<User> Users
    {
        get { return _users; }
        set
        {
            _users = value;
            OnPropertyChanged(nameof(Users));
        }
    }

    private User _userSelected = null!;
    public User UserSelected
    {
        get { return _userSelected; }
        set
        {
            _userSelected = value;
            OnPropertyChanged();
        }
    }

    private string _employeeInput = null!;
    public string EmployeeInput
    {
        get { return _employeeInput; }
        set
        {
            _employeeInput = value;
            OnPropertyChanged();
        }
    }

    private string _newUserName = null!;
    public string NewUserName
    {
        get { return _newUserName; }
        set
        {
            _newUserName = value;
            OnPropertyChanged();
        }
    }

    private AuthorizationLevel _authorizationLevelSelected;
    public AuthorizationLevel AuthorizationLevelSelected
    {
        get { return _authorizationLevelSelected; }
        set
        {
            _authorizationLevelSelected = value;
            OnPropertyChanged();
            if (EmployeeSelected != null)
            {
                NewUserName = _acronymForPermissionLevel.GenereateAcronym(
                    EmployeeSelected.AgentNumber,
                    AuthorizationLevelSelected
                );
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

    private ICommand _addUserCommand = null!;
    public ICommand AddUserCommand =>
        _addUserCommand ??= new RelayCommand(
            () =>
            {
                CreateUser();
            },
            () => _newUserName != null
        );

    #endregion
    #region Constructor
    public RegisterUserViewModel()
    {
        this._userController = new UserController();
        Employees = new ObservableCollection<Employee>(_employeeController.GetEmployees());
        ApplyFilter(FilterText);
        NextPageCommand = new RelayCommand<object>(execute => IncreaseMenuPage());
        BackPageCommand = new RelayCommand<object>(execute => DecreaseMenuPage());
        RemoveProfilePageCommand = new RelayCommand<object>(execute => RemoveProfileMenuPage());
        AddProfilePageCommand = new RelayCommand<object>(execute => AddProfileMenuPage());
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
        if (Users.Count() == 0)
        {
            ErrorLabel = "Denna säljare har ingen profil";
        }
        else if (EmployeeSelected != null)
        {
            MenuPage = 2;
            ErrorLabel = string.Empty;
        }
    }

    private void RemoveUser()
    {
        if (UserSelected != null)
        {
            _userController.RemoveUserById(UserSelected.UserID);
            Users = new ObservableCollection<User>(
                _userController.GetUsers(_employeeSelected.AgentNumber)
            );
            MessageBox.Show("Profil borttagen");
        }
        else
        {
            ErrorLabel = "Välj en profil";
        }
    }

    private void CreateUser()
    {
        if (PasswordInput != passwordInputControll)
        {
            ErrorLabel = "Lösenorden måste matcha";
        }
        else if (string.IsNullOrWhiteSpace(PasswordInput))
        {
            ErrorLabel = "Var vänlig fyll i lösenord";
        }
        else if (AuthorizationLevelSelected == null)
        {
            ErrorLabel = "Var vänlig fyll i roll.";
        }
        else
        {
            try
            {
                _userController.CreateUser(
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
        return employee.FirstName.Contains(filterText, StringComparison.OrdinalIgnoreCase)
            || employee.LastName.Contains(filterText, StringComparison.OrdinalIgnoreCase)
            || employee.AgentNumber.Contains(filterText, StringComparison.OrdinalIgnoreCase);
    }
    #endregion
}
