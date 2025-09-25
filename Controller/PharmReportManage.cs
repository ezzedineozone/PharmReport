using PharmReport.EF;
using PharmReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmReport.Controller
{
    internal class PharmReportManage
    {
        private PharmReportDBContext _pharmReportDBContext;

        public PharmReportManage(PharmReportDBContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _pharmReportDBContext = context;
        }

        public void AddPharmacy(Pharmacy pharmacy)
        {
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            _pharmReportDBContext.Pharmacies.Add(pharmacy);
            _pharmReportDBContext.SaveChanges();
        }

        public void RemovePharmacy(Pharmacy pharmacy)
        {
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            _pharmReportDBContext.Pharmacies.Remove(pharmacy);
            _pharmReportDBContext.SaveChanges();
        }

        public void RemovePharmacy(int pharmacyId)
        {
            Pharmacy? pharmacy = _pharmReportDBContext.Pharmacies.Where(p => p.Id == pharmacyId).FirstOrDefault();
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            RemovePharmacy(pharmacy);
        }

        public void AddPharmacist(Pharmacist pharmacist, Pharmacy pharmacy)
        {
            if (pharmacist == null)
            {
                throw new ArgumentNullException(nameof(pharmacist));
            }
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            pharmacy.Pharmacists.Add(pharmacist);
            _pharmReportDBContext.Pharmacists.Add(pharmacist);
            _pharmReportDBContext.SaveChanges();
        }

        public void RemovePharmacistFromPharmacy(Pharmacist pharmacist, Pharmacy pharmacy)
        {
            if (pharmacist == null)
            {
                throw new ArgumentNullException(nameof(pharmacist));
            }
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            pharmacy.Pharmacists.Remove(pharmacist);
            _pharmReportDBContext.Pharmacists.Remove(pharmacist);
            _pharmReportDBContext.SaveChanges();
        }

        public void RemovePharmacistFromPharmacy(int pharmacistId, int pharmacyId)
        {
            Pharmacist? pharmacist = _pharmReportDBContext.Pharmacists.Where(p => p.Id == pharmacistId).FirstOrDefault();
            if (pharmacist == null)
            {
                throw new ArgumentNullException(nameof(pharmacist));
            }
            Pharmacy? pharmacy = _pharmReportDBContext.Pharmacies.Where(p => p.Id == pharmacyId).FirstOrDefault();
            if (pharmacy == null)
            {
                throw new ArgumentNullException(nameof(pharmacy));
            }
            RemovePharmacistFromPharmacy(pharmacist, pharmacy);
        }

        public void RemovePharmacist(Pharmacist pharmacist)
        {
            if (pharmacist == null)
            {
                throw new ArgumentNullException(nameof(pharmacist));
            }
            _pharmReportDBContext.Pharmacists.Remove(pharmacist);
            _pharmReportDBContext.SaveChanges();
        }

        public void RemovePharmacist(int pharmacistId)
        {
            Pharmacist? pharmacist = _pharmReportDBContext.Pharmacists.Where(p => p.Id == pharmacistId).FirstOrDefault();
            if (pharmacist == null)
            {
                throw new ArgumentNullException(nameof(pharmacist));
            }
            RemovePharmacist(pharmacist);
        }
    }
}
