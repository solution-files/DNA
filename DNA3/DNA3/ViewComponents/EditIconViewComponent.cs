#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace DNA3.ViewComponents {

    public class EditIconViewComponent(ILogger<EditIconViewComponent> logger) : ViewComponent {

        #region Variables

        private readonly ILogger<EditIconViewComponent> Logger = logger;

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string controller, string action, string id, string css, string title) {
            Task<IViewComponentResult> ComponentTask = null;
            try {
                ViewData["Controller"] = controller;
                ViewData["Action"] = action;
                ViewData["Id"] = id;
                ViewData["Css"] = css;
                ViewData["Title"] = title;
                ComponentTask = Task.FromResult((IViewComponentResult)View("EditIcon"));
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return await ComponentTask;
        }

        #endregion

    }

}
