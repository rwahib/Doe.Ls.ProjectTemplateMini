using System.Collections.Generic;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService
{
    public interface IPdfService
    {
       void GeneratePdfFromHtml(string templateText, string cssFile, string outputFilenameFullPath);
        PdfResult GeneratePdf(Position position);
        }    
}
