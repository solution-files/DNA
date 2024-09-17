#region Usings

using DNA3.Classes;
using DNA3.Models;
using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class ActivityController : Controller {

		#region Variables

		// Variables
		private readonly MainContext Context;
		private readonly ILogger<ActivityController> Logger;
		private readonly string Title = "Activity";

		#endregion

		#region Class Methods

		// Constructor
		public ActivityController(MainContext context, ILogger<ActivityController> logger) {
			Context = context;
			Logger = logger;
		}

		#endregion

		#region Controller Actions

		// Index (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Index() {
			string message;
			try {
				var result = await Context.Activity.OrderByDescending(x => x.Id).ToListAsync();
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
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
		public IActionResult New() {
			Activity instance = new Activity();
			try {
				instance.TimeStamp = DateTime.Now;
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
			Activity instance = new Activity();
			try {
				instance = await Context.Activity.FindAsync(id);
				ViewBag.UserList = await Context.User.OrderBy(x => x.Last).ToListAsync();
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.Id})");
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
		public async Task<IActionResult> Save(Activity instance) {
			string message;
			try {
				if (ModelState.IsValid) {
					if (instance.Id == 0) {
						Context.Add(instance);
						message = $"Added New {Title}";
					} else {
						Context.Update(instance);
						message = $"Updated {Title} ({instance.Id})";
					}
					await Context.SaveChangesAsync();
					Site.Messages.Enqueue(message);
					Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
					return RedirectToAction("Index");
				} else {
					message = "Please correct the data entry errors indicated below";
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
		public async Task<IActionResult> Delete(Activity instance) {
			string message;
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.Id})";
				Context.Activity.Remove(instance);
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
			return RedirectToAction("Index", "Dashboard");
		}

		// Clear Notifications
		[ApiExplorerSettings(IgnoreApi = true)]
		[Route("activity/clearnotificationsasync")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ClearNotificationsAsync() {
			string message;
			try {
				int rows = await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM [Activity] WHERE UserId = {User.UserId()}");
				message = "Notifications Cleared";
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, ex.Message);
			}
			return RedirectToAction("Index");
		}

		// Clear All
		[ApiExplorerSettings(IgnoreApi = true)]
		[Route("activity/clearallasync")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ClearAllAsync() {
			string message;
			try {
				int rows = await Context.Database.ExecuteSqlInterpolatedAsync($"TRUNCATE TABLE [Activity]");
				message = "Activity Log Cleared";
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, ex.Message);
			}
			return RedirectToAction("Index");
		}

	}

	#endregion

}
