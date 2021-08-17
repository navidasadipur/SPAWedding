﻿using MaryamRahimiFard.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaryamRahimiFard.Infrastructure.Repositories
{
    public class ProductMainFeaturesRepository : BaseRepository<ProductMainFeature, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ProductMainFeaturesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public ProductMainFeature GetByProductId(int productId)
        {
            return _context.ProductMainFeatures.FirstOrDefault(f => f.IsDeleted == false && f.ProductId == productId);
        }
        public ProductMainFeature GetByProductId(int productId,int mainFeatureId)
        {
            return _context.ProductMainFeatures.FirstOrDefault(f => f.IsDeleted == false && f.ProductId == productId && f.Id == mainFeatureId);
        }
        public List<ProductMainFeature> GetProductMainFeatures(int productId)
        {
            return _context.ProductMainFeatures.Include(f=>f.Feature).Include(f=>f.SubFeature).Where(f => f.IsDeleted == false && f.ProductId == productId).ToList();
        }

        public ProductMainFeature GetLastActiveMainFeature(int productId, int mainFeatureId)
        {
            var mainFeature = _context.ProductMainFeatures.FirstOrDefault(f => f.ProductId == productId && f.IsDeleted == false && f.Id==mainFeatureId);

            return mainFeature;
        }
    }
}
