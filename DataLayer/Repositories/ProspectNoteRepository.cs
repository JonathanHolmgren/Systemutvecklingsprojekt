﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class ProspectNoteRepository : Repository<ProspectNote>
    {
        public ProspectNoteRepository(Context context) : base(context) { }

    }
}
