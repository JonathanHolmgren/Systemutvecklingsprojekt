using DataLayer;
using Models;

namespace ServiceLayer;

public class UserController
{
 UnitOfWorkIM unitOfWork = new UnitOfWorkIM();


 public void CreateUser(Employee employee, string password,AuthorizationLevel authorizationLevel)
 {
    
    PasswordHasher passwordHasher = new PasswordHasher();
    string hashPassoword = passwordHasher.Hash(password);

    User user = new User(employee, hashPassoword, authorizationLevel);
     
     unitOfWork.UserRepository.Add(user);
     unitOfWork.SaveChanges();

 }

 public List<User> GetAllUsers()
 {
     unitOfWork.UserRepository.GetAll(u => u.Employee).ToString();


 }
 
 
 
 
 
 
}


