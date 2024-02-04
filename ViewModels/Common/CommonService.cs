using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace ViewModels.Common
{
    public class CommonService
    {
        public static int Generate6NumberRandomString()
        {
            string randomString = string.Empty;
            Random random = new Random();
            randomString = random.Next(0, 9).ToString() + random.Next(0, 9).ToString() + random.Next(0, 9).ToString() + random.Next(0, 9).ToString() + random.Next(0, 9).ToString() + random.Next(0, 9).ToString();
            return Convert.ToInt32(randomString);
        }
        public static void SendEmail(string mailFrom, string mailTo, string strSubject, string strBody, bool isHTML, string mailCC = null, string mailBCC = null, string replyTo = null, byte[] attachmentFile = null, string attachmentName = null)
        {
            //do not add try-catch
            try
            {
                string smtpUserName = "renishribadiya10@outlook.com";
                string smtpPassword = "Renish@321";

                // Instantiate a new instance of MailMessage
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                if (!string.IsNullOrEmpty(mailFrom))
                {
                    // Set the sender address of the mail message
                    mailMessage.From = new MailAddress(mailFrom, mailFrom, System.Text.Encoding.UTF8);
                }

                foreach (string semail in mailTo.Split(','))
                    if (semail.Trim() != string.Empty)
                        mailMessage.To.Add(semail);

                // Set the subject of the mail message
                mailMessage.Subject = strSubject;

                // Set the body of the mail message
                mailMessage.Body = strBody;

                // Set the format of the mail message body as HTML
                mailMessage.IsBodyHtml = isHTML;

                if (mailCC != string.Empty && mailCC != null)
                    mailMessage.CC.Add(mailCC);

                if (mailBCC != string.Empty && mailBCC != null)
                    mailMessage.Bcc.Add(mailBCC);

                if (replyTo != string.Empty && replyTo != null)
                    mailMessage.ReplyToList.Add(replyTo);

                // Set the priority of the mail message to normal
                mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                // Instantiate a new instance of SmtpClient
                SmtpClient smtpClient = new SmtpClient();

                // set host name
                smtpClient.Host = "smtp-mail.outlook.com";
                smtpClient.UseDefaultCredentials = false;
                
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);

                int smtpPort = 587;
                smtpClient.Port = smtpPort;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (attachmentFile != null)
                {
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachmentFile), attachmentName));
                }

                // Send the mail message
                smtpClient.Send(mailMessage);
                mailMessage.Attachments.Dispose();
            }
            catch (Exception e)
            {
                var a = e;
            }
        }
    }
}
