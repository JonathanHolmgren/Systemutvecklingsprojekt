﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(Context context)
            : base(context) { }

        public User? GetUserByID(int iD)
        {
            return Context.Set<User>().FirstOrDefault(user => user.UserID == iD);
        }

        public User? GetSpecificUser(string username)
        {
            return Context
                .Set<User>()
                .Include(e => e.Employee)
                .FirstOrDefault(u => u.UserName == username);
        }

        public IList<User> GetUsersByAgentNumber(string agentNumber)
        {
            return Context
                .Set<User>()
                .Where(user => user.Employee.AgentNumber == agentNumber)
                .ToList();
        }

        public bool DoesUserExist(string username)
        {
            var existingUser = Context.Set<User>().FirstOrDefault(u => u.UserName == username);

            return existingUser != null;
        }
    }
}
