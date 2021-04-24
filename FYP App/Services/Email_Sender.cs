using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace FYP_App.Services
{
    public class Email_Sender
    {
        #region Forget_Password_Email
        public void Password_Email(string ToEmail,String Message )
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("autoislamabad@gmail.com");
            msg.To.Add(ToEmail);
            msg.Subject = "Your Password";
            msg.Body = "Dear User, Your Password is " + Message;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("autoislamabad@gmail.com",
               "iot3base4");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
        }
        #endregion
    }
}