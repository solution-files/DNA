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
    public class ReportController : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<ReportController> Logger;
		private readonly string Title = "Action";

		#endregion

		#region Class Methods

		// Constructor
		public ReportController(IConfiguration configuration, ILogger<ReportController> logger) {
			Configuration = configuration;
			Context = new MainContext(new DbContextOptionsBuilder<MainContext>().Options, Configuration);
			Logger = logger;
		}

		#endregion

		#region Controller Actions

		// Index (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public IActionResult Index() {
			string message;
			try {
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"Opened {Title}");
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View();
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

	}

	#endregion

}
