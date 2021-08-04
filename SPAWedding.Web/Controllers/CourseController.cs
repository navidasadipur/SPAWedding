using SPAWedding.Core.Models;
using SPAWedding.Core.Utility;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPAWedding.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly CoursesRepository _coursesRepo;
        private readonly CourseCategoriesRepository _courseCategoriesRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly CourseAuthorsRepository _courseAuthorsRepo;

        //private readonly CourseTagsRepository _courseTagsRepo;

        public CourseController(
            CoursesRepository coursesRepo
            , CourseCategoriesRepository courseCategoriesRepo
            , StaticContentDetailsRepository staticContentDetailsRepo
            , CourseAuthorsRepository courseAuthorsRepo
            //, CourseTagsRepository courseTagsRepo
            )
        {
            _coursesRepo = coursesRepo;
            _courseCategoriesRepo = courseCategoriesRepo;
            _staticContentRepo = staticContentDetailsRepo;
            _courseAuthorsRepo = courseAuthorsRepo;
            _courseCategoriesRepo = courseCategoriesRepo;
            //_courseTagsRepo = courseTagsRepo;
        }
        // GET: Course
        public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        {
            var courses = new List<Course>();
            var take = 6;
            var skip = pageNumber * take - take;
            var count = 0;
            if (category != null)
            {
                courses = _coursesRepo.GetCoursesList(skip, take, category.Value);
                count = _coursesRepo.GetCoursesCount(category.Value);
                var cat = _courseCategoriesRepo.Get(category.Value);
                ViewBag.CategoryId = category;
                ViewBag.Title = $"دسته {cat.Title}";
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                courses = _coursesRepo.GetCoursesList(skip, take, searchString);
                count = _coursesRepo.GetCoursesCount(searchString);
                ViewBag.SearchString = searchString;
                ViewBag.Title = $"جستجو: {searchString}";
            }
            else
            {
                courses = _coursesRepo.GetCoursesList(skip, take);
                count = _coursesRepo.GetCoursesCount();
                ViewBag.Title = "کلاس";
            }

            var pageCount = (int) Math.Ceiling((double) count / take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageNumber;

            var vm = new List<LatestCoursesViewModel>();
            foreach (var item in courses)
                vm.Add(new LatestCoursesViewModel(item));

            var banner = "";
            try
            {
                banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ کلاس").Image;
                banner = "/Files/StaticContentImages/Image/" + banner;
            }
            catch
            {

            }

            ViewBag.banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(vm);
        }
        public ActionResult CourseCategoriesSection()
        {
            var categories = _coursesRepo.GetCourseCategories();

            var courseCategoriesVm = new List<CourseCategoriesViewModel>();

            foreach (var item in categories)
            {
                var vm = new CourseCategoriesViewModel();
                vm.Id = item.Id;
                vm.Title = item.Title;
                vm.CourseCount = _coursesRepo.GetCoursesCount(item.Id);
                courseCategoriesVm.Add(vm);
            }

            return PartialView(courseCategoriesVm);
        }
        public ActionResult TopCoursesSection(int take)
        {
            var vm = new List<LatestCoursesViewModel>();
            var courses = _coursesRepo.GetTopCourses(take);
            foreach (var item in courses)
                vm.Add(new LatestCoursesViewModel(item));

            return PartialView(vm);
        }

        //public ActionResult AdCourseSection()
        //{
        //    var model = _staticContentRepo.GetStaticContentDetail((int)StaticContents.CourseAd);

        //    return PartialView(model);
        //}

        //public ActionResult TagsSection()
        //{
        //    //SocialViewModel model = new SocialViewModel();

        //    //model.Instagram = _staticContentDetailsRepo.GetStaticContentDetail(1009).Link;
        //    //model.Aparat = _staticContentDetailsRepo.GetStaticContentDetail(1012).Link;

        //    var tags = _courseTagsRepo.GetAll();

        //    return PartialView(tags);
        //}

        [Route("Course/CourseDetails/{id}/{title}")]

        public ActionResult CourseDetails(int id)
        {
            _coursesRepo.UpdateCourseViewCount(id);
            var course = _coursesRepo.GetCourse(id);
            var courseDetailsVm = new CourseDetailsViewModel(course);
            var courseComments = _coursesRepo.GetCourseComments(id);
            var courseCommentsVm = new List<CourseCommentViewModel>();

            foreach (var item in courseComments)
                courseCommentsVm.Add(new CourseCommentViewModel(item));

            courseDetailsVm.CourseComments = courseCommentsVm;

            courseDetailsVm.CourseAuthor = _courseAuthorsRepo.GetCourseAuthorByCourseId(id);

            //var courseTags = _coursesRepo.GetCourseTags(id);
            //courseDetailsVm.Tags = courseTags;

            var courseHeadlines = _coursesRepo.GetCourseHeadlines(id);
            courseDetailsVm.HeadLines = courseHeadlines;

            //get next course
            Course nextCourse = null;
            var nextId = id;

            var latestCourse = _coursesRepo.GetLatestCourses(1).FirstOrDefault();

            while (nextCourse == null && nextId < latestCourse.Id)
            {
                nextId++;
                nextCourse = _coursesRepo.GetCourse(nextId);
            }

            courseDetailsVm.Next = nextCourse;

            //get previous course
            Course previousCourse = null;
            var previousId = id;

            while (previousCourse == null && previousId > 1)
            {
                previousId--;
                previousCourse = _coursesRepo.GetCourse(previousId);
            }

            courseDetailsVm.Previous = previousCourse;

            //var banner = "";
            //try
            //{
            //    banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ وسلاس").Image;
            //    banner = "/Files/StaticContentImages/Image/" + banner;
            //}
            //catch
            //{

            //}

            //ViewBag.banner = banner;

            //ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            ViewBag.Phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone).ShortDescription;

            return View(courseDetailsVm);
        }
        [HttpPost]
        public ActionResult PostComment(CommentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                var comment = new CourseComment()
                {
                    CourseId = form.CourseId.Value,
                    ParentId = form.ParentId,
                    Name = form.Name,
                    Email = form.Email,
                    Message = form.Message,
                    AddedDate = DateTime.Now,
                };
                _coursesRepo.AddComment(comment);
                return RedirectToAction("ContactUsSummary", "Home");
            }
            return RedirectToAction("CourseDetails", new { id = form.CourseId });
        }

        public ActionResult SocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Linkedin = _staticContentRepo.GetStaticContentDetail((int)StaticContents.LinkedIn);

            return PartialView(model);
        }

        public ActionResult RelatedCoursesSection(int? categoryId, int take)
        {
            var relatedCourses = new List<LatestCoursesViewModel>();

            var takedCourses = new List<Course>();

            if (categoryId != null)
            {
                var courses = _coursesRepo.GetCoursesByCategoryId(categoryId.Value).OrderByDescending(b => b.InsertDate).ToList();

                if (courses.Count() < take)
                {
                    takedCourses = courses;
                }
                else
                {
                    takedCourses = courses.GetRange(0, take);
                }

            }

            foreach (var course in takedCourses)
            {
                var vm = new LatestCoursesViewModel(course);

                relatedCourses.Add(vm);
            }

            return PartialView(relatedCourses);
        }
    }
}