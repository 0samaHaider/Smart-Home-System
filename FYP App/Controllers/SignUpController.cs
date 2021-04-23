using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_App.Models;
using FYP_App.Controllers;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace FYP_App.Controllers
{
    public class SignUpController : Controller
    {

        #region Database_Objects
        readonly SqlConnection con = new SqlConnection();
        readonly SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        #endregion

        #region Connectionstring
        void connectionstring()
        {
            con.ConnectionString = "Data Source=DESKTOP-BSAK185; Initial Catalog =FYP database; Integrated Security = True";
        }
        #endregion

        #region Create_Account
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sign_Up sign_Up)
        {
            if (ModelState.IsValid)
            {
                connectionstring();
                using (DbModels dbModel = new DbModels())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "Select * from  Sign_Up where Email='" + sign_Up.Email + "'";
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        ViewBag.alreadyregistermail = "Ohh ! \r You Can't create Your Account with this Email ❌ . \r Try Another One !!";
                    }
                    else
                    {

                        dbModel.Sign_Up.Add(sign_Up);
                        dbModel.SaveChanges();
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("autoislamabad@gmail.com");
                        msg.To.Add(sign_Up.Email);
                        msg.Subject = "Account Created";
                        msg.Body = "Dear User,Your Account Created Successfully, Wait for Admin approval  !!";
                        //msg.Priority = MailPriority.High; 
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
                        ViewBag.SuccessMessage = "Account Successfully ✔️ Wait for approval  !!";
                        ModelState.Clear();
                    }
                }
            }
            else
            {
                return View();
            }
            return View();
        }
        #endregion
    }
}

