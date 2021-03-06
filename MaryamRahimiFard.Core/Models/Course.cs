using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace MaryamRahimiFard.Core.Models
{
    public class Course : IBaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "عنوان دوره")]
        [MaxLength(600,ErrorMessage = "{0} باید از 600 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح کوتاه")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public int ViewCount { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        public DateTime? AddedDate { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "طول دوره(ساعت)")]
        public long? DurationInHours { get; set; }
        [Display(Name = "تعداد جلسات")]
        public int? SessionsNumber { get; set; }

        public int? CourseCategoryId { get; set; }
        public CourseCategory CourseCategory { get; set; }

        public int? CourseAuthorId { get; set; }
        public CourseAuthor CourseAuthor { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<CourseHeadLine> CourseHeadLines { get; set; }
        //public ICollection<CourseTag> CourseTags { get; set; }
        public ICollection<CourseComment> CourseComments { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
