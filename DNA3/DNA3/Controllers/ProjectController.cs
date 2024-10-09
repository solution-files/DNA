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

    [Authorize(Policy = "Users")]
    public class ProjectController : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<ProjectController> Logger;
        private readonly string Title = "Project";

        #endregion

        #region Class Methods

        // Constructor
        public ProjectController(IConfiguration configuration, ILogger<ProjectController> logger) {
            Configuration = configuration;
            Context = new MainContext(new DbContextOptionsBuilder<MainContext>().Options, Configuration);
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
                var result = await Context.Project.OrderBy(x => x.Date).ToListAsync();
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
        public async Task<IActionResult> NewAsync() {
            Project instance = new();
            try {
                instance.Date = DateTime.Now;
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Initiate New {Title}");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

        // Edit (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            Project instance = new();
            try {
                instance = await Context.Project.FindAsync(id);
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} ({instance.ProjectId})");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

        // Save (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(Project instance) {
            string message;
            try {
                if (ModelState.IsValid) {
                    if (instance.ProjectId == 0) {
                        Context.Add(instance);
                        message = $"Added New {Title}";
                    } else {
                        Context.Update(instance);
                        message = $"Updated {Title} ({instance.ProjectId})";
                    }
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                    return RedirectToAction("Edit", "Project", new { id = instance.ProjectId });
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
            return View("Detail", instance);
        }

        // Delete
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Project instance) {
            string message;
            try {
                if (instance == null) {
                    throw new Exception($"{Title} not found. It may have been deleted by another user.");
                }
                Context.Project.Remove(instance);
                await Context.SaveChangesAsync();
                message = $"Deleted {Title} ({instance.ProjectId})";
                Site.Messages.Enqueue(message);
                Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                return RedirectToAction("Index");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
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

    }

    #endregion

}
