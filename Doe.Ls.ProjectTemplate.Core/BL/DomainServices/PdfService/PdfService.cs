using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;
using it = iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService
{
    public class PdfService : IPdfService
    {
      public virtual void GeneratePdfFromHtml(string templateText, string cssText, string outputFilenameFullPath)
        {
            Byte[] bytes;
            //clear the old file
            if (File.Exists(outputFilenameFullPath))
            {
                File.Delete(outputFilenameFullPath);
            }
            
            //escape illegle characters before parse
            templateText = EscapeCharsForParser(templateText);

            using (var ms = new MemoryStream())
            {
                using (var doc = new it.Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        doc.Open();

                        //Convert the CSS, HTML strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(Encoding.UTF8.GetBytes(cssText)))
                        {

                            using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes(templateText)))
                            {
                                //Parse the HTML
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }

                        doc.Close();
                    }
                }

                //Grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }

            //Save the bytes to a file
            File.WriteAllBytes(outputFilenameFullPath, bytes);

        }

         public virtual PdfResult GeneratePdf(Position position)
        {
            if (position.RolePositionDescription != null)
            {
                return position.RolePositionDescription.IsPositionDescription
                    ? GeneratePdfPositionDescByPosition(position)
                    : GeneratePdfRoleDescByPosition(position);
            }

            var serviceRep = new ServiceRepository(this.RepositoryFactory);
            var rpd =
                serviceRep.RolePositionDescriptionRepository()
                    .GetRolePositionDescById(position.RolePositionDescriptionId);
            return rpd.IsPositionDescription
               ? GeneratePdfPositionDescByPosition(position)
               : GeneratePdfRoleDescByPosition(position);
        }

        #region protected memebers

        protected PdfResult GeneratePdfRoleDescByPosition(Position position)
        {
            var serviceRep = new ServiceRepository(this.RepositoryFactory);
            var result = new PdfResult();
            var outputPath = Resolver.GetPdfOutputFolderPath();
            var inputFilePath = Resolver.GetPdfTemplateFolderPath();

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            if (!Directory.Exists(inputFilePath))
            {
                throw new DirectoryNotFoundException(
                    "The folders for PDF templates didn't exist.");
            }

            var templateFile = inputFilePath + "RD_Template.html";
            var cssFile = inputFilePath + "PDFGenerator.css";

            var htmlText = ReadFileToText(templateFile);
            var cssText = ReadFileToText(cssFile);

            if (position != null)
            {
                var rolePosDesc = serviceRep.RolePositionDescriptionRepository()
                    .ListForRoleDescriptions()
                    .SingleOrDefault(rp => rp.RolePositionDescId == position.RolePositionDescriptionId);

                if (rolePosDesc != null && rolePosDesc.RoleDescription != null)
                    rolePosDesc.RoleDescription.RoleCapabilities =
                        RoleDescriptionExtensions.SortCapabilityGroup(rolePosDesc.RoleDescription).ToList();

                string directorateOverview;
                string directorateDisplay;
                var divisionDisplay = string.Empty;
                var businessUnitDisplay = string.Empty;
                var sb = new StringBuilder();

                if (position.Unit != null && position.Unit.BusinessUnit != null &&
                    position.Unit.BusinessUnit.Directorate != null)
                {
                    directorateDisplay = position.Unit.BusinessUnit.Directorate.DirectorateName;
                    directorateOverview = position.Unit.BusinessUnit.Directorate.DirectorateOverview;
                    divisionDisplay = position.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle;
                    businessUnitDisplay = position.Unit.BusinessUnit.BUnitName;
                }
                else
                {
                    directorateDisplay = string.Empty;
                    directorateOverview = string.Empty;
                }

                var unitDisplay = "";

                if (position != null && position.Unit != null)
                {
                    unitDisplay = position.Unit.UnitName;
                }
                else
                {
                    unitDisplay = "NA";
                }

                var logo = GetImagePath("dec-pdf.png");
                //filling the value in htmlText

                //1. top position table
                htmlText = htmlText.Replace("[Logo]", logo);
                htmlText = htmlText.Replace("[PosTitle]", position.PositionTitle);
                htmlText = htmlText.Replace("[Agency]", rolePosDesc.RoleDescription.Agency);
                htmlText = htmlText.Replace("[Division]", divisionDisplay);

                if (rolePosDesc.GradeCode != CommonHelper.GetPSSE3Code())
                {
                    //if PSSE3, hide Directorate, Business Unit and Team
                    //show direcotorate
                    htmlText = htmlText.Replace("[Directorate]", directorateDisplay);
                }
                else
                {
                    htmlText = htmlText.Replace("/ [Directorate]", "");
                }
                if (rolePosDesc.GradeCode != CommonHelper.GetPSSE3Code() &&
                    rolePosDesc.GradeCode != CommonHelper.GetPSSE2Code())
                {
                    //If PSSE2, hide Business unit and Team
                    //Show Business Unit
                    htmlText = htmlText.Replace("[BusinessUnit]", businessUnitDisplay);

                }
                else
                {
                    htmlText = htmlText.Replace("/ [BusinessUnit]", "");
                }
                
                if (rolePosDesc.Grade.GradeType == CommonHelper.GetNameOfPSSEType())
                {

                    sb.Clear();
                    sb.Append("<tr>");
                    sb.Append("<th>Senior Executive Work Level Standards</th>");
                    sb.Append("<td>" + rolePosDesc.RoleDescription.SeniorExecutiveWorkLevelStandards + "</td>");
                    sb.Append("</tr>");
                    htmlText = htmlText.Replace("[SeniorExecutiveWorkLevelStandards]", sb.ToString());
                 }
                else
                {
                    htmlText = htmlText.Replace("[SeniorExecutiveWorkLevelStandards]", "");
                }

                htmlText = htmlText.Replace("[Location]", position.Location.Name);
                htmlText = htmlText.Replace("[GradeCode]", rolePosDesc.Grade.GradeTitle);
                //htmlText = htmlText.Replace("[EmploymentType]",
                //    position.PositionInformation.PositionType.PositionTypeName);
                htmlText = htmlText.Replace("[PositionNumber]", position.PositionNumber);
                htmlText = htmlText.Replace("[ANZSCO]", rolePosDesc.RoleDescription.ANZSCOCode);
                htmlText = htmlText.Replace("[PCAT]", rolePosDesc.RoleDescription.PCATCode);
                htmlText = htmlText.Replace("[DateApproved]", rolePosDesc.DateOfApproval.ToEasyDateFormat());
                htmlText = htmlText.Replace("[AgencyWebsite]", rolePosDesc.RoleDescription.AgencyWebsite);

                //2. paragrahs
                htmlText = htmlText.Replace("[AgencyOverview]", rolePosDesc.RoleDescription.AgencyOverview);
                htmlText = htmlText.Replace("[Executive]",
                    position.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle);
                htmlText = htmlText.Replace("[DivisionOverview]", position.DivisionOverview);

                if (!string.IsNullOrWhiteSpace(position.PositionInformation.OtherOverview))
                {
                    htmlText = htmlText.Replace("[Additional Overview]", $"{position.PositionInformation.OtherOverview}");
                }
                else
                {
                   htmlText = htmlText.Replace("[Additional Overview]", "");
                }

                if (rolePosDesc.GradeCode != CommonHelper.GetPSSE3Code() &&
                    !string.IsNullOrEmpty(position.Unit.BusinessUnit.Directorate.DirectorateOverview))
                {
                    sb.Clear();
                    htmlText = htmlText.Replace("[DirectorateOverview]",
                        ShowDirectorateOverview(sb, directorateDisplay, directorateOverview));
                }
                else
                {
                    htmlText = htmlText.Replace("[DirectorateOverview]", "");
                }

                htmlText = htmlText.Replace("[PrimaryPurposse]", rolePosDesc.RoleDescription.RolePrimaryPurpose);
                htmlText = htmlText.Replace("[KeyAccountabilities]", rolePosDesc.RoleDescription.KeyAccountabilities);
                htmlText = htmlText.Replace("[KeyChallenges]", rolePosDesc.RoleDescription.KeyChallenges);
                
                //Key relationships
                var hasMinisteria = false;
                var hasInternal = false;
                var hasExternal = false;
                foreach (var key in rolePosDesc.RoleDescription.KeyRelationships)
                {
                    if (key.ScopeId == (int)Enums.ScopeType.Internal)
                    {
                        hasInternal = true;
                    }
                    else if (key.ScopeId == (int)Enums.ScopeType.External)
                    {
                        hasExternal = true;
                    }
                    else if (key.ScopeId == (int)Enums.ScopeType.Ministerial)
                    {
                        hasMinisteria = true;
                    }
                }
                sb.Clear();
                if (hasMinisteria)
                {
                    //[MinisterialTitleBody]
                    DisplayKeyRelationship((int)Enums.ScopeType.Ministerial, rolePosDesc, sb);
                }
                htmlText = htmlText.Replace("[MinisterialTitleBody]", sb.ToString());

                sb.Clear();
                if (hasInternal)
                {
                    DisplayKeyRelationship((int)Enums.ScopeType.Internal, rolePosDesc, sb);
                }
                htmlText = htmlText.Replace("[InternalBody]", sb.ToString());
                
                sb.Clear();
                if (hasExternal)
                {
                    DisplayKeyRelationship((int)Enums.ScopeType.External, rolePosDesc, sb);
                }
                htmlText = htmlText.Replace("[ExternalBody]", sb.ToString());

                
                //Role dimensions - [RoleDimensionsIntro]
                htmlText = htmlText.Replace("[RoleDimensionsIntro]", "");
                //Decision making, [DecisionMaking]
                htmlText = htmlText.Replace("[DecisionMaking]", rolePosDesc.RoleDescription.DecisionMaking);

                //Reportingline, DirectReports
                var displayObj = SetRoleDescForPositionDisplay(position, rolePosDesc.GradeCode);
                htmlText = htmlText.Replace("[ReportingLine]", displayObj.ReportingLineDisplay);
                //Direct reports only show Approved, Imported.
                htmlText = htmlText.Replace("[DirectReports]", displayObj.DirectReportsDisplay);
                if (displayObj.DirectReportsDisplay == Enums.DirectReportDefault.Nil.ToString())
                {
                    htmlText = htmlText.Replace("[DirectReportContent]", "");
                }
                else
                {
                    htmlText = htmlText.Replace("[DirectReportContent]",
                        "The role has the following direct reports: <br/>");
                }

                if (displayObj.ReportingLineDisplay == Enums.DirectReportDefault.Nil.ToString())
                {
                    htmlText = htmlText.Replace("[ReportLineContent]", "");
                }
                else
                {
                    htmlText = htmlText.Replace("[ReportLineContent]", "This role reports to: <br/>");
                }
                sb.Clear();

                //[BudgetExp], [BudgetExpValue]

                if (!string.IsNullOrEmpty(rolePosDesc.RoleDescription.BudgetExpenditureValue))
                {
                    htmlText = htmlText.Replace("[BudgetExp]",
                         "The role has a financial delegation of up to: $");

                    htmlText = htmlText.Replace("[BudgetExpValue]",
                        rolePosDesc.RoleDescription.BudgetExpenditureValue + "<br />"
                        + rolePosDesc.RoleDescription.BudgetExtraNotes);
                }
                else
                {
                    htmlText = htmlText.Replace("[BudgetExp]", "Nil");
                    htmlText = htmlText.Replace("[BudgetExpValue]", "");
                }


                htmlText = htmlText.Replace("[EssentialReqs]", rolePosDesc.RoleDescription.EssentialRequirements);
                htmlText = htmlText.Replace("[CapIntro]", rolePosDesc.RoleDescription.RoleCapabilityItems);
                htmlText = htmlText.Replace("[CapSummary]", rolePosDesc.RoleDescription.CapabilitySummary);
                htmlText = htmlText.Replace("[FocusCapabilities]", rolePosDesc.RoleDescription.FocusCapabilities);

                //Build capability framework table
                var groupedList = rolePosDesc.RoleDescription.RoleCapabilities.GroupBy(
                    rc => rc.CapabilityName.CapabilityGroup.GroupName)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var count = 0;
                var bold = string.Empty;
                foreach (var obj in groupedList)
                {

                    var image = ProjectTemplateSettings.Site.PdfTemplatePath +
                                GetCapabilityGroupImageName(obj.Value);
                    count = 1;
                    foreach (var x in obj.Value.OrderBy(r => r.CapabilityNameId))
                    {
                        bold = "";
                        sb.Append("<tr class=\"body\">");
                        if (count == 1)
                        {
                            sb.Append("<td rowspan=\"" + obj.Value.Count + "\"><img src=\"" + image +
                                      "\" width=\"90\" height=\"90\" alt=\"" + obj.Key + "\" /></td>");

                        }

                        if (x.Highlighted)
                        {
                            bold = " class=\"bold\"";
                        }
                        sb.Append("<td" + bold + ">" + x.CapabilityName + "</td>");
                        sb.Append("<td" + bold + ">" + x.CapabilityLevel.LevelName + "</td>");
                        sb.Append("</tr>");
                        count++;
                    }
                }
                count = 0;
                htmlText = htmlText.Replace("[CapFrameTable]", sb.ToString());

                //Build Framework indicator table
                sb.Clear();

                var prevGroupId = 0;
                foreach (var c in rolePosDesc.RoleDescription.RoleCapabilities.OrderBy(r=>r.CapabilityNameId))
                {
                    if (c.Highlighted)
                    {
                        var currentGroupId = c.CapabilityName.CapabilityGroupId;
                        if (prevGroupId != currentGroupId)
                        {
                            sb.Append("<tr class=\"subheading\">");
                            sb.Append("<td colspan=\"3\"><span>Capability Group: <em>"+ c.CapabilityName.CapabilityGroup.GroupName + "</em></span></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr class=\"subheading\">");
                            sb.Append("<td class=\"col1\">Capability Set</td>");
                            sb.Append("<td class=\"col2\">Level</td>");
                            sb.Append("<td class=\"col3\">Behavioural indicators</td>");
                            sb.Append("</tr>");

                            prevGroupId = currentGroupId;
                        }
                        
                        sb.Append("<tr class=\"body\">");
                        sb.Append("<td>" + c.CapabilityName + "</td>");
                        sb.Append("<td class=\"level\">" + c.CapabilityLevel.LevelName + "</td>");
                        sb.Append("<td class=\"indicator\">" +
                                  c.CapabilityName.CapabilityBehaviourIndicators.FirstOrDefault(
                                      rc =>
                                          rc.CapabilityLevelId == c.CapabilityLevelId &&
                                          rc.CapabilityNameId == c.CapabilityNameId) + "</td>");
                        sb.Append("</tr>");
                    }
                }

                htmlText = htmlText.Replace("[FocusIndicators]", sb.ToString());

                htmlText = htmlText.Replace("[DOCNumber]", rolePosDesc.DocNumber);
                htmlText = htmlText.Replace("[lastUpdated]", rolePosDesc.LastModifiedDate.ToShortDateString());


                result.OutputFileName = GetPdfFileName(position.PositionNumber, position.PositionTitle,
                    rolePosDesc.GradeCode);
                result.OutputFileFullPath = outputPath + result.OutputFileName;

                GeneratePdfFromHtml(htmlText, cssText, result.OutputFileFullPath);

            }

            return result;
        }

        private static void DisplayKeyRelationship(int selectedTypeId, RolePositionDescription rolePosDesc, StringBuilder sb)
        {
            var typeName = Enum.GetName(typeof(Enums.ScopeType), selectedTypeId);
            sb.Append("<tr class=\"row subheading\">");
            sb.Append("<td colspan=\"2\">"+ typeName + "</td>");
            sb.Append("</tr>");

            foreach (var key in rolePosDesc.RoleDescription.KeyRelationships.ToList()
            .Where(k => k.ScopeId == selectedTypeId))
            {
                sb.Append("<tr class=\"body\">");
                sb.Append("<td valign=\"top\">" + key.Who + "</td>");
                sb.Append("<td valign=\"top\">" + key.Why + "</td>");
                sb.Append("</tr>");
            }
        }

        protected PdfResult GeneratePdfPositionDescByPosition(Position position)
        {
            var serviceRep = new ServiceRepository(this.RepositoryFactory);
            var result = new PdfResult();
            var outputPath = Resolver.GetPdfOutputFolderPath();
            var inputFilePath = Resolver.GetPdfTemplateFolderPath();

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            if (!Directory.Exists(inputFilePath))
            {
                throw new DirectoryNotFoundException(
                    "The folders for PDF templates didn't exist.");
            }


            var templateFile = inputFilePath + "PD_Template.html";
            var cssFile = inputFilePath + "PDFGenerator.css";

            string htmlText = ReadFileToText(templateFile);
            string cssText = ReadFileToText(cssFile);

            if (position != null)
            {
                var rolePosDesc = serviceRep.RolePositionDescriptionRepository()
                .ListForPositionDescriptions().SingleOrDefault(rp => rp.RolePositionDescId == position.RolePositionDescriptionId);


                StringBuilder selectionCriteria = new StringBuilder();
                if (rolePosDesc != null && rolePosDesc.PositionDescription != null)
                {
                    var focusCriteria =
                         rolePosDesc.PositionDescription.PositionFocusCriterias
                        .OrderBy(c => c.LookupFocusGradeCriteria.Focus.OrderList);

                    selectionCriteria.Append("<ul>");

                    foreach (var fc in focusCriteria)
                    {
                        selectionCriteria.Append("<li>");
                        if(!string.IsNullOrEmpty(fc.LookupCustomContent))
                        {
                            selectionCriteria.Append(fc.LookupCustomContent);
                        }
                        else
                        {
                            selectionCriteria.Append(fc.LookupFocusGradeCriteria.SelectionCriteria.Criteria);
                        }
                        selectionCriteria.Append("</li>");
                    }
                    
                    selectionCriteria.Append("</ul>");
                }
                var directorateDisplay = string.Empty;
                var divisionDisplay = string.Empty;

                if (position != null && position.Unit != null && position.Unit.BusinessUnit != null &&
                    position.Unit.BusinessUnit.Directorate != null)
                {
                    directorateDisplay = position.Unit.BusinessUnit.Directorate.DirectorateName;
                    divisionDisplay = position.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle;
                }
                else
                {
                    directorateDisplay = "NA";
                    divisionDisplay = "NA";
                }

                var unitDisplay = string.Empty;

                if (position != null && position.Unit != null)
                {
                    unitDisplay = position.Unit.BusinessUnit.BUnitName;
                }
                else
                {
                    unitDisplay = "NA";
                }

                var logo = GetImagePath("dec-pdf.png");
                //filling the value in htmlText
                htmlText = htmlText.Replace("[Logo]", logo);
                htmlText = htmlText.Replace("[Division]", divisionDisplay);
                htmlText = htmlText.Replace("[Directorate]", directorateDisplay);
                htmlText = htmlText.Replace("[BUnit]", unitDisplay);
                htmlText = htmlText.Replace("[PositionNumber]", position.PositionNumber);
                htmlText = htmlText.Replace("[Title]", position.PositionTitle);
                htmlText = htmlText.Replace("[GradeCode]", rolePosDesc.Grade.GradeTitle);
                htmlText = htmlText.Replace("[BriefRoleStatement]",
                    rolePosDesc.PositionDescription.BriefRoleStatement);
                htmlText = htmlText.Replace("[StatementOfDuties]", rolePosDesc.PositionDescription.StatementOfDuties);
                htmlText = htmlText.Replace("[Criteria]", selectionCriteria.ToString());
                htmlText = htmlText.Replace("[DOCNumber]", rolePosDesc.DocNumber);
                htmlText = htmlText.Replace("[lastUpdated]", rolePosDesc.LastModifiedDate.ToShortDateString());


                result.OutputFileName = GetPdfFileName(position.PositionNumber, position.PositionTitle,
                    rolePosDesc.GradeCode);
                result.OutputFileFullPath = outputPath + result.OutputFileName;

                GeneratePdfFromHtml(htmlText, cssText, result.OutputFileFullPath);
            }
            return result;
        }

        protected RolePositionDescriptionLight SetRoleDescForPositionDisplay(Position position, string gradeCode)
        {
            var serviceRep = new ServiceRepository(this.RepositoryFactory);

            //Reporting line is who you report to
            var reportToPosition = serviceRep.PositionRepository().ListForReportTo()
                .SingleOrDefault(p => p.PositionId == position.ReportToPositionId);

            string reportingLine;
            if (position.ReportToPositionId == -1)
            {
                reportingLine = Enums.DirectReportDefault.Nil.ToString();
            }
            else if (reportToPosition != null)
            {
                reportingLine = reportToPosition.PositionNumber + " " + reportToPosition.PositionTitle + " ";
                if (reportToPosition.RolePositionDescription != null)
                {
                    reportingLine = reportingLine + reportToPosition.RolePositionDescription.GradeCode;
                }

            }
            else
            {
                reportingLine = Enums.DirectReportDefault.Nil.ToString();
            }
            //DirectReports is the subodinates whom report to you
            //Only show those status in Approved, Imported
            var subOrdinates = serviceRep.PositionRepository().BaseList().Where(s => s.ReportToPositionId == position.PositionId
                   && (s.StatusId == (int)Enums.StatusValue.Approved || s.StatusId == (int)Enums.StatusValue.Imported));
            //Get direct reports (subordinates) positions



            var sb = new StringBuilder();

            if (subOrdinates.Any())
            {

                //Get title grouped and get count
                var titleGrouped = subOrdinates.GroupBy(n => new { n.PositionTitle, n.RolePositionDescription.GradeCode }).
                    Select(group =>
                        new
                        {
                            PositionTitle = group.Key.PositionTitle,
                            GradeCode = group.Key.GradeCode,
                            Count = group.Count()
                        });

                sb.Append("<ul>");
                foreach (var p in titleGrouped)
                {
                    if (p.Count > 1)
                    {
                        sb.Append("<li>" + p.PositionTitle + " " + p.GradeCode + " (" + p.Count + ")</li>");
                    }
                    else
                    {
                        sb.Append("<li>" + p.PositionTitle + " " + p.GradeCode + "</li>");
                    }
                }
                sb.Append("</ul>");
            }

            var displayObj = new RolePositionDescriptionLight
            {
                IsRoleDescForPosition = true,
                DirectorateOverview = position.Unit.BusinessUnit.Directorate.DirectorateOverview,
                DirectReportsDisplay = string.IsNullOrEmpty(sb.ToString()) ?
                Enums.DirectReportDefault.Nil.ToString() : sb.ToString(),
                ReportingLineDisplay = reportingLine,
                GradeCode = gradeCode
            };
            return displayObj;
        }

       
        protected string ShowDirectorate(StringBuilder sb, string directorate)
        {
            sb.Append("<tr>");
            sb.Append("<th>Directorate</th>");
            sb.Append("<td>" + directorate + "</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        protected string ShowBusinessUnit(StringBuilder sb, string businessUnit)
        {
            sb.Append("<tr>");
            sb.Append("<th>Business Unit</th>");
            sb.Append("<td>" + businessUnit + "</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        protected string ShowTeam(StringBuilder sb, string team)
        {
            sb.Append("<tr>");
            sb.Append("<th>Team</th>");
            sb.Append("<td>" + team + "</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

        protected string ShowDirectorateOverview(StringBuilder sb, string directorate, string overview)
        {
            sb.Append("<div class=\"section\">");
            sb.Append("<h4>" + directorate + " overview</h4>");
            sb.Append(overview);
            sb.Append("</div>");
            return sb.ToString();
        }

        protected string GetPdfFileName(string positionNumber, string positionTitle, string gradeCode)
        {

            return
                $"{positionNumber.CleanFilename()}_{positionTitle.CleanFilename()}_{gradeCode.CleanFilename()}.pdf";
        }

        protected string GetImagePath(string fileName)
        {
            return ProjectTemplateSettings.Site.PdfTemplatePath + fileName;
        }

        //This is mainly to read a template and css to a string.
        protected string ReadFileToText(string fileName)
        {
            string contentText;
            using (var streamReader = new StreamReader(fileName, Encoding.UTF8))
            {
                contentText = streamReader.ReadToEnd();
            }
            return contentText;

        }

        protected string GetCapabilityGroupImageName(List<RoleCapability> capabilityNames)
        {
            if (capabilityNames.Any())
                return capabilityNames.FirstOrDefault().CapabilityName.CapabilityGroup.GroupImage;

            return string.Empty;
        }

        protected static string EscapeCharsForParser(string htmlText)
        {
            htmlText = htmlText.Replace("&nbsp;", "");
            htmlText = htmlText.Replace("&rsquo;", "'");
            htmlText = htmlText.Replace("&quot;", "\"");
            htmlText = htmlText.Replace("&amp;", "&");
            htmlText = htmlText.Replace("&", "&amp;");
            return htmlText;
        }
        #endregion

        #region dependencies 
        [Unity.Attributes.Dependency]
        public IRepositoryFactory RepositoryFactory { get; set; }
        [Unity.Attributes.Dependency]
        public IFileResolver Resolver { get; set; }

        private ServiceRepository _serviceRepository;
        public ServiceRepository ServiceRepository => _serviceRepository ?? (_serviceRepository = new ServiceRepository(RepositoryFactory));

        #endregion


        }
}

