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
    public class BlogController : Controller
    {
        private readonly ArticlesRepository _articlesRepo;
        private readonly ArticleCategoriesRepository _articleCategoriesRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly ArticleTagsRepository _articleTagsRepo;

        public BlogController(
            ArticlesRepository articlesRepo,
            ArticleCategoriesRepository articleCategoriesRepo,
            StaticContentDetailsRepository staticContentDetailsRepo,
            ArticleTagsRepository articleTagsRepo
            )
        {
            _articlesRepo = articlesRepo;
            _articleCategoriesRepo = articleCategoriesRepo;
            _staticContentRepo = staticContentDetailsRepo;
            _articleCategoriesRepo = articleCategoriesRepo;
            _articleTagsRepo = articleTagsRepo;
        }
        // GET: Blog
        public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        {
            var articles = new List<Article>();
            var take = 6;
            var skip = pageNumber * take - take;
            var count = 0;
            if (category != null)
            {
                articles = _articlesRepo.GetArticlesList(skip, take, category.Value);
                count = _articlesRepo.GetArticlesCount(category.Value);
                var cat = _articleCategoriesRepo.Get(category.Value);
                ViewBag.CategoryId = category;
                ViewBag.Title = $"دسته {cat.Title}";
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                articles = _articlesRepo.GetArticlesList(skip, take, searchString);
                count = _articlesRepo.GetArticlesCount(searchString);
                ViewBag.SearchString = searchString;
                ViewBag.Title = $"جستجو: {searchString}";
            }
            else
            {
                articles = _articlesRepo.GetArticlesList(skip, take);
                count = _articlesRepo.GetArticlesCount();
                ViewBag.Title = "بلاگ";
            }

            var pageCount = (int) Math.Ceiling((double) count / take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageNumber;

            var vm = new List<LatestArticlesViewModel>();
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            var banner = "";
            try
            {
                banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ وبلاگ").Image;
                banner = "/Files/StaticContentImages/Image/" + banner;
            }
            catch
            {

            }

            ViewBag.banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(vm);
        }
        public ActionResult ArticleCategoriesSection()
        {
            var categories = _articlesRepo.GetArticleCategories();

            var articleCategoriesVm = new List<ArticleCategoriesViewModel>();

            foreach (var item in categories)
            {
                var vm = new ArticleCategoriesViewModel();
                vm.Id = item.Id;
                vm.Title = item.Title;
                vm.ArticleCount = _articlesRepo.GetArticlesCount(item.Id);
                articleCategoriesVm.Add(vm);
            }

            return PartialView(articleCategoriesVm);
        }
        public ActionResult TopArticlesSection(int take)
        {
            var vm = new List<LatestArticlesViewModel>();
            var articles = _articlesRepo.GetTopArticles(take);
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            return PartialView(vm);
        }

        public ActionResult AdBlogSection()
        {
            var model = _staticContentRepo.GetStaticContentDetail((int)StaticContents.BlogAd);

            return PartialView(model);
        }

        public ActionResult TagsSection()
        {
            //SocialViewModel model = new SocialViewModel();

            //model.Instagram = _staticContentDetailsRepo.GetStaticContentDetail(1009).Link;
            //model.Aparat = _staticContentDetailsRepo.GetStaticContentDetail(1012).Link;

            var tags = _articleTagsRepo.GetAll();

            return PartialView(tags);
        }

        [Route("Blog/ArticleDetails/{id}/{title}")]
        [Route("Blog/Article/{id}/{title}")]

        public ActionResult ArticleDetails(int id)
        {
            _articlesRepo.UpdateArticleViewCount(id);
            var article = _articlesRepo.GetArticle(id);
            var articleDetailsVm = new ArticleDetailsViewModel(article);
            var articleComments = _articlesRepo.GetArticleComments(id);
            var articleCommentsVm = new List<ArticleCommentViewModel>();

            foreach (var item in articleComments)
                articleCommentsVm.Add(new ArticleCommentViewModel(item));

            articleDetailsVm.ArticleComments = articleCommentsVm;
            var articleTags = _articlesRepo.GetArticleTags(id);
            articleDetailsVm.Tags = articleTags;
            var articleHeadlines = _articlesRepo.GetArticleHeadlines(id);
            articleDetailsVm.HeadLines = articleHeadlines;

            //get next article
            Article nextArticle = null;
            var nextId = id;

            var latestArticle = _articlesRepo.GetLatestArticles(1).FirstOrDefault();

            while (nextArticle == null && nextId < latestArticle.Id)
            {
                nextId++;
                nextArticle = _articlesRepo.GetArticle(nextId);
            }

            articleDetailsVm.Next = nextArticle;

            //get previous article
            Article previousArticle = null;
            var previousId = id;

            while (previousArticle == null && previousId > 1)
            {
                previousId--;
                previousArticle = _articlesRepo.GetArticle(previousId);
            }

            articleDetailsVm.Previous = previousArticle;

            var banner = "";
            try
            {
                banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ وبلاگ").Image;
                banner = "/Files/StaticContentImages/Image/" + banner;
            }
            catch
            {

            }

            ViewBag.banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(articleDetailsVm);
        }
        [HttpPost]
        public ActionResult PostComment(CommentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                var comment = new ArticleComment()
                {
                    ArticleId = form.ArticleId.Value,
                    ParentId = form.ParentId,
                    Name = form.Name,
                    Email = form.Email,
                    Message = form.Message,
                    AddedDate = DateTime.Now,
                };
                _articlesRepo.AddComment(comment);
                return RedirectToAction("ContactUsSummary", "Home");
            }
            return RedirectToAction("ArticleDetails", new { id = form.ArticleId });
        }

        public ActionResult SocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Linkedin = _staticContentRepo.GetStaticContentDetail((int)StaticContents.linkedin);

            return PartialView(model);
        }

        public ActionResult RelatedBlogsSection(int? categoryId, int take)
        {
            var relatedArticles = new List<LatestArticlesViewModel>();

            var takedArticles = new List<Article>();

            if (categoryId != null)
            {
                var articles = _articlesRepo.GetArticlesByCategoryId(categoryId.Value).OrderByDescending(b => b.InsertDate).ToList();

                if (articles.Count() < take)
                {
                    takedArticles = articles;
                }
                else
                {
                    takedArticles = articles.GetRange(0, take);
                }

            }

            foreach (var blog in takedArticles)
            {
                var vm = new LatestArticlesViewModel(blog);

                relatedArticles.Add(vm);
            }

            return PartialView(relatedArticles);
        }
    }
}