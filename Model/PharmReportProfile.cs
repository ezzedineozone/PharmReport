using PharmReport.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmReport.Models
{
    public class PharmReportProfile
    {
        private PharmReportDBContext PharmReportDBContext;
        private int pharmacyKey;
        private int pharmacistKey;

        public Pharmacy? Pharmacy = null;
        public Pharmacist? Pharmacist = null;
        public DateTime? lastLogin = null;

        public PharmReportProfile(string dataBaseName)
        {
            PharmReportDBContext = new PharmReportDBContext(dataBaseName);
            var lastProfile = PharmReportDBContext.PharmReportProfiles.OrderByDescending(p => p.lastLogin).FirstOrDefault();
            if(lastProfile != null)
            {
                this.pharmacyKey = lastProfile.pharmacyKey;
                this.pharmacistKey = lastProfile.pharmacistKey;
                this.lastLogin = lastProfile.lastLogin;
            }
            _populateProfile();
        }

        public PharmReportProfile(string dataBaseName, int pharmacyKey) : this(dataBaseName)
        {

            if(pharmacyKey == 0)
            {
                throw new ArgumentNullException("pharmacyKey", "pharmacyKey cannot be null");
            }
            this.pharmacyKey = pharmacyKey;

        }

        public PharmReportProfile(string dataBaseName, int pharmacyKey, int pharmacistKey) : this(dataBaseName, pharmacyKey)
        {

            if (pharmacistKey == 0)
            {
                throw new ArgumentNullException("pharmacistKey", "pharmacistKey cannot be null");
            }
            this.pharmacistKey = pharmacistKey;

        }



        private void _populateProfile()
        {
            if (pharmacyKey != 0)
            {
                Pharmacy = PharmReportDBContext.Pharmacies.Where(p => p.Id == pharmacyKey).FirstOrDefault();
            }
            if (pharmacistKey != 0)
            {
                Pharmacist = PharmReportDBContext.Pharmacists.Where(p => p.Id == pharmacistKey).FirstOrDefault();
            }
        }
    }
}
