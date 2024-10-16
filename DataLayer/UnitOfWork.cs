using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer
{
    public class UnitOfWork
    {
        private bool isDisposed = false;
        private readonly bool disposeContext = false;
        protected Context Context {  get; }

        public CommisionRateRepository CommisionRateRepository { get; private set; }
        public CompanyCustomerRepository CompanyCustomerRepository { get; private set;}
        public CustomerRepository CustomerRepository { get; private set; }
        public EmployeeRepository EmployeeRepository { get; private set; }
        public InsuranceRepository InsuranceRepository { get; private set; }
        public InsuranceSpecRepository InsuranceSpecRepository { get; private set; }
        public InsuranceTypeAttributeRepository InsuranceTypeAttributeRepository { get; private set; }
        public InsuranceTypeRepository InsuranceTypeRepository { get; private set; }
        public InsuredPersonRepository InsuredPersonRepository { get; private set; }
        public PostalCodeCityRepository PostalCodeCityRepository { get; private set;}
        public PrivateCustomerRepository PrivateCustomerRepository { get; private set; }
        public UserRepository UserRepository { get; private set; }


        public UnitOfWork(Context context)
        {
            // context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Patients OFF;");
            Context = context;
            CommisionRateRepository = new CommisionRateRepository(context);
            CompanyCustomerRepository = new CompanyCustomerRepository(context);
            CustomerRepository = new CustomerRepository(context);
            EmployeeRepository = new EmployeeRepository(context);
            InsuranceRepository = new InsuranceRepository(context);
            InsuranceSpecRepository = new InsuranceSpecRepository(context);
            InsuranceTypeAttributeRepository = new InsuranceTypeAttributeRepository(context);
            InsuranceTypeRepository = new InsuranceTypeRepository(context);
            InsuredPersonRepository = new InsuredPersonRepository(context);
            PostalCodeCityRepository = new PostalCodeCityRepository(context);
            PrivateCustomerRepository = new PrivateCustomerRepository(context);
            UserRepository = new UserRepository(context);
        }
      
        public UnitOfWork()
               : this(new Context())
        {
            disposeContext = true;
        }

      
        public void Update<T>(T entity) where T : class
        {
            try
            {
                Context.Update(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (disposing)
            {
                if (disposeContext)
                {
                    Context.Dispose();
                }
            }
            isDisposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }


    }
}
