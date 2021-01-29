//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System.Collections.Generic;
using System.Net.Mail;

namespace Mvp24Hours.Core.ValueObjects.Infrastructure
{
    public class MailMessage : BaseVO
    {
        public List<string> To { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReplyTo { get; set; }
        public List<string> CopyTo { get; set; }
        public List<string> CopyToBackground { get; set; }
        public List<Attachment> Attachments { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return To;
            yield return From;
            yield return Subject;
            yield return ReplyTo;
            yield return CopyTo;
            yield return CopyToBackground;
        }
    }
}
