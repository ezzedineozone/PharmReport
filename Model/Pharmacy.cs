using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmReport.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Name_AR { get; set; }
        public string LicenseNumber { get; set; }
        public string? LicenseNumber_AR { get; set; }
        public Pharmacist HeadPharmacist { get; set; }
        public Pharmacist? HeadPharmacist_AR { get; set; }
        public List<Pharmacist> Pharmacists { get; set; }
        public List<Pharmacist> Pharmacists_AR { get; set; }

        public string? CNSSNumber { get; set; }
        public string? CNSSNumber_AR { get; set; }
        public string? Address { get; set; }
        public string? Address_AR { get; set; }
        public string? Telephone { get; set; }
        public string? Telephone_AR { get; set; }

        public Pharmacy(string Name, string LicenseNumber, Pharmacist headPharmacist)
            : this(Name, LicenseNumber, headPharmacist, null, null, null)
        {
        }

        public Pharmacy(string Name, string LicenseNumber, Pharmacist HeadPharmacist, string? CNSSNumber = null, string? Address = null, string? Telephone = null)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
            this.Name_AR = null;
            this.LicenseNumber = LicenseNumber ?? throw new ArgumentNullException(nameof(LicenseNumber));
            this.LicenseNumber_AR = null;
            this.CNSSNumber = CNSSNumber;
            this.CNSSNumber_AR = null;
            this.Address = Address;
            this.Address_AR = null;
            this.Telephone = Telephone;
            this.Telephone_AR = null;
            this.HeadPharmacist = HeadPharmacist ?? throw new ArgumentNullException(nameof(HeadPharmacist));
            this.HeadPharmacist_AR = null;
            Pharmacists = new List<Pharmacist>();
            Pharmacists_AR = new List<Pharmacist>();
            populateArabicFields();
        }

        public Pharmacy(string Name, string LicenseNumber, string HeadPharmacistName, string HeadPharmacistSurname, string? HeadPharmacistPhone = null, string? CNSSNumber = null, string? Address = null, string? Telephone = null)
            : this(Name, LicenseNumber, new Pharmacist(HeadPharmacistName ?? throw new ArgumentNullException(nameof(HeadPharmacistName)), HeadPharmacistSurname, string.Empty, HeadPharmacistPhone), CNSSNumber, Address, Telephone)
        {
        }

        private void populateArabicFields()
        {
            return;
        }
    }

}
