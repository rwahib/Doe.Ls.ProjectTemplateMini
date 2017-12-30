using ServiceStack.DataAnnotations;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{
    public enum OnlineRecordStatus {
        [Description("Not a 'HP RM record'")]
        NoMatchingRecord = -1,
        [Description("Record is approved but attachment is not published to Trim yet")]
        NotPublished =10,
        [Description("Record is approved and  attachment is the same version in Trim")]
        UpToDate =20,
        [Description("Record is approved but the attachment is not the same version as Trim version")]
        OutOfSync =30
    }
}