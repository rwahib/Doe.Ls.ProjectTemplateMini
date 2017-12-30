namespace Doe.Ls.EntityBase.MsgQueue
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public object MessageBody { get; set; }

    }
}