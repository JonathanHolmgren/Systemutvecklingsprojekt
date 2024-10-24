using Microsoft.EntityFrameworkCore;
using Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
 
        public UserRepository(Context context) : base(context) { }

        public User GetUserByID(int iD)
        {
            return Context.Set<User>().FirstOrDefault(user => user.UserID == iD);
 
        public UserRepository(Context context)
            : base(context) { }

        public List<User> GetUsersByAgentNumber(string agentNumber)
        {
            return Context
                .Set<User>()
                .Where(user => user.Employee.AgentNumber == agentNumber)
        }
    }
}
