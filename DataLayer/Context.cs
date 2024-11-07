using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace DataLayer
{
    public class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Commission> CommissionRates { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<InsuredPerson> InsuredPersons { get; set; }
        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<InsuranceTypeAttribute> InsuranceTypeAttributes { get; set; }
        public DbSet<InsuranceSpec> InsuranceSpecs { get; set; }
        public DbSet<PrivateCustomer> PrivateCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<ProspectNote> ProspectNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=sqlutb2-db.hb.se,56077;Database=suht2410;User Id=suht2410;Password=VOB279;TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEmployeeRelations(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

       
        private void ConfigureEmployeeRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.AgentNumber);

            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Commission)
                .WithMany(c => c.Employees)
                .HasForeignKey("CommissionId");

          
        }

        public void Reset()
        {
            #region Remove Tables
            using (SqlConnection conn = new SqlConnection(""))
            using (
                SqlCommand cmd = new SqlCommand(
                    "EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'; EXEC sp_msforeachtable 'DROP TABLE ?'",
                    conn
                )
            )
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
