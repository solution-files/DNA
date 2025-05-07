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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class SectionController(IConfiguration configuration, MainContext context, ILogger<SectionController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<SectionController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private readonly string Title = "Section";

        #endregion

        #region Controller Actions

        // Index
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index(string Criteria) {
            string message;
            IList<Section> result = default;
            try {
                if (string.IsNullOrEmpty(Criteria)) {
                    result = await Context.Section.Include(x => x.Page).OrderBy(x => x.Page.Name).ThenBy(x => x.Subject).ToListAsync();
                } else {
                    result = await Context.Section.Include(x => x.Page).Where(x => x.Page.Name.Contains(Criteria) || x.Slug.Contains(Criteria) || x.Name.Contains(Criteria) || x.Subject.Contains(Criteria) || x.Description.Contains(Criteria)).OrderBy(x => x.Page.Name).ThenBy(x => x.Subject).ToListAsync();
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
            Section instance = new();
            try {
                instance.Date = DateTime.Now;
                instance.Columns = 0;
                instance.Limit = 0;
                HttpContextAccessor.HttpContext.Session.SetString("sectionReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            Section instance = new();
            try {
                instance = await Context.Section.FindAsync(id);
                HttpContextAccessor.HttpContext.Session.SetString("sectionReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.SectionId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Section instance) {
            string message = "Data entry error(s)";
            try {
                if (ModelState.IsValid) {
                    if (instance.SectionId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    }
                    else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.SectionId})";
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
            return View("Detail", instance);
        }

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Section instance) {
            string message = "";
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                message = $"Deleted {Title} ({instance.SectionId})";
                Context.Section.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            ViewBag.PageList = await Context.Page.OrderBy(x => x.Name).ToListAsync();
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("sectionReturnUrl"));
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
