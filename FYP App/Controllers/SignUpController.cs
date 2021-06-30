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
using FYP_App.Services;

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
        [ValidateAntiForgeryToken]
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
                       // Email_Sender email_Sender = new Email_Sender();
                       // email_Sender.SignUp_Email(sign_Up.Email);
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

