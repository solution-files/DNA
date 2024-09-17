#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA.Controllers {

    public class ProductsController : Controller {

        #region Variables

        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<ProductsController> Logger;

        #endregion

        #region Class Methods

        public ProductsController(IConfiguration configuration, MainContext context, ILogger<ProductsController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Detail (Get)
        public async Task<IActionResult> Detail(int? id) {
            string message;
            Product instance = new Product();
            try {
                instance = await Context.Product.Include(x => x.Status).Where(x => x.ProductId == id).SingleOrDefaultAsync();
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
