#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

#endregion

#nullable disable

namespace DNA3.Models {
    public partial class MainContext : DbContext {

        #region Variables

        public readonly IConfiguration Configuration;

        #endregion

        #region Methods

        public MainContext() {
        }

        public MainContext(DbContextOptions<MainContext> options, IConfiguration configuration) : base(options) {
            Configuration = configuration;
        }

        #endregion

        #region Mapped

        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Deviceservice> Deviceservice { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<Commission> Commission { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<Workorder> Workorder { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Disposition> Disposition { get; set; }
        public virtual DbSet<Associate> Associate { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Residence> Residence { get; set; }
        public virtual DbSet<Source> Source { get; set; }
        public virtual DbSet<Homeowner> Homeowner { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItem { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Condition> Condition { get; set; }
        public virtual DbSet<SortOrder> SortOrder { get; set; }
        public virtual DbSet<ListingType> ListingType { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        //public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<WorkorderComment> WorkorderComment { get; set; }
        public virtual DbSet<CustomerComment> CustomerComment { get; set; }

        #endregion

        #region Not Mapped

        public virtual DbSet<Claimant> Claimant { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Databases> Databases { get; set; }
        public virtual DbSet<DailyServiceSummary> DailyServiceSummary { get; set; }
        public virtual DbSet<MonthlyServiceSummary> MonthlyServiceSummary { get; set; }

        #endregion

        #region Configuring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MainContext"));
            }
        }

        #endregion

        #region Creating

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Claimant>().HasNoKey();
            modelBuilder.Entity<Config>().HasNoKey();
            modelBuilder.Entity<Databases>().HasKey(e => e.database_id);
            modelBuilder.Entity<DailyServiceSummary>().HasNoKey();
            modelBuilder.Entity<MonthlyServiceSummary>().HasNoKey();

            modelBuilder.Entity<Project>(entity => {
                entity.Property(e => e.ProjectId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(300);
            });

            modelBuilder.Entity<Note>(entity => {
                entity.Property(e => e.NoteId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Subject).HasMaxLength(100);
            });

            modelBuilder.Entity<WorkorderComment>(entity => {
                entity.Property(e => e.WorkorderCommentId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.CommentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerComment>(entity => {
                entity.Property(e => e.CustomerCommentId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.CommentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Resource>(entity => {
                entity.Property(e => e.ResourceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Unit).HasMaxLength(10);
            });

            modelBuilder.Entity<Commission>(entity => {
                entity.Property(e => e.CommissionId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Percentage).HasColumnType("decimal(10, 3)");
            });

            modelBuilder.Entity<Table>(entity => {
                entity.Property(e => e.TableId).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(30);
            });

            modelBuilder.Entity<Workorder>(entity => {
                entity.Property(e => e.WorkorderId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(300);

                entity.Property(e => e.Type).HasMaxLength(30);

                entity.Property(e => e.Status).HasMaxLength(30);

                entity.Property(e => e.Payment).HasMaxLength(10);

                entity.Property(e => e.Labor).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Parts).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Other).HasColumnType("numeric(10, 2)");
            });

            modelBuilder.Entity<DailyServiceSummary>(entity => {
                entity.Property(e => e.Labor).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Parts).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Other).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Total).HasColumnType("numeric(10, 2)");
            });

            modelBuilder.Entity<MonthlyServiceSummary>(entity => {
                entity.Property(e => e.Labor).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Parts).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Total).HasColumnType("numeric(10, 2)");
            });

            modelBuilder.Entity<Promotion>(entity => {
                entity.Property(e => e.PromotionId).UseIdentityColumn(10000, 1);

                entity.HasKey(e => e.PromotionId);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Zone>(entity => {
                entity.Property(e => e.ZoneId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<History>(entity => {
                entity.Property(e => e.HistoryId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Cycle).HasMaxLength(10);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Labor).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Parts).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Saltlevel).HasColumnType("numeric(3, 1)");

                entity.Property(e => e.Tax).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Total).HasColumnType("numeric(10, 2)");
            });

            modelBuilder.Entity<Disposition>(entity => {
                entity.Property(e => e.DispositionId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Associate>(entity => {
                entity.Property(e => e.AssociateId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Email)
                    .HasColumnName("EMail")
                    .HasMaxLength(100);

                entity.Property(e => e.First).HasMaxLength(30);

                entity.Property(e => e.Last).HasMaxLength(30);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Plus4).HasMaxLength(4);

                entity.Property(e => e.State).HasMaxLength(30);

                entity.Property(e => e.Zip).HasMaxLength(5);

            });

            modelBuilder.Entity<Source>(entity => {
                entity.Property(e => e.SourceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(30);
            });

            modelBuilder.Entity<Material>(entity => {
                entity.Property(e => e.MaterialId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Carrier).HasMaxLength(10);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.County).HasMaxLength(30);

                entity.Property(e => e.First1).HasMaxLength(50);

                entity.Property(e => e.First2).HasMaxLength(50);

                entity.Property(e => e.Last1).HasMaxLength(50);

                entity.Property(e => e.Last2).HasMaxLength(50);

                entity.Property(e => e.Name1).HasMaxLength(100);

                entity.Property(e => e.Name2).HasMaxLength(100);

                entity.Property(e => e.Phone1).HasMaxLength(20);

                entity.Property(e => e.Phone2).HasMaxLength(20);

                entity.Property(e => e.Plus4).HasMaxLength(4);

                entity.Property(e => e.Published).HasColumnType("date");

                entity.Property(e => e.Recorded).HasColumnType("date");

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.Value).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Zip).HasMaxLength(5);
            });

            modelBuilder.Entity<Residence>(entity => {
                entity.Property(e => e.ResidenceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Carrier).HasMaxLength(10);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.County).HasMaxLength(30);

                entity.Property(e => e.First1).HasMaxLength(50);

                entity.Property(e => e.First2).HasMaxLength(50);

                entity.Property(e => e.Last1).HasMaxLength(50);

                entity.Property(e => e.Last2).HasMaxLength(50);

                entity.Property(e => e.Name1).HasMaxLength(100);

                entity.Property(e => e.Name2).HasMaxLength(100);

                entity.Property(e => e.Phone1).HasMaxLength(13);

                entity.Property(e => e.Phone2).HasMaxLength(13);

                entity.Property(e => e.Plus4).HasMaxLength(4);

                entity.Property(e => e.Published).HasColumnType("date");

                entity.Property(e => e.Recorded).HasColumnType("date");

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.Value).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Zip).HasMaxLength(5);
            });

            modelBuilder.Entity<Homeowner>(entity => {
                entity.Property(e => e.HomeownerId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Carrier).HasMaxLength(10);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.County).HasMaxLength(30);

                entity.Property(e => e.First1).HasMaxLength(50);

                entity.Property(e => e.First2).HasMaxLength(50);

                entity.Property(e => e.Last1).HasMaxLength(50);

                entity.Property(e => e.Last2).HasMaxLength(50);

                entity.Property(e => e.Name1).HasMaxLength(100);

                entity.Property(e => e.Name2).HasMaxLength(100);

                entity.Property(e => e.Phone1).HasMaxLength(13);

                entity.Property(e => e.Phone2).HasMaxLength(13);

                entity.Property(e => e.Plus4).HasMaxLength(4);

                entity.Property(e => e.Published).HasColumnType("date");

                entity.Property(e => e.Recorded).HasColumnType("date");

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.Value).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Zip).HasMaxLength(5);
            });

            modelBuilder.Entity<Invoice>(entity => {
                entity.ToTable("Invoice");

                entity.Property(e => e.InvoiceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Reference).HasMaxLength(10);
            });

            modelBuilder.Entity<InvoiceItem>(entity => {
                entity.ToTable("InvoiceItem");

                entity.Property(e => e.InvoiceItemId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Quantity).HasColumnType("numeric(10, 2)");
            });

            modelBuilder.Entity<Customer>(entity => {
                entity.Property(e => e.CustomerId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.First1).HasMaxLength(30);

                entity.Property(e => e.Last1).HasMaxLength(30);

                entity.Property(e => e.Phone1).HasMaxLength(20);

                entity.Property(e => e.First2).HasMaxLength(30);

                entity.Property(e => e.Last2).HasMaxLength(30);

                entity.Property(e => e.Phone2).HasMaxLength(20);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.State).HasMaxLength(2);

                entity.Property(e => e.Zip).HasMaxLength(5);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Plus4).HasMaxLength(4);

                entity.Property(e => e.Installed).HasColumnType("date");

                entity.Property(e => e.Recorded).HasColumnType("date");

                entity.Property(e => e.HighRisk).HasMaxLength(30);

                entity.Property(e => e.PromoIdentity).HasMaxLength(100);
            });


            modelBuilder.Entity<Reference>(entity => {
                entity.ToTable("Reference");

                entity.Property(e => e.ReferenceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Target).HasMaxLength(256);

                entity.Property(e => e.TargetName).HasMaxLength(100);

                entity.Property(e => e.Icon).HasMaxLength(50);
            });

            modelBuilder.Entity<Account>(entity => {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity => {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Icon).HasMaxLength(30);

                entity.Property(e => e.Image).HasMaxLength(260);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Target).HasMaxLength(100);

                entity.Property(e => e.TargetName).HasMaxLength(30);
            });

            modelBuilder.Entity<Device>(entity => {
                entity.ToTable("Device");

                entity.Property(e => e.DeviceId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Connected).HasColumnType("datetime");

                entity.Property(e => e.Hostname).HasMaxLength(30);

                entity.Property(e => e.Ipv4).HasMaxLength(15);

                entity.Property(e => e.Message).HasMaxLength(100);
            });

            modelBuilder.Entity<SortOrder>(entity => {
                entity.HasKey(e => e.SortorderId);

                entity.ToTable("SortOrder");

                entity.Property(e => e.Code)
                    .HasColumnName("Code")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ListingType>(entity => {
                entity.HasKey(e => e.ListingtypeId);

                entity.ToTable("ListingType");

                entity.Property(e => e.Code)
                    .HasColumnName("Code")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Condition>(entity => {
                entity.HasKey(e => e.ConditionId);

                entity.ToTable("Condition");

                entity.Property(e => e.Code)
                    .HasColumnName("Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity => {
                entity.HasKey(e => e.TaskId);

                entity.ToTable("Task");

                entity.Property(e => e.TaskId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Ticket>(entity => {
                entity.HasKey(e => e.TicketId);

                entity.ToTable("Ticket");

                entity.Property(e => e.TicketId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Finish).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");
            });


            modelBuilder.Entity<Facility>(entity => {
                entity.HasKey(e => e.FacilityId);

                entity.ToTable("Facility");

                entity.Property(e => e.FacilityId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Contact).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.State).HasMaxLength(30);

                entity.Property(e => e.Zip).HasMaxLength(10);
            });

            modelBuilder.Entity<Vendor>(entity => {
                entity.HasKey(e => e.VendorId);

                entity.ToTable("Vendor");

                entity.Property(e => e.VendorId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.State).HasMaxLength(30);

                entity.Property(e => e.Zip).HasMaxLength(5);

                entity.Property(e => e.Zip4).HasMaxLength(4);

                entity.Property(e => e.Contact).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(100);

            });

            modelBuilder.Entity<Cart>(entity => {
                entity.HasKey(e => e.CartId);

                entity.ToTable("Cart");

                entity.Property(e => e.CartId).ValueGeneratedNever();

                entity.Property(e => e.CartId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<Item>(entity => {
                entity.HasKey(e => e.ItemId);

                entity.ToTable("Item");

                entity.Property(e => e.ItemId).UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("Product");

                entity.Property(e => e.ProductId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Icon).HasMaxLength(30);

                entity.Property(e => e.Image).HasMaxLength(260);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Target).HasMaxLength(100);

                entity.Property(e => e.TargetName).HasMaxLength(30);
            });

            modelBuilder.Entity<Asset>(entity => {
                entity.HasKey(e => e.AssetId);

                entity.ToTable("Asset");

                entity.Property(e => e.AssetId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Area).HasMaxLength(30);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Model).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.Serial).HasMaxLength(30);

            });

            modelBuilder.Entity<Action>(entity => {
                entity.HasKey(e => e.ActionId);

                entity.ToTable("Action");

                entity.Property(e => e.ActionId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Class).HasMaxLength(30);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Icon).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Target).HasMaxLength(100);

                entity.Property(e => e.TargetName).HasMaxLength(50);

                entity.Property(e => e.NewWindow).HasDefaultValue(false);

            });

            modelBuilder.Entity<Menu>(entity => {
                entity.HasKey(e => e.MenuId);

                entity.ToTable("Menu");

                entity.Property(e => e.MenuId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Icon).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Target).HasMaxLength(100);

                entity.Property(e => e.TargetName).HasMaxLength(50);
            });

            //modelBuilder.Entity<Application>(entity => {
            //    entity.HasKey(e => e.ApplicationId);

            //    entity.ToTable("Application");

            //    entity.Property(e => e.ApplicationId).UseIdentityColumn(10000, 1);

            //    entity.Property(e => e.Icon).HasMaxLength(30);

            //    entity.Property(e => e.Type).HasMaxLength(50);

            //    entity.Property(e => e.Code).HasMaxLength(50);

            //    entity.Property(e => e.Name).HasMaxLength(50);

            //    entity.Property(e => e.Target).HasMaxLength(50);

            //    entity.Property(e => e.TargetName).HasMaxLength(50);

            //    entity.Property(e => e.Area).HasMaxLength(50);

            //    entity.Property(e => e.Controller).HasMaxLength(50);

            //    entity.Property(e => e.Action).HasMaxLength(50);

            //});

            modelBuilder.Entity<Page>(entity => {
                entity.HasKey(e => e.PageId);

                entity.ToTable("Page");

                entity.Property(e => e.PageId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PageId).ValueGeneratedOnAdd();

                entity.Property(e => e.Slug).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(100);
            });

            modelBuilder.Entity<Article>(entity => {
                entity.HasKey(e => e.ArticleId);

                entity.ToTable("Article");

                entity.Property(e => e.ArticleId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(260);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(300);

                entity.Property(e => e.Target).HasMaxLength(100);

                entity.Property(e => e.TargetName).HasMaxLength(50);

                entity.Property(e => e.Tags).HasMaxLength(300);
            });

            modelBuilder.Entity<Category>(entity => {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(50);

                entity.Property(e => e.Subject).HasMaxLength(100);
            });

            modelBuilder.Entity<Client>(entity => {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("Client");

                entity.Property(e => e.ClientId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Address1).HasMaxLength(50);

                entity.Property(e => e.Address2).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.Zip).HasMaxLength(50);

                entity.Property(e => e.Zip1).HasMaxLength(50);
            });

            modelBuilder.Entity<Login>(entity => {
                entity.HasKey(e => e.LoginId);

                entity.ToTable("Login");

                entity.Property(e => e.LoginId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Email).HasMaxLength(190);

                entity.Property(e => e.Password).HasMaxLength(512);

                entity.Property(e => e.Provider).HasMaxLength(50);
            });

            modelBuilder.Entity<Request>(entity => {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("Request");

                entity.Property(e => e.RequestId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Company).HasMaxLength(50);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(190);

                entity.Property(e => e.First).HasMaxLength(100);

                entity.Property(e => e.Last).HasMaxLength(100);

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.Subscribe).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(30);
            });

            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("Role");

                entity.Property(e => e.RoleId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Section>(entity => {
                entity.HasKey(e => e.SectionId);

                entity.ToTable("Section");

                entity.Property(e => e.SectionId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<Status>(entity => {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("Status");

                entity.Property(e => e.StatusId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);

            });

            modelBuilder.Entity<Campaign>(entity => {
                entity.HasKey(e => e.CampaignId);

                entity.ToTable("Campaign");

                entity.Property(e => e.CampaignId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Icon).HasMaxLength(30);

                entity.Property(e => e.Code).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasKey(e => e.UserId);

                entity.ToTable("User");

                entity.Property(e => e.UserId).UseIdentityColumn(10000, 1);

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.First).HasMaxLength(50);

                entity.Property(e => e.Last).HasMaxLength(50);

                entity.Property(e => e.Persist).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotpKey).HasMaxLength(10);

                entity.Property(e => e.TotpManualSetup).HasMaxLength(30);

                entity.Property(e => e.TotpDeviceName).HasMaxLength(30);

                entity.Property(e => e.TotpCode).HasMaxLength(10);

                entity.Property(e => e.Token).HasMaxLength(512);

                entity.Property(e => e.TokenDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #endregion

    }
}
