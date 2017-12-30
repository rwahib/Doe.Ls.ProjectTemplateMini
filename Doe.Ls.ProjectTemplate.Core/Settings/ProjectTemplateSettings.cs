using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Doe.Ls.ProjectTemplate.Core.Settings
{
    public class ProjectTemplateSettings
    {

        public static class ConnectionStrings
        {
            public static string SampleProjectTemplateEntities
                {
                get { return ConfigurationManager.ConnectionStrings[@"SampleProjectTemplateEntities"].ConnectionString.Trim(); }
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
                get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DecUserInfoServiceUrl"].Trim(); }
            }
            public static string DetPortalISecurityFactory
            {
                get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["det.portal.ISecurityFactory"].Trim(); }
            }
            public static string PageLogon
            {
                get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["PageLogon"].Trim(); }
            }
            public static string ConfirmationPageUrl
            {
                get
                {
                    var confirmationUrl = (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["ConfirmationPageUrl"].Trim();
                    confirmationUrl = confirmationUrl.Replace("~/", "");
                    return $"{Site.AppUrl}{confirmationUrl}";
                }
            }
            public static bool DebugAutoAuthentication
            {
                get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DebugAutoAuthentication"].Trim().ToBoolean(); }
            }
            public static string DebugAutoAuthenticationUser
            {
                get { return (ConfigurationManager.GetSection(@"portal") as NameValueCollection)["DebugAutoAuthenticationUser"].Trim(); }
            }
        }

        public static class Notification
        {
            public static string AdminEmailAddresses
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["AdminEmailAddresses"].Trim(); }
            }
            public static string NotificationEmailAddress
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["NotificationEmailAddress"].Trim(); }
            }
            public static bool UseRealEmail
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["UseRealEmail"].Trim().ToBoolean(); }
            }
            public static string Subject
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["Subject"].Trim(); }
            }
            public static string Host
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["Host"].Trim(); }
            }
            public static string GetContactEmailAddresses()
            {
                var emailWithDisplay = (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["ContactEmailAddresses"].Trim();
                if(emailWithDisplay.Split(';', ':').Length > 1) return emailWithDisplay.Split(';', ':')[0].Trim();

                return emailWithDisplay.Trim();
                }
            public static string GetDisplayContactEmailAddresses()
                {
                var emailWithDisplay = (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["ContactEmailAddresses"].Trim();
                if(emailWithDisplay.Split(';',':').Length > 1) return emailWithDisplay.Split(';', ':')[1].Trim();

                return emailWithDisplay.Trim();
                }
            public static string ContactSubject
            {
                get { return (ConfigurationManager.GetSection(@"notification") as NameValueCollection)["ContactSubject"].Trim(); }
            }
            public static bool PerformanceMonitorMode
                {
                get
                {
                    var nameValueCollection = ConfigurationManager.GetSection(@"notification") as NameValueCollection;
                    return nameValueCollection != null && nameValueCollection["performanceMonitorMode"].Trim().ToBoolean();
                }
                }
        }

        public static class Site
        {
            public static string GetUserGuidePath()
            {
                return System.IO.Path.Combine(AssetFolderPath, "UserGuideContent");
            }


            public static bool UseGoogleAnalytic
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["UseGoogleAnalytic"].Trim().ToBoolean(); }
            }

          

            public static string GoogleAnalyticKey
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["GoogleAnalyticKey"].Trim(); }
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
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AssetFolderPath"].Trim(); }
            }
            public static string AppUrl
            {
                get { return FixedUrl? (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AppUrl"].Trim(): EntityBase.Http.HttpHelper.GetAppUrl(); }
            }
            public static bool FixedUrl
                {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["FixedUrl"].Trim().ToBoolean(); }
                }

            public static decimal AssetMaxSizeMb
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AssetMaxSizeMb"].Trim().ToDecimal(); }
            }

            public static bool ShowUpcomingEvents
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["ShowUpcomingEvents"].Trim().ToBoolean(); }
            }
            public static string PdfTemplatePath
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["PdfTemplatePath"].Trim(); }
            }

            public static string PdfOutputPath
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["PdfOutputPath"].Trim(); }
            }

            public static string OldDescriptionFileFolder
            {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["oldDescriptionFileFolder"].Trim(); }
            }

            public static bool TrimIntegration {
                get { return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["TrimIntegration"].Trim().ToBoolean(); }
                }

            }

        public static class Logger
        {
            public static string AppLoggerServiceUrl
            {
                get { return (ConfigurationManager.GetSection(@"logger") as NameValueCollection)["AppLoggerServiceUrl"].Trim(); }
            }
            public static string AppLoggerId
            {
                get { return (ConfigurationManager.GetSection(@"logger") as NameValueCollection)["AppLoggerID"].Trim(); }
            }
        }

        public static class Webservices
        {
            public static string AustPostSuburbApiUrl
            {
                get { return (ConfigurationManager.GetSection(@"webservices") as NameValueCollection)["AustPostSuburbApiUrl"].Trim(); }
            }

            public static string AustPostAuthKey
            {
                get { return (ConfigurationManager.GetSection(@"webservices") as NameValueCollection)["AustPostAuthKey"].Trim(); }
            }
        }

    }/*End Class GlobalSettings*/
}/*End namespace*/
