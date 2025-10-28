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
        Failed = 2,
        [Display(Name = "Rejected")]
        Rejected = 2,
        [Display(Name = "Finished")]
        Finished = 3,
        [Display(Name = "Done")]
        Done = 3
    }
}