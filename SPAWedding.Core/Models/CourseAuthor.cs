using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MaryamRahimiFard.Core.Models
{
    public class CourseAuthor : IBaseEntity
    {
        public int Id { get; set; }
        [Display(Name ="نام")]
        [MaxLength(400,ErrorMessage = "نام باید از 400 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [MaxLength(400, ErrorMessage = "نام خانوادگی باید از 400 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }
        [Display(Name = "زمینه های تدریس")]
        [DataType(DataType.MultilineText)]
        public string Filed { get; set; }
        [Display(Name = "درباره مدرس")]
        [DataType(DataType.MultilineText)]
        public string AboutAuthor { get; set; }
        [Display(Name = "توضیحات بیشتر")]
        [DataType(DataType.MultilineText)]
        public string MoreInfo { get; set; }
        public ICollection<Course> Courses { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
