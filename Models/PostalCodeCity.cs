using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PostalCodeCity
    {
        public string PostalCode { get; set; }
        public string City { get; set; }
        public PostalCodeCity()
        {
                
        }
        public PostalCodeCity(string postalCode, string city)
        {
            this.PostalCode = postalCode;
            this.City = city;
        }

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
