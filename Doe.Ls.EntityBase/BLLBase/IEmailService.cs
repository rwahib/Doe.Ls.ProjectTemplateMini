using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.BLLBase
{
    public interface IEmailService : IDomainService
    {
        Result SendEmail(EmailMessage msg);
    }
}