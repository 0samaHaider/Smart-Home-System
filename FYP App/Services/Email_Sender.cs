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

        #region Complaint _Status_Updated
        public void Complaint_Email(string ToEmail,int id, string status)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("autoislamabad@gmail.com");
            msg.To.Add(ToEmail);
            msg.Subject = "Complaint Update";
            msg.Body = "Dear User,Your Complaint ID = " + id + " has been seen by admin.Your complaint status is updated to " + status + ".";


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

        #region Sign_Up_Email
        public void SignUp_Email(string ToEmail)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("autoislamabad@gmail.com");
            msg.To.Add(ToEmail);
            msg.Subject = "Account Created";
            msg.Body = "Dear User,Your Account Created Successfully, Wait for Admin approval  !!";


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

        #region User_Profile_Update_By_Admin
        public void Profile_Update(string ToEmail)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("autoislamabad@gmail.com");
            msg.To.Add(ToEmail);
            msg.Subject = "Account Approved";
            msg.Body = "Dear User,Your Account Approved Successfully,You Can Login Now !!";


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