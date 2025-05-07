#region Usings

using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA3.Controllers {

    public class CheckoutController(PaypalClient paypalClient, MainContext context) : Controller {

        #region Variables

        private readonly PaypalClient _paypalClient = paypalClient;
        private readonly MainContext Context = context;

        #endregion

        #region Controller Actions

        public async Task<IActionResult> IndexAsync() {
            // ViewBag.ClientId is used to get the Paypal Checkout javascript SDK
            ViewBag.ClientId = _paypalClient.ClientId;
            Client instance = await Context.Client.FindAsync(User.ClientId());
            return View(instance);
        }

        [HttpPost]
        public async Task<IActionResult> Order(CancellationToken cancellationToken) {
            try {
                // set the transaction price and currency
                var price = "100.00";
                var currency = "USD";

                // "reference" is the transaction key
                var reference = "INV001";

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            } catch (Exception e) {
                var error = new {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken) {
            try {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key

                return Ok(response);
            } catch (Exception e) {
                var error = new {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public IActionResult Success() {
            return View();
        }

        #endregion

    }

}