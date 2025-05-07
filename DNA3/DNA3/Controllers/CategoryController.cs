﻿#region Usings

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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class CategoryController(IConfiguration configuration, MainContext context, ILogger<CategoryController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<CategoryController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private readonly string Title = "Category";

        #endregion

        #region Controller Actions

        // Index
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index(string Criteria) {
            string message;
            IList<Category> result = default;
            try {
                if (string.IsNullOrEmpty(Criteria)) {
                    result = await Context.Category.Include(x => x.Section).OrderBy(x => x.Section.Name).ThenBy(x => x.Subject).ToListAsync();
                } else {
                    result = await Context.Category.Include(x => x.Section).Where(x => x.Section.Name.Contains(Criteria) || x.Slug.Contains(Criteria) || x.Name.Contains(Criteria) || x.Subject.Contains(Criteria) || x.Description.Contains(Criteria)).OrderBy(x => x.Section.Name).ThenBy(x => x.Subject).ToListAsync();
                }
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                return View(result);
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        // New (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> New() {
            Category instance = new();
            try {
                instance.Date = DateTime.Now;
                ViewBag.SectionList = await Context.Section.OrderBy(x => x.Description).ToListAsync();
                HttpContextAccessor.HttpContext.Session.SetString("categoryReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return View("Detail", instance);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            Category instance = new();
            try {
                instance = await Context.Category.FindAsync(id);
                ViewBag.SectionList = await Context.Section.OrderBy(x => x.Description).ToListAsync();
                HttpContextAccessor.HttpContext.Session.SetString("categoryReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.CategoryId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View("Detail", instance);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Category instance) {
            string message = "Data entry error(s)";
            try {
                if (ModelState.IsValid) {
                    if (instance.CategoryId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    } else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.CategoryId})";
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
            ViewBag.SectionList = await Context.Section.OrderBy(x => x.Description).ToListAsync();
            return View("Detail", instance);
        }

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category instance) {
            string message;
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                message = $"Deleted {Title} ({instance.CategoryId})";
                Context.Category.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("categoryReturnUrl"));
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

    }

    #endregion

}
