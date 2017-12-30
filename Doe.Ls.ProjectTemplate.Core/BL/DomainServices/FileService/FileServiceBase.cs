using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService
{
    public abstract class FileServiceBase : IFileService
    {
        private string _assetFolderPath;

        [Unity.Attributes.Dependency]
        public IFileResolver FileResolver { get; set; }

        public abstract string SaveAssetFileAs(object fileObject, string fileName, bool overwrite);

        public void DeleteAssetFile(string fileName)
        {
            File.Delete(GetAssetFullPath(fileName));            
        }

        public bool AssetExists(string fileName)
        {
        return File.Exists(GetAssetFullPath(fileName));
            
        }

        protected string GetAssetFullPath(string fileName)
        {
            return FileResolver.GetAssetFullFilePath(fileName);
        }

       

      
        
    }
}