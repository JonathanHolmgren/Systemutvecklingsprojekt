

using System.Windows;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.ViewModels;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
    
            var services = new ServiceCollection();
            ConfigureServices(services);
    
            var serviceProvider = services.BuildServiceProvider();


            // Starta huvudfönstret
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        //private void ConfigureServices(ServiceCollection services)
        //{
        //    // Ladda konfiguration från appsettings.json
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .Build();

        //    // Registrera IConfiguration för DI
        //    services.AddSingleton<IConfiguration>(configuration);

        //    // Registrera DbContext
        //    services.AddDbContext<Context>(options =>
        //    {
        //        var connectionString = configuration.GetConnectionString("toppforsakringar");
        //        options.UseSqlServer(connectionString);
        //    });

        //    // Registrera MyService så den kan få Context via DI
        //    services.AddTransient<MyService>();

        //    // Registrera MainWindow
        //    services.AddTransient<MainWindow>();
        //}
    }
}
