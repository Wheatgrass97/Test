using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    [Table("ErrorLog")]
    public class ErrorLogModels
    {
        public int ID { get; set; }
        [Display(Name = "Error Message")]
        public string ErrorMessage { get; set; }
        [Display(Name = "Parameters")]
        public string Parameters { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
    }
}