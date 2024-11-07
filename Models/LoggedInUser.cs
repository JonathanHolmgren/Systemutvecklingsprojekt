using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LoggedInUser
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string AgentNumber { get; set; }
        public string Email { get; set; }
        public AuthorizationLevel AuthorizationLevel { get; set; }
    }
}
