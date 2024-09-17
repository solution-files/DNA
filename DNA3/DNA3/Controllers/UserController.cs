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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class UserController : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<UserController> Logger;
		private readonly string Title = "User";

		#endregion

		#region Class Methods

		// Constructor
		public UserController(IConfiguration configuration, MainContext context, ILogger<UserController> logger) {
			Configuration = configuration;
			Context = context;
			Logger = logger;
		}

		#endregion

		#region Controller Actions

		// Index
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet, HttpPost]
		public async Task<IActionResult> Index(int? id) {
			string message;
            IList<User> result = default;
            try {
                if (id == null) {
                    result = await Context.User.Include(x => x.Role).Include(x => x.Status).ToListAsync();
                    Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                } else {
                    result = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.ClientId == id).ToListAsync();
                    Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List for {id}");
                }
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
		public async Task<IActionResult> New() {
			User instance = new User();
			try {
				instance.ClientId = User.ClientId();
				ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
				ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
				ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
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
		public IActionResult Edit(int? id) {
			User instance = new User();
			try {
                instance = Context.User.Where(x => x.UserId == id).SingleOrDefault();
                ViewBag.ClientList = Context.Client.OrderBy(x => x.Company).ToList();
				ViewBag.RoleList = Context.Role.OrderBy(x => x.Description).ToList();
				ViewBag.StatusList = Context.Status.OrderBy(x => x.Description).ToList();
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.UserId})");
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
		public async Task<IActionResult> Save(User instance) {
			string message = "Data entry error(s)";
			try {
				if (ModelState.IsValid) {
					if (instance.UserId == 0) {
						Context.Add(instance);
						message = $"Added New {Title}";
					} else {
						Context.Update(instance);
						message = $"Updated {Title} ({instance.UserId})";
					}
					await Context.SaveChangesAsync();
					Site.Messages.Enqueue(message);
					Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
					// return RedirectToAction("Index");
				}
			} catch (Exception ex) {
				message = ex.Message;
				Logger.LogError(ex, message);
			}
			ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
			ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
			return View("Detail", instance);
		}

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(User instance) {
			string message = "";
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.UserId})";
				Context.User.Remove(instance);
				await Context.SaveChangesAsync();
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
				return RedirectToAction("Index");
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, ex.Message);
			}
			ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
			ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
			return View("Detail", instance);
		}

        // Profile (Get)
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Profile() {
			string message;
			User instance = new User();
			try {
				instance = await Context.User.FindAsync(User.UserId());
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View(instance);
		}

        // Profile (Post)
        [Authorize(Policy = "Managers")]
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Profile(User instance) {
			string message;
			try {
				if (ModelState.IsValid) {
					Context.Update(instance);
					await Context.SaveChangesAsync();
					message = $"Updated {Title} ({instance.UserId})";
					Site.Messages.Enqueue(message);
					Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
				}
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View(instance);
		}

		// Close
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

	}

	#endregion

}
