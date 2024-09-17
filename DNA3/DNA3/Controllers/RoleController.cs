#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class RoleController : Controller {

        #region Variables

        // Variables
        private IConfiguration Configuration;
        private MainContext Context;
        private ILogger<RoleController> Logger;
        private string Title = "Role";

        #endregion

        #region Class Methods

        // Constructor
        public RoleController(IConfiguration configuration, MainContext context, ILogger<RoleController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

		#endregion

		#region Controller Actions

		// Index
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Index() {
            string message;
            try {
                var result = await Context.Role.ToListAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                return View("Index", result);
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
		public IActionResult New() {
            Role instance = new Role();
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View("Detail", instance);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            Role instance = new Role();
            try {
                instance = await Context.Role.FindAsync(id);
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.RoleId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View("Detail", instance);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Role instance) {
            string message = "Data entry error(s)";
            try {
                if (ModelState.IsValid) {
                    if (instance.RoleId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    }
                    else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.RoleId})";
                    }
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                    return RedirectToAction("Index");
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            return View("Detail", instance);
        }

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Role instance) {
            string message = "";
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                message = $"Deleted {Title} ({instance.RoleId})";
                Context.Role.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            return View("Detail", instance);
        }

		// Close
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public IActionResult Close() {
            string message ;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

    }

    #endregion

}
