using SPAWedding.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAWedding.Infrastructure.Repositories
{
    public class CourseHeadLinesRepository : BaseRepository<CourseHeadLine, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public CourseHeadLinesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<CourseHeadLine> GetCourseHeadLines(int courseId)
        {
            return _context.CourseHeadLines.Where(h => h.CourseId == courseId & h.IsDeleted == false).ToList();
        }
        public string GetCourseName(int courseId)
        {
            return _context.Courses.Find(courseId).Title;
        }
    }
}
