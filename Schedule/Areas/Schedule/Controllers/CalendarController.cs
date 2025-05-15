#region Usings

using BoldReports;
using DNA3.Classes;
using DNA3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Schedule.Models;
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

        // Index
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index() {
            try {
                Log.Logger.ForContext("UserId", User.UserId()).Warning($"View {Title} Overview");
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return View();
        }

        #endregion

        #region Scheduler Methods

        [HttpPost]
        public async Task<List<Appointment>> LoadData([FromBody] Params param) {
            List<Appointment> result = [];
            try {
                DateTime start = (param.CustomStart != new DateTime()) ? param.CustomStart : param.StartDate;
                DateTime end = (param.CustomEnd != new DateTime()) ? param.CustomEnd : param.EndDate;
                ViewBag.AppointmentTypes = await Context.AppointmentType.OrderBy(x => x.Name).ToListAsync();
                result = [.. Context.Appointment.Where(app => (app.StartTime >= start && app.StartTime <= end) || (app.RecurrenceRule != null && app.RecurrenceRule != ""))];
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        [HttpPost]
        public List<Appointment> UpdateData([FromBody] EditParams param) {
            try {
                if (param.action == "insert" || (param.action == "batch" && param.added.Count > 0)) {
                    int intMax = Context.Appointment.ToList().Count > 0 ? Context.Appointment.ToList().Max(p => p.AppointmentId) : 1;
                    for (var i = 0; i < param.added.Count; i++) {
                        var value = (param.action == "insert") ? param.value : param.added[i];
                        Appointment appt = new() {
                            StartTime = value.StartTime.ToUniversalTime(),
                            EndTime = value.EndTime.ToUniversalTime(),
                            Subject = value.Subject,
                            Location = value.Location,
                            AllDay = value.AllDay,
                            StartTimeZone = value.StartTimeZone,
                            EndTimeZone = value.EndTimeZone,
                            RecurrenceRule = value.RecurrenceRule,
                            Description = value.Description,
                        };
                        Context.Appointment.Add(appt);
                        Context.SaveChanges();
                    }
                }
                if (param.action == "update" || (param.action == "batch" && param.changed.Count > 0)) {
                    var value = (param.action == "update") ? param.value : param.changed[0];
                    var filterData = Context.Appointment.Where(c => c.AppointmentId == Convert.ToInt32(value.AppointmentId));
                    if (filterData.Any()) {
                        Appointment appt = Context.Appointment.Single(A => A.AppointmentId == Convert.ToInt32(value.AppointmentId));
                        appt.StartTime = value.StartTime.ToLocalTime();
                        appt.EndTime = value.EndTime.ToLocalTime();
                        appt.StartTimeZone = value.StartTimeZone;
                        appt.EndTimeZone = value.EndTimeZone;
                        appt.Subject = value.Subject;
                        appt.Location = value.Location;
                        appt.AllDay = value.AllDay;
                        appt.RecurrenceRule = value.RecurrenceRule;
                        appt.Description = value.Description;
                    }
                    Context.SaveChanges();
                }
                if (param.action == "remove" || (param.action == "batch" && param.deleted.Count > 0)) {
                    if (param.action == "remove") {
                        int key = Convert.ToInt32(param.key);
                        Appointment? appt = Context.Appointment.Where(c => c.AppointmentId == key).FirstOrDefault();
                        if (appt != null) Context.Appointment.Remove(appt);
                    } else {
                        foreach (var apps in param.deleted) {
                            Appointment? appt = Context.Appointment.Where(c => c.AppointmentId == apps.AppointmentId).FirstOrDefault();
                            if (appt != null) Context.Appointment.Remove(appt);
                        }
                    }
                    Context.SaveChanges();
                }
            } catch(Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return [.. Context.Appointment];
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

    }

}
