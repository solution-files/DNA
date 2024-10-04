#region Usings

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

#endregion

namespace DNA3.Models {

    #region Secret

    // AppSettings object = JsonConvert.DeserializeObject<AppSettings>(jsoncontent);
    public class Secret {

        [Display(Name = "Connection Strings")]
        public ConnectionStrings ConnectionStrings { get; set; }

        [Display(Name = "Password Complexity Rules")]
        public PasswordRules PasswordRules { get; set; }

        [Display(Name = "Document Management")]
        public DOCS DOCS { get; set; }

        [Display(Name = "SQL Server Management Objects")]
        public SmoSettings SmoSettings { get; set; }

        [Display(Name = "SMTP Protocol")]
        public Smtp Smtp { get; set; }

        [Display(Name = "JWT Authentication")]
        public Jwt Jwt { get; set; }

        [Display(Name = "Serilog Configuration")]
        public Serilog Serilog { get; set; }

        [Display(Name = "OKTA Configuration")]
        public OktaSettings OktaSettings { get; set; }

        [Display(Name = "GitHub Configuration")]
        public GitHubSettings GitHubSettings { get; set; }

        [Display(Name = "Namecheap Registration")]
        public NameCheapSettings NameCheapSettings { get; set; }

        [Display(Name = "Verisign Configuration")]
        public VerisignSettings VerisignSettings { get; set; }

        [Display(Name = "PayPal Settings")]
        public PayPalSettings PayPalSettings { get; set; }

        [Display(Name = "OVH Cloud Configuration")]
        public OvhCloudSettings OvhCloudSettings { get; set; }

        [Display(Name = "EBay Settings")]
        public EBaySettings EBaySettings { get; set; }

        [Display(Name = "EBid Settings")]
        public EBidSettings EBidSettings { get; set; }

    }

    #endregion

    #region Connection Strings

    public class ConnectionStrings {

        [Display(Name = "Primary Connection String")]
        [Required(ErrorMessage = "{0} Cannot be empty")]
        public string MainContext { get; set; }

    }

    #endregion

    #region Document Settings

    public class DOCS {

        [Display(Name = "DOC Source Folder")]
        public string SourceFolderName { get; set; }

        [Display(Name = "Backup Path")]
        public string BackupStoragePath { get; set; }

    }

    #endregion

    #region EBay Settings

    public class EBaySettings {

        [Display(Name = "EBay App ID")]
        public string AppId { get; set; }

        [Display(Name = "Dev ID")]
        public string DevId { get; set; }

        [Display(Name = "Cert ID")]
        public string CertId { get; set; }
    }

    #endregion

    #region EBid Settings

    public class EBidSettings {

        [Display(Name = "EBid Application ID")]
        public string ApplicationId { get; set; }

        [Display(Name = "Application Key")]
        public string ApplicationKey { get; set; }

        [Display(Name = "Access Token")]
        public string UserAccessToken { get; set; }

    }

    #endregion

    #region GitHub Settings

    public class GitHubSettings {

        [Display(Name = "GitHub Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }
    }

    #endregion

    #region JWT Settings

    public class Jwt {

        [Display(Name = "JWT Key")]
        public string Key { get; set; }

        [Display(Name = "JWT Issuer")]
        public string Issuer { get; set; }

        [Display(Name = "JWT Expiry Days")]
        public int ExpireDays { get; set; }

    }

    #endregion

    #region Namecheap Settings

    public class NameCheapSettings {

        [Display(Name = "Namecheap API Key")]
        public string ApiKey { get; set; }

        [Display(Name = "API User")]
        public string ApiUser { get; set; }

        [Display(Name = "API Address")]
        public string ApiAddress { get; set; }

    }

    #endregion

    #region Okta Settings

    public class OktaSettings {

        [Display(Name = "Okta Domain")]
        public string Domain { get; set; }

        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

    }

    #endregion

    #region OvhCloud Settings

    public class OvhCloudSettings {

        [Display(Name = "OVHCloud Allowed IP")]
        public string AllowedIp { get; set; }

        [Display(Name = "Application Key")]
        public string ApplicationKey { get; set; }

        [Display(Name = "Application Secret")]
        public string ApplicationSecret { get; set; }

        [Display(Name = "Consumer Key")]
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

    public class PayPalSettings {

        [Display(Name = "PayPal Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

        [Display(Name = "Mode")]
        public string Mode { get; set; }
    }

    #endregion

    #region Smo Settings

    public class SmoSettings {

        [Display(Name = "SMO Server Instance")]
        public string ServerInstance { get; set; }

        [Display(Name = "Secure")]
        public string LoginSecure { get; set; }

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Backup Path")]
        public string BackupStoragePath { get; set; }

    }

    #endregion

    #region SMTP Settings

    public class Smtp {

        [Display(Name = "SMTP Host")]
        public string Host { get; set; }

        [Display(Name = "SMTP Port")]
        public string Port { get; set; }

        [Display(Name = "SMTP User")]
        public string User { get; set; }

        [Display(Name = "SMTP Password")]
        public string Password { get; set; }

        [Display(Name = "SMTP From Address")]
        public string From { get; set; }
    }

    #endregion

    #region Serilog

    public partial class Serilog {

        public string[] Using { get; set; }

        public string MinimumLevel { get; set; }

        public WriteToElement[] WriteTo { get; set; }
    }

    public struct WriteToElement {
        public string String;
        public WriteToClass WriteToClass;

        public static implicit operator WriteToElement(string String) => new WriteToElement { String = String };
        public static implicit operator WriteToElement(WriteToClass WriteToClass) => new WriteToElement { WriteToClass = WriteToClass };
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

    #endregion

    #region Verisign Settings

    public class VerisignSettings {

        [Display(Name = "Verisign User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Portal")]
        public string Portal { get; set; }

    }

    #endregion

}
