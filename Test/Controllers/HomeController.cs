using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.DAL;
using Test.Helpers;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        TestContext db = new TestContext();
        public ActionResult Index()
        {
            var stream = new StreamReader(@"C:\Users\Kaixuan\Desktop\testBook.csv");
            var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap<TransactionClassMap>();
            var records = csvReader.GetRecords<CSVTransactionModels>().ToList();

            

            foreach (var record in records)
            {
                bool hasNull = record.GetType()
                    .GetProperties()
                    .Any(prop => prop.GetValue(record) == null);
                if (!hasNull)
                {
                    TransactionStatus statusValue = TransactionStatus.Rejected;
                    TransactionStatusExtensions.TryParse(record.Status, out statusValue);
                    var transaction = new TransactionModels
                    {
                        Code = record.Code,
                        Amount = record.Amount,
                        CurrencyCode = record.CurrencyCode,
                        CreatedDate = record.CreatedDate,
                        Status = statusValue
                    };
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ErrorMessage = "CSV contains invalid data. Please check for missing values.";
                    return View();
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public class TransactionClassMap : ClassMap<CSVTransactionModels>
        {
            public TransactionClassMap()
            {
                Map(m => m.Code).Name("Transaction Identificator");
                Map(m => m.Amount).Name("Amount");
                Map(m => m.CurrencyCode).Name("Currency Code");
                Map(m => m.CreatedDate).Name("Created Date").TypeConverterOption.Format("dd/MM/yyyy HH:mm:ss"); ;
                Map(m => m.Status).Name("Status");
            }
        }

        public class CSVTransactionModels
        {
            [Name("Transaction Identificator")]
            public string Code { get; set; }
            [Name("Amount")]
            public decimal Amount { get; set; }
            [Name("Currency Code")]
            public string CurrencyCode { get; set; }
            [Name("Created Date")]
            public DateTime CreatedDate { get; set; }
            [Name("Status")]
            public string Status { get; set; }
        }
    }
}