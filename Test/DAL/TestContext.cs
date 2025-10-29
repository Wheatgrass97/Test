using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Test.Models;

namespace Test.DAL
{
    public class TestContext : DbContext
    {
        public TestContext() : base("TestContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<TransactionModels> Transactions{ get; set; }
        public DbSet<CurrencyModels> Currencies { get; set; }
        public DbSet<ErrorLogModels> ErrorLogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity mappings here if needed
        }
    }
}