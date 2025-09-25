using System;

namespace PharmReport.Models
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Name_AR { get; set; }
        public string OrderRegistrationNumber { get; set; }
        public string? OrderRegistrationNumber_AR { get; set; }
        public string? Surname { get; set; }
        public string? Surname_AR { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumber_AR { get; set; }

        public Pharmacist(string Name, string OrderRegistrationNumber)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
            this.Name_AR = null;
            this.OrderRegistrationNumber = string.IsNullOrWhiteSpace(OrderRegistrationNumber) ? throw new ArgumentException("Order registration number is required", nameof(OrderRegistrationNumber)) : OrderRegistrationNumber;
            this.OrderRegistrationNumber_AR = null;
        }

        public Pharmacist(string Name, string Surname, string OrderRegistrationNumber, string? PhoneNumber = null)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
            this.Name_AR = null;
            this.Surname = Surname;
            this.Surname_AR = null;
            this.PhoneNumber = PhoneNumber;
            this.PhoneNumber_AR = null;
            this.OrderRegistrationNumber = string.IsNullOrWhiteSpace(OrderRegistrationNumber) ? throw new ArgumentException("Order registration number is required", nameof(OrderRegistrationNumber)) : OrderRegistrationNumber;
            this.OrderRegistrationNumber_AR = null;
        }
    }
}
