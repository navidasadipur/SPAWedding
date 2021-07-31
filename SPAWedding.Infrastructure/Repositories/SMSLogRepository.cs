﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPAWedding.Core.Models;

namespace SPAWedding.Infrastructure.Repositories
{
    public class SMSLogRepository : BaseRepository<SMSLog, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;

        public SMSLogRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}