using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Doe.Ls.EntityBase.Models
{
    
    public class EmailMessage
    {
        public int MessageId { get; set; }
        [Required(ErrorMessage = "From should not be empty")]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }
        [Required(ErrorMessage = "To should not be empty")]
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Cc { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Bcc { get; set; }
       
        public Attachment Attachment { get; set; }
        public List <Attachment> Attachments { get; set; }
        [Required(ErrorMessage = "Subject should not be empty")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Message should not be empty")]
        public string Message { get; set; }

    }
}
