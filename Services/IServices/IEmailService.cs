using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmail(string toMail, string subject, string body);
    }
}