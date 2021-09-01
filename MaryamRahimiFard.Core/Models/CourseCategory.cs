using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MaryamRahimiFard.Core.Models
{
    public class CourseCategory : IBaseEntity
    {
        public int Id { get; set; }
        [Display(Name ="نام دسته")]
        [MaxLength(400,ErrorMessage = "نام دسته باید از 400 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد کنید")]
        public string Title { get; set; }
        public ICollection<Course> Courses { get; set; }

        public int? ParentId { get; set; }
        public virtual CourseCategory Parent { get; set; }
        public virtual ICollection<CourseCategory> Children { get; set; }

        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
