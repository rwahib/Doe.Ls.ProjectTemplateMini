using System.ComponentModel;

namespace Doe.Ls.EntityBase.Logging
{
    public enum LogCategory
    {
        [Description("General")]
        General = 4,
        [Description("Database")]
        Database = 0,
        [Description("Data access layer")]
        DataAccessLayer = 1,
        [Description("Business layer")]
        BusinessLayer = 2,
        [Description("User interface")]
        UI = 3,
        [Description("Client script")]
        ClientScript = 5
    }
}
