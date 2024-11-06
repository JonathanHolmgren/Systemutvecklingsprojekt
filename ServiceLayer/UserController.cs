using DataLayer;
using Models;
using ServiceLayer.services;

namespace ServiceLayer;

public class UserController
{
    UnitOfWork unitOfWork = new UnitOfWork();
    AcronymForPermissionLevel acronymForPermissionLevel = new AcronymForPermissionLevel();

    public Employee GetEmployee(string agentNumber)
    {
        return unitOfWork.EmployeeRepository.FirstOrDefault(e => e.AgentNumber == agentNumber);
    }

    public List<User> GetUsers(string agentNumber)
    {
        return unitOfWork.UserRepository.GetUsersByAgentNumber(agentNumber);
    }

    public User GetUser(int id)
    {
        return unitOfWork.UserRepository.GetUserByID(id);
    }

    public void CreateUser(
        string password,
        Employee employee,
        AuthorizationLevel authorizationLevel
    )
    {
        PasswordHasher passwordHasher = new PasswordHasher();
        string hashPassoword = passwordHasher.Hash(password);
        string userName = acronymForPermissionLevel.GenereateAcronym(
            employee.AgentNumber,
            authorizationLevel
        );

        User user = new User(hashPassoword, authorizationLevel, employee, userName);

        unitOfWork.UserRepository.Add(user);
        unitOfWork.SaveChanges();
    }

    public void RemoveUserById(int userSelectedUserId)
    {
        User userToRemove = unitOfWork.UserRepository.FirstOrDefault(x =>
            x.UserID == userSelectedUserId
        );

        if (userToRemove != null)
        {
            unitOfWork.UserRepository.Remove(userToRemove);
            unitOfWork.SaveChanges();
        }
    }
}
