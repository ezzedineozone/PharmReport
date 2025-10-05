using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmReport.Models;
using System.IO;
namespace PharmReport.EF
{
    public class PharmReportDBContext : DbContext
    {
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<PharmReportProfile> PharmReportProfiles { get; set; }

        private string dbName = "PharmReport.db";


        public PharmReportDBContext(string databaseName = "PharmReport")
        {
            var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            Directory.CreateDirectory(baseDir);
            this.dbName = Path.Combine(baseDir, databaseName + ".db");
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={dbName}");
        }

    }
}
