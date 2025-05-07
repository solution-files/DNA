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
    public class ProductController : Controller {

		#region Variables

		// Variables
		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<ProductController> Logger;
        private readonly IHttpContextAccessor HttpContextAccessor;
		private readonly string Title = "Product";

		#endregion

		#region Class Methods

		// Constructor
		public ProductController(IConfiguration configuration, ILogger<ProductController> logger, IHttpContextAccessor httpContextAccessor) {
			Configuration = configuration;
			Context = new MainContext(new DbContextOptionsBuilder<MainContext>().Options, Configuration);
			Logger = logger;
            HttpContextAccessor = httpContextAccessor;
		}

		#endregion

		#region Controller Actions

		// Index
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Index() {
			string message;
			try {
				var result = await Context.Product.OrderBy(x => x.Price).ToListAsync();
				ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
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
		public async Task<IActionResult> NewAsync() {
			Product instance = new();
			try {
                HttpContextAccessor.HttpContext.Session.SetString("pageReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
			} catch (Exception ex) {
				string message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, "{message}", message);
			}
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
			return View("Detail", instance);
		}

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
			Product instance = new();
			try {
				instance = await Context.Product.FindAsync(id);
				ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
                HttpContextAccessor.HttpContext.Session.SetString("pagetReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.ProductId})");
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, "{message}", ex.Message);
			}
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
			return View("Detail", instance);
		}

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Save(Product instance) {
			string message;
			try {
				if (ModelState.IsValid) {
					if (instance.ProductId == 0) {
						Context.Add(instance);
						message = $"Added New {Title}";
					} else {
						Context.Update(instance);
						message = $"Updated {Title} ({instance.ProductId})";
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
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
			return View("Detail", instance);
		}

		// Delete
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Product instance) {
			string message;
			try {
				if (instance == null) {
					throw new Exception($"{Title} not found. It may have been deleted by another user.");
				}
				message = $"Deleted {Title} ({instance.ProductId})";
				Context.Product.Remove(instance);
				await Context.SaveChangesAsync();
				Site.Messages.Enqueue(message);
				Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
				return RedirectToAction("Index");
			} catch (Exception ex) {
				Site.Messages.Enqueue(ex.Message);
				Logger.LogError(ex, "{message}", ex.Message);
			}
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("productReturnUrl"));
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
