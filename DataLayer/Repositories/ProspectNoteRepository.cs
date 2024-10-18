using Microsoft.EntityFrameworkCore;
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

        public IList<ProspectNote> GetProspectNotes()
        {
            return Context.Set<ProspectNote>()

                .Include(c => c.User)
                    .ThenInclude(b => b.Employee)
                        .ToList(); // Konvertera till en lista
        }
        public ProspectNote GetSpecificCompanyCustomer(int prospectNoteId)
        {
            return Context.Set<ProspectNote>()
                  .Include(c => c.User)
                    .ThenInclude(b => b.Employee)
                        .FirstOrDefault(); // Konvertera till ett objekt
        }

    }
}
