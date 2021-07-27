using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAWedding.Core.Models
{
    public class EmailSubscription
    {
        public int Id { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست")]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
