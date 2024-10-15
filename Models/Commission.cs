using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Commission
    {
        public int CommissionId { get; set; }
        public double CommisionRate { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        // public ICollection<Customer> Customers { get; set; }

        public Commission(double commisionRate)
        {
            CommisionRate = commisionRate;
        }
    }
}
