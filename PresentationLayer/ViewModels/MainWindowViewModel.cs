using Microsoft.EntityFrameworkCore.ChangeTracking;
using PresentationLayer.Command;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using PresentationLayer.Views;
using System.Diagnostics;

namespace PresentationLayer.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {

        // Close App

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView)); // Viktigt för att uppdatera UI
                }
            }
        }


        // Close App Command
        private ICommand _closeCommand;

        public ICommand CloseAppCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand<object>(execute => CloseApp(execute));
                }
                return _closeCommand;
            }
        }

        public ICommand ChangeViewCommand { get; }
        public MainWindowViewModel()
        {
            
            CurrentView = new ExportBillingInformationView(); // Startvy 
            ChangeViewCommand = new RelayCommand<Type>(ChangeViewByType);

        }
        // Maximize App
        //public void MaxApp(object obj)
        //{
        //    MainWindow win = obj as MainWindow;

        //    if (win.WindowState == WindowState.Normal)
        //    {
        //        win.WindowState = WindowState.Maximized;
        //    }
        //    else if (win.WindowState == WindowState.Maximized)
        //    {
        //        win.WindowState = WindowState.Normal;
        //    }
        //}
        private void ChangeViewByType(Type viewType)
        {

            if (viewType != null)
            {
                CurrentView = (UserControl)Activator.CreateInstance(viewType);
                Debug.WriteLine($"Nu kör vi: {viewType.Name}");
            }
        }
        public void ChangeView(UserControl newView)
        {
            CurrentView = newView;
        }


        // Maximize App Command
        //private ICommand _maxCommand;
        //public ICommand MaxAppCommand
        //{
        //    get
        //    {
        //        if (_maxCommand == null)
        //        {
        //            _maxCommand = new RelayCommand<object>(execute => MaxApp(execute));
        //        }
        //        return _maxCommand;
        //    }
        //}


        public void CloseApp(object obj)
        {
            MainWindow win = obj as MainWindow;
            win.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

       

       
}
