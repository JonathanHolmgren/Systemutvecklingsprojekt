using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int UserID {  get; set; }
        public string Password { get; set; }
        public Employee Employee { get; set; }
        public AuthorizationLevel AuthorizationLevel { get; set; }
        
    }
}
