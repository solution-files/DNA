#region Usings

using BoldReports;
using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace Schedule.Controllers {

    [Area("Schedule")]
    [Authorize(Policy = "Users")]
    public class CalendarController(IConfiguration configuration, MainContext context, ILogger<CalendarController> logger, IDNATools dnatools) : Controller {

        #region Variables

        // Variables
        private readonly IConfiguration Configuration = configuration;
        private readonly MainContext Context = context;
        private readonly ILogger<CalendarController> Logger = logger;
        private readonly IDNATools DNATools = dnatools;
        private readonly string Title = "Calendar";

        #endregion

        #region Controller Actions

        #region Page Editing

        // Index
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() {
            string message;
            try {
                List<AppointmentData> result =
                [
                    new AppointmentData { Id = 1, Subject = "Explosion of Betelgeuse Star", StartTime = new DateTime(2025, 4, 11, 9, 30, 0), EndTime = new DateTime(2025, 4, 11, 11, 0, 0) },
                    new AppointmentData { Id = 2, Subject = "Thule Air Crash Report", StartTime = new DateTime(2025, 4, 12, 12, 0, 0), EndTime = new DateTime(2025, 4, 12, 14, 0, 0) },
                    new AppointmentData { Id = 3, Subject = "Blue Moon Eclipse", StartTime = new DateTime(2025, 4, 13, 9, 30, 0), EndTime = new DateTime(2025, 4, 13, 11, 0, 0) },
                    new AppointmentData { Id = 4, Subject = "Meteor Showers in 2022", StartTime = new DateTime(2025, 4, 14, 13, 0, 0), EndTime = new DateTime(2025, 4, 14, 14, 30, 0) },
                    new AppointmentData { Id = 5, Subject = "Milky Way as Melting pot", StartTime = new DateTime(2025, 4, 15, 12, 0, 0), EndTime = new DateTime(2025, 4, 15, 14, 0, 0) },
                ];
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} Overview");
                return View(result);
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        #endregion

        #region Common Actions

        // Close
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Close() {
            string message;
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"Closed {Title}");
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, "{message}", message);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        #endregion

        #endregion

    }


}
