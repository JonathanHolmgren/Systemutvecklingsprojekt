using System.Security.Cryptography;
using DataLayer;
using Models;

namespace ServiceLayer;

public class LoginUser
{
    UnitOfWork unitOfWork = new UnitOfWork();
    PasswordHasher passwordHasher = new PasswordHasher();

    public LoggedInUser ValidateUser(string username, string password)
    {
        User user = unitOfWork.UserRepository.GetSpecificUser(username);

        if (user is null)
        {
            throw new Exception("Ogiltigt användarnamn eller lösenord");
        }

        bool verified = passwordHasher.Verify(password, user.Password);

        if (!verified)
        {
            throw new Exception("Ogiltigt användarnamn eller lösenord");
        }
        LoggedInUser loggedInUser = new LoggedInUser();
        loggedInUser.UserID = user.UserID;
        loggedInUser.FirstName = user.Employee.FirstName;
        loggedInUser.LastName = user.Employee.LastName;
        loggedInUser.AgentNumber = user.Employee.AgentNumber;
        loggedInUser.AuthorizationLevel = user.AuthorizationLevel;
        loggedInUser.UserName = user.UserName;
        loggedInUser.Email = user.Employee.Email;

        return loggedInUser;
    }
}
