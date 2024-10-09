#region Usings

using DNA3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Action = DNA3.Models.Action;

#endregion

namespace DNA3.Classes {

    #region Interface

    public interface IDNATools {

        Task<int> GetSourceKeyValue(string value);
        Task<int> GetDispositionKeyValue(string value);
        Task<int> GetPageKeyValue(string value);
        Task<int> GetSectionKeyValue(string value);
        Task<int> GetCategoryKeyValue(string value);
        Task<int> GetStatusKeyValue(string value);
        Task<bool> Initialize();
        IList<Assembly> GetAssemblyList();

    }

    #endregion

    #region Class

    public class DNATools : IDNATools {

        #region Services

        // Variables
        private readonly MainContext Context;
        private readonly ILogger<DNATools> Logger;

        #endregion

        #region Class Methods

        public DNATools(MainContext context, ILogger<DNATools> logger) {
            Context = context;
            Logger = logger;
        }

        #endregion

        #region Common Methods

        // Get Source Key Value
        public async Task<int> GetSourceKeyValue(string value) {
            int result = 0;
            try {
                Source instance = await Context.Source.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.SourceId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Disposition Key Value
        public async Task<int> GetDispositionKeyValue(string value) {
            int result = 0;
            try {
                Disposition instance = await Context.Disposition.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.DispositionId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Page Key Value
        public async Task<int> GetPageKeyValue(string value) {
            int result = 0;
            try {
                Page instance = await Context.Page.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.PageId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Section Key Value
        public async Task<int> GetSectionKeyValue(string value) {
            int result = 0;
            try {
                Section instance = await Context.Section.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.SectionId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Category Key Value
        public async Task<int> GetCategoryKeyValue(string value) {
            int result = 0;
            try {
                Category instance = await Context.Category.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.CategoryId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Status Key Value
        public async Task<int> GetStatusKeyValue(string value) {
            int result = 0;
            try {
                Status instance = await Context.Status.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.StatusId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Table Key Value
        public async Task<int> GetTableKeyValue(string value) {
            int result = 0;
            try {
                Table instance = await Context.Table.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.TableId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Role Key Value
        public async Task<int> GetRoleKeyValue(string value) {
            int result = 0;
            try {
                Role instance = await Context.Role.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.RoleId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Menu Key Value
        public async Task<int> GetMenuKeyValue(string value) {
            int result = 0;
            try {
                Menu instance = await Context.Menu.Where(x => x.Code == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.MenuId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        // Get Assembly List
        public IList<Assembly> GetAssemblyList() {
            IList<Assembly> result = default;
            try {
                result = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.Contains("Microsoft") && !x.FullName.Contains("System") && !x.FullName.Contains("Serilog") && !x.FullName.Contains("Telerik") && !x.FullName.Contains("Syncfusion") && !x.FullName.Contains("Netstandard") && !x.FullName.Contains("Swashbuckle")).OrderBy(x => x.FullName).ToList();
            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return result;
        }

        #endregion

        #region Application Initialization

        // Initialize Database
        public async Task<bool> Initialize() {
            bool Result = false;
            int statusid;
            string code;
            string name;

            try {

                // Tables
                IList<Table> Tables = await Context.Table.Where(x => x.TableId > 0).ToListAsync();
                if (Tables.Count == 0) {

                    Context.Table.Add(new Table { TableId = 10000, Code = "ACT", Name = "Action", Description = "Menu Action" });
                    Context.Table.Add(new Table { TableId = 10001, Code = "ATY", Name = "Activity", Description = "Activity Log" });
                    Context.Table.Add(new Table { TableId = 10002, Code = "APP", Name = "Application", Description = "Application Definition" });
                    Context.Table.Add(new Table { TableId = 10003, Code = "APT", Name = "Appointment", Description = "Service Appointments (DEPRECATED)" });
                    Context.Table.Add(new Table { TableId = 10004, Code = "ART", Name = "Article", Description = "Website Posts" });
                    Context.Table.Add(new Table { TableId = 10005, Code = "ASC", Name = "Associate", Description = "Employees and Associates" });
                    Context.Table.Add(new Table { TableId = 10006, Code = "CAM", Name = "Campaign", Description = "Marketing Campaign" });
                    Context.Table.Add(new Table { TableId = 10007, Code = "CAR", Name = "Cart", Description = "Shopping Cart" });
                    Context.Table.Add(new Table { TableId = 10008, Code = "CAT", Name = "Category", Description = "Article Category" });
                    Context.Table.Add(new Table { TableId = 10009, Code = "CLI", Name = "Client", Description = "Client" });
                    Context.Table.Add(new Table { TableId = 10010, Code = "CON", Name = "Condition", Description = "Ebay Item Condition" });
                    Context.Table.Add(new Table { TableId = 10011, Code = "CUS", Name = "Customer", Description = "Customer" });
                    Context.Table.Add(new Table { TableId = 10012, Code = "DEV", Name = "Device", Description = "Device" });
                    Context.Table.Add(new Table { TableId = 10013, Code = "DIS", Name = "Disposition", Description = "Marketing Disposition" });
                    Context.Table.Add(new Table { TableId = 10014, Code = "FAC", Name = "Facility", Description = "Corporate Facilities" });
                    Context.Table.Add(new Table { TableId = 10015, Code = "HIS", Name = "History", Description = "Call History" });
                    Context.Table.Add(new Table { TableId = 10016, Code = "HOM", Name = "Homeowner", Description = "Homeowners" });
                    Context.Table.Add(new Table { TableId = 10017, Code = "ITM", Name = "Item", Description = "Shopping Cart Items" });
                    Context.Table.Add(new Table { TableId = 10018, Code = "LST", Name = "Listingtype", Description = "Ebay Listing Types" });
                    Context.Table.Add(new Table { TableId = 10019, Code = "LGN", Name = "Login", Description = "User Login Identities" });
                    Context.Table.Add(new Table { TableId = 10020, Code = "MAT", Name = "Material", Description = "Marketing Propects" });
                    Context.Table.Add(new Table { TableId = 10021, Code = "MNU", Name = "Menu", Description = "Menu Definitions" });
                    Context.Table.Add(new Table { TableId = 10022, Code = "PAG", Name = "Page", Description = "Website Pages" });
                    Context.Table.Add(new Table { TableId = 10023, Code = "PRI", Name = "Price", Description = "Product Prices" });
                    Context.Table.Add(new Table { TableId = 10024, Code = "PRD", Name = "Product", Description = "Products and Services" });
                    Context.Table.Add(new Table { TableId = 10025, Code = "PRO", Name = "Promotion", Description = "Promotions and Incentives" });
                    Context.Table.Add(new Table { TableId = 10026, Code = "REF", Name = "Reference", Description = "References" });
                    Context.Table.Add(new Table { TableId = 10027, Code = "REQ", Name = "Request", Description = "Contact Requests" });
                    Context.Table.Add(new Table { TableId = 10028, Code = "RES", Name = "Residence", Description = "Mailing Address Database" });
                    Context.Table.Add(new Table { TableId = 10029, Code = "ROL", Name = "Role", Description = "User Role" });
                    Context.Table.Add(new Table { TableId = 10030, Code = "SEC", Name = "Section", Description = "Article Sections" });
                    Context.Table.Add(new Table { TableId = 10031, Code = "SRT", Name = "Sortorder", Description = "Ebay Sort Order" });
                    Context.Table.Add(new Table { TableId = 10032, Code = "SRC", Name = "Source", Description = "Lead Sources" });
                    Context.Table.Add(new Table { TableId = 10033, Code = "STA", Name = "Status", Description = "Status Codes" });
                    Context.Table.Add(new Table { TableId = 10034, Code = "TBL", Name = "Table", Description = "Table Definitions" });
                    Context.Table.Add(new Table { TableId = 10035, Code = "TSK", Name = "Task", Description = "Task Definitions" });
                    Context.Table.Add(new Table { TableId = 10036, Code = "TIC", Name = "Ticket", Description = "Support Tickets" });
                    Context.Table.Add(new Table { TableId = 10037, Code = "USR", Name = "User", Description = "User Profiles" });
                    Context.Table.Add(new Table { TableId = 10038, Code = "VND", Name = "Vendor", Description = "Vendor Definitions" });
                    Context.Table.Add(new Table { TableId = 10039, Code = "WRK", Name = "Workorder", Description = "Workorders" });
                    Context.Table.Add(new Table { TableId = 10040, Code = "ZON", Name = "Workorder", Description = "Marketing Zone Definitions" });

                    await Context.SaveChangesAsync();
                }

                // Status Codes
                IList<Status> Codes = await Context.Status.Where(x => x.StatusId > 0).ToListAsync();
                if (Codes.Count == 0) {
                    int tableid = await GetTableKeyValue("User");
                    Context.Status.Add(new Status { TableId = tableid, Code = "Active", Name = "Active", Description = "Active" });
                    Context.Status.Add(new Status { TableId = tableid, Code = "Inactive", Name = "Inactive", Description = "Inactive" });
                    await Context.SaveChangesAsync();
                }

                statusid = await GetStatusKeyValue("Active");

                // Roles
                IList<Role> Roles = await Context.Role.Where(x => x.RoleId > 0).ToListAsync();
                if (Roles.Count == 0) {
                    Context.Role.Add(new Role { Code = "Admin", Name = "Admin", Description = "Admin" });
                    Context.Role.Add(new Role { Code = "Manager", Name = "Manager", Description = "Manager" });
                    Context.Role.Add(new Role { Code = "User", Name = "User", Description = "User" });
                    await Context.SaveChangesAsync();
                }

                int roleid = await GetRoleKeyValue("Admin");

                // Clients, Users, and User Identities (Logins)
                IList<Client> Clients = await Context.Client.Where(x => x.ClientId > 0).ToListAsync();
                if (Clients.Count == 0) {

                    Client c = new() { Company = "PCDMZ", Address1 = "123 Main Street", Address2 = "PO Box 12", City = "Anytown", State = "IN", Zip = "46203", Zip1 = "1234", Phone = "(800) 555-1212", Comment = "", StatusId = statusid };
                    c.Users = new List<User>();
                    User u = new() { First = "System", Last = "Administrator", RoleId = await GetRoleKeyValue("Admin"), StatusId = statusid, Persist = true, Comment = "" };
                    u.Logins = new List<Login>();
                    c.Users.Add(u);
                    Login l = new() { Provider = "Local", Email = "admin@clicktickdone.com", Password = Utilities.Security.CreateHash("P@ssw0rd") };
                    u.Logins.Add(l);
                    Context.Client.Add(c);

                    await Context.SaveChangesAsync();
                }

                // Products
                IList<Product> Products = await Context.Product.Where(x => x.ProductId > 0).ToListAsync();
                if (Products.Count == 0) {
                    Context.Product.Add(new Product {
                        Code = "BAS",
                        Name = "Basic Edition",
                        Description = "Basic Edition",
                        Features = "<ul><li>Unlimited Users</li><li>Private Server</li><li>Security Certificates</li><li>1GB Database</li><li>Use by 1 (one) company</li><li>Basic Support</li></ul>",
                        Image = "",
                        Target = "/product/detail/",
                        TargetName = "Purchase Now",
                        Price = 79, Icon = "far fa-circle",
                        StatusId = statusid
                    });
                    Context.Product.Add(new Product {
                        Code = "STD",
                        Name = "Standard Edition",
                        Description = "Standard Edition",
                        Features = "<ul><li>Unlimited Users</li><li>Private Server</li><li>Security Certificates</li><li>10GB Database</li><li>Use by 1 (one) company</li><li>Standard Support</li></ul>",
                        Image = "",
                        Target = "/product/detail/",
                        TargetName = "Purchase Now",
                        Price = 99,
                        Icon = "far fa-circle",
                        StatusId = statusid
                    });
                    Context.Product.Add(new Product {
                        Code = "PRO",
                        Name = "Professional Edition",
                        Description = "Professional Edition",
                        Features = "<ul><li>Unlimited Users</li><li>Private Server</li><li>Security Certificates</li><li>Unlimited Database</li><li>Use by 1 (one) company</li><li>Priority Support</li></ul>",
                        Image = "",
                        Target = "/product/detail/",
                        TargetName = "Purchase Now",
                        Price = 129,
                        Icon = "far fa-circle",
                        StatusId = statusid
                    });
                    await Context.SaveChangesAsync();
                }

                // Pages
                IList<Page> Pages = await Context.Page.Where(x => x.PageId > 0).ToListAsync();
                if (Pages.Count == 0) {
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "introduction", Name = "Introduction", Subject = "Web Application Framework  for ASP.NET Core", Content = "A fully documented and modular solution to serve as the foundation for your next application", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "home", Name = "Home", Subject = "Web Application Template for ASP.NET Core", Content = "A responsive, cross-platform, open-source system completely refactored for ASP.NET Core and Syncfusion", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "features", Name = "Features", Subject = "Application Features", Content = "Just three projects provide everything you need for a complete solution. Authentication, Authorization, User Registration, and \r\nContent Management are all baked right in. Just add your own custom features.", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "about", Name = "About", Subject = "Application DNA", Content = "<p>\r\n    A company website should be much more than an electronic business card. It should encompass every aspect of your business \r\n    from Marketing and Sales to Installation, Service and Support. The problem arises when you attempt to address these challenges \r\n    with nothing more than a canned application. Every business is unique, and a canned solution presents myriad features you may\r\n    not want or need.\r\n</p>", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "products", Name = "Products", Subject = "Products", Content = "Our Products", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "faq", Name = "FAQ", Subject = "Frequently Asked Questions", Content = "Frequently Asked Questions", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "team", Name = "Team", Subject = "Meet Our Team", Content = "Meet Our Team", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "contact", Name = "Contact", Subject = "How can we help?", Content = "Please select the topic most closely related to your inquiry. If you don't find what you need, please fill out the form and someone will be in touch.", Icon = "fas fa-home" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "testimonials", Name = "Testimonials", Subject = "Testimonials", Content = "A testimonial is an honest endorsement of a product or service that comes from a customer, colleague, or peer who has had a positive experience from the work you did for them.", Icon = "far fa-thumbs-up" });
                    Context.Page.Add(new Page { Date = DateTime.Now, Slug = "content-management", Name = "Content Management", Subject = "Content Management", Content = "<h2>Introduction</h2>\r\n\r\n<p>\r\n    In today’s digital-first world, content plays a vital role in engaging users and driving business success. A Content \r\n    Management System (CMS) is a software solution that enables non-technical users to easily create, edit, and publish \r\n    digital content. While standalone CMS platforms are prevalent, there are substantial benefits to building a custom \r\n    CMS directly into your application. This approach can provide a tailored, seamless, and efficient solution that fits \r\n    the unique needs of your project.\r\n</p>\r\n\r\n<p>\r\n    In this article, we'll explore the specific advantages of integrating a CMS directly into your application.\r\n</p>\r\n\r\n<h2>Seamless Integration</h2>\r\n\r\n<p>\r\n    When you build a CMS directly into your application, it integrates tightly with the rest of your codebase, offering much \r\n    better control over how content is managed and displayed. Here’s how:\r\n</p>\r\n\r\n<ul>\r\n\t<li>\r\n        Tailored Data Models: Unlike off-the-shelf CMS platforms that provide generic content types, a built-in CMS allows \r\n        you to customize your content models exactly to fit your needs. For instance, you can define content types (e.g., \r\n        blog posts, product descriptions, portfolio items) that align perfectly with your business requirements.\r\n    </li>\r\n    \r\n\t<li>\r\n        Full Control Over UI/UX: With an embedded CMS, you have total control over the user interface and experience of your \r\n        site. You are not limited by predefined templates or themes, which gives you the flexibility to design a website that \r\n        represents your brand.\r\n    </li>\r\n    \r\n\t<li>\r\n        Custom Workflows: A built-in CMS enables you to define and implement custom workflows for content approval, publishing, \r\n        and versioning. This is especially useful for businesses that need unique approval or editorial processes.\r\n    </li>  \r\n</ul>\r\n\r\n<h2>Flexibility in Content Distribution</h2>\r\n\r\n<p>\r\n    A custom-built CMS can offer flexibility that is harder to achieve with external CMS platforms, especially when it comes to \r\n    content distribution and integration with other systems:\r\n</p>\r\n\r\n<ul>\r\n    <li>\r\n        API-Driven Content: With powerful support for RESTful APIs and gRPC, you can easily build a headless CMS that allows \r\n        content to be consumed by a variety of front-end applications (e.g., websites, mobile apps, IoT devices) via APIs. \r\n        This is particularly useful for organizations looking to distribute content across multiple platforms.\r\n    </li>\r\n    \r\n    <li>\r\n        Third-Party Integrations: If you need to pull data from external systems or integrate with other services \r\n        (e.g., CRMs, ERPs, marketing platforms), you can design your CMS to work seamlessly with these systems through APIs, \r\n        webhooks, or event-driven architectures.\r\n    </li>\r\n    <li>\r\n        Custom Delivery Mechanisms: You have full control over how and where content is delivered, whether it’s through dynamic \r\n        HTML pages, single-page applications (SPA), or as structured data (JSON) for mobile apps.\r\n    </li>\r\n</ul>", Icon = "fas fa-pen-to-square" });
                    await Context.SaveChangesAsync();
                }

                // Sections
                IList<Section> Sections = await Context.Section.Where(x => x.SectionId > 0).ToListAsync();
                if (Sections.Count == 0) {
                    int pageid = await GetPageKeyValue("Home");
                    Context.Section.Add(new Section { PageId = pageid, Date = DateTime.Now, Slug = "features", Name = "Features", Subject = "Application Features", Description = "Application Features", Icon = "fas fa-circle", Columns = 0, Limit = 0 });
                    Context.Section.Add(new Section { PageId = pageid, Date = DateTime.Now, Slug = "faq", Name = "FAQ", Subject = "Frequently Asked Questions", Description = "Frequently Asked Questions", Icon = "fas fa-circle", Columns = 0, Limit = 0 });
                    await Context.SaveChangesAsync();
                }

                // Categories
                IList<Category> Categories = await Context.Category.Where(x => x.CategoryId > 0).ToListAsync();
                if (Categories.Count == 0) {
                    int sectionid = await GetSectionKeyValue("Features");
                    Context.Category.Add(new Category { SectionId = sectionid, Date = DateTime.Now, Slug = "products", Name = "Products", Subject = "Products", Description = "Products", Icon = "fas fa-circle" });
                    await Context.SaveChangesAsync();
                }

                // Articles
                IList<Article> Articles = await Context.Article.Where(x => x.ArticleId > 0).ToListAsync();
                if (Articles.Count == 0) {
                    int pageid = await GetPageKeyValue("Home");
                    int sectionid = await GetSectionKeyValue("Features");
                    int categoryid = await GetCategoryKeyValue("Products");
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-website", Name = "Domain Registration", Image = "", Target = "/#features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Domain Registration", Description = "Create and renew domains from multiple registrars with the convenience of a single user interface." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-pencil", Name = "Firewall Management", Image = "", Target = "/#features", TargetName = "Learn More", Tags = "Products, Features, Firewall", Subject = "Firewall Management", Description = "A Hardware Firewall, Client Certificates, and MFA techniques all work together to keep your data safe and sound." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-stats-up", Name = "Report Builder", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features, Documentation", Subject = "Report Builder", Description = "Crystal Reports-like Report Builders for Web and Desktop. Beautiful reports that run right in your Web Browser." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-protection", Name = "Security", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Security", Description = "A Hardware Firewall, Client Certificates, and MFA techniques all work together to keep your data safe and sound." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-cog", Name = "Lead Generation", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features, Leads", Subject = "Lead Generation", Description = "Import New Homeowners from your favorite provider and filter out those you've already dispositioned." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-map", Name = "Route Planning", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features, Documentation", Subject = "Route Planning", Description = "Select a group of Workorders and be presented with driving directions, traffic conditions, and distance." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "First Class Presort", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "First Class Presort", Description = "Using our unique, three dimensional sort and postcard templates will save you thousands in printing and postage." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-circle-plus", Name = "Extensible", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Service Management", Description = "A wide array of pre-built assemblies ensures you'll have just what your need at your fingertips." });
                    sectionid = await GetSectionKeyValue("FAQ");
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Content", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Can I create and edit webpage content?", Description = "You can create and edit pages, sections, categories, articles, apps, and menus just like you do in WordPress. Just click the pencil icon anywhere and you'll get the content. It's that easy." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Datacenter", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Will my server be hosted in my part of the world?", Description = "We work with OVH, Hostwinds, IONOS, and others to provide more than 40 Datacenters across 4 continents and 44 redundant Points of Presence. We're everywhere, so we can provide you with a Datacenter or a Point of Presence that's close to where you work or live." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Platform", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Can I run my application on a Linux server?", Description = "While our applications are capable of running on a Linux, we choose to provide Windows for its developer features, familiarity, ease of use, and commercial support. That said, if you have your heart set on Linux please contact customer service and we'll do our best to accomodate you with a Debian distro." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Reports", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Can I create and edit reports?", Description = "Absolutely. You can create or edit reports right from your dashboard using the built-in Web Editor, or for the ultimate in flexibility, use the desktop version where you can insert charts, graphs, and sub-reports." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Security", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "What security features do you offer?", Description = "First of all, we provide you with a Virtual Private Server dedicated to your application so there are no worries about sharing the same webspace. Next we provision a separate, hardware-based Firewall with up to 28 configurable rules. In addition, we support SSL encryption, User-Mapped Client Certificates, Cookie, JSON Web Token, and Microsoft authentication schemes, and you can  use Multifactor Authentication and 3rd-Party authentication providers like Microsoft, Google, and Twitter." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Hosting", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Where do I go for application hosting?", Description = "All of our products include a Virtual Private Server for application hosting. Simply specify the domain name you would like to use during checkout. You can use a domain you already own, or we can help you select something new." });
                    Context.Article.Add(new Article { Date = DateTime.Now, PageId = pageid, SectionId = sectionid, CategoryId = categoryid, StatusId = statusid, Icon = "lni lni-dropbox", Name = "Google", Image = "", Target = "/home/features", TargetName = "Learn More", Tags = "Products, Features", Subject = "Why can’t I sign in to a different Google Account?", Description = "If you are signed into multiple Google accounts, then you will be prompted to select the account to use for signing in. If you are logged into a single Google account however, the system will bypass the account selection process. To work around the issue, simply sign into Google with more than one account prior to running your application." });
                    await Context.SaveChangesAsync();
                }

                // EBay Condition
                IList<Condition> Conditions = await Context.Condition.Where(x => x.ConditionId > 0).ToListAsync();
                if (Conditions.Count == 0) {
                    Context.Condition.Add(new Condition { Code = "1000", Name = "New", Description = "A brand-new, unused, unopened, unworn, undamaged item. Most categories support this condition (as long as condition is an applicable concept)" });
                    Context.Condition.Add(new Condition { Code = "1500", Name = "New other", Description = "A brand-new new, unused item with no signs of wear. Packaging may be missing or opened. The item may be a factory second or have defects." });
                    Context.Condition.Add(new Condition { Code = "1750", Name = "New with defects", Description = "A brand-new, unused, and unworn item. The item may have cosmetic defects, and/or may contain mismarked tags (e.g., incorrect size tags from the manufacturer). Packaging may be missing or opened. The item may be a new factory second or irregular." });
                    Context.Condition.Add(new Condition { Code = "2000", Name = "Certified - Refurbished", Description = "The item is in a pristine, like-new condition. It has been professionally inspected, cleaned, and refurbished by the manufacturer or a manufacturer-approved vendor to meet manufacturer specifications. The item will be in new packaging with original or new accessories." });
                    Context.Condition.Add(new Condition { Code = "2010", Name = "Excellent - Refurbished", Description = "An item that is in like-new condition, backed by a one year warranty. It has been professionally refurbished, inspected, and cleaned to excellent condition by qualified sellers. The item includes original or new accessories and will come in new generic packaging. See the seller's listing for full details." });
                    Context.Condition.Add(new Condition { Code = "2020", Name = "Very Good - Refurbished", Description = "An item that shows minimal wear and is backed by a one year warranty. It is fully functional and has been professionally refurbished, inspected, and cleaned to very good condition by qualified sellers. The item includes original or new accessories and will come in new generic packaging. See the seller's listing for full details." });
                    Context.Condition.Add(new Condition { Code = "2030", Name = "Good - Refurbished", Description = "An item that shows moderate wear and is backed by a one year warranty. It is fully functional and has been professionally refurbished, inspected, and cleaned to good condition by qualified sellers. The item includes original or new accessories and will come in a new generic packaging. See the seller's listing for full details." });
                    Context.Condition.Add(new Condition { Code = "2500", Name = "Seller refurbished", Description = "An item that has been restored to working order by the eBay seller or a third party. This means the item was inspected, cleaned, and repaired to full working order and is in excellent condition. This item may or may not be in original packaging." });
                    Context.Condition.Add(new Condition { Code = "2750", Name = "Like New", Description = "An item that looks as if it was just taken out of shrink wrap. No visible wear, and all facets of the item are flawless and intact. See the seller's listing for full details and description of any imperfections." });
                    Context.Condition.Add(new Condition { Code = "3000", Name = "Used", Description = "An item that has been used previously. The item may have some signs of cosmetic wear, but is fully operational and functions as intended. This item may be a floor model or store return that has been used. Most categories support this condition (as long as condition is an applicable concept)" });
                    Context.Condition.Add(new Condition { Code = "4000", Name = "Very Good", Description = "An item that is used but still in very good condition. No obvious damage to the cover or jewel case. No missing or damaged pages or liner notes. The instructions (if applicable) are included in the box. May have very minimal identifying marks on the inside cover. Very minimal wear and tear." });
                    Context.Condition.Add(new Condition { Code = "5000", Name = "Good", Description = "An item in used but good condition. May have minor external damage including scuffs, scratches, or cracks but no holes or tears. For books, liner notes, or instructions, the majority of pages have minimal damage or markings and no missing pages." });
                    Context.Condition.Add(new Condition { Code = "6000", Name = "Acceptable", Description = "An item with obvious or significant wear, but still operational. For books, liner notes, or instructions, the item may have some damage to the cover but the integrity is still intact. Instructions and/or box may be missing. For books, possible writing in margins, etc., but no missing pages or anything that would compromise the legibility or understanding of the text." });
                    Context.Condition.Add(new Condition { Code = "7000", Name = "For parts or not working", Description = "An item that does not function as intended and is not fully operational. This includes items that are defective in ways that render them difficult to use, items that require service or repair, or items missing essential components. Supported in categories where parts or non-working items are of interest to people who repair or collect related items." });
                    await Context.SaveChangesAsync();
                }

                // EBay Listing Type
                IList<ListingType> ListingTypes = await Context.ListingType.Where(x => x.ListingtypeId > 0).ToListAsync();
                if (ListingTypes.Count == 0) {
                    Context.ListingType.Add(new ListingType { Code = "Auction", Name = "Auction", Description = "Retrieve matching auction listings (i.e., listings eligible for competitive bidding at auction) only. Excludes auction items with Buy It Now." });
                    Context.ListingType.Add(new ListingType { Code = "AuctionWithBIN", Name = "Auction (Buy it now)", Description = "Retrieve all matching auction listings with Buy It Now available. Excludes auction listings without Buy It Now. An auction listed with Buy It Now will not be returned if a valid bid has been placed on the auction." });
                    Context.ListingType.Add(new ListingType { Code = "Classified", Name = "Classified", Description = "Retrieves Classified Ad format (i.e., Classified and AdFormat listing type) listings only." });
                    Context.ListingType.Add(new ListingType { Code = "FixedPrice", Name = "Fixed Price", Description = "Retrieve matching fixed price items only. Excludes Store Inventory format items." });
                    Context.ListingType.Add(new ListingType { Code = "StoreInventory", Name = "Store Inventory", Description = "Retrieve Store Inventory format items only." });
                    Context.ListingType.Add(new ListingType { Code = "All", Name = "All Listing Types", Description = "Retrieve matching items for any listing type." });
                    await Context.SaveChangesAsync();
                }

                // EBay Sort Order
                IList<SortOrder> SortOrders = await Context.SortOrder.Where(x => x.SortorderId > 0).ToListAsync();
                if (SortOrders.Count == 0) {
                    Context.SortOrder.Add(new SortOrder { Code = "BestMatch", Name = "Best Match", Description = "Sorts items by Best Match, which is based on community buying activity and other relevance-based factors." });
                    Context.SortOrder.Add(new SortOrder { Code = "BidCountFewest", Name = "Fewest Bids", Description = "Sorts items by the number of bids they have received, with items that have received the fewest bids first." });
                    Context.SortOrder.Add(new SortOrder { Code = "BidCountMost", Name = "Most Bids", Description = "Sorts items by the number of bids they have received, with items that have received the most bids first." });
                    Context.SortOrder.Add(new SortOrder { Code = "CountryAscending", Name = "Country (Ascending)", Description = "Sorts items available on the the given site (as specified by global ID in the HTTP header or URL parameter) by the country in which they are located. For CountryAscending, items located in the country most closely associated with the site appear first, followed by items in related countries, and then items from other countries." });
                    Context.SortOrder.Add(new SortOrder { Code = "CountryDescending", Name = "Country (Descending)", Description = "Sorts items available on the the given site (as specified by global ID in the HTTP header or URL parameter) by the country in which they are located. For CountryDescending, items are sorted in reverse order of CountryAscending. That is, items in countries not specifically related to the site appear first, sorted in descending alphabetical order by English country name. For example, when searching the Ireland site, items located in countries like Yugoslavia or Uganda are returned first. Items located in Ireland (IE) will be returned last." });
                    Context.SortOrder.Add(new SortOrder { Code = "CurrentPriceHighest", Name = "Current Price Highest", Description = "Sorts items by their current price, with the highest price first." });
                    Context.SortOrder.Add(new SortOrder { Code = "DistanceNearest", Name = "Nearest", Description = "Sorts items by distance from the buyer in ascending order. The request must also include a buyerPostalCode." });
                    Context.SortOrder.Add(new SortOrder { Code = "EndTimeSoonest", Name = "Ending Soonest", Description = "Sorts items by end time, with items ending soonest listed first." });
                    Context.SortOrder.Add(new SortOrder { Code = "PricePlusShippingHighest", Name = "Price High to Low", Description = "Sorts items by the combined cost of the item price plus the shipping cost, with highest combined price items listed first. Items are returned in the following groupings: highest total-cost items (for items where shipping was properly specified) appear first, followed by freight- shipping items, and then items for which no shipping was specified. Each group is sorted by price." });
                    Context.SortOrder.Add(new SortOrder { Code = "PricePlusShippingLowest", Name = "Price Low to High", Description = "Sorts items by the combined cost of the item price plus the shipping cost, with the lowest combined price items listed first. Items are returned in the following groupings: lowest total-cost items (for items where shipping was properly specified) appear first, followed by freight- shipping items, and then items for which no shipping was specified. Each group is sorted by price." });
                    Context.SortOrder.Add(new SortOrder { Code = "StartTimeNewest", Name = "Newest Listings", Description = "Sorts items by the start time, the most recently listed (newest) items appear first." });
                    Context.SortOrder.Add(new SortOrder { Code = "WatchCountDecreaseSort", Name = "Watch Count", Description = "Sorts items by watch count in decreasing order for the given site. The items with highest watch count appear earlier in results than those with lower watch count." });
                    await Context.SaveChangesAsync();
                }

                // Applications

                Menu m = new();

                code = "AP";
                name = "Accounts Payable";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-dollar-sign", Target = "javascript:void()", TargetName = name, Weight = 100 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/AP", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Vendors", Name = "Vendors", Description = $"{name} Vendor List", Icon = "far fa-circle", Target = "/AP/Vendor", TargetName = "Vendors", Weight = 200 },
                            new() { RoleId = roleid, Code = "Invoices", Name = "Invoices", Description = $"{name} Invoice List", Icon = "far fa-circle", Target = "/AP/Invoice", TargetName = "Invoices", Weight = 300 },
                            new() { RoleId = roleid, Code = "Payments", Name = "Payments", Description = $"{name} Payment List", Icon = "far fa-circle", Target = "/AP/Payment", TargetName = "Payments", Weight = 400 }
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "AR";
                name = "Accounts Receivable";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-chart-line", Target = "javascript:void()", TargetName = name, Weight = 200 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/AR", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Customers", Name = "Customers", Description = $"{name} Customer List", Icon = "far fa-circle", Target = "/AR/Customer", TargetName = "Customers", Weight = 200 },
                            new() { RoleId = roleid, Code = "Invoices", Name = "Invoices", Description = $"{name} Invoice List", Icon = "far fa-circle", Target = "/AR/Invoice", TargetName = "Invoices", Weight = 300 },
                            new() { RoleId = roleid, Code = "Payments", Name = "Payments", Description = $"{name} Payment List", Icon = "far fa-circle", Target = "/AR/Payment", TargetName = "Payments", Weight = 400 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "EBay";
                name = "EBay Auction Manager";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = "EBay Auction Manager", Description = name, Icon = "fab fa-ebay", Target = "javascript:void()", TargetName = name, Weight = 300 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/EBAY", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Account", Name = "Account", Description = $"{name} Account", Icon = "far fa-circle", Target = "/EBAY/Account", TargetName = "Account", Weight = 200 },
                            new() { RoleId = roleid, Code = "Active", Name = "Active Listings", Description = $"{name} Active Listings", Icon = "far fa-circle", Target = "/EBAY/Active", TargetName = "Active Listings", Weight = 300 },
                            new() { RoleId = roleid, Code = "Completed", Name = "Completed Listings", Description = $"{name} Completed Listings", Icon = "far fa-circle", Target = "/EBAY/Completed", TargetName = "Completed Listings", Weight = 400 },
                            new() { RoleId = roleid, Code = "Copy", Name = "Copy Listing", Description = $"{name} Copy Listing", Icon = "far fa-circle", Target = "/EBAY/Browse", TargetName = "Copy Listing", Weight = 500 },
                            new() { RoleId = roleid, Code = "Insights", Name = "Marketplace Insights", Description = $"{name} Marketplace Insights", Icon = "far fa-circle", Target = "/EBAY/Insights", TargetName = "Marketplace Insights", Weight = 600 },
                            new() { RoleId = roleid, Code = "Estimate", Name = "Price Estimator", Description = $"{name} Price Estimator", Icon = "far fa-circle", Target = "/EBAY/Estimate", TargetName = "Price Estimator", Weight = 700 },
                            new() { RoleId = roleid, Code = "Search", Name = "Search Listings", Description = $"{name} Search Listings", Icon = "far fa-circle", Target = "/EBAY/Search", TargetName = "Search Listings", Weight = 800 },
                            new() { RoleId = roleid, Code = "Watch", Name = "Watch List", Description = $"{name} Watch List", Icon = "far fa-circle", Target = "/EBAY/Watch", TargetName = "Watch List", Weight = 900 },
                            new() { RoleId = roleid, Code = "API", Name = "API Documentation", Description = $"{name} API Documentation", Icon = "far fa-circle", Target = "https://developer.ebay.com/", TargetName = "API Documentation", NewWindow = true, Weight = 1000 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "EBid";
                name = "EBid Auction Manager";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = name, Icon = "fas fa-gavel", Target = "javascript:void()", TargetName = name, Weight = 400 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/EBid", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Account", Name = "Account", Description = $"{name} Account", Icon = "far fa-circle", Target = "/EBid/Account", TargetName = "Account", Weight = 200 },
                            new() { RoleId = roleid, Code = "Active", Name = "Active Listings", Description = $"{name} Active Listings", Icon = "far fa-circle", Target = "/EBid/Active", TargetName = "Active Listings", Weight = 300 },
                            new() { RoleId = roleid, Code = "Completed", Name = "Completed Listings", Description = $"{name} Completed Listings", Icon = "far fa-circle", Target = "/EBid/Completed", TargetName = "Completed Listings", Weight = 400 },
                            new() { RoleId = roleid, Code = "Estimate", Name = "Price Estimator", Description = $"{name} Price Estimator", Icon = "far fa-circle", Target = "/EBid/Estimate", TargetName = "Price Estimator", Weight = 500 },
                            new() { RoleId = roleid, Code = "Search", Name = "Search Listings", Description = $"{name} Search Listings", Icon = "far fa-circle", Target = "/EBid/Search", TargetName = "Search Listings", Weight = 600 },
                            new() { RoleId = roleid, Code = "Watch", Name = "Watch List", Description = $"{name} Watch List", Icon = "far fa-circle", Target = "/EBid/Watch", TargetName = "Watch List", Weight = 700 },
                            new() { RoleId = roleid, Code = "API", Name = "API Documentation", Description = $"{name} API Documentation", Icon = "far fa-circle", Target = "https://ebid.3scale.net", TargetName = "API Documentation", NewWindow = true, Weight = 800 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "Etsy";
                name = "Etsy Store Manager";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = name, Icon = "fab fa-etsy", Target = "javascript:void()", TargetName = name, Weight = 400 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/Etsy", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Account", Name = "Account", Description = $"{name} Account", Icon = "far fa-circle", Target = "/Etsy/Account", TargetName = "Account", Weight = 200 },
                            new() { RoleId = roleid, Code = "Active", Name = "Active Listings", Description = $"{name} Active Listings", Icon = "far fa-circle", Target = "/Etsy/Active", TargetName = "Active Listings", Weight = 300 },
                            new() { RoleId = roleid, Code = "Completed", Name = "Completed Listings", Description = $"{name} Completed Listings", Icon = "far fa-circle", Target = "/Etsy/Completed", TargetName = "Completed Listings", Weight = 400 },
                            new() { RoleId = roleid, Code = "Estimate", Name = "Price Estimator", Description = $"{name} Price Estimator", Icon = "far fa-circle", Target = "/Etsy/Estimate", TargetName = "Price Estimator", Weight = 500 },
                            new() { RoleId = roleid, Code = "Search", Name = "Search Listings", Description = $"{name} Search Listings", Icon = "far fa-circle", Target = "/Etsy/Search", TargetName = "Search Listings", Weight = 600 },
                            new() { RoleId = roleid, Code = "Watch", Name = "Watch List", Description = $"{name} Watch List", Icon = "far fa-circle", Target = "/Etsy/Watch", TargetName = "Watch List", Weight = 700 },
                            new() { RoleId = roleid, Code = "API", Name = "API Documentation", Description = $"{name} API Documentation", Icon = "far fa-circle", Target = "https://developers.etsy.com/documentation/", TargetName = "API Documentation", NewWindow = true, Weight = 800 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "MKT";
                name = "Marketing Management";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "far fa-envelope", Target = "javascript:void()", TargetName = name, Weight = 500 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = $"/{code}", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Campaigns", Name = "Campaigns", Description = $"{name} Campaign List", Icon = "far fa-circle", Target = $"/{code}/Campaign", TargetName = "Campaigns", Weight = 200 },
                            new() { RoleId = roleid, Code = "Sources", Name = "Data Sources", Description = $"{name} Data Source List", Icon = "far fa-circle", Target = $"/{code}/Source", TargetName = "Data Sources", Weight = 300 },
                            new() { RoleId = roleid, Code = "Homeowners", Name = "Homeowners", Description = $"{name} Homeowner List", Icon = "far fa-circle", Target = $"/{code}/Homeowner", TargetName = "Homeowners", Weight = 400 },
                            new() { RoleId = roleid, Code = "Mailings", Name = "Mailings", Description = $"{name} Mailing Results", Icon = "far fa-circle", Target = $"/{code}/Mailing", TargetName = "Mailings", Weight = 500 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "SVC";
                name = "Service Management";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-bell-concierge", Target = "javascript:void()", TargetName = name, Weight = 600 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = $"/{code}", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Associates", Name = "Associates", Description = $"{name} Associate List", Icon = "far fa-circle", Target = $"/{code}/Associate", TargetName = "Associates", Weight = 200 },
                            new() { RoleId = roleid, Code = "Schedule", Name = "Schedule", Description = $"{name} Schedule", Icon = "far fa-circle", Target = $"/{code}/Schedule", TargetName = "Schedule", Weight = 300 },
                            new() { RoleId = roleid, Code = "Workorders", Name = "Workorders", Description = $"{name} Workorder List", Icon = "far fa-circle", Target = $"/{code}/Workorder", TargetName = "Workorders", Weight = 400 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "NMS";
                name = "Network Monitoring System";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = "Network Monitor", Description = "Network Monitoring System", Icon = "fas fa-network-wired", Target = "javascript:void()", TargetName = "Network Monitoring", Weight = 700 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = "/NMS", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Device", Name = "Device List", Description = $"{name} Device List", Icon = "far fa-circle", Target = "/NMS/Device", TargetName = "Device List", Weight = 200 },
                            new() { RoleId = roleid, Code = "Service", Name = "Service List", Description = $"{name} Service List", Icon = "far fa-circle", Target = "/NMS/Service", TargetName = "Service List", Weight = 300 }
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "PW";
                name = "Parent Website";
                if (!Context.Menu.Any(x => x.Code == code)) {
                    m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-chart-line", Target = "javascript:void()", TargetName = name, Weight = 800 };
                    m.Actions = new List<Action>() {
                        new() { RoleId = roleid, Code = "Home", Name = "Home", Description = $"{name} Introduction", Icon = "far fa-circle", Target = "/#home", TargetName = "Home", Weight = 100 },
                        new() { RoleId = roleid, Code = "Features", Name = "Features", Description = $"{name} Features Section", Icon = "far fa-circle", Target = "/#features", TargetName = "Features", Weight = 200 },
                        new() { RoleId = roleid, Code = "About", Name = "About", Description = $"{name} About Us Section", Icon = "far fa-circle", Target = "/#about", TargetName = "About us", Weight = 300 },
                        new() { RoleId = roleid, Code = "Pricing", Name = "Pricing", Description = $"{name} Pricing Section", Icon = "far fa-circle", Target = "/#pricing", TargetName = "Pricing", Weight = 400 },
                        new() { RoleId = roleid, Code = "FAQ", Name = "FAQ", Description = $"{name} Frequently Asked Questions", Icon = "far fa-circle", Target = "/#faq", TargetName = "FAQ", Weight = 500 },
                        new() { RoleId = roleid, Code = "Testimonials", Name = "Testimonials", Description = $"{name} Customer Testimonials", Icon = "far fa-circle", Target = "/#testimonials", TargetName = "Testimonials", Weight = 600 },
                        new() { RoleId = roleid, Code = "Team", Name = "Team", Description = $"{name} Meet the Team", Icon = "far fa-circle", Target = "/#team", TargetName = "Meet the team", Weight = 700 },
                        new() { RoleId = roleid, Code = "Contact", Name = "Contact", Description = $"{name} Contact Section", Icon = "far fa-circle", Target = "/#contact", TargetName = "Contact us", Weight = 800 },
                        new() { RoleId = roleid, Code = "How It Works", Name = "How it works", Description = $"{name} How It Works", Icon = "far fa-circle", Target = "/HowItWorks", TargetName = "How it works", Weight = 900 },
                        new() { RoleId = roleid, Code = "Terms", Name = "Terms", Description = $"{name} Terms Of Service", Icon = "far fa-circle", Target = "/Terms", TargetName = "Terms of service", Weight = 1000 },
                        new() { RoleId = roleid, Code = "Privacy", Name = "Privacy", Description = $"{name} Privacy Policy", Icon = "far fa-circle", Target = "/Privacy", TargetName = "Privacy policy", Weight = 1100 },
                        new() { RoleId = roleid, Code = "Refunds", Name = "Refunds", Description = $"{name} Refund Policy", Icon = "far fa-circle", Target = "/Refunds", TargetName = "Refund policy", Weight = 1200 },
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                code = "PM";
                name = "Project Manager";
                if (!Context.Menu.Any(x => x.Code == code)) {
                    m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name}", Icon = "fas fa-list-check", Target = "javascript:void()", TargetName = name, Weight = 900 };
                    m.Actions = new List<Action>() {
                        new() { RoleId = roleid, Code = "Project List", Name = "Project List", Description = $"{name} Project List", Icon = "far fa-circle", Target = "/project", TargetName = "Project List", Weight = 100 },
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                code = "PP";
                name = "Partner Portals";
                if (!Context.Menu.Any(x => x.Code == code)) {
                    m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name}", Icon = "fas fa-user-tie", Target = "javascript:void()", TargetName = name, Weight = 1000 };
                    m.Actions = new List<Action>() {
                        new() { RoleId = roleid, Code = "BS", Name = "Bootstrap", Description = $"{name} Bootstrap", Icon = "far fa-circle", Target = "https://getbootstrap.com/", TargetName = "Bootstrap", NewWindow = true, Weight = 100 },
                        new() { RoleId = roleid, Code = "BI", Name = "Bootstrap Icons", Description = $"{name} Bootstrap Icons", Icon = "far fa-circle", Target = "https://icons.getbootstrap.com/", TargetName = "Bootstrap Icons", NewWindow = true, Weight = 200 },
                        new() { RoleId = roleid, Code = "FA", Name = "Fontawesome", Description = $"{name} Fontawesome", Icon = "far fa-circle", Target = "https://fontawesome.com/icons", TargetName = "Fontawesome", NewWindow = true, Weight = 300 },
                        new() { RoleId = roleid, Code = "USPSBCG", Name = "USPS Business Gateway", Description = $"{name} USPS Business Customer Gateway", Icon = "far fa-circle", Target = "https://gateway.usps.com/eAdmin/view/signin", TargetName = "USPS Business Gateway", NewWindow = true, Weight = 400 },
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                // About
                if (!Context.Menu.Any(x => x.Code == "About")) {
                    m = new() { RoleId = roleid, TopLevel = false, Code = "About", Name = "About", Description = "About Us", Icon = "fas fa-info-circle", Target = "/home/about", TargetName = "About Us", Weight = 1100 };
                    m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Home", Name = "Home", Description = "Home", Icon = "far fa-circle", Target = "/", TargetName = "Home Page", Weight = 100 },
                            new() { RoleId = roleid, Code = "About", Name = "About Us", Description = "About Us", Icon = "far fa-circle", Target = "/home/about", TargetName = "About Us", Weight = 200 },
                            new() { RoleId = roleid, Code = "Contact", Name = "Contact", Description = "Contact Page", Icon = "far fa-circle", Target = "/home/contact", TargetName = "Contact Us", Weight = 300 },
                            new() { RoleId = roleid, Code = "Features", Name = "Features", Description = "Features Page", Icon = "far fa-circle", Target = "/home/features", TargetName = "Features", Weight = 400 }
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                code = "SMO";
                name = "SQL Server";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = $"{name} Management Objects", Description = $"{name} Management Objects", Icon = "fas fa-database", Target = "javascript:void()", TargetName = name, Weight = 1200 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = $"/{code}", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Backup", Name = "Backup", Description = $"{name} Backup", Icon = "far fa-circle", Target = $"/{code}/Backup", TargetName = "Backup", Weight = 200 },
                            new() { RoleId = roleid, Code = "Download", Name = "Download", Description = $"{name} Download", Icon = "far fa-circle", Target = $"/{code}/Download", TargetName = "Download", Weight = 300 },
                            new() { RoleId = roleid, Code = "Upload", Name = "Upload", Description = $"{name} Upload", Icon = "far fa-circle", Target = $"/{code}/Upload", TargetName = "Upload", Weight = 400 },
                            new() { RoleId = roleid, Code = "Restore", Name = "Restore", Description = $"{name} Restore", Icon = "far fa-circle", Target = $"/{code}/Restore", TargetName = "Restore", Weight = 500 },
                            new() { RoleId = roleid, Code = "Script", Name = "Script", Description = $"{name} Script", Icon = "far fa-circle", Target = $"/{code}/Script", TargetName = "Script", Weight = 600 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "Namecheap";
                name = "Namecheap";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-file-signature", Target = "javascript:void()", TargetName = name, Weight = 1300 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = $"{name} Dashboard", Icon = "far fa-circle", Target = $"/{code}", TargetName = "Dashboard", Weight = 100 },
                            new() { RoleId = roleid, Code = "Domain", Name = "Domain Registration", Description = $"{name} Domain Registration", Icon = "far fa-circle", Target = $"/{code}/Domain", TargetName = "Domains", Weight = 200 },
                            new() { RoleId = roleid, Code = "API", Name = "API Documentation", Description = $"{name} Documentation", Icon = "far fa-circle", Target = $"https://www.namecheap.com/support/api/intro/", TargetName = "API Documentation", NewWindow = true, Weight = 300 },
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "OVH";
                name = "OVH Cloud";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = name, Description = $"{name} System", Icon = "fas fa-cloud", Target = "javascript:void()", TargetName = name, Weight = 1400 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = "OVH Cloud Control Center Dashboard", Icon = "far fa-circle", Target = "/OVH", TargetName = "Dashboard", NewWindow = false, Weight = 100 },
                            new() { RoleId = roleid, Code = "API", Name = "API Manager", Description = "API Manager", Icon = "far fa-circle", Target = "/OVH/Api", TargetName = "API Manager", Weight = 200 },
                            new() { RoleId = roleid, Code = "Network", Name = "Network Manager", Description = "Network Manager", Icon = "far fa-circle", Target = "/OVH/Network", TargetName = "Network Manager", NewWindow = false, Weight = 300 },
                            new() { RoleId = roleid, Code = "License", Name = "License Manager", Description = "License Manager", Icon = "far fa-circle", Target = "/OVH/License", TargetName = "License Manager", NewWindow = false, Weight = 400 },
                            new() { RoleId = roleid, Code = "Dedicated", Name = "Dedicated Servers", Description = "Dedicated Server Management", Icon = "far fa-circle", Target = "/OVH/Dedicated", TargetName = "Dedicated Servers", NewWindow = false, Weight = 500 },
                            new() { RoleId = roleid, Code = "VPS", Name = "Virtual Servers", Description = "Virtual Private Server Management", Icon = "far fa-circle", Target = "/OVH/Virtual", TargetName = "Virtual Servers", NewWindow = false, Weight = 600 },
                            new() { RoleId = roleid, Code = "DOC", Name = "API Documentation", Description = "OVH Cloud API Documentation", Icon = "far fa-circle", Target = "https://api.us.ovhcloud.com/", TargetName = "API Documentation", NewWindow = true, Weight = 1000 }
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                code = "USPS";
                name = "USPS Web Tools";
                if (GetAssemblyList().Any(x => x.FullName.Contains(code))) {
                    if (!Context.Menu.Any(x => x.Code == code)) {
                        m = new Menu { RoleId = roleid, TopLevel = true, Code = code, Name = "USPS Console", Description = "USPS Control Center", Icon = "fab fa-usps", Target = "javascript:void()", TargetName = "USPS Console", Weight = 1500 };
                        m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Dashboard", Name = "Dashboard", Description = "USPS Control Center Dashboard", Icon = "far fa-circle", Target = "/USPS", TargetName = "Dashboard", NewWindow = false, Weight = 100 },
                            new() { RoleId = roleid, Code = "Address", Name = "Address Correction", Description = "USPS Address Standardization and Correction", Icon = "far fa-circle", Target = "/USPS/Address", TargetName = "Address Correction", NewWindow = false, Weight = 200 },
                            new() { RoleId = roleid, Code = "Carrier", Name = "Carrier Pickup", Description = "Carrier Pickup Scheduler", Icon = "far fa-circle", Target = "/USPS/Carrier", TargetName = "Schedule Pickup", NewWindow = false, Weight = 300 },
                            new() { RoleId = roleid, Code = "Labels", Name = "Domestic Labels", Description = "Purchase and Print Docmestic Labels", Icon = "far fa-circle", Target = "/USPS/Labels", TargetName = "Domestic Labels", NewWindow = false, Weight = 400 },
                            new() { RoleId = roleid, Code = "Pricing", Name = "Domestic Pricing", Description = "Estimate Domestic Shipping Cost", Icon = "far fa-circle", Target = "/USPS/Pricing", TargetName = "Domestic Pricing", NewWindow = false, Weight = 500 },
                            new() { RoleId = roleid, Code = "ILabels", Name = "International Labels", Description = "Purchase and Print International Labels", Icon = "far fa-circle", Target = "/OVH/ILabels", TargetName = "International Labels", NewWindow = false, Weight = 600 },
                            new() { RoleId = roleid, Code = "IPricing", Name = "International Pricing", Description = "Estimate International Shipping Cost", Icon = "far fa-circle", Target = "/USPS/IPricing", TargetName = "International Pricing", NewWindow = false, Weight = 700 },
                            new() { RoleId = roleid, Code = "Tracking", Name = "Delivery Tracking", Description = "Detailed Delivery Expectation and Status", Icon = "far fa-circle", Target = "/USPS/tracking", TargetName = "Shipment Tracking", NewWindow = false, Weight = 800 },
                            new() { RoleId = roleid, Code = "Organization", Name = "Organization Detail", Description = "Create or update Organization Details", Icon = "far fa-circle", Target = "/USPS/Organization", TargetName = "Organization Details", NewWindow = false, Weight = 900 },
                            new() { RoleId = roleid, Code = "Locations", Name = "Drop-off Locations", Description = "Determine the most convenient drop-off location", Icon = "far fa-circle", Target = "/USPS/Location", TargetName = "Drop-off Locations", NewWindow = false, Weight = 1000 },
                            new() { RoleId = roleid, Code = "Containers", Name = "Aggregate Types", Description = "Organize shipments into containers and pallets", Icon = "far fa-circle", Target = "/USPS/Container", TargetName = "Container Details", NewWindow = false, Weight = 1100 },
                            new() { RoleId = roleid, Code = "Standards", Name = "Estimate Delivery Interval", Description = "Estimate delivery interval", Icon = "far fa-circle", Target = "/USPS/Standards", TargetName = "Delivery Interval", NewWindow = false, Weight = 1200 },
                            new() { RoleId = roleid, Code = "Documentation", Name = "API Documentation", Description = "USPS Web Tools API Documentation", Icon = "far fa-circle", Target = "https://www.usps.com/business/web-tools-apis/documentation-updates.htm", TargetName = "API Documentation", NewWindow = true, Weight = 1200 }
                        };
                        Context.Menu.Add(m);
                        await Context.SaveChangesAsync();
                    }
                } else {
                    if (Context.Menu.Any(x => x.Code == code)) {
                        int menuid = Context.Menu.Where(x => x.Code == code).First().MenuId;
                        Context.Action.RemoveRange(await Context.Action.Where(x => x.MenuId == menuid).ToListAsync());
                        Context.Menu.RemoveRange(await Context.Menu.Where(x => x.Code == code).ToListAsync());
                        await Context.SaveChangesAsync();
                    }
                }

                // About
                if (!Context.Menu.Any(x => x.Code == "About")) {
                    m = new() { RoleId = roleid, TopLevel = false, Code = "About", Name = "About", Description = "About Us", Icon = "fas fa-info-circle", Target = "/home/about", TargetName = "About Us", Weight = 1600 };
                    m.Actions = new List<Action>() {
                            new() { RoleId = roleid, Code = "Home", Name = "Home", Description = "Home", Icon = "far fa-circle", Target = "/", TargetName = "Home Page", Weight = 100 },
                            new() { RoleId = roleid, Code = "About", Name = "About Us", Description = "About Us", Icon = "far fa-circle", Target = "/home/about", TargetName = "About Us", Weight = 200 },
                            new() { RoleId = roleid, Code = "Contact", Name = "Contact", Description = "Contact Page", Icon = "far fa-circle", Target = "/home/contact", TargetName = "Contact Us", Weight = 300 },
                            new() { RoleId = roleid, Code = "Features", Name = "Features", Description = "Features Page", Icon = "far fa-circle", Target = "/home/features", TargetName = "Features", Weight = 400 }
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                // Features
                if (!Context.Menu.Any(x => x.Code == "Features")) {
                    m = new Menu { RoleId = roleid, TopLevel = false, Code = "Features", Name = "Features", Description = "Features", Icon = "far fa-star", Target = "/home/features", TargetName = "Features", Weight = 1700 };
                    m.Actions = new List<Action>() {
                        new() { RoleId = roleid, Code = "How", Name = "How It Works", Description = "How It Works", Icon = "far fa-circle", Target = "/home/features", TargetName = "How It Works", Weight = 100 },
                        new() { RoleId = roleid, Code = "Privacy", Name = "Privacy Policy", Description = "Privacy Policy", Icon = "far fa-circle", Target = "/home/privacy", TargetName = "Privacy Policy", Weight = 200 },
                        new() { RoleId = roleid, Code = "Terms", Name = "Terms of Service", Description = "Terms of Service", Icon = "far fa-circle", Target = "/home/terms", TargetName = "Terms of Service", Weight = 300 },
                        new() { RoleId = roleid, Code = "Refund", Name = "Refund Policy", Description = "Refund Policy", Icon = "far fa-circle", Target = "/home/refund", TargetName = "Refund Policy", Weight = 400 }
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

                // Footer
                if (!Context.Menu.Any(x => x.Code == "Footer")) {
                    m = new Menu { RoleId = roleid, TopLevel = false, Code = "Footer", Name = "Footer Menu", Description = "Footer Menu", Icon = "far fa-star", Target = "javascript:void()", TargetName = "Footer", Weight = 1800 };
                    m.Actions = new List<Action>() {
                        new() { RoleId = roleid, Code = "How", Name = "How It Works", Description = "How It Works", Icon = "far fa-circle", Target = "/home/features", TargetName = "How It Works", Weight = 100 },
                        new() { RoleId = roleid, Code = "Privacy", Name = "Privacy Policy", Description = "Privacy Policy", Icon = "far fa-circle", Target = "/home/privacy", TargetName = "Privacy Policy", Weight = 200 },
                        new() { RoleId = roleid, Code = "Terms", Name = "Terms of Service", Description = "Terms of Service", Icon = "far fa-circle", Target = "/home/terms", TargetName = "Terms of Service", Weight = 300 },
                        new() { RoleId = roleid, Code = "Refund", Name = "Refund Policy", Description = "Refund Policy", Icon = "far fa-circle", Target = "/home/refund", TargetName = "Refund Policy", Weight = 400 }
                    };
                    Context.Menu.Add(m);
                    await Context.SaveChangesAsync();
                }

            } catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
            return Result;
        }

        #endregion

    }

    #endregion

}