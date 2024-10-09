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

	public class FeaturesController : Controller {

        #region Variables

        private readonly ApplicationPartManager PartManager;
        private readonly ILogger<FeaturesController> Logger;
        private readonly MainContext Context;
        private readonly ITools Tools;
        private readonly string Title = "Feature";

        #endregion

        #region Class Events

        // Constructor
        public FeaturesController(MainContext context, ApplicationPartManager partmanager, ILogger<FeaturesController> logger, ITools tools) {
            Context = context;
            PartManager = partmanager;
            Logger = logger;
            Tools = tools;
        }

		#endregion

		#region Controller Actions

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
        public async Task<IActionResult> IndexAsync() {
            FeaturesView ViewModel = new FeaturesView();
            try {

                // Features Page
                ViewModel.FeaturesPage = await Context.Page.Where(x => x.Name == "Features").SingleOrDefaultAsync();
                ViewModel.FeaturesSection = await Context.Section.Where(x => x.Name == "Features Section").SingleOrDefaultAsync();
                if (ViewModel.FeaturesPage == null) {
                    ViewModel.FeaturesPage = new Page {
                        Subject = "Application Features",
                        Content = "Through the use of the application parts functionality of ASP.NET Core, Models, Views, and Controllers can be added via separate Projects. In this fashion, applications can be constructed to stand-alone or function as a part of a much larger solution."
                    };
                }

                // Assemblies
                ViewModel.Assemblies = Tools.GetAssemblyList();

                // Controllers
                ControllerFeature ControllerFeature = new ControllerFeature();
                PartManager.PopulateFeature(ControllerFeature);
                ViewModel.Controllers = ControllerFeature.Controllers.OrderBy(x => x.Name).ToList();

                // Tag Helpers
                TagHelperFeature TagHelperFeature = new TagHelperFeature();
                PartManager.PopulateFeature(TagHelperFeature);
                ViewModel.TagHelpers = TagHelperFeature.TagHelpers.Where(x => !x.Namespace.Contains("Kendo") && !x.Namespace.Contains("Microsoft") && !x.Namespace.Contains("Syncfusion") && !x.Name.Contains("__")).OrderBy(x => x.Name).ToList();

                // View Components
                ViewComponentFeature ViewComponentFeature = new ViewComponentFeature();
                PartManager.PopulateFeature(ViewComponentFeature);
                ViewModel.ViewComponents = ViewComponentFeature.ViewComponents.ToList();

                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} List");

            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
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