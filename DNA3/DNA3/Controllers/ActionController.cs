#region Usings

using DNA3.Classes;
using Utilities;
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

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class ActionController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<ActionController> Logger;
        private readonly string Title = "Action";

        #endregion

        #region Class Methods

        // Constructor
        public ActionController(IConfiguration configuration, ILogger<ActionController> logger) {
            Configuration = configuration;
			Context = new MainContext(new DbContextOptionsBuilder<MainContext>().Options, Configuration);
            Logger = logger;
        }

		#endregion

		#region Controller Actions

		// Index (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet, HttpPost]
        public async Task<IActionResult> Index(int id) {
            string message;
            try {
                var result = await Context.Action.Where(x => x.MenuId == id).OrderBy(x => x.Weight).ToListAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                HttpContext.Session.SetInt32("MenuId", id);
                return View(result);
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

		// New (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> NewAsync() {
            DNA3.Models.Action instance = new();
            try {
				instance.MenuId = HttpContext.Session.GetInt32("MenuId");
				instance.NewWindow = false;
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            ViewBag.RoleList = await Context.Role.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            DNA3.Models.Action instance = new();
            try {
                instance = await Context.Action.FindAsync(id);
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.ActionId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewBag.RoleList = await Context.Role.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(DNA3.Models.Action instance) {
            string message;
            try {
                if (ModelState.IsValid) {
                    if (instance.ActionId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    } else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.MenuId})";
                    }
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                    return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("MenuId")});
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            ViewBag.RoleList = await Context.Role.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DNA3.Models.Action instance) {
            string message;
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                message = $"Deleted {Title} ({instance.ActionId})";
                Context.Action.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("MenuId") });
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewBag.RoleList = await Context.Role.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

		// Close (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Edit", "Menu", new { id = HttpContext.Session.GetInt32("MenuId") });
        }

    }

    #endregion

}
