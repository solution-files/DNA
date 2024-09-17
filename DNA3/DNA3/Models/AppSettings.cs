#region Usings

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

#region Class

namespace DNA3.Models {

    #region AppSettings

    // AppSettings object = JsonConvert.DeserializeObject<AppSettings>(jsoncontent);
    public class AppSettingsGrr {

        [Display(Name = "Application")]
        public App App { get; set; }

        [Display(Name = "Connection Strings")]
        public ConnectionStrings ConnectionStrings { get; set; }

        [Display(Name = "Password Complexity Rules")]
        public PasswordRules PasswordRules { get; set; }

        [Display(Name = "Document Management")]
        public DOCS DOCS { get; set; }

        [Display(Name = "SQL Server Management Objects")]
        public SMOSettings SMOSettings { get; set; }

        [Display(Name = "SMTP Protocol")]
        public Smtp Smtp { get; set; }

        [Display(Name = "JWT Authentication")]
        public Jwt Jwt { get; set; }

        [Display(Name = "Serilog Configuration")]
        public Serilog Serilog { get; set; }

        [Display(Name = "OKTA Configuration")]
        public Okta Okta { get; set; }

        [Display(Name = "GitHub Configuration")]
        public GitHub GitHub { get; set; }

        [Display(Name = "Namecheap Registration")]
        public NameCheapSettings NameCheapSettings { get; set; }

        [Display(Name = "Verisign Configuration")]
        public Verisign Verisign { get; set; }

        [Display(Name = "PayPal Settings")]
        public PayPal PayPal { get; set; }

        [Display(Name = "OVH Cloud Configuration")]
        public OVHCloud OVHCloud { get; set; }

        [Display(Name = "EBay Settings")]
        public EBaySettings EBaySettings { get; set; }

        [Display(Name = "EBid Settings")]
        public EBidSettings EBidSettings { get; set; }

        [Display(Name = "Allowed Hosts")]
        public string AllowedHosts { get; set; }
    }

    #endregion

    #region App

    public class App {

        [Display(Name = "Application Name")]
        public string Name { get; set; }

        [Display(Name = "Short Name")]
        public string Shortname { get; set; }

        [Display(Name = "Application Version")]
        public string Version { get; set; }

        [Display(Name = "Domain Name")]
        public string Domain { get; set; }

        [Display(Name = "Application URL")]
        public string URL { get; set; }

        [Display(Name = "Application Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Application Tagline")]
        public string Tagline { get; set; }

        [Display(Name = "Application Description")]
        public string Description { get; set; }

        [Display(Name = "Application Copyright")]
        public string Copyright { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "Street Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string Zip { get; set; }

        [Display(Name = "Telephone Number")]
        public string Phone { get; set; }

        [Display(Name = "Application Author")]
        public string Author { get; set; }

        [Display(Name = "Billing Name")]
        public string BillingName { get; set; }

        [Display(Name = "Billing E-Mail Address")]
        public string BillingAddress { get; set; }

        [Display(Name = "Sales Name")]
        public string SalesName { get; set; }

        [Display(Name = "Sales E-Mail Address")]
        public string SalesAddress { get; set; }

        [Display(Name = "Support Name")]
        public string SupportName { get; set; }

        [Display(Name = "Support E-Mail Address")]
        public string SupportAddress { get; set; }

        [Display(Name = "Apple Link Address")]
        public string Apple { get; set; }

        [Display(Name = "Facebook Link Address")]
        public string Facebook { get; set; }

        [Display(Name = "GetHub Length")]
        public string Github { get; set; }

        [Display(Name = "Required Link Address")]
        public string Google { get; set; }

        [Display(Name = "Instagram Link Address")]
        public string Instagram { get; set; }

        [Display(Name = "LinkedIn Link Address")]
        public string Linkedin { get; set; }

        [Display(Name = "Mastodon Link Address")]
        public string Mastodon { get; set; }

        [Display(Name = "Microsoft Link Address")]
        public string Microsoft { get; set; }

        [Display(Name = "Twitter Link Address")]
        public string Twitter { get; set; }

        [Display(Name = "Xing Link Address")]
        public string Xing { get; set; }

        [Display(Name = "Theme Name")]
        public string Themename { get; set; }

        [Display(Name = "Theme Path")]
        public string Themepath { get; set; }
    }

    #endregion

    #region Connection Strings

    public class ConnectionStrings {

        [Display(Name = "Primary Database Context")]
        [Required(ErrorMessage = "{0} Cannot be empty")]
        public string MainContext { get; set; }

    }

    #endregion

    #region Document Settings

    public class DOCS {
        public string SourceFolderName { get; set; }
        public string BackupStoragePath { get; set; }
    }

    #endregion

    #region EBay Settings

    public class EBaySettings {
        public string AppID { get; set; }
        public string DevID { get; set; }
        public string CertID { get; set; }
    }

    #endregion

    #region EBid Settings

    public class EBidSettings {
        public string ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string UserAccessToken { get; set; }
    }

    #endregion

    #region GitHub Settings

    public class GitHub {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    #endregion

    #region JWT Settings

    public class Jwt {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int ExpireDays { get; set; }
    }

    #endregion

    #region Namecheap Settings

    public class NameCheapSettings {
        public string APIUser { get; set; }
        public string APIKey { get; set; }
        public string APIAddress { get; set; }
    }

    #endregion

    #region Okta Settings

    public class Okta {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    #endregion

    #region OVHCloud

    public class OVHCloud {
        public string AllowedIP { get; set; }
        public string ApplicationKey { get; set; }
        public string ApplicationSecret { get; set; }
        public string ConsumerKey { get; set; }
    }

    #endregion

    #region Password Rules

    public class PasswordRules {

        [Display(Name = "Required Length")]
        public string RequiredLength { get; set; }

        [Display(Name = "Unique Characters Required")]
        public string RequiredUniqueChars { get; set; }

        [Display(Name = "Require Digits")]
        public bool RequireDigit { get; set; }

        [Display(Name = "Required Lower Case")]
        public bool RequireLowercase { get; set; }

        [Display(Name = "Require Alphanumeric")]
        public bool RequireNonAlphanumeric { get; set; }

        [Display(Name = "Require Upper Case")]
        public bool RequireUppercase { get; set; }
    }

    #endregion

    #region PayPal Settings

    public class PayPal {

        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

        [Display(Name = "Mode")]
        public string Mode { get; set; }
    }

    #endregion

    public struct WriteToElement {
        public string String;
        public WriteToClass WriteToClass;

        public static implicit operator WriteToElement(string String) => new WriteToElement { String = String };
        public static implicit operator WriteToElement(WriteToClass WriteToClass) => new WriteToElement { WriteToClass = WriteToClass };
    }

    public partial class Serilog {

        public string[] Using { get; set; }

        public string MinimumLevel { get; set; }

        public WriteToElement[] WriteTo { get; set; }
    }

    public partial class WriteToClass {

        public string Name { get; set; }

        public Args Args { get; set; }
    }

    public partial class Args {

        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public bool AutoCreateSqlTable { get; set; }

        public ColumnOptionsSection ColumnOptionsSection { get; set; }
    }

    public partial class ColumnOptionsSection {

        public string[] RemoveStandardColumns { get; set; }

        public CustomColumn[] CustomColumns { get; set; }
    }

    public partial class CustomColumn {

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public bool AllowNull { get; set; }
    }

    public class SMOSettings {
        public string ServerInstance { get; set; }
        public string LoginSecure { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string BackupStoragePath { get; set; }
    }

    public class Smtp {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }

    public class Verisign {
        public string Portal { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    #endregion

}
