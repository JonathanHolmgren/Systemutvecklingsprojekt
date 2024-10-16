

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

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<InMemoryDbContext>(options =>
             //   options.UseInMemoryDatabase("InMemoryDb"));

        
            services.AddScoped<RegisterUserViewModel>();
            services.AddScoped<MainWindow>();
        }
    }
}
