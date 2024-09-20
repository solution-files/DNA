#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    [Authorize(Policy = "Users")]
    public class CartController : Controller {

        #region Variables

        private readonly IConfiguration Configuration;
        private readonly MainContext Context;
        private readonly ILogger<CartController> Logger;

        #endregion

        #region Class Methods

        public CartController(IConfiguration configuration, MainContext context, ILogger<CartController> logger) {
            Configuration = configuration;
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        // Index (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public async Task<IActionResult> IndexAsync() {
            string message;
            Cart instance = new();
            instance.Items = new List<Item>();
            decimal? Total = 0;
            try {
                string returnurl = HttpContext.Request.Query["ReturnUrl"].ToString();
                instance = await Context.Cart.Include(x => x.Status).Include(x => x.Items).ThenInclude(x => x.Product).Where(x => x.CartId == User.ClientId()).SingleOrDefaultAsync();
                if (instance != null) {
                    foreach (Item item in instance.Items) {
                        Total += (item.Product.Price * item.Quantity);
                    }
                }
                ViewBag.Total = Total;
                if (instance == null) {
                    instance = new Cart {
                        Items = new List<Item>()
                    };
                }
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View(instance);
        }

        // Add (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public async Task<IActionResult> Add(int? id) {
            string message;
            Cart cart = default;
            Item item = default;
            try {

                cart = await Context.Cart.Include(x => x.Items).Where(x => x.CartId == User.ClientId()).FirstOrDefaultAsync();
                if (cart == null) {
                    cart = new Cart {
                        CartId = User.ClientId(),
                        UserId = User.UserId(),
                        Date = DateTime.Now,
                        Items = new List<Item>()
                    };
                    Context.Cart.Add(cart);
                }

                // If Product exists, increment quantity, otherwise add a new item to the Cart
                item = cart.Items.SingleOrDefault(x => x.ProductId == id);
                if (item != null) {
                    item.Quantity++;
                } else {
                    item = new Item {
                        CartId = cart.CartId,
                        ProductId = id,
                        Quantity = 1
                    };
                    cart.Items.Add(item);
                }

                // Update Database
                await Context.SaveChangesAsync();

            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

        // Remove (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public async Task<IActionResult> Remove(int? id) {
            string message;
            try {
                var instance = await Context.Item.Include(x => x.Product).Where(x => x.ItemId == id).FirstOrDefaultAsync();
                if (instance != null) {
                    message = $"Removed ({instance.Product.Name})";
                    Context.Item.Remove(instance);
                    await Context.SaveChangesAsync();
                    Site.Messages.Enqueue(message);
                    Log.Logger.ForContext("UserId", User.UserId()).Warning(message);
                }
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

        // Empty (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public async Task<IActionResult> EmptyContents() {
            string message;
            try {
                var instance = await Context.Cart.Include(x => x.Items).SingleAsync(x => x.CartId == User.ClientId());
                instance.Items.Clear();
                await Context.SaveChangesAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Warning("Emptied Shopping Cart");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

        // Close (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet, HttpPost]
        public IActionResult Close() {
            string message;
            try {

            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return RedirectToAction("Index");
        }

        #endregion

    }

}
