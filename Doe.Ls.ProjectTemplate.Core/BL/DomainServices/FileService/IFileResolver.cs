using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService
{
    public interface IFileResolver
    {
        string GetAssetFullFilePath(string fileName);
        string GetThumbnailFullFilePath(string fileName);
        string GetAssetAbsoluteLink(int assetId);
        string GetAssetServerLink(int assetId);
        string GetThumbnailServerLink(int assetId);
        string GetThumbnailAbsoluteLink(int assetId);
        string GetAppDataFilePath(string fileName);
        string GetIconFilePath(string fileName);
        string GetPdfTemplateFolderPath();
        string GetPdfOutputFolderPath();

    }


   

   
}
