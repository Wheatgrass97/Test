using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    [Table("Currency")]
    public class CurrencyModels
    {
        [Key]
        [StringLength(3)]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}