using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using PresentationLayer.Command;
using PresentationLayer.Models;
using PresentationLayer.Services;
using PresentationLayer.Views;
using ServiceLayer;

namespace PresentationLayer.ViewModels
{
    public class MainWindowViewModel : ObservableObject, ICloseWindows
    {
        #region Initation of objects
        private LoggedInUser _user;
        public LoggedInUser User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }
        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                if (_employee != value)
                {
                    _employee = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isSubmenuOpen;

        public bool IsSubmenuOpen
        {
            get => _isSubmenuOpen;
            set
            {
                _isSubmenuOpen = value;
                OnPropertyChanged();
            }
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;

                    OnPropertyChanged();
                }
            }
        }
        #endregion
        #region Commands
        public ICommand ShowSubMenuCommand =>
            new RelayCommand(() => IsSubmenuOpen = !IsSubmenuOpen);
        public Action Close { get; set; }
        private ICommand exitCommand = null!;
        public ICommand ExitCommand =>
            exitCommand ??= exitCommand = new RelayCommand(() => App.Current.Shutdown());
        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand ??= new RelayCommand(Logout);

        private ICommand _changeViewCommand = null!;

        //Alternating usercontrolls via ViewModels
        public ICommand ChangeViewCommand =>
            _changeViewCommand ??= new RelayCommand<object>(parameter =>
            {
                switch (parameter.ToString())
                {
                    case "RegisterCompanyCustomerViewModel":
                        Mediator.Notify("ChangeView", new RegisterCompanyCustomerViewModel());
                        break;
                    case "RegisterPrivateCustomerViewModel":
                        Mediator.Notify("ChangeView", new RegisterPrivateCustomerViewModel());
                        break;
                    case "ShowProspectsViewModel":
                        Mediator.Notify("ChangeView", new ShowProspectsViewModel());
                        break;
                    case "ExportBillingInformationViewModel":
                        Mediator.Notify("ChangeView", new ExportBillingInformationViewModel());
                        break;
                    case "CalculateComissionViewModel":
                        Mediator.Notify("ChangeView", new CalculateComissionViewModel());
                        break;
                    case "SalesStatisticsViewModel":
                        Mediator.Notify("ChangeView", new SalesStatisticsViewModel());
                        break;
                    case "RegisterUserViewModel":
                        Mediator.Notify("ChangeView", new RegisterUserViewModel());
                        break;
                    case "SearchCustomerProfileViewModel":
                        Mediator.Notify("ChangeView", new SearchCustomerProfileViewModel(_user));
                        break;
                }
            });
        private IWindowService windowService { get; set; }
        private ICommand _maxCommand;
        public ICommand MaxAppCommand
        {
            get
            {
                if (_maxCommand == null)
                {
                    _maxCommand = new RelayCommand<object>(execute => MaxApp(execute));
                }
                return _maxCommand;
            }
        }
        private ICommand _minCommand;
        public ICommand MinAppCommand
        {
            get
            {
                if (_minCommand == null)
                {
                    _minCommand = new RelayCommand<object>(execute => MinApp(execute));
                }
                return _minCommand;
            }
        }

        public ICommand DragWindowCommand { get; }
        #endregion
        #region Constructors
        public MainWindowViewModel()
        {
            CurrentView = new WelcomePageView();
        }

        public MainWindowViewModel(LoggedInUser loggedInUser)
        {
            _user = loggedInUser;
            CurrentView = new WelcomePageView();
            Mediator.Register("ChangeView", ChangeView);
            windowService = new WindowService();
            DragWindowCommand = new RelayCommand<Window>(OnDragWindow);
        }
        #endregion
        #region Methods
        private void OnDragWindow(Window window)
        {
            window?.DragMove();
        }

        private void ChangeView(object viewModel)
        {
            CurrentView = viewModel;
        }

        private void Logout()
        {
            var loginWindow = new LoginUserWindow { DataContext = new LoginViewModel() };
            loginWindow.Show();

            Application.Current.MainWindow = loginWindow;

            Close?.Invoke();
        }

        private void MaxApp(object parameter)
        {
            if (parameter is Window window)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }
            }
        }

        private void MinApp(object parameter)
        {
            if (parameter is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
