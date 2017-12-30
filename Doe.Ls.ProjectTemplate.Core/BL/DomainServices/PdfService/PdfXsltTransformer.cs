using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService
    {
    /// <summary>
    /// This service is working only fopr position service
    /// </summary>
    public class PdfXsltTransformer: PdfService
        {
        public override PdfResult GeneratePdf(Position position)
        {
            if(position.RolePositionDescription.IsPositionDescription)
                {
                return GeneratePdfForPositionDescription(position);
                }
            else
                {

                return base.GeneratePdf(position);
                }

            
        }
        #region private
        private PdfResult GeneratePdfForPositionDescription(Position position)
            {
            var html = GenerateHtml(position);

            var result = new PdfResult();
            var outputPath = Resolver.GetPdfOutputFolderPath();
            var inputFilePath = Resolver.GetPdfTemplateFolderPath();

            if(!Directory.Exists(outputPath))
                {
                Directory.CreateDirectory(outputPath);
                }

            if(!Directory.Exists(inputFilePath))
                {
                throw new DirectoryNotFoundException(
                    "The folders for PDF templates didn't exist.");
                }

            var cssFile = inputFilePath + "PDFGenerator.css";

            var cssText = ReadFileToText(cssFile);
            result.OutputFileName = GetPdfFileName(position.PositionNumber, position.PositionTitle,
                  position.RolePositionDescription.GradeCode);
            result.OutputFileFullPath = outputPath + result.OutputFileName;

            GeneratePdfFromHtml(html, cssText, result.OutputFileFullPath);
            return result;

            }
        private string GenerateHtml(Position position)
            {
            var positionService = this.ServiceRepository.PositionRepository();
            var positionXml = positionService.XmlSerialize(position);
            var inputTemplatePath = Resolver.GetPdfTemplateFolderPath();

            var xsltFilePath = GetXsltTemplateName(position);

            if(!Directory.Exists(inputTemplatePath))
                {
                throw new DirectoryNotFoundException(
                    "The folders for PDF templates didn't exist.");
                }
            var xsltFullPath = Path.Combine(inputTemplatePath, xsltFilePath);
            if(!File.Exists(xsltFullPath))
                {
                throw new FileNotFoundException("XSLT file is not exists");

                }

            var result = TransformXmltoHtml(positionXml.ToString(), File.ReadAllText(xsltFullPath));

            return result;
            }
        private static string TransformXmltoHtml(string inputXml, string xsltString)
            {
            var transform = new XslCompiledTransform();
            using(var reader = XmlReader.Create(new StringReader(xsltString)))
                {
                transform.Load(reader);
                }
            var results = new StringWriter();
            using(var reader = XmlReader.Create(new StringReader(inputXml)))
                {
                transform.Transform(reader, null, results);
                }
            return results.ToString();
            }
        private string GetXsltTemplateName(Position position)
            {
            return position.RolePositionDescription.IsPositionDescription ? "PPD_Template.xslt" : "PRD_Template.xslt";

            }
        #endregion
        }
    }
