using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Password { get; set; }
        public AuthorizationLevel AuthorizationLevel { get; set; }

        // Navigation property
        public Employee Employee { get; set; }
        public ICollection<Insurance> Insurances { get; set; }

        public User() { }

        public User(string password, AuthorizationLevel authorizationLevel, Employee employee)
        {
            Password = password;
            AuthorizationLevel = authorizationLevel;
            Employee = employee;
        }
    }
}
