using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAWedding.Core.Models;
using SPAWedding.Infrastructure;
using SPAWedding.Infrastructure.Repositories;

namespace SPAWedding.Infratructure.Repositories
{
    public class TestimonialsRepository : BaseRepository<Testimonial, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public TestimonialsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
