
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PresentationLayer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Skapa och visa huvudfönstret


            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
