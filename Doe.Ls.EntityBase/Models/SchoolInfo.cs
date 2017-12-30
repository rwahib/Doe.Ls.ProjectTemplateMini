using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.EntityBase.Models
{
    public class SchoolInfo
    {
        public int SchoolCode { get; set; }
        public string SchoolShortName { get; set; }
        public string SchoolFullName { get; set; }
        public string SchoolEmail { get; set; }
        public int EducationalServicesNumber { get; set; }
        public int NetworkID { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string GovNonGov { get; set; }
        public string SchoolType { get; set; }
        public string Gender { get; set; }
        public string SchoolFullTitle { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public string EducationalServicesName { get; set; }
        public string SchoolNetworkName { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public bool? IsPriorityFunded { get; set; }
        public int? DataEntryCode { get; set; }
    }
}
