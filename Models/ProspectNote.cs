using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProspectNote
    {
        public int ProspectNoteId { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }

        //Navigation properties
        public User? User { get; set; }
        public Customer? Customer { get; set; }

        public ProspectNote() { }
        public ProspectNote(string note, DateTime date, User user, Customer customer)            
        {
            Note = note;
            Date = date;
            User = user;
            Customer = customer;
        }

    }
}
