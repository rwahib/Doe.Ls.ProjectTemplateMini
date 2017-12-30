using System;
using System.IO;
using System.Web;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService {
    class HttpFileService : FileServiceBase {
        public override string SaveAssetFileAs(object fileObject, string fileName, bool overwrite)
        {
            if (fileObject == null) throw new ArgumentNullException($"{nameof(fileObject)} is null for file name {fileName}");

            var fi = fileObject as HttpPostedFileBase;
            if (fi == null) throw new ArgumentNullException($" invalid file object for file name {fileName}");
            

            fileName = fileName.CleanFilename();
            var absoluteLocation = GetAssetFullPath(fileName);
            if (overwrite)
            {

                fi.SaveAs(absoluteLocation);
                return absoluteLocation;
            }

            var counter = 0;
            while (File.Exists(absoluteLocation))
            {
                ++counter;
                fileName = Path.GetFileNameWithoutExtension(absoluteLocation) + "_" + counter + Path.GetExtension(absoluteLocation);
                absoluteLocation = GetAssetFullPath(fileName);
                if (counter > 20) break;
            }

            fi.SaveAs(absoluteLocation);
            return absoluteLocation;

        }
        
    }
}