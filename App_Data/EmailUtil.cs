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
    private static  String SUBJECT { get; set; }
    private static  String MESSAGE { get; set; }
    private static String[] CC = {"fokatd@gmail.com","mayankjhawar18@gmail.com"};
    private static String support = "fokatdealscomsupport@fokat.freshdesk.com";

    public static  String[] BCC
    {
        get { return CC; }
    }

    public static void CreateSupportTicket(string reply, string subject, string body)
    {
        String[] to = { support };
        Thread email = new Thread(delegate()
        {
            SendEmail(to, null, BCC, subject, body, "", reply);
        });
        email.IsBackground = true;
        email.Start();
    }

    public static void SendEmail(string[] to, string[] cc, string[] bcc, string subject, string body, string postedFile,string reply)
    {
        string from = "fokatd@gmail.com";
        using (MailMessage mailMessage = new MailMessage())
        {
            MailAddress from1 = new MailAddress(from, "Fokatdeals.com");
            mailMessage.Sender = from1;
            mailMessage.From = from1;
            mailMessage.Subject = subject;
            if (reply != null)
            {
                mailMessage.Headers.Add("reply-to", reply);
                mailMessage.ReplyToList.Add(new MailAddress(reply, reply));
            }
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

    public static void RegisterEmail(String userEmail,String username, String password)
    {
        String[] to = { userEmail };
        SUBJECT = "New User Registration";
        MESSAGE = "Welcome "+username+","+" Your password :- "+password;
        Thread email = new Thread(delegate()
        {
            SendEmail(to, null, BCC, SUBJECT, MESSAGE, "", null);
        });
        email.IsBackground = true;
        email.Start();
        
    }

    public static void ForgotPasswordEmail(String userEmail, String phone, String password)
    {
        String[] to = { userEmail };
        SUBJECT = "Forgot Password";
        MESSAGE = "Welcome " + userEmail + "," + " Your New Password :- " + password;
        Thread email = new Thread(delegate()
        {
            SendEmail(to, null, BCC, SUBJECT, MESSAGE, "", null);
        });
        email.IsBackground = true;
        email.Start();

    }

}
