
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService {
    public interface IFileService
    {
        
        string SaveAssetFileAs(object fileObject, string fileName, bool overwrite);
                
        void DeleteAssetFile(string fileName);
        
        bool AssetExists(string fileName);

        
        IFileResolver FileResolver { get; set; }
    }
}