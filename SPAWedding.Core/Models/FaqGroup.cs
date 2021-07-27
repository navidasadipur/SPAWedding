using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAWedding.Core.Models
{
    public class FaqGroup : IBaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Title { get; set; }
        public ICollection<Faq> Faqs { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
