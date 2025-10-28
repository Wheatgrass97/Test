using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Xml;
using System.Xml.Serialization;
using Test.CsvClasses;
using Test.DAL;
using Test.Helpers;
using Test.Models;
using Test.XmlClasses;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        TestContext db = new TestContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                //ViewBag.ErrorMessage = "Please select a file to upload.";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please select a file to upload.");
            }

            var allowedExtensions = new[] { ".csv", ".xml" };
            var fileExt = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExt) || !allowedExtensions.Contains(fileExt))
            {
                //ViewBag.ErrorMessage = "Invalid file type. Only .csv files are allowed.";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid file type. Only .csv files are allowed.");
            }

            const int maxBytes = 1024 * 1024;
            if (file.ContentLength > maxBytes)
            {
                //ViewBag.ErrorMessage = "File too large. Maximum allowed size is 1 MB.";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File too large. Maximum allowed size is 1 MB.");
            }

            ViewBag.Message = "Unknown format.";

            try
            {
                var stream = new StreamReader(file.InputStream);
                List<TransactionModels> transactionList = new List<TransactionModels>();
                if (fileExt == ".xml")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XmlClasses.Transactions));
                    XmlClasses.Transactions transactions = (XmlClasses.Transactions)serializer.Deserialize(stream);

                    transactionList = transactions.Items.Select(t =>
                    {
                        Models.TransactionStatus statusValue = new Models.TransactionStatus();
                        TransactionStatusExtensions.TryParse(t.Status, out statusValue);

                        return new TransactionModels
                        {
                            Code = t.Id,
                            Amount = t.PaymentDetails?.Amount ?? 0,
                            CurrencyCode = t.PaymentDetails?.CurrencyCode,
                            CreatedDate = t.TransactionDate,
                            Status = statusValue  // assign enum value
                        };
                    }).ToList();

                    ViewBag.Message = "XML file imported successfully.";
                }

                if (fileExt == ".csv")
                {
                    CsvReader csvReader = new CsvReader(stream, CultureInfo.InvariantCulture);
                    csvReader.Context.RegisterClassMap<TransactionClassMap>();
                    List<CSVTransactionModels> records = csvReader.GetRecords<CSVTransactionModels>().ToList();

                    transactionList = records.Select(t =>
                    {
                        Models.TransactionStatus statusValue = new Models.TransactionStatus();
                        TransactionStatusExtensions.TryParse(t.Status, out statusValue);

                        return new TransactionModels
                        {
                            Code = t.Code,
                            Amount = t.Amount,
                            CurrencyCode = t.CurrencyCode,
                            CreatedDate = t.CreatedDate,
                            Status = statusValue  // assign enum value
                        };
                    }).ToList();
                    ViewBag.Message = "CSV file imported successfully.";
                }

                Exception exImportDB = ImportToDB(transactionList);
                if (exImportDB != null)
                {
                    //ViewBag.ErrorMessage = "File contains invalid data. Please check for missing values.";
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File contains invalid data. Please check for missing values.");
                }
            }
            catch (Exception ex) {
                var error = new ErrorLogModels
                {
                    ErrorMessage = $"An unknown error occurred while processing the file: {ex.Message}, {ex.InnerException}",
                    CreatedDate = DateTime.Now
                };
                db.Dispose();

                using (var errorContext = new TestContext())
                {
                    errorContext.ErrorLogs.Add(new ErrorLogModels
                    {
                        ErrorMessage = $"An unknown error occurred while processing the file: {ex.Message} {ex.InnerException}",
                        CreatedDate = DateTime.Now
                    });
                    errorContext.SaveChanges();
                }

                //ViewBag.ErrorMessage = "An error occurred while processing the file.";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "An unknown error occurred while processing the file.");
            }

            
            Response.StatusCode = (int)HttpStatusCode.OK;
            return View();
        }

        //public bool HasNull(List<TransactionModels> tList)
        //{
        //    return tList.Any(item => item
        //                    .GetType()
        //                    .GetProperties()
        //                    .Any(prop => prop.GetValue(item) == null));
        //}

        public Exception ImportToDB(List<TransactionModels> tList)
        {
            foreach (var item in tList)
            {
                var nullProps = item.GetType()
                    .GetProperties()
                    .Where(prop => prop.GetValue(item) == null && !string.Equals(prop.Name, "Currency"))
                    .Select(prop => prop.Name)
                    .ToList();
                if (!nullProps.Any())
                {
                    var transaction = new TransactionModels
                    {
                        Code = item.Code,
                        Amount = item.Amount,
                        CurrencyCode = item.CurrencyCode,
                        CreatedDate = item.CreatedDate,
                        Status = item.Status
                    };
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }
                else 
                {
                    string missingFields = string.Join(", ", nullProps);
                    string parameters = string.Join(", ",
                        item.GetType()
                            .GetProperties()
                            .Select(p => $"{p.Name}={p.GetValue(item) ?? "NULL"}"));

                    var error = new ErrorLogModels
                    {
                        ErrorMessage = $"Transaction has null fields: {missingFields}",
                        Parameters = parameters,
                        CreatedDate = DateTime.Now
                    };

                    db.ErrorLogs.Add(error);
                    db.SaveChanges();

                    return new Exception("");
                }    
            }
            return null;
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

        
    }
}