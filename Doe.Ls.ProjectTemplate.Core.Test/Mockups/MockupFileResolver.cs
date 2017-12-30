using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.Settings;
using System;
using System.IO;

namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    public class MockupFileResolver : IFileResolver
    {
        public string GetAssetAbsoluteLink(int assetId)
        {
           return $"{ProjectTemplateSettings.Site.AppUrl}Asset/File/{assetId}";
        }
     
 public string GetSourceAssetPath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\TestFiles", fileName));

           
        }

        public string GetAssetFullFilePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\OutputFiles", fileName));

        }

        public string GetAssetServerLink(int assetId)
        {
            return $"~/Asset/File/{assetId}";
        }

        public string GetThumbnailAbsoluteLink(int assetId)
        {
            return $"{ProjectTemplateSettings.Site.AppUrl}/Asset/Thumbnail/{assetId}";
        }

        public string GetThumbnailFullFilePath(string fileName)
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\TestFiles\content\", fileName));
            
        }

        public string GetThumbnailServerLink(int assetId)
        {
            return $"~/Asset/Thumbnail/{assetId}";

        }

        public string GetAppDataFilePath(string fileName)
        {
            return $@"..\App_Data\{fileName}";
        }

        public string GetIconFilePath(string fileName)
        {
            return $@"..\Images\icons\{fileName}";
        }

        public string GetPdfTemplateFolderPath()
        {
            return Path.GetFullPath(@"..\App_Data\Template\");
        }

        public string GetPdfOutputFolderPath()
        {
            return Path.Combine(Path.GetTempPath(),@"PdfOutputs\");
        }
    }
}
