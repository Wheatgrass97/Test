using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Test.XmlClasses
{
    [XmlRoot("Transactions")]
    public class Transactions
    {
        [XmlElement("Transaction")]
        public List<Transaction> Items { get; set; }
    }

    public class Transaction
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [XmlElement("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }
    }

    public class PaymentDetails
    {
        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
