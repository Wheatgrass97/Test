using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Test.CsvClasses
{
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
