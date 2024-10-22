﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(Context context) : base(context) { }

        public User GetUser(int iD)
        {
            return Context.Set<User>().AsNoTracking().FirstOrDefault(user => user.UserID == iD);
        }
    }
}
