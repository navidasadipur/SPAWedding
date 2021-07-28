using SPAWedding.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace SPAWedding.Infrastructure.Repositories
{
    public class CourseCommentsRepository : BaseRepository<CourseComment, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public CourseCommentsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<CourseComment> GetCourseComments(int courseId)
        {
            return _context.CourseComments.Where(h => h.CourseId == courseId & h.IsDeleted == false).ToList();
        }
        public string GetCourseName(int courseId)
        {
            return _context.Courses.Find(courseId).Title;
        }
        public CourseComment DeleteComment(int id)
        {
            var comment = _context.CourseComments.Find(id);
            var children = _context.CourseComments.Where(c=>c.ParentId == id).ToList();
            foreach (var child in children)
            {
                child.IsDeleted = true;
                _context.Entry(child).State = EntityState.Modified;
                _context.SaveChanges();
            }
            comment.IsDeleted = true;
            _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogEvent(comment.GetType().Name, comment.Id, "Delete");
            return comment;
        }
    }
}
