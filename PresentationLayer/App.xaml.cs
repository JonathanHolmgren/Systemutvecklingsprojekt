
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
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Skapa MainWindow från DI-container och visa det
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            
        }

        private void ConfigureServices(ServiceCollection services)
        {
        
            // Registrera MainWindow
            services.AddTransient<MainWindow>();
        }
    }
}
