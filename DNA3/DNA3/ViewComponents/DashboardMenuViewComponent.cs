#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA3.ViewComponents {

	public class DashboardMenuViewComponent(Models.MainContext context, ILogger<DashboardMenuViewComponent> logger) : ViewComponent {

		#region Variables

		private readonly DNA3.Models.MainContext Context = context;
		private readonly ILogger<DashboardMenuViewComponent> Logger = logger;
		private Models.Menu model;

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string MenuCode, string Class, string ShowIcon) {
			Task<IViewComponentResult> ComponentTask = null;
			try {
				ShowIcon ??= "False";
				ViewData["Class"] = Class;
				ViewData["ShowIcon"] = ShowIcon;
				model = await Context.Menu.Include(x => x.Actions).ThenInclude(x => x.Role).Where(x => x.Code == MenuCode).SingleOrDefaultAsync();
				ComponentTask = Task.FromResult((IViewComponentResult)View("DashboardMenu", model));
			} catch (Exception ex) {
				Logger.LogError(ex, "{message}", ex.Message);
			}
			return await ComponentTask;
		}

		#endregion

	}

}
