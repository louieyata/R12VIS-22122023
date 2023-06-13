using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace R12VIS.Models
{
    public class DbContextR12: DbContext
    {
        public DbContextR12() : base("DbContextR12")
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<CityMunicipality> CityMunicipalities { get; set; }
        public DbSet<Barangay> Barangays { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<EthnicGroup> EthnicGroups { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Adverse> Adverses { get; set; }
        public DbSet<Deferral> Deferrals { get; set; }
        public DbSet<Dose> Dose { get; set; }
        public DbSet<PriorityGroup> PriorityGroups { get; set; }
        public DbSet<Vaccination>  Vaccinations { get; set; }





        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}