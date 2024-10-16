using DataLayer;
using Models;

namespace ServiceLayer;

public class UserController
{
    UnitOfWork unitOfWork = new UnitOfWork();

    public void CreateUser(string password)
    {
        PostalCodeCity postalCodeCity3 = new PostalCodeCity("50333", "Borås");

        Employee employee1 = new Employee(
            "3432",
            "19540314-3243",
            "Kalle",
            "Ben",
            "Vasagatan 12",
            postalCodeCity3,
            "Kalle.Ben@exempel.se",
            "Turist",
            "070-123 45 67"
        );

        PasswordHasher passwordHasher = new PasswordHasher();
        string hashPassoword = passwordHasher.Hash(password);

        User user = new User(hashPassoword, AuthorizationLevel.Admin, employee1);

        unitOfWork.UserRepository.Add(user);
        unitOfWork.SaveChanges();
    }

    // public List<User> GetAllUsers()
    // {
    //     //    unitOfWork.UserRepository.GetAll(u => u.Employee).ToString();
    // }
}
