#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA.ViewComponents {

	public class AboutUsViewComponent : ViewComponent {

		#region Variables

		private readonly DNA3.Models.MainContext Context;
		private readonly ILogger<AboutUsViewComponent> Logger;
		private DNA3.Models.Page model;

		#endregion

		#region Methods

		public AboutUsViewComponent(DNA3.Models.MainContext context, ILogger<AboutUsViewComponent> logger) {
			Context = context;
			Logger = logger;
		}

		public async Task<IViewComponentResult> InvokeAsync() {
			Task<IViewComponentResult> ComponentTask = null;
			try {
				model = await Context.Page.Where(x => x.Name == "Introduction").SingleOrDefaultAsync();
				ComponentTask = Task.FromResult((IViewComponentResult)View("AboutUs", model));
			} catch (Exception ex) {
				Logger.LogError(ex, ex.Message);
			}
			return await ComponentTask;
		}

		#endregion

	}

}
