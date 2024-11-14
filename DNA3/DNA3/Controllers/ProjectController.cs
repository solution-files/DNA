#region Usings

using BoldReports;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
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
        public ProjectController(IConfiguration configuration, MainContext context, ILogger<ProjectController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        #region Inline Editing

        // Create
        public async Task<JsonResult> Create([FromBody] SyncfusionGrid<Project> instance) {
            string message;
            try {
                if (ModelState.IsValid) {
                    Context.Add(instance.value);
                    await Context.SaveChangesAsync();
                    message = $"Updated {Title} ({instance.value.ProjectId})";
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            return Json(instance.value);
        }

        // Read (Inline)
        public IActionResult Read([FromBody] DataManagerRequest dm) {
            IEnumerable DataSource = Context.Project.OrderBy(x => x.ProjectId).ToList();
            DataOperations operation = new DataOperations();
            int count = DataSource.Cast<Project>().Count();
            if (dm.Skip != 0) {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0) {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            ViewBag.StatusList = Context.Status.OrderBy(x => x.Name).ToList();
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }

        // Update
        public async Task<JsonResult> Update([FromBody] SyncfusionGrid<Project> instance) {
            string message;
            try {
                if (ModelState.IsValid) {
                    Context.Update(instance.value);
                    await Context.SaveChangesAsync();
                    message = $"Updated {Title} ({instance.value.ProjectId}) with StatusId: {instance.value.StatusId} of type {instance.value.StatusId.GetType().ToString()}";
                }
            } catch(Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            return Json(instance.value);
        }

        // Remove
        public async Task<JsonResult> Remove([FromBody] SyncfusionGrid<Project> instance) {
            string message;
            try {
                if (ModelState.IsValid) {
                    Context.Remove(instance.value);
                    await Context.SaveChangesAsync();
                    message = $"Removed {Title} ({instance.value.ProjectId})";
                }
            } catch (Exception ex) {
                message = ex.Message;
                Logger.LogError(ex, message);
            }
            return Json(instance.value);
        }

        #endregion

        #region Page Editing

        // Index
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<IActionResult> Index() {
            string message;
            try {
                var result = await Context.Project.OrderBy(x => x.Date).ToListAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");
                ViewBag.StatusList = await Context.Status.OrderBy(x => x.Name).ToListAsync();
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
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
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

        #endregion

        #region Common Actions

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

        #endregion

    }


}
