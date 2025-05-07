#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

	public class FeaturesController(MainContext context, ApplicationPartManager partmanager, ILogger<FeaturesController> logger, ITools tools) : Controller {

        #region Variables

        private readonly ApplicationPartManager PartManager = partmanager;
        private readonly ILogger<FeaturesController> Logger = logger;
        private readonly MainContext Context = context;
        private readonly ITools Tools = tools;
        private readonly string Title = "Feature";

        #endregion

        #region Controller Actions

        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
        public async Task<IActionResult> IndexAsync() {
            FeaturesView ViewModel = new();
            try {

                // Features Page
                ViewModel.FeaturesPage = await Context.Page.Where(x => x.Name == "Features").SingleOrDefaultAsync();
                ViewModel.FeaturesSection = await Context.Section.Where(x => x.Name == "Features Section").SingleOrDefaultAsync();
                ViewModel.FeaturesPage ??= new Page {
                        Subject = "Application Features",
                        Content = "Through the use of the application parts functionality of ASP.NET Core, Models, Views, and Controllers can be added via separate Projects. In this fashion, applications can be constructed to stand-alone or function as a part of a much larger solution."
                    };

                // Assemblies
                ViewModel.Assemblies = Tools.GetAssemblyList();

                // Controllers
                ControllerFeature ControllerFeature = new();
                PartManager.PopulateFeature(ControllerFeature);
                ViewModel.Controllers = [.. ControllerFeature.Controllers.OrderBy(x => x.Name)];

                // Tag Helpers
                TagHelperFeature TagHelperFeature = new();
                PartManager.PopulateFeature(TagHelperFeature);
                ViewModel.TagHelpers = [.. TagHelperFeature.TagHelpers.Where(x => !x.Namespace.Contains("Kendo") && !x.Namespace.Contains("Microsoft") && !x.Namespace.Contains("Syncfusion") && !x.Name.Contains("__")).OrderBy(x => x.Name)];

                // View Components
                ViewComponentFeature ViewComponentFeature = new();
                PartManager.PopulateFeature(ViewComponentFeature);
                ViewModel.ViewComponents = [.. ViewComponentFeature.ViewComponents];

                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");

            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View(ViewModel);
        }

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public IActionResult Close() {
			return RedirectToAction("Index", "Dashboard");
		}

        #endregion

    }

}