using System.Collections.Specialized;
using System.Configuration;

namespace Doe.Ls.ProjectTemplate.Core.Settings  
{ 
    public class PositionEstablishmentSettings  
    {
	   
	    public static class ConnectionStrings 
	    {
	
	        public static string SampleProjectTemplateEntities
	        {
	            get { return  ConfigurationManager.ConnectionStrings[@"SampleProjectTemplateEntities"].ConnectionString.Trim();     }
	        }
        }
	   
	    public static class Portal 
	    {
			public static bool IsRealPortal
	        {
				get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["IsRealPortal"].Trim().ToBoolean(); }
	        }
	
	        public static string DecUserInfoServiceUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DecUserInfoServiceUrl"].Trim();     }
	        }
	
	        public static string DetPortalISecurityFactory
	        {
	            get { return  (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["det.portal.ISecurityFactory"].Trim();     }
	        }
	
	        public static string PageLogon
	        {
	            get { return  (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["PageLogon"].Trim();     }
	        }
	
	        public static string ConfirmationPageUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["ConfirmationPageUrl"].Trim();     }
	        }
			public static bool DebugAutoAuthentication
	        {
				get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DebugAutoAuthentication"].Trim().ToBoolean(); }
	        }
	
	        public static string DebugAutoAuthenticationUser
	        {
	            get { return  (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DebugAutoAuthenticationUser"].Trim();     }
	        }
        }
	   
	    public static class Trim 
	    {
	
	        public static string TrimApi
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["TrimApi"].Trim();     }
	        }
	
	        public static string TrimUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["TrimUrl"].Trim();     }
	        }
	
	        public static string DocumentUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["DocumentUrl"].Trim();     }
	        }
	
	        public static string TrimBoxUri
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["TrimBoxUri"].Trim();     }
	        }
	
	        public static string TrimServiceUsername
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["TrimServiceUsername"].Trim();     }
	        }
	
	        public static string TrimServicePassword
	        {
	            get { return  (ConfigurationManager.GetSection(@"trim") as NameValueCollection)["TrimServicePassword"].Trim();     }
	        }
        }
	   
	    public static class Notification 
	    {
	
	        public static string AdminEmailAddresses
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["AdminEmailAddresses"].Trim();     }
	        }
	
	        public static string NotificationEmailAddress
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["NotificationEmailAddress"].Trim();     }
	        }
			public static bool UseRealEmail
	        {
				get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["UseRealEmail"].Trim().ToBoolean(); }
	        }
	
	        public static string Subject
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["Subject"].Trim();     }
	        }
	
	        public static string Host
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["Host"].Trim();     }
	        }
	
	        public static string ContactEmailAddresses
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["ContactEmailAddresses"].Trim();     }
	        }
	
	        public static string ContactSubject
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["ContactSubject"].Trim();     }
	        }
	
	        public static string PerformanceMonitorMode
	        {
	            get { return  (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["performanceMonitorMode"].Trim();     }
	        }
        }
	   
	    public static class Site 
	    {
			public static bool UseGoogleAnalytic
	        {
				get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["UseGoogleAnalytic"].Trim().ToBoolean(); }
	        }
	
	        public static string GoogleAnalyticKey
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["GoogleAnalyticKey"].Trim();     }
	        }
			public static bool IsTestSite
	        {
				get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["IsTestSite"].Trim().ToBoolean(); }
	        }
			public static bool UseShareToolbar
	        {
				get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["UseShareToolbar"].Trim().ToBoolean(); }
	        }
	
	        public static string AssetFolderPath
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AssetFolderPath"].Trim();     }
	        }
			public static string AppUrl
	        {
	            get { return  EntityBase.Http.HttpHelper.GetAppUrl();     }
	        }
	
	        public static string FixedUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["FixedUrl"].Trim();     }
	        }
	
	        public static string AssetMaxSizeMb
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AssetMaxSizeMb"].Trim();     }
	        }
	
	        public static string PdfTemplatePath
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["PdfTemplatePath"].Trim();     }
	        }
	
	        public static string PdfOutputPath
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["PdfOutputPath"].Trim();     }
	        }
	
	        public static string OldDescriptionFileFolder
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["oldDescriptionFileFolder"].Trim();     }
	        }
	
	        public static string TrimIntegration
	        {
	            get { return  (ConfigurationManager.GetSection(@"site") as NameValueCollection)["TrimIntegration"].Trim();     }
	        }
        }
	   
	    public static class Logger 
	    {
	
	        public static string AppLoggerServiceUrl
	        {
	            get { return  (ConfigurationManager.GetSection(@"logger") as NameValueCollection)["AppLoggerServiceUrl"].Trim();     }
	        }
	
	        public static string AppLoggerId
	        {
	            get { return  (ConfigurationManager.GetSection(@"logger") as NameValueCollection)["AppLoggerID"].Trim();     }
	        }
        }
	   
	    public static class SystemNet 
	    {
		   
		    public static class DefaultProxy 
		    {
			   
			    public static class Bypasslist 
			    {
		        }
	        }
        }

    }/*End Class GlobalSettings*/
}/*End namespace*/
