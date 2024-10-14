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

        public PostalCodeCity(string postalCode, string city)
        {
            this.PostalCode = postalCode;
            this.City = city;
        }

    }
}
