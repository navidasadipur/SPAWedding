using SPAWedding.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPAWedding.Infrastructure.Repositories
{
    public class GalleriesRepository : BaseRepository<Gallery, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public GalleriesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
