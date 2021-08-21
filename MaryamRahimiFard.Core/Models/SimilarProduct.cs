using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Core.Models
{
    public class SimilarProduct : IBaseEntity
    {
        public int Id { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SimilarProductId { get; set; } // similar product id

    }
}
