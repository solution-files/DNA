#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

#endregion

namespace DNA3.ViewComponents {

    public class MessagesViewComponent(ILogger<MessagesViewComponent> logger) : ViewComponent {

        #region Variables

        private readonly ILogger<MessagesViewComponent> Logger = logger;

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync() {
            Task<IViewComponentResult> ComponentTask = null;
            try {
                ComponentTask = Task.FromResult((IViewComponentResult)View("Messages"));
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return await ComponentTask;
        }

        #endregion

    }

}
