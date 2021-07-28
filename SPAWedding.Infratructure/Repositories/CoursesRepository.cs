using SPAWedding.Core.Models;
using SPAWedding.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace SPAWedding.Infrastructure.Repositories
{
    public class CoursesRepository : BaseRepository<Course, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public CoursesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Course GetCourse(int id)
        {
            return _context.Courses.Include(a=>a.User).Include(a=>a.CourseCategory).Include(a=>a.CourseHeadLines).FirstOrDefault(a=>a.Id == id);
        }
        public List<Course> GetCourses()
        {
            return _context.Courses.Where(a=>a.IsDeleted == false).Include(a => a.User).Include(a=>a.CourseCategory).OrderBy(a=>a.InsertDate).ToList();
        }
        public List<CourseCategory> GetCourseCategories()
        {
            return _context.CourseCategories.Where(a => a.IsDeleted == false).ToList();
        }
        public void AddCourse(Course course)
        {
            var user = GetCurrentUser();
            course.InsertDate = DateTime.Now;
            course.InsertUser = user.UserName;
            course.AddedDate = DateTime.Now;
            course.UserId = user.Id;
            _context.Courses.Add(course);
            _context.SaveChanges();
            _logger.LogEvent(course.GetType().Name, course.Id, "Add");
        }

        //public string GetCourseTagsStr(int courseId)
        //{
        //    var courseTags = _context.CourseTags.Where(t => t.CourseId == courseId && t.IsDeleted == false).Select(t=>t.Title).ToList();
        //    var tagsStr = string.Join("-", courseTags.ToList());
        //    return tagsStr;
        //}

        //public void AddCourseTags(int courseId, string courseTags)
        //{
        //    if (string.IsNullOrEmpty(courseTags))
        //        return;
        //    var oldTags = _context.CourseTags.Where(t => t.CourseId == courseId).ToList();
        //    foreach (var tag in oldTags)
        //    {
        //        _context.CourseTags.Remove(tag);
        //        _context.SaveChanges();
        //    }

        //    string[] tagsArr = courseTags.Trim().Split('-');
        //    foreach (var tag in tagsArr)
        //    {
        //        var tagObj = new CourseTag();
        //        tagObj.CourseId = courseId;
        //        tagObj.Title = tag.Trim();

        //        _context.CourseTags.Add(tagObj);
        //        _context.SaveChanges();
        //    }
        //}

        public void AddCourseHeadLine(CourseHeadLine headLine)
        {
            var user = GetCurrentUser();
            headLine.InsertDate = DateTime.Now;
            headLine.InsertUser = user.UserName;
            _context.CourseHeadLines.Add(headLine);
            _context.SaveChanges();
            _logger.LogEvent(headLine.GetType().Name, headLine.Id, "Add");
        }

        public List<Course> GetLatestCourses(int take)
        {
            return _context.Courses.Where(a => a.IsDeleted == false).Include(a => a.User).OrderByDescending(a => a.Id).Take(take).ToList();
        }
        public List<Course> GetTopCourses(int take)
        {
            return _context.Courses.Where(a => a.IsDeleted == false).Include(a=>a.User)
                .OrderByDescending(a => a.ViewCount).Take(take).ToList();
        }
        #region Get Courses List
        public List<Course> GetCoursesList(int skip, int take)
        {
            return _context.Courses.Where(a => a.IsDeleted == false).Include(a=>a.User).OrderByDescending(a=>a.AddedDate).Skip(skip).Take(take).ToList();
        }
        public List<Course> GetCoursesList(int skip, int take, int categoryId)
        {
            return _context.Courses.Where(a => a.IsDeleted == false && a.CourseCategoryId == categoryId).Include(a => a.User).OrderByDescending(a => a.AddedDate).Skip(skip).Take(take).ToList();
        }

        public List<Course> GetCoursesList(int skip, int take, string searchString)
        {
            var searchedCourses = new List<Course>();

            var trimedSearchString = searchString.Trim().ToLower();

            var courses = _context.Courses
                    .Where(a => a.IsDeleted == false && (
                           a.Title.Trim().ToLower().Contains(trimedSearchString) 
                        || a.ShortDescription != null && a.ShortDescription.Trim().ToLower().Contains(trimedSearchString) 
                        || a.Description != null && a.Description.Trim().ToLower().Contains(trimedSearchString)
                     ))
                .Include(a => a.User).OrderByDescending(a => a.AddedDate).Skip(skip).Take(take).ToList();

            //var tags = _context.CourseTags
            //        .Where(t => t.IsDeleted == false && (
            //               t.Title != null && t.Title.ToLower().Trim().Contains(trimedSearchString)
            //       ))
            //       .OrderByDescending(t => t.InsertDate).Skip(skip).Take(take).ToList();

            //foreach (var tag in tags)
            //{
            //    searchedCourses.Add(GetCourse(tag.CourseId));
            //}

            foreach (var course in courses)
            {
                if (!searchedCourses.Contains(course))
                {
                    searchedCourses.Add(course);
                }
            }

            //|| a.CourseTags != null && a.CourseTags.Any(t => t.Title != null && t.Title.ToLower().Trim().Contains(trimedSearchString)

            return searchedCourses;
        }
        #endregion
        #region Get Count
        public int GetCoursesCount()
        {
            return _context.Courses.Where(a => a.IsDeleted == false).Count();
        }
        public int GetCoursesCount(int categoryId)
        {
            return _context.Courses.Where(a => a.IsDeleted == false && a.CourseCategoryId == categoryId).Count();
        }
        public int GetCoursesCount(string searchString)
        {
            return _context.Courses
                .Where(a => a.IsDeleted == false && (a.Title.Trim().ToLower().Contains(searchString.Trim().ToLower()) || a.ShortDescription.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                .Count();
        }
        #endregion
        public List<CourseComment> GetCourseComments(int courseId)
        {
            return _context.CourseComments.Where(c => c.IsDeleted == false && c.CourseId == courseId).ToList();
        }

        //public List<CourseTag> GetCourseTags(int courseId)
        //{
        //    return _context.CourseTags.Where(c => c.IsDeleted == false && c.CourseId == courseId).ToList();
        //}

        public List<CourseHeadLine> GetCourseHeadlines(int courseId)
        {
            return _context.CourseHeadLines.Where(c => c.IsDeleted == false && c.CourseId == courseId).ToList();
        }

        public List<Course> GetCoursesByCategoryId(int categoryId)
        {
            return _context.Courses.Where(a => a.CourseCategoryId == categoryId && a.IsDeleted == false).Include(a => a.User).Include(a => a.CourseCategory).OrderBy(a => a.InsertDate).ToList();
        }

        public void UpdateCourseViewCount(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            course.ViewCount++;
            _context.Entry(course).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void AddComment(CourseComment comment)
        {
            _context.CourseComments.Add(comment);
            _context.SaveChanges();
        }


        //public Course DeleteCourse(int courseId)
        //{
        //    var course = _context.Courses.Find(courseId);
        //    if (course == null)
        //        return null;
        //    var courseTags = _context.CourseTags.Where(t => t.CourseId == courseId).ToList();
        //    var articeheadLines = _context.
        //}
    }
}
