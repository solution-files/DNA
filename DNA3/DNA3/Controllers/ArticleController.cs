#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class ArticleController(IConfiguration configuration, MainContext context, ILogger<ArticleController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<ArticleController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private readonly string Title = "Article";

        #endregion

        #region Controller Actions

        // Index
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Index() {
            try {
				var result = await Context.Article.Include(x => x.Page).Include(x => x.Section).Include(x => x.Category).Include(x => x.Status).OrderBy(x => x.Page.Name).ThenBy(x => x.Section.Name).ThenBy(x => x.Category.Name).ThenBy(x => x.Name).ToListAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                return View("Index", result);
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

		// New (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> New(Article instance) {
            try {
				instance ??= new Article {
                        Date = DateTime.Now
                    };
                HttpContextAccessor.HttpContext.Session.SetString("clientReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            ViewBag.SectionList = await Context.Section.OrderBy(x => x.Name).ToListAsync();
            ViewBag.CategoryList = await Context.Category.OrderBy(x => x.Name).ToListAsync();
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
            return View("Detail", instance);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            Article instance = new();
            try {
                instance = await Context.Article.FindAsync(id);
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.ArticleId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            ViewBag.SectionList = await Context.Section.OrderBy(x => x.Name).ToListAsync();
            ViewBag.CategoryList = await Context.Category.OrderBy(x => x.Name).ToListAsync();
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
            return View("Detail", instance);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Article instance) {
            string message = "Data entry error(s)";
            try {
                if (ModelState.IsValid) {
                    if (instance.ArticleId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    } else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.ArticleId})";
                    }
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                    return RedirectToAction("Index");
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, "{message}", message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            ViewBag.SectionList = await Context.Section.OrderBy(x => x.Name).ToListAsync();
            ViewBag.CategoryList = await Context.Category.OrderBy(x => x.Name).ToListAsync();
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
            return View("Detail", instance);
        }

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Article instance) {
            string message = "";
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                message = $"Deleted {Title} ({instance.ArticleId})";
                Context.Article.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            ViewBag.SectionList = await Context.Section.OrderBy(x => x.Name).ToListAsync();
            ViewBag.CategoryList = await Context.Category.OrderBy(x => x.Name).ToListAsync();
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
            return View("Detail", instance);
        }

        // Close
        [HttpGet, HttpPost]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("articleReturnUrl"));
        }

        // Close Index
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public IActionResult CloseIndex() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        #endregion

    }

}
