#region Usings

using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

#endregion

[Area("API")]
public class ReportViewerController : Controller, IReportController {

    #region Variables

    // Report Viewer requires a memory cache to store the information of consecutive client requests 
    //and have the rendered Report Viewer information in the server.
    private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

    // IWebHostEnvironment used with sample to get the application data from wwwroot.
    private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

    #endregion

    #region Methods

    // Post action to process the report from server-based JSON parameters and send the result back to the client.
    public ReportViewerController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache,
    Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment) {
        _cache = memoryCache;
        _hostingEnvironment = hostingEnvironment;
    }

    #endregion

    #region Controller Actions

    // Post action to process the report from server-based JSON parameters and send the result back to the client.
    [HttpPost]
    public object PostReportAction([FromBody] Dictionary<string, object> jsonArray) {
        //Contains helper methods that help process a Post or Get request from the Report Viewer control and return the response to the Report Viewer control.
        return ReportHelper.ProcessReport(jsonArray, this, this._cache);
    }

    // Method will be called to initialize the report information to load the report with ReportHelper for processing.
    [NonAction]
    public void OnInitReportOptions(ReportViewerOptions reportOption) {
        string basePath = _hostingEnvironment.WebRootPath;
        // Here, we have loaded the sales-order-detail.rdl report from the application folder wwwroot\Report. The sales-order-detail.rdl file should be in the wwwroot\Report application folder.
        FileStream inputStream = new FileStream(basePath
        + reportOption.ReportModel.ReportPath,
        FileMode.Open, FileAccess.Read);
        MemoryStream reportStream = new MemoryStream();
        inputStream.CopyTo(reportStream);
        reportStream.Position = 0;
        inputStream.Close();
        reportOption.ReportModel.Stream = reportStream;
    }

    // Method will be called when report is loaded internally to start to layout process with ReportHelper.
    [NonAction]
    public void OnReportLoaded(ReportViewerOptions reportOption) {
    }

    //Get action for getting resources from the report.
    [ActionName("GetResource")]
    [AcceptVerbs("GET")]
    // Method will be called from Report Viewer client to get the image src for the Image report item.
    public object GetResource(ReportResource resource) {
        return ReportHelper.GetResource(resource, this, _cache);
    }

    [HttpPost]
    public object PostFormReportAction() {
        return ReportHelper.ProcessReport(null, this, _cache);
    }

    #endregion

}
