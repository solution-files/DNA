#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
    public class ClientController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<ClientController> Logger;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly string Title = "Client";
        private readonly string returnUrl;

        #endregion

        #region Class Methods and Events

        // Constructor
        public ClientController(IConfiguration configuration, MainContext context, ILogger<ClientController> logger, IHttpContextAccessor httpcontextaccessor) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
            HttpContextAccessor = httpcontextaccessor;
        }

        #endregion

        #region Controller Actions

        // Index
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Index() {
            string message;
            try {
                var result = await Context.Client.ToListAsync();
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
		public IActionResult New() {
            Client client = new();
            HttpContextAccessor.HttpContext.Session.SetString("clientReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
            return View("Detail", client);
        }

		// Edit (Get)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Edit(int? id) {
            Client client = new();
            try {
                client = await Context.Client.FindAsync(id);
                HttpContextAccessor.HttpContext.Session.SetString("clientReturnUrl", HttpContextAccessor.HttpContext.Request.Headers.Referer.ToString());
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
            ViewData["Users"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.ClientId == User.ClientId()).OrderBy(x => x.First).OrderBy(x => x.Last).ToListAsync();
            return View("Detail", client);
        }

		// Save (Post)
		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Client instance) {
            string message;
            try {
                if (ModelState.IsValid) {

                    // Make sure the phone number is an unformatted, numeric value
                    instance.Phone = Utilities.Strings.NumericValue(instance.Phone);

                    if (instance.ClientId == 0) {

                        // Make sertain the address is USPS validated
                        if (String.IsNullOrEmpty(instance.City) || String.IsNullOrEmpty(instance.State)) {
                            throw new ArgumentException("Please verify the mailing address before attempting to save a new record");
                        }

                        // Make certain the address is unique
                        List<Client> result = await Context.Client.Where(x => x.Address1 == instance.Address1 && x.Zip == instance.Zip).ToListAsync();
                        if (result.Count > 0) {
                            throw new ArgumentException("Client already exists at this address, please locate and edit the existing record");
                        }

                        Context.Add(instance);
                        message = $"Added New {Title}";

                    } else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.ClientId})";
                    }
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    return RedirectToAction("Edit", "Client", new { id = instance.ClientId });
                }
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
			ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
			ViewData["Users"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.ClientId == User.ClientId()).OrderBy(x => x.First).OrderBy(x => x.Last).ToListAsync();
            return View("Detail", instance);
        }

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Delete(Client instance) {
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                string message = $"Deleted {Title} ({instance.ClientId})";
                Context.Client.Remove(instance);
                await Context.SaveChangesAsync();
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View("Detail", instance);
        }

        // Clean
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Clean(Client instance) {
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE [Login] FROM [Login] AS l JOIN [User] AS u ON l.UserId = u.UserId WHERE u.ClientId = {instance.ClientId}");
                await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE [User] FROM [User] AS u WHERE u.ClientId = {instance.ClientId}");
                Context.Client.Remove(instance);
                await Context.SaveChangesAsync();
                string message = $"Deleted Client ({instance.ClientId}) with Users and Identites";
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View("Detail", instance);
        }

        // Verify
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Verify(Client instance) {
            string message;
            try {
                USPSAddress request = new() {
                    Address1 = instance.Address1,
                    City = instance.City,
                    State = instance.State,
                    Zip5 = instance.Zip
                };

                USPSAddress result = AddressApi.Standardize(request);
                ModelState["Address1"].RawValue = result.Address2;
                ModelState["City"].RawValue = result.City;
                ModelState["State"].RawValue = result.State;
                ModelState["Zip"].RawValue = result.Zip5;
                ModelState["Zip1"].RawValue = result.Zip4;

            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
            ViewData["Users"] = await Context.User.Include(x => x.Role).Include(x => x.Status).Where(x => x.ClientId == User.ClientId()).OrderBy(x => x.First).OrderBy(x => x.Last).ToListAsync();
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
            return RedirectPermanent(HttpContextAccessor.HttpContext.Session.GetString("clientReturnUrl"));
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
