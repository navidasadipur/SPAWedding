using SPAWedding.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPAWedding.Web.ViewModels
{
    public class CourseFormViewModel
    {
        public int Id { get; set; }
        [Display(Name = "عنوان مقاله")]
        [MaxLength(600, ErrorMessage = "{0} باید از 600 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public int CourseCategoryId { get; set; }
        public HttpPostedFileBase CourseImage { get; set; }

        public List<CourseHeadLineViewModel> CourseHeadLines { get; set; }
    }
    public class CourseHeadLineViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
    }

    public class CourseInfoViewModel
    {
        public CourseInfoViewModel()
        {
            
        }

        public CourseInfoViewModel(Course course)
        {
            this.Id = course.Id;
            this.Title = course.Title;
            this.Author = course.User != null? $"{course.User.FirstName} {course.User.LastName}" : "-";
            this.CourseCategory = course.CourseCategory != null? course.CourseCategory.Title : "-";
            this.PersianAddedDate = course.AddedDate != null? new PersianDateTime(course.AddedDate.Value).ToString() : "-";
            this.AddedDate = course.AddedDate;
        }
        public int Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "نویسنده")]
        public string Author { get; set; }
        [Display(Name = "دسته بندی")]
        public string CourseCategory { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianAddedDate { get; set; }
        public DateTime? AddedDate { get; set; }
    }
    public class CourseCommentWithPersianDateViewModel : CourseComment
    {
        public CourseCommentWithPersianDateViewModel(CourseComment comment)
        {
            this.Comment = comment;
            this.PersianDate = comment.AddedDate != null ? new PersianDateTime(comment.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
        }
        public CourseComment Comment { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianDate { get; set; }
    }

    public class LatestCoursesViewModel
    {
        public LatestCoursesViewModel()
        {
        }

        public LatestCoursesViewModel(Course course)
        {
            this.Id = course.Id;
            this.Title = course.Title;
            this.Image = course.Image;
            this.ShortDescription = course.ShortDescription;
            this.Author = $"{course.User.FirstName} {course.User.LastName}";
            this.PersianDate = course.AddedDate != null ? new PersianDateTime(course.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
            if (course.CourseCategory != null)
            {
                this.Category = course.CourseCategory;
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Author { get; set; }
        public string PersianDate { get; set; }
        public CourseCategory Category { get; set; }
    }
    public class CourseDetailsViewModel
    {
        public CourseDetailsViewModel()
        {
            this.HeadLines = new List<CourseHeadLine>();
            this.CourseComments = new List<CourseCommentViewModel>();
        }
        public CourseDetailsViewModel(Course course)
        {
            this.Id = course.Id;
            this.CategoryId = course.CourseCategoryId.Value;
            this.CategoryTitle = course.CourseCategory.Title;
            this.Title = course.Title;
            this.Image = course.Image;
            this.ShortDescription = course.ShortDescription;
            this.Description = course.Description;
            this.Author = course.User != null ? $"{course.User.FirstName} {course.User.LastName}" : "-";
            this.AuthorImage = course.User != null ? course.User.Avatar : "user-avatar.png";
            this.AuthorInfo = course.User != null ? course.User.Information : "";
            this.PersianDate = course.AddedDate != null ? new PersianDateTime(course.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";

            this.StartDate = course.StartDate != null ? new PersianDateTime(course.StartDate.Value).ToString("dddd d MMMM yyyy") : "-";
            this.EndDate = course.EndDate != null ? new PersianDateTime(course.EndDate.Value).ToString("dddd d MMMM yyyy") : "-";
            this.SessionsNumber = course.SessionsNumber;

            this.HeadLines = new List<CourseHeadLine>();
            this.CourseComments = new List<CourseCommentViewModel>();
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string AuthorImage { get; set; }
        public string AuthorInfo { get; set; }
        public string PersianDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? SessionsNumber { get; set; }

        public CourseAuthor CourseAuthor { get; set; }

        public List<CourseHeadLine> HeadLines { get; set; }
        //public List<CourseTag> Tags { get; set; }
        public List<CourseCommentViewModel> CourseComments { get; set; }
        public CommentFormViewModel CommentForm { get; set; }

        public Course Next { get; set; }
        public Course Previous { get; set; }
    }

    public class CourseCommentViewModel
    {
        public CourseCommentViewModel()
        {

        }

        public CourseCommentViewModel(CourseComment comment)
        {
            this.Id = comment.Id;
            this.ParentId = comment.ParentId;
            this.Name = comment.Name;
            this.Email = comment.Email;
            this.Message = comment.Message;
            this.AddedDate = comment.AddedDate != null ? new PersianDateTime(comment.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string AddedDate { get; set; }
    }

    public class CourseCategoriesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseCount { get; set; }
    }
}