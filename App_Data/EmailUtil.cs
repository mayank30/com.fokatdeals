using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
/// <summary>
/// Summary description for SMSEmail
/// </summary>
public class EmailUtil
{
    public void SendEmail(string[] to, string[] cc, string[] bcc, string subject, string body, string postedFile)
    {
        string from = "fokatd@gmail.com";
        using (MailMessage mailMessage = new MailMessage())
        {
            MailAddress from1 = new MailAddress(from, "Fokatdeals.com");
            mailMessage.Sender = from1;
            mailMessage.From = from1;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            if ((to != null) && (to.Length > 0))
            {
                foreach (string item in to)
                {
                    mailMessage.To.Add(item);
                }
            }
            if ((cc != null) && (cc.Length > 0))
            {
                foreach (string item in cc)
                {
                    mailMessage.CC.Add(item);
                }
            }
            if ((bcc != null) && (bcc.Length > 0))
            {
                foreach (string item in bcc)
                {
                    mailMessage.Bcc.Add(item);
                }
            }
            if (postedFile != "")
            {
                Attachment attach = new Attachment(postedFile);
                mailMessage.Attachments.Add(attach);
            }
            System.Net.NetworkCredential networkCredentials = new
            System.Net.NetworkCredential(from, "demo@123");
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredentials;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.Send(mailMessage);
        }
    }
}