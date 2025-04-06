#region Usings

using DNA3.Classes;
using DNA3.Models;
using EBay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

#endregion

namespace EBay.Controllers {

    #region Class

    [Area("EBay")]
    [Authorize(Policy = "Users")]
    public class AccountController : Controller {

        #region Variables

        private readonly ILogger<AccountController> Logger;

        #endregion

        #region Methods

        // Constructor
        public AccountController(ILogger<AccountController> logger) {
            Logger = logger;
        }

        #endregion

        #region Controller Actions

        [HttpGet, HttpPost]
        public IActionResult Index() {
            try {

            } catch(Exception ex) {

            }
            return View();
        }

        // Account Deletion or Closure Notification
        [HttpGet, HttpPost, HttpPut, HttpPatch]
        [AllowAnonymous]
        [Route("/EBAY/Account/ClosureNotification")]
        [Produces("application/json")]
        public IActionResult ClosureNotification(string challenge_code) {
            string challengeCode = challenge_code;
            string endpoint = "https://dotnetadmin.com/ebay/account/closurenotification";
            string verificationToken = "Z59ZXV0G75GAMPOO47VCPRQ22W85HGTL";
            string result;
            try {
                IncrementalHash sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
                sha256.AppendData(Encoding.UTF8.GetBytes(challengeCode));
                sha256.AppendData(Encoding.UTF8.GetBytes(verificationToken));
                sha256.AppendData(Encoding.UTF8.GetBytes(endpoint));
                byte[] bytes = sha256.GetHashAndReset();
                result = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
                Response.StatusCode = 200;
            } catch(Exception ex) {
                result = "";
                Logger.LogError(ex, ex.Message);
            }
            return Ok(new { StatusCode = StatusCodes.Status200OK, ChallengeResponse = result } );
        }

        #endregion

    }

    #endregion

}
