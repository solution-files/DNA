#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace DNA.ViewComponents {

    public class AlertsViewComponent(ILogger<AlertsViewComponent> logger) : ViewComponent {

        #region Variables

        private readonly ILogger<AlertsViewComponent> Logger = logger;

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync() {
            Task<IViewComponentResult>? ComponentTask = null;
            try {
                ComponentTask = Task.FromResult((IViewComponentResult)View("Alerts"));
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return await ComponentTask;
        }

        #endregion

    }

}
