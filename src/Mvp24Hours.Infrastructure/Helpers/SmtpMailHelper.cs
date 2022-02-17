//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace Mvp24Hours.Helpers
{
    /// <summary>
    /// Contains functions for sending email
    /// </summary>
    public static class SmtpMailHelper
    {
        private static readonly ILoggingService _logger;

        static SmtpMailHelper() => _logger = LoggingService.GetLoggingService();

        /// <summary>
        /// Sends email from the email message
        /// </summary>
        public static void Send(this Core.ValueObjects.Infrastructure.MailMessage dto)
        {
            try
            {
                var _mailFrom = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:MailFrom");
                var _mailFromName = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:MailFromName");
                var _rplyTo = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:ReplyTo");
                var _rplyToName = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:ReplyToName");

                using System.Net.Mail.MailMessage m = new();
                using SmtpClient smtpCli = SmtpLoad();
                m.IsBodyHtml = true;
                m.Body = dto.Body;
                m.Subject = dto.Subject;
                if (string.IsNullOrEmpty(dto.From))
                {
                    m.From = new MailAddress(_mailFrom, (string.IsNullOrEmpty(dto.FromName) ? _mailFromName : dto.FromName));
                    if (!string.IsNullOrEmpty(_rplyTo))
                    {
                        m.ReplyToList.Add(new MailAddress(_rplyTo, (string.IsNullOrEmpty(_rplyToName) ? _rplyTo : _rplyToName)));
                    }
                    else if (!string.IsNullOrEmpty(dto.ReplyTo))
                    {
                        m.ReplyToList.Add(new MailAddress(dto.ReplyTo));
                    }
                    else
                    {
                        m.ReplyToList.Add(new MailAddress(_mailFrom));
                    }
                }
                else
                {
                    m.From = new MailAddress(dto.From);
                    m.ReplyToList.Add(new MailAddress(dto.From));
                }
                if (dto.CopyToBackground != null)
                {
                    m.Bcc.Add(string.Join(",", dto.CopyToBackground));
                }

                if (dto.Attachments != null)
                {
                    dto.Attachments.ForEach(p => m.Attachments.Add(p));
                }

                foreach (string email in dto.To)
                {
                    m.To.Add(new MailAddress(email));
                }

                if (dto.CopyTo != null)
                {
                    m.CC.Add(string.Join(",", dto.CopyTo));
                }

                smtpCli.Send(m);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private static SmtpClient SmtpLoad()
        {
            var _mailServer = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:SmtpServer");
            var _mailUser = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:SmtpUser");
            var _mailPassword = ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:SmtpPassword");

            int.TryParse(ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:SmtpPort"), out int _mailPort);
            if (_mailPort <= 0)
            {
                _mailPort = 25;
            }

            bool.TryParse(ConfigurationHelper.GetSettings("Mvp24Hours:SmtpMail:SmtpSSL"), out bool _mailSSL);

            SmtpClient smtpCli = new();

            smtpCli.Port = _mailPort;
            if (string.IsNullOrEmpty(_mailServer))
            {
                throw new ArgumentException("SmtpMail:SmtpServer has not been defined in the config.");
            }
            else
            {
                smtpCli.Host = _mailServer;
            }

            smtpCli.EnableSsl = _mailSSL;
            if (string.IsNullOrEmpty(_mailUser) || string.IsNullOrEmpty(_mailPassword))
            {
                throw new ArgumentException("SmtpMail:SmtpUser or SmtpMail:SmtpPassword not defined in the config.");
            }
            else
            {
                smtpCli.Credentials = new NetworkCredential(_mailUser, _mailPassword);
            }

            return smtpCli;
        }
    }
}
