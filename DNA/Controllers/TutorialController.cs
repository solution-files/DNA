#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA.Controllers {

    public class TutorialController : Controller {

		#region Variables

		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<TutorialController> Logger;

		#endregion

		#region Class Methods

		public TutorialController(IConfiguration configuration, MainContext context, ILogger<TutorialController> logger) {
			Configuration = configuration;
			Context = context;
			Logger = logger;
		}

		#endregion

		#region Controller Actions

		// Index (Get)
		public async Task<IActionResult> IndexAsync() {
			string message;
			List<Article> instance = new List<Article>();
			try {
				instance = await Context.Article.Include(x => x.Page).Include(x => x.Section).Include(x => x.Category).ToListAsync();
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View(instance);
		}

		#endregion

	}

}
