using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmReport.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }


        public Medicine(string Name, string Price)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
            this.Price = double.TryParse(Price, out double price) ? price : throw new ArgumentException("Price must be a valid number", nameof(Price));
        }
    }
}
