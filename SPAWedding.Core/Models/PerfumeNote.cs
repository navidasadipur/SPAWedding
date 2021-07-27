using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAWedding.Core.Models
{
    public class PerfumeNote : IBaseEntity
    {
        public int Id { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Display(Name = "نوع نت")]
        public PerfumeNoteType PerfumeNoteType { get; set; }


        [Display(Name = "تصویر مرتبط")]
        public string Image { get; set; }


        [Display(Name = "لینک")]
        public string Link { get; set; }


        [Display(Name = "نام نت")]
        public string Title { get; set; }

    }

    public enum PerfumeNoteType
    {
        Beginning = 1,
        Middle = 2,
        Ending = 3
    }
}
