using PharmReport.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WebUI;

namespace PharmReport.Models
{
    public class PharmReportProfile
    {
        public int Id { get; set; }


        public Pharmacy Pharmacy { get; set; }
        public Pharmacist Pharmacist { get; set; }
        public DateTime? lastLogin { get; set; } = null;


        public PharmReportProfile()
        {
            Pharmacy = new Pharmacy();
            Pharmacist = new Pharmacist();
        }   

        public PharmReportProfile(Pharmacy pharmacy)
        {
            this.Pharmacy = pharmacy ?? throw new ArgumentNullException("pharmacy", "pharmacy cannot be null");
            this.Pharmacist = new Pharmacist();
        }

        public PharmReportProfile(Pharmacy pharmacy, Pharmacist pharmacist)
        {

            this.Pharmacist = pharmacist ?? throw new ArgumentNullException("pharmacist", "pharmacist cannot be null");
            this.Pharmacy = pharmacy ?? throw new ArgumentNullException("pharmacy", "pharmacy cannot be null");

        }
    }
}
