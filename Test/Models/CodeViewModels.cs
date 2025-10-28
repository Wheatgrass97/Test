using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public enum TransactionStatus
    {
        [Display(Name = "Approved")]
        Approved = 1,
        [Display(Name = "Failed")]
        Rejected = 2,
        [Display(Name = "Finished")]
        Done = 3
    }
    
}