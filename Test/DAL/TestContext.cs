using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; // Assuming Entity Framework is used for data access
using System.Data.Entity.Infrastructure; // For DbContext and related classes
using Test.Models; // Assuming MoneySheet is defined in this namespace

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