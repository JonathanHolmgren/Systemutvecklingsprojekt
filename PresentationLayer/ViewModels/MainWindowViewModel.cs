﻿using System;
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
        private User user = null;
        public User User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged();
                }
            }
        }
        private Employee employee = null;
        public Employee Employee
        {
            get { return employee; }
            set
            {
                if (employee != value)
                {
                    employee = value;
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

        public ICommand ShowSubMenuCommand =>
            new RelayCommand(() => IsSubmenuOpen = !IsSubmenuOpen);
        public Action Close { get; set; }
        private ICommand exitCommand = null!;
        public ICommand ExitCommand =>
            exitCommand ??= exitCommand = new RelayCommand(() => App.Current.Shutdown());

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

        private void OnDragWindow(Window window)
        {
            window?.DragMove();
        }

        // Close App Command



        public MainWindowViewModel()
        {
            CurrentView = new SearchCustomerProfileViewModel(); // Startvy
            Mediator.Register("ChangeView", ChangeView);
            DragWindowCommand = new RelayCommand<Window>(OnDragWindow);
        }

        private void ChangeView(object viewModel)
        {
            CurrentView = viewModel;
        }

        private void MaxApp(object parameter)
        {
            if (parameter is Window window)
            {
                // Kontrollera om fönstret redan är maximerat
                if (window.WindowState == WindowState.Maximized)
                {
                    // Om ja, återställ till normalt läge
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    // Annars maximera fönstret
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
    }
}
