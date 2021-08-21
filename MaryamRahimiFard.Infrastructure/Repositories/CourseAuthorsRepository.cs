using MaryamRahimiFard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Infrastructure.Repositories
{
    public class CourseAuthorsRepository : BaseRepository<CourseAuthor, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public CourseAuthorsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public CourseAuthor GetCourseAuthorByCourseId(int courseId)
        {
            var course = _context.Courses.Where(c => c.Id == courseId && c.IsDeleted == false).FirstOrDefault();

            return _context.CourseAuthors.Where(ca => ca.Id == course.CourseAuthorId && ca.IsDeleted == false).FirstOrDefault();
        }
    }
}
