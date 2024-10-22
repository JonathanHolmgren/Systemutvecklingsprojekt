using Models;
using PresentationLayer.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class LogInViewModel : ObservableObject
    {
        UserController userController = new UserController();

        //var authorizationLevel = userController.GetAuthorizationLevelForUser(username, password);
        //OpenViewBasedOnAuthorizationLevel(authorizationLevel);
        private void OpenViewBasedOnAuthorizationLevel(AuthorizationLevel authorizationLevel)
        {
            switch (authorizationLevel)
            {
                case AuthorizationLevel.Admin:
                    // Öppna Admin-vyn
                    break;

                case AuthorizationLevel.EconomyAssistant:
                    // Öppna Economy Assistant-vyn
                    break;

                case AuthorizationLevel.SalesManager:
                    // Öppna Sales Manager-vyn
                    break;

                case AuthorizationLevel.SalesPerson:
                    // Öppna Sales Person-vyn
                    break;

                case AuthorizationLevel.CEO:
                    // Öppna CEO-vyn
                    break;

                case AuthorizationLevel.SalesAssistant:
                    // Öppna Sales Assistant-vyn
                    break;

                default:
                    break;
            }
        }
    }
}
