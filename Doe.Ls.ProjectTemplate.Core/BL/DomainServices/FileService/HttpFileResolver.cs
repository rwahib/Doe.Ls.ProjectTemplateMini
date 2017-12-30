using Doe.Ls.ProjectTemplate.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService
{

    public class HttpFileResolver : IFileResolver
    {
        public string GetAssetAbsoluteLink(int assetId)
        {
            return $"{ProjectTemplateSettings.Site.AppUrl}File/{assetId}";

        }


        public string GetAssetFullFilePath(string fileName)
        {
            return Path.Combine(ProjectTemplateSettings.Site.AssetFolderPath,fileName);


        }

        public string GetAssetServerLink(int assetId)
        {
            return $"~/Asset/File/{assetId}";

        }

        public string GetAppDataFilePath(string fileName)
        {
            return HttpContext.Current.Server.MapPath($@"~\App_Data\{fileName}");
        }

        public string GetIconFilePath(string fileName)
        {
            return HttpContext.Current.Server.MapPath($@"~\Images\icons\{fileName}");
        }

        public string GetPdfTemplateFolderPath()
        {
            return ProjectTemplateSettings.Site.PdfTemplatePath;
        }

        public string GetPdfOutputFolderPath()
        {
            return ProjectTemplateSettings.Site.PdfOutputPath;
        }

        public string GetThumbnailAbsoluteLink(int assetId)
        {
            return $"{ProjectTemplateSettings.Site.AppUrl}/Thumbnail/{assetId}";

        }

        public string GetThumbnailFullFilePath(string fileName)
        {
            return HttpContext.Current.Server.MapPath("~/Images/icons/");


        }

        public string GetThumbnailServerLink(int assetId)
        {
            return $"~/Thumbnail/{assetId}";

        }
    }
}
