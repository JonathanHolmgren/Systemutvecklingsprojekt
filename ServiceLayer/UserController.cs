using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataLayer.Repositories;

namespace ServiceLayer
{
    public class UserController
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public AuthorizationLevel GetAuthorizationLevelForUser(string username, string password)
        {
            var user = unitOfWork.UserRepository.GetUserByCredentials(username, password);
            if (user != null)
            {
                return user.AuthorizationLevel;
            }

            throw new Exception("Invalid login credentials.");
        }

    }
}
