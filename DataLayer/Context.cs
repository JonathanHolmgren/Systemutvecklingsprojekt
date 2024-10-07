using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CommisionRate> ComissionRates { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<InsuredPerson> InsuredPersons { get; set; }
        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<InsuranceTypeAttribute> InsuranceTypeAttributes { get; set; }
        public DbSet<InsuranceSpec> InsuranceSpecs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PrivateCustomer> PrivateCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<PostalCodeCity> PostalCodeCities { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureInsurance(modelBuilder);
            ConfigureEmployeeRelations(modelBuilder);
            ConfigureCustomerRelations(modelBuilder);
            ConfigureInsuranceTypeAttributes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureInsurance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.User)
                .WithMany(u => u.Insurances)
                .HasForeignKey(i => i.UserName);

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.InsuredPerson)
                .WithMany(ip => ip.Insurances)
                .HasForeignKey(i => i.InsuredPersonID);

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.InsuranceType)
                .WithMany(it => it.Insurances)
                .HasForeignKey(i => i.InsuranceTypeID);

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Insurances)
            .HasForeignKey(i => i.CustomerID);
        }

        private void ConfigureEmployeeRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.PostalCodeCity)
                .WithMany(pc => pc.Employees)
            .HasForeignKey(i => i.PostalCode);
        }


        private void ConfigureCustomerRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.PostalCodeCity)
                .WithMany(pc => pc.Customers)
            .HasForeignKey(i => i.PostalCode);
        }

        private void ConfigureInsuranceTypeAttributes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsuranceTypeAttribute>()
                .HasOne(ita => ita.InsuranceType)
                .WithMany(it => it.InsuranceTypeAttributes)
                .HasForeignKey(ita => ita.InsuranceTypeID);

            modelBuilder.Entity<InsuranceSpec>()
                .HasOne(isp => isp.InsuranceTypeAttribute)
                .WithMany(ita => ita.InsuranceSpec)
                .HasForeignKey(isp => isp.InsuranceTypeAttributeID);
        }


        public void Reset()
        {
            #region Remove Tables
            using (SqlConnection conn = new SqlConnection())
            using (SqlCommand cmd = new SqlCommand("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'; EXEC sp_msforeachtable 'DROP TABLE ?'", conn))
            {
                conn.Open();
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Exception)
                    {
                        // throw;
                    }
                }
                conn.Close();
            }
            #endregion
        }

    }
}
