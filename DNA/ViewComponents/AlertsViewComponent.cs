#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace DNA.ViewComponents {

    public class AlertsViewComponent : ViewComponent {

        #region Variables

        private readonly ILogger<AlertsViewComponent> Logger;

        #endregion

        #region Methods

        public AlertsViewComponent(ILogger<AlertsViewComponent> logger) {
            Logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            Task<IViewComponentResult> ComponentTask = null;
            try {
                ComponentTask = Task.FromResult((IViewComponentResult)View("Alerts"));
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return await ComponentTask;
        }

        #endregion

    }

}
