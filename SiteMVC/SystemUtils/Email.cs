using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SystemUtils
{
    public class Email
    {
        public Email()
        {
            smtp.Host = "mail.nedvijimost-ua.com";
            smtp.Port = 25;
        }

        private MailMessage Mail = null;
        private SmtpClient smtp = new SmtpClient();
        private string smtpLogin = "support@nedvijimost-ua.com";
        private string smtpPassword = "gtycbz1990";
        private string emailFrom = "support@nedvijimost-ua.com";

        public void SendMail(string emailTo, string subject, string message)
        {
            Mail = new MailMessage(emailFrom, emailTo);
            Mail.Subject = subject;
            Mail.Body = message;
            Mail.IsBodyHtml = true;

            smtp.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
            try
            {
                smtp.Send(Mail);
            }
            catch
            {
                throw new Exception("Cannot send email.");
            }
        }
    }
}