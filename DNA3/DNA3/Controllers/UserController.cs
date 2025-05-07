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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    public class UserController(IConfiguration configuration, MainContext context, ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor) : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration = configuration;
		private readonly MainContext Context = context;
		private readonly ILogger<UserController> Logger = logger;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
		private readonly string Title = "User";

        #endregion

        #region Controller Actions

        // Index
        [HttpGet, HttpPost]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
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
				Logger.LogError(ex, "{message}", message);
			}
			return RedirectToAction("Index", "Dashboard");
		}

        // New (Get)
        [HttpGet]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> New() {
			User instance = new();
			try {
				instance.ClientId = User.ClientId();
				ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
				ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
				ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
                HttpContextAccessor.HttpContext.Session.SetString("userReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
			} catch (Exception ex) {
				string message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, "{message}", message);
			}
			return View("Detail", instance);
		}

        // Edit (Get)
        [HttpGet]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult Edit(int? id) {
			User instance = new();
			try {
                instance = Context.User.Where(x => x.UserId == id).SingleOrDefault();
                ViewBag.ClientList = Context.Client.OrderBy(x => x.Company).ToList();
				ViewBag.RoleList = Context.Role.OrderBy(x => x.Description).ToList();
				ViewBag.StatusList = Context.Status.OrderBy(x => x.Description).ToList();
                HttpContextAccessor.HttpContext.Session.SetString("userReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.UserId})");
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, "{message}", ex.Message);
			}
			return View("Detail", instance);
		}

        // Save (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
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
				Logger.LogError(ex, "{message}", message);
			}
			ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
			ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
			return View("Detail", instance);
		}

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrators")]
        [ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> Delete(User instance) {
			string message = "";
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.UserId}) and all its identities";
                var identities = Context.Login.Where(x => x.UserId == instance.UserId);
                Context.RemoveRange(identities);
				Context.User.Remove(instance);
				await Context.SaveChangesAsync();
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
				return RedirectToAction("Index");
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, "{message}", ex.Message);
			}
			ViewBag.ClientList = await Context.Client.OrderBy(x => x.Company).ToListAsync();
			ViewBag.RoleList = await Context.Role.OrderBy(x => x.Description).ToListAsync();
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Description).ToListAsync();
			return View("Detail", instance);
		}

        // Profile (Get)
        [HttpGet]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> Profile() {
			string message;
			Login instance = new();
			try {
				instance = await Context.Login.Include(x => x.User).ThenInclude(x => x.Client).Where(x => x.Email == User.EmailAddress()).FirstOrDefaultAsync();
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, "{message}", message);
			}
			return View(instance);
		}

        // Profile (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
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
				Logger.LogError(ex, "{message}", message);
			}
			return View(instance);
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("userReturnUrl"));
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
