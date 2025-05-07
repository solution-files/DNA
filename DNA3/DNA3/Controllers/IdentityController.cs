#region Usings

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
    public class IdentityController(IConfiguration configuration, MainContext context, ILogger<IdentityController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration = configuration;
		private readonly MainContext Context = context;
		private readonly ILogger<IdentityController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
		private readonly string Title = "Identity";

        #endregion

        #region Controller Actions

        // Index (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet, HttpPost]
		public async Task<IActionResult> Index(int id) {
			string message;
			try {
				var result = await Context.Login.Where(x => x.UserId == id).OrderBy(x => x.Email).ToListAsync();
				Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
				HttpContext.Session.SetInt32("UserId", id);
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
		public IActionResult New() {
            DNA3.Models.Login instance = new();
			try {
				instance.UserId = HttpContext.Session.GetInt32("UserId");
                HttpContextAccessor.HttpContext.Session.SetString("identityReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
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
            DNA3.Models.Login instance = new();
			try {
				instance = await Context.Login.FindAsync(id);
                HttpContextAccessor.HttpContext.Session.SetString("identityReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.LoginId})");
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
		public async Task<IActionResult> Save(DNA3.Models.Login instance) {
			string message;
			try {
				if (ModelState.IsValid) {
					if (instance.LoginId == 0) {
						Context.Add(instance);
						message = $"Added New {Title}";
					} else {
						Context.Update(instance);
						message = $"Updated {Title} ({instance.LoginId})";
					}
					await Context.SaveChangesAsync();
					Site.Messages.Enqueue(message);
					Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
					// return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("UserId") });
				}
			} catch (Exception ex) {
				message = ex.Message;
				Logger.LogError(ex, "{message}", message);
			}
			return View("Detail", instance);
		}

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(DNA3.Models.Login instance) {
			string message;
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.LoginId})";
				Context.Login.Remove(instance);
				await Context.SaveChangesAsync();
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
				return RedirectToAction("Index", new { id = HttpContext.Session.GetInt32("UserId") });
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("identityReturnUrl"));
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("userReturnUrl"));
        }

        // Change (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public IActionResult Change(int id) {
            DNA3.Models.Password instance = new();
            try {
                instance.LoginId = id;
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate Password Change");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View("Password", instance);
        }

        // Update (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(DNA3.Models.Password instance) {
            try {
                if (ModelState.IsValid) {
                    var hash = Utilities.Security.CreateHash(instance.Proposed);
                    var rows = await Context.Database.ExecuteSqlAsync($"UPDATE [Login] SET [Password]={hash} WHERE LoginId={instance.LoginId}");
                    if (rows != 1) {
                        throw new ArgumentException($"Password was not updated as expected, {rows} rows were changed.");
                    }
                    Site.Messages.Enqueue("Password changed successfully");
                    Log.Logger.ForContext("UserId", User.UserId()).Warning("Password Changed");
                    return RedirectToAction("Edit", new { id = instance.LoginId });
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View("Password", instance);
        }

        #endregion

        #region Remote Validation Methods

        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidPasswordOptions(string proposed) {
            string result = Utilities.Security.ValidPasswordOptions(proposed);
            if (result != "") {
                return Json(result);
            }
            return Json(true);
        }

        #endregion

    }

}
