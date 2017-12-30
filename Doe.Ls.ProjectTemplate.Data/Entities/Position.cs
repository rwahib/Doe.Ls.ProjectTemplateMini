


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Data
{
    [MetadataType(typeof(PositionMetadata))]
    public partial class Position
    {
        public override string ToString()
        {
            var status = StatusValue!=null?StatusValue.StatusName :StatusId.ToString();
            return $"{PositionId}-{PositionNumber}-{this.PositionTitle}-{Unit}-{status}";
        }
        public Position Clone()
            {
            var cloned=new Position();
            this.To(cloned);
            return cloned;
            }

        public int[] GetPositionsFromPath()
        {
            char[] separatingChars = { '/' };
            var arr = this.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<int>();
            foreach (var item in arr)
            {
                int testInt;
                if (int.TryParse(item, out testInt))
                {
                    result.Add(testInt);
                }
                else
                {
                    throw new InvalidCastException($"{item} is not a valid position id");
                }
            }
            return result.ToArray();
        }
    }

    public class PositionMetadata
    {
        
        [Display(Name ="Position id" )]
        public System.Int32 PositionId {get;set;}
        
        [Display(Name ="Report to position" )]
        [Required(ErrorMessage = "Report to position  is required")]
        public System.Int32 ReportToPositionId {get;set;}
        
        [Display(Name ="Position number" )]
        [Required(ErrorMessage = "Position number is required")]
        [MaxLength(10, ErrorMessage = "Exceeding the max length, allowed only 10 character")]
        public System.String PositionNumber {get;set;}
        
        [Display(Name ="Role position description id" )]
        public System.Int32 RolePositionDescriptionId {get;set;}
        
        [Display(Name ="Team" )]
        [Required(ErrorMessage = "Unit id is required")]
        public System.Int32 UnitId {get;set;}
        
        [Display(Name ="Position title" )]
        [Required(ErrorMessage = "Position title is required")]
        [MaxLength(250, ErrorMessage = "Exceeding the max length, allowed only 100 character")]
        public System.String PositionTitle {get;set;}
        
        [Display(Name ="Description" )]
        [DataType(DataType.MultilineText)]
        [MaxLength(1073741823, ErrorMessage = "Exceeding the max length, allowed only 1073741823 character")]
        public System.String Description {get;set;}
        
        [Display(Name ="Position level" )]
        [Required(ErrorMessage = "Position level id is required")]
        public System.Int32 PositionLevelId {get;set;}
        
        [Display(Name ="Status" )]
        [Required(ErrorMessage = "Status id is required")]
        public System.Int32 StatusId {get;set;}
        
        [Display(Name ="Position path" )]
        [MaxLength(500, ErrorMessage = "Exceeding the max length, allowed only 500 character")]
        public System.String PositionPath {get;set;}
        
        [Display(Name ="Location" )]
        [Required(ErrorMessage = "Location id is required")]
        public System.Int32 LocationId {get;set;}

      /*  [Display(Name = "Unit chief position")]
        // [Required(ErrorMessage = "Unit chief position id is required")]
        public System.Int32 UnitChiefPositionId { get; set; }*/

        [Display(Name ="Created date" )]
        [Required(ErrorMessage = "Created date is required")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate {get;set;}
        
        [Display(Name ="Last modified date" )]
        [Required(ErrorMessage = "Last modified date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime LastModifiedDate {get;set;}
        
        [Display(Name ="Created by" )]
        [Required(ErrorMessage = "Created by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String CreatedBy {get;set;}
        
        [Display(Name ="Last modified by" )]
        [Required(ErrorMessage = "Last modified by is required")]
        [MaxLength(120, ErrorMessage = "Exceeding the max length, allowed only 120 character")]
        public System.String LastModifiedBy {get;set;}
  



        
        [Display(Name ="Cost centre detail" )]
        public object CostCentreDetail {get;set;}
        
        [Display(Name ="Employee positions" )]
        public object EmployeePositions {get;set;}
        
        [Display(Name ="Location" )]
        public object Location {get;set;}
        
        [Display(Name = "Positions")]
        public object Positions { get;set;}
        
        [Display(Name ="Report to position" )]
        public object ParentPosition { get;set;}
        
        [Display(Name ="Position level" )]
        public object PositionLevel {get;set;}
        
        [Display(Name ="Status value" )]
        public object StatusValue {get;set;}

      

        [Display(Name ="Team" )]
        public object Unit {get;set;}
        
        [Display(Name ="Position information" )]
        public object PositionInformation {get;set;}

        [Display(Name = "Role Position description")]
        public object RolePositionDescription { get; set; }

        [Display(Name = "Position histories")]
        public object PositionHistories { get; set; }


        }
    }

