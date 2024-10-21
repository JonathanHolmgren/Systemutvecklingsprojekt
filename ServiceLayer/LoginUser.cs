using System.Security.Cryptography;
using DataLayer;
using Models;

namespace ServiceLayer;

public class LoginUser
{
    UnitOfWork unitOfWork = new UnitOfWork();
    PasswordHasher passwordHasher = new PasswordHasher();

    public User ValidateUser(string username, string password)
    {
        User? user = unitOfWork.UserRepository.FirstOrDefault(u => u.UserName == username);

        if (user is null)
        {
            throw new Exception("Ogiltigt användarnamn eller lösenord");
        }

        bool verified = passwordHasher.Verify(password, user.Password);

        if (!verified)
        {
            throw new Exception("Ogiltigt användarnamn eller lösenord");
        }

        return user;
    }
}
