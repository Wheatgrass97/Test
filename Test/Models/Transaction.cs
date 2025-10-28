using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Test.Models
{
    [Table("Transaction")]
    public class TransactionModels
    {
        public int ID { get; set; }
        [Display(Name = "Transaction Identificator")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters.")]
        public string Code { get; set; }
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Display(Name = "Currency Code")]
        [StringLength(3, ErrorMessage = "Currency code must be 3 letters.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency code must be 3 uppercase letters conforming to ISO 4217.")]
        public string CurrencyCode { get; set; }

        [ForeignKey("CurrencyCode")]
        public virtual CurrencyModels Currency { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Status")]
        public TransactionStatus Status { get; set; }
    }
}