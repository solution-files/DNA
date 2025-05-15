#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA.ViewComponents {

	public class FooterMenuViewComponent(DNA3.Models.MainContext context, ILogger<FooterMenuViewComponent> logger) : ViewComponent {

		#region Variables

		private readonly DNA3.Models.MainContext Context = context;
		private readonly ILogger<FooterMenuViewComponent> Logger = logger;
		private DNA3.Models.Menu? model;

        #endregion

        #region Methods
        public async Task<IViewComponentResult> InvokeAsync(string MenuCode, string Class, string ShowIcon) {
			Task<IViewComponentResult>? ComponentTask = null;
			try {
				ShowIcon ??= "False";
				ViewData["Class"] = Class;
				ViewData["ShowIcon"] = ShowIcon;
				model = await Context.Menu.Include(x => x.Actions).Where(x => x.Code == MenuCode).SingleOrDefaultAsync();
				ComponentTask = Task.FromResult((IViewComponentResult)View("FooterMenu", model));
			} catch (Exception ex) {
				Logger.LogError(ex, "{message}", ex.Message);
			}
			return await ComponentTask;
		}

		#endregion

	}

}
