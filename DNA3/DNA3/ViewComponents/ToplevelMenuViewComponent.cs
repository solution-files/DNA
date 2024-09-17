#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA3.ViewComponents {

    public class ToplevelMenuViewComponent : ViewComponent {

        #region Variables

        private readonly DNA3.Models.MainContext Context;
        private readonly ILogger<ToplevelMenuViewComponent> Logger;
        private IList<Models.Menu> model;

        #endregion

        #region Methods

        public ToplevelMenuViewComponent(Models.MainContext context, ILogger<ToplevelMenuViewComponent> logger) {
            Context = context;
            Logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            Task<IViewComponentResult> ComponentTask = null;
            try {
                model = await Context.Menu.Include(x => x.Role).OrderBy(x => x.Weight).Where(x => x.TopLevel == true).ToListAsync();
                ComponentTask = Task.FromResult((IViewComponentResult)View("ToplevelMenu", model));
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return await ComponentTask;
        }

        #endregion

    }

}
