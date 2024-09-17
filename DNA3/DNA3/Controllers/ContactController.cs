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
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Administrators")]
    public class ContactController : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<ContactController> Logger;
		private readonly string Title = "Contact";

		#endregion

		#region Class Methods

		// Constructor
		public ContactController(IConfiguration configuration, MainContext context, ILogger<ContactController> logger) {
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
				var result = await Context.Request.OrderByDescending(x => x.Date).ToListAsync();
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
            DNA3.Models.Request instance = new DNA3.Models.Request();
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
            DNA3.Models.Request instance = new DNA3.Models.Request();
			try {
				instance = await Context.Request.FindAsync(id);
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.RequestId})");
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
		public async Task<IActionResult> Save(DNA3.Models.Request instance) {
			string message;
			try {
				if (ModelState.IsValid) {
					if (instance.RequestId == 0) {
						Context.Add(instance);
						message = $"Added New {Title}";
					} else {
						Context.Update(instance);
						message = $"Updated {Title} ({instance.RequestId})";
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
		public async Task<IActionResult> Delete(DNA3.Models.Request instance) {
			string message;
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.RequestId})";
				Context.Request.Remove(instance);
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

		#endregion

	}

}
