#region Usings

using DNA3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utilities;

#endregion

namespace DNA.Controllers {

    public class HomeController : Controller {

		#region Variables

		private readonly IConfiguration Configuration;
		private readonly MainContext Context;
		private readonly ILogger<HomeController> Logger;
        private readonly DNA3.Classes.IDNATools DNATools;

		#endregion

		#region Methods

		public HomeController(IConfiguration configuration, MainContext context, ILogger<HomeController> logger, DNA3.Classes.IDNATools dnatools) {
			Configuration = configuration;
			Context = context;
			Logger = logger;
            DNATools = dnatools;
		}

		#endregion

		#region Controller Actions

		// Index (Get)
		public async Task<IActionResult> IndexAsync(){
			string message;
			try {
                await DNATools.Initialize();
                ViewBag.HomePage = await Context.Page.Where(x => x.Name == "Home").FirstOrDefaultAsync();
                ViewBag.FeaturesPage = await Context.Page.Where(x => x.Name == "Features").FirstOrDefaultAsync();
                ViewBag.FeaturesList = await Context.Article.Where(x => x.Page.Name == "Home" && x.Section.Name == "Features").OrderBy(x => x.Weight).ToListAsync();
                ViewBag.AboutPage = await Context.Page.Where(x => x.Name == "About").FirstOrDefaultAsync();
                ViewBag.ProductsPage = await Context.Page.Where(x => x.Name == "Products").FirstOrDefaultAsync();
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
                ViewBag.TestimonialsPage = await Context.Page.Where(x => x.Name == "Testimonials").FirstOrDefaultAsync();
                ViewBag.TeamPage = await Context.Page.Where(x => x.Name == "Team").FirstOrDefaultAsync();
                ViewBag.ContactPage = await Context.Page.Where(x => x.Name == "Contact").FirstOrDefaultAsync();
			} catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View();
		}

		// About (Get)
		public async Task<IActionResult> AboutAsync() {
			string message;
			try {
				ViewBag.AboutPage = await Context.Page.Where(x => x.Name == "About").FirstOrDefaultAsync();
                ViewBag.TeamPage = await Context.Page.Where(x => x.Name == "Team").FirstOrDefaultAsync();
            } catch (Exception ex) {
				message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View();
		}

        // Contact (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<IActionResult> ContactAsync() {
            Request instance = new();
            try {
                ViewBag.ContactPage = await Context.Page.Where(x => x.Name == "Contact").FirstOrDefaultAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Information($"Initiate New Contact Form");
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View(instance);
        }

        // Contact (Post)
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([FromForm] Request request) {
            try {
                if (ModelState.IsValid) {
                    Context.Request.Add(request);
                    Context.SaveChanges();
                    if (SendMessage(request)) {
                        Site.Messages.Enqueue("Your request was sent successfully");
                        return RedirectToAction("Index");
                    }
                };
            } catch (Exception ex) {
                Site.Messages.Enqueue(ex.Message);
                Logger.LogError(ex, ex.Message);
            }
            ViewBag.ContactPage = await Context.Page.Where(x => x.Name == "Contact").FirstOrDefaultAsync();
            ViewBag.FeatureList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "App Features").OrderBy(x => x.Weight).ToListAsync();
            ViewBag.SolutionSection = await Context.Section.Where(x => x.Name == "Solution Features").SingleOrDefaultAsync();
            ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            return View(request);
        }

        // Features (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> FeaturesAsync() {
			Request instance = new();
			try {
                ViewBag.FeaturesPage = await Context.Page.Where(x => x.Name == "Features").FirstOrDefaultAsync();
                ViewBag.FeaturesList = await Context.Article.Where(x => x.Page.Name == "Home" && x.Section.Name == "Features").OrderBy(x => x.Weight).ToListAsync();
                ViewBag.TeamPage = await Context.Page.Where(x => x.Name == "Team").FirstOrDefaultAsync();
                Log.Logger.ForContext("UserId", User.UserId()).Information("View Features Page");
			} catch (Exception ex) {
				string message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View(instance);
		}

        // FAQ (Get)
        public async Task<IActionResult> FAQAsync() {
            string message;
            try {
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            } catch (Exception ex) {
                message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View();
        }
        
        // Pricing (Get)
        [ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet]
		public async Task<IActionResult> Pricing() {
			Request instance = new();
			try {
				ViewBag.ProductsPage = await Context.Page.Where(x => x.Name == "Products").FirstOrDefaultAsync();
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            } catch (Exception ex) {
				string message = ex.Message;
				Site.Messages.Enqueue(message);
				Logger.LogError(ex, message);
			}
			return View(instance);
		}

        // Privacy Policy
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("/privacy")]
        public async Task<IActionResult> Privacy() {
            try {
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View();
        }

        // Refund Policy
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("/refund")]
        public async Task<IActionResult> Refund() {
            try {
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View();
        }

        // Terms of Service
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("/terms")]
        public async Task<IActionResult> Terms() {
            try {
                ViewBag.FAQPage = await Context.Page.Where(x => x.Name == "FAQ").FirstOrDefaultAsync();
                ViewBag.FAQList = await Context.Article.Include(x => x.Section).Where(x => x.Section.Name == "FAQ").ToListAsync();
            } catch (Exception ex) {
                string message = ex.Message;
                Site.Messages.Enqueue(message);
                Logger.LogError(ex, message);
            }
            return View();
        }

        // Error
        [ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult Error(int? id) {
			DNA.Models.Error instance = new();
			switch (id) {
				case 400:
					instance.Code = 400;
					instance.Name = "Bad Request";
					instance.Description = "The server cannot or will not process the request due to something that is perceived to be a client error (e.g., malformed request syntax, invalid request message framing, or deceptive request routing).";
					break;
				case 401:
					instance.Code = 401;
					instance.Name = "Unauthorized";
					instance.Description = "Although the HTTP standard specifies unauthorized, semantically this response means unauthenticated. That is, the client must authenticate itself to get the requested response.";
					break;
				case 402:
					instance.Code = 402;
					instance.Name = "Payment Required";
					instance.Description = "This response code is reserved for future use. The initial aim for creating this code was using it for digital payment systems, however this status code is used very rarely and no standard convention exists.";
					break;
				case 403:
					instance.Code = 403;
					instance.Name = "Forbidden";
					instance.Description = "The client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401 Unauthorized, the client's identity is known to the server.";
					break;
				case 404:
					instance.Code = 404;
					instance.Name = "Not Found";
					instance.Description = "The server can not find the requested resource. In the browser, this means the URL is not recognized. In an API, this can also mean that the endpoint is valid but the resource itself does not exist. Servers may also send this response instead of 403 Forbidden to hide the existence of a resource from an unauthorized client. This response code is probably the most well known due to its frequent occurrence on the web.";
					break;
				case 405:
					instance.Code = 405;
					instance.Name = "Method Not Allowed";
					instance.Description = "The request method is known by the server but is not supported by the target resource. For example, an API may not allow calling DELETE to remove a resource.";
					break;
				case 406:
					instance.Code = 406;
					instance.Name = "Not Acceptable";
					instance.Description = "This response is sent when the web server, after performing server-driven content negotiation, doesn't find any content that conforms to the criteria given by the user agent.";
					break;
				case 407:
					instance.Code = 407;
					instance.Name = "Proxy Authentication Required";
					instance.Description = "This is similar to 401 Unauthorized but authentication is needed to be done by a proxy.";
					break;
				case 408:
					instance.Code = 408;
					instance.Name = "Request Timeout";
					instance.Description = "This response is sent on an idle connection by some servers, even without any previous request by the client. It means that the server would like to shut down this unused connection. This response is used much more since some browsers, like Chrome, Firefox 27+, or IE9, use HTTP pre-connection mechanisms to speed up surfing. Also note that some servers merely shut down the connection without sending this message.";
					break;
				case 409:
					instance.Code = 409;
					instance.Name = "Conflict";
					instance.Description = "This response is sent when a request conflicts with the current state of the server.";
					break;
				case 410:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This response is sent when the requested content has been permanently deleted from server, with no forwarding address. Clients are expected to remove their caches and links to the resource. The HTTP specification intends this status code to be used for ; limited - time, promotional services. APIs should not feel compelled to indicate resources that have been deleted with this status code.";
					break;
				case 411:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 412:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 413:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 414:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 415:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 416:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				case 417:
					instance.Code = 410;
					instance.Name = "Gone";
					instance.Description = "This";
					break;
				default:
					break;
			}
			return View(instance);
		}

        #endregion

        #region Common Methods

        // Build Message Body
        private string BuildMessageBody(Request request) {
            StringBuilder result = new();
            try {
                result = result.AppendLine($"<p>The following message was sent from {Configuration["Application:Domain"]} by {request.First} {request.Last}");
                result = result.AppendLine($"on {String.Format(DateTime.Now.ToString(), "D")}</p>");
                result = result.AppendLine(@$"<table>");
                result = result.AppendLine($"<tr><td>Host Name:</td><td>{Dns.GetHostName()}</td></tr>");
                result = result.AppendLine($"<tr><td>IP Address:</td><td>{Request.HttpContext.Connection.RemoteIpAddress}</td></tr>");
                result = result.AppendLine($"</table>");
                result = result.AppendLine($"<p>{request.Content}</p>");
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result.ToString();
        }

        // Send Message
        private bool SendMessage(Request request) {
            bool result = true;
            try {
                MailMessage message = new() {
                    From = new MailAddress(Configuration["Smtp:From"], Configuration["Application:SalesName"]),
                    Subject = $"{Configuration["Application:Shortname"]} - {request.Subject}",
                    Body = BuildMessageBody(request),
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true
                };
                message.To.Add(new MailAddress(Configuration["Smtp:From"], Configuration["Application:SalesName"]));
                message.CC.Add(new MailAddress(request.Email, $"{request.First} {request.Last}"));
                message.ReplyToList.Add(new MailAddress(request.Email, $"{request.First} {request.Last}"));

                NetworkCredential credentials = new(Configuration["Smtp:User"], Configuration["Smtp:Password"]);

                SmtpClient smtpclient = new() {
                    Host = Configuration["Smtp:Host"],
                    Port = Convert.ToInt16(Configuration["Smtp:Port"]),
                    Credentials = credentials,
                    EnableSsl = true
                };

                smtpclient.Send(message);

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                result = false;
            }
            return result;
        }

        #endregion

    }

}
