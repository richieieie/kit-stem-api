using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KST.Api.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmail(string toMail, string subject, string body);
    }
}