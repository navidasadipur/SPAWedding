using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Infrastructure;
using MaryamRahimiFard.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Infrastructure.Repositories
{
    public class EPaymentLogRepository : BaseRepository<EPaymentLog, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public EPaymentLogRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
