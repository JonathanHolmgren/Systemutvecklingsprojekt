using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

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
            //// Ladda konfiguration från appsettings.json
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .Build();

            //// Registrera IConfiguration för DI
            //services.AddSingleton<IConfiguration>(configuration);

            //// Registrera DbContext
            //services.AddDbContext<Context>(options =>
            //{
            //    var connectionString = configuration.GetConnectionString("toppforsakringar");
            //    options.UseSqlServer(connectionString);
            //});

            //// Registrera MyService så den kan få Context via DI
            //services.AddTransient<MyService>();

            // Registrera MainWindow
            services.AddTransient<MainWindow>();
        }
    }

}
