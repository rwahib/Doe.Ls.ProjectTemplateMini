using System;
using System.IO;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService
{
    public class FileSystemService : FileServiceBase
    {
       public override string SaveAssetFileAs(object fileObject, string fileName, bool overwrite)
        {
            if (fileObject == null) throw new ArgumentNullException(nameof(fileObject) + " is null");

            var fi = fileObject as FileInfo;
            if (fi == null) throw new InvalidOperationException($"Invalid File object for file name {fileName}");

            fileName = fileName.CleanFilename();
            var absoluteLocation = GetAssetFullPath(fileName);
            if (overwrite)
            {
               
                fi.CopyTo(absoluteLocation, true);
                return fileName;
            }
            
            var counter = 0;
            while (File.Exists(absoluteLocation))
            {
                ++counter;
                fileName = Path.GetFileNameWithoutExtension(absoluteLocation) + "_" + counter + Path.GetExtension(absoluteLocation);
                absoluteLocation = GetAssetFullPath(fileName);
                if (counter > 20) break;
            }

            fi.CopyTo(absoluteLocation);
            return fileName;

        }
        
    }
}
