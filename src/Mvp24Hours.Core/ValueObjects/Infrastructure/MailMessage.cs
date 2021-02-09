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
    /// <summary>
    /// Represents email message
    /// </summary>
    public class MailMessage : BaseVO
    {
        #region [ Ctor ]
        public MailMessage(string from, string fromName, string subject, string body, string replyTo = null)
        {
            From = from;
            FromName = fromName;
            Subject = subject;
            Body = body;
            ReplyTo = replyTo;
        }
        #endregion

        #region [ Fields ]
        IList<string> to;
        IList<string> copyTo;
        IList<string> copyToBackground;
        IList<Attachment> attachments;
        #endregion

        #region [ Properties ]
        /// <summary>
        /// Gets the collection of addresses that contains the recipients of this email message.
        /// </summary>
        public IList<string> To
        {
            get
            {
                return to ??= new List<string>();
            }
        }
        /// <summary>
        /// Gets or sets the address for this email message.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Gets or sets the name of the sender of this e-mail message.
        /// </summary>
        public string FromName { get; set; }
        /// <summary>
        /// Gets or sets the subject line for this email message.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the ReplyTo address for the email message.
        /// </summary>
        public string ReplyTo { get; set; }
        /// <summary>
        /// Gets the collection of addresses that contains the CC (carbon copy) recipients of this email message.
        /// </summary>
        public IList<string> CopyTo
        {
            get
            {
                return copyTo ??= new List<string>();
            }
        }
        /// <summary>
        /// Gets the collection of addresses that contains the CCO recipients (with blind copy) of this email message.
        /// </summary>
        public IList<string> CopyToBackground
        {
            get
            {
                return copyToBackground ??= new List<string>();
            }
        }
        /// <summary>
        /// Gets the collection of attachments used to store data attached to this email message.
        /// </summary>
        public IList<Attachment> Attachments
        {
            get
            {
                return attachments ??= new List<Attachment>();
            }
        }
        #endregion

        #region [ Overrides ]
        /// <summary>
        /// <see cref="Mvp24Hours.Core.ValueObjects.BaseVO.GetEqualityComponents"/>
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return To;
            yield return From;
            yield return Subject;
            yield return ReplyTo;
            yield return CopyTo;
            yield return CopyToBackground;
        }
        #endregion
    }
}
