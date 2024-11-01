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

        //public DbSet<Customer> Customers { get; set; }
        public DbSet<PrivateCustomer> PrivateCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<ProspectNote> ProspectNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=sqlutb2-db.hb.se,56077;Database=suht2410;User Id=suht2410;Password=VOB279;TrustServerCertificate=True;"
            //);

            // optionsBuilder.UseSqlServer(
            //
            //   @"Server=sqlutb2-db.hb.se,56077;Database=suht2410;User Id=suht2410;Password=VOB279;TrustServerCertificate=True;");

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ToppFörsäkringar;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ConfigureInsurance(modelBuilder);
            ConfigureEmployeeRelations(modelBuilder);
            //ConfigureCustomerRelations(modelBuilder);




            //ConfigureInsuranceTypeAttributes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        // private void ConfigureInsurance(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Insurance>()
        //         .HasOne(i => i.User) // An Insurance has one User
        //         .WithMany(u => u.Insurances) // A User has many Insurances
        //         .HasForeignKey(i => i.); // Foreign key is UserName
        //
        //     modelBuilder.Entity<Insurance>()
        //         .HasOne(i => i.InsuredPerson)
        //         .WithMany(ip => ip.Insurances)
        //         .HasForeignKey(i => i.InsuredPersonID);
        //
        //     modelBuilder.Entity<Insurance>()
        //         .HasOne(i => i.InsuranceType)
        //         .WithMany(it => it.Insurances)
        //         .HasForeignKey(i => i.InsuranceTypeID);
        //
        //     modelBuilder.Entity<Insurance>()
        //         .HasOne(i => i.Customer)
        //         .WithMany(c => c.Insurances)
        //     .HasForeignKey(i => i.CustomerID);
        // }
        //
        private void ConfigureEmployeeRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.AgentNumber);

            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Commission)
                .WithMany(c => c.Employees) // Assuming Commission has a collection of Employees
                .HasForeignKey("CommissionId"); // Adjust if you use a different name for the foreign key property

            // modelBuilder.Entity<Employee>()
            //     .HasOne(e => e.PostalCodeCity)
            //     .WithMany(pc => pc.Employees)
            // .HasForeignKey(i => i.PostalCode);
        }



            //modelbuilder.entity<employee>()
            //    .hasone(e => e.postalcodecity)
            //    .withmany(pc => pc.employees)
            //.hasforeignkey(i => i.postalcode);
        

        //
        private void ConfigureCustomerRelations(ModelBuilder modelBuilder)
        {
            // Eftersom vi vill att information från Customer ska finnas i både PrivateCustomer och CompanyCustomer
            // så mappas bas-klassen Customer inte till någon tabell

            // Konfigurera PrivateCustomer så att den mappas till en egen tabell och innehåller alla egenskaper från Customer
            modelBuilder
                .Entity<PrivateCustomer>()
                .ToTable("PrivateCustomers")
                .HasBaseType<Customer>(); // Detta säkerställer att alla Customer-fält inkluderas i PrivateCustomer-tabellen

            // Konfigurera CompanyCustomer så att den mappas till en egen tabell och innehåller alla egenskaper från Customer
            modelBuilder
                .Entity<CompanyCustomer>()
                .ToTable("CompanyCustomers")
                .HasBaseType<Customer>(); // Detta säkerställer att alla Customer-fält inkluderas i CompanyCustomer-tabellensäkerställer att alla Customer-fält inkluderas i CompanyCustomer-tabellen
        }

        //
        // private void ConfigureInsuranceTypeAttributes(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<InsuranceTypeAttribute>()
        //         .HasOne(ita => ita.InsuranceType)
        //         .WithMany(it => it.InsuranceTypeAttributes)
        //         .HasForeignKey(ita => ita.InsuranceTypeID);
        //
        //     modelBuilder.Entity<InsuranceSpec>()
        //         .HasOne(isp => isp.InsuranceTypeAttribute)
        //         .WithMany(ita => ita.InsuranceSpec)
        //         .HasForeignKey(isp => isp.InsuranceTypeAttributeID);
        // }

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
