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

    public class TutorialController(IConfiguration configuration, MainContext context, ILogger<TutorialController> logger) : Controller {

		#region Variables

		private readonly IConfiguration Configuration = configuration;
		private readonly MainContext Context = context;
		private readonly ILogger<TutorialController> Logger = logger;

        #endregion

        #region Controller Actions

        // Index (Get)
        public async Task<IActionResult> IndexAsync() {
			string message;
			List<Article> instance = [];
			try {
				instance = await Context.Article.Include(x => x.Page).Include(x => x.Section).Include(x => x.Category).ToListAsync();
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, "{message}", message);
			}
			return View(instance);
		}

		#endregion

	}

}
