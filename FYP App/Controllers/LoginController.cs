using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using FYP_App.Models;
using System.Net.Mail;
using System.Net;
using System.Data;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FYP_App.Services;

namespace FYP_App.Controllers
{
    public class LoginController : Controller
    {
        #region Database Objects

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        DbModel1s db1 = new DbModel1s();
        DbModels db2 = new DbModels();
        #endregion

        #region UserDashBoard
        public ActionResult Count()
        {
            connectionstring();
            ViewBag.Countapproveduser = db2.Sign_Up.Count(a => a.Status == "Approved");
            ViewBag.Countuser = db2.Sign_Up.Count();
            ViewBag.Counttotal = db1.Complaint_DB.Count();
            ViewBag.Count = db1.Complaint_DB.Count(a => a.Status == "Solved");
            return View();
        }
        #endregion

        #region Connectionstring
        void connectionstring()
        {
            con.ConnectionString = "Data Source=DESKTOP-BSAK185; Initial Catalog =FYP database; Integrated Security = True";
        }
        #endregion

        #region Login
     
        [HttpGet]
        [OutputCache(Duration = 500)]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Verify(Login login)
        {
            Count();
            Session["Name"] = login.Name;
            connectionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Sign_Up where Email='" + login.Name + "' and Password='" + login.Password + "' and Status='Approved'  ";
            dr = com.ExecuteReader();
            if (dr.Read() == true)
            {

                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand("select Status from Sign_Up where Email='" + login.Name + "'", con);
                SqlCommand cmds = new SqlCommand("select COUNT(*) from Complaint_DB where User_Email= '" + login.Name +"'", con);
                SqlCommand cmdss = new SqlCommand("select COUNT(*) from Complaint_DB where User_Email='" + Session["Name"] + "' AND  Status = 'Solved'", con);

                var result = (string)cmd.ExecuteScalar();
                var result2 = (int)cmds.ExecuteScalar();
                var result3 = (int)cmdss.ExecuteScalar();
                ViewBag.Account_Status = result;
                ViewBag.Total_Complaint_Status = result2;
                ViewBag.Solved_Complaint_Status = result3;
                //con.Close();
                return RedirectToAction("User_Dashborad");
            }

            else if (login.Name == "autoislamabad@gmail.com" && login.Password == "olx")
            {
                return RedirectToAction("Admin", "UserInfo", null);

               // return RedirectToAction("Admin");

              //  return View("Admin");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View("Login");


        }
        #endregion

        #region ForgotPassword
        [HttpGet]
        [OutputCache(Duration = 500)]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]

        public ActionResult ForgotPassword(Login login)
        {
            
            String password;
            String mycon = "Data Source=DESKTOP-BSAK185; Initial Catalog =FYP database; Integrated Security = True";
            String myquery = "select * from Sign_Up where Email='" + login.Name + "' ";
            SqlConnection con = new SqlConnection(mycon);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = myquery;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                password = ds.Tables[0].Rows[0]["Password"].ToString();
                Email_Sender email_Sender = new Email_Sender();
                email_Sender.Password_Email(login.Name , password);
                
                ViewBag.SuccessMessage = "Check Your Email Inbox ✔️";
            }
            else
            {
                
                ViewBag.errors = "Invalid Email ❌";
            }

            return View("ForgotPassword");
        }
        #endregion

        #region Error_Action
        public ActionResult Error()
        {
            return View();
        }
        #endregion

        #region User_Dashborad
        public ActionResult User_Dashborad(Login login)
        {
            connectionstring();
            con.Open();
            SqlCommand cmd = new SqlCommand("select Status from Sign_Up where Email='" + Session["Name"] + "'", con);
            SqlCommand cmds = new SqlCommand("select COUNT(*) from Complaint_DB where User_Email= '" + Session["Name"] + "'", con);
            SqlCommand cmdss = new SqlCommand("select COUNT(*) from Complaint_DB where User_Email='" + Session["Name"] + "' AND  Status = 'Solved'", con);
            var result = (string)cmd.ExecuteScalar();
            var result2 = (int)cmds.ExecuteScalar();
            var result3 = (int)cmdss.ExecuteScalar();
            ViewBag.Account_Status = result;
            ViewBag.Total_Complaint_Status = result2;
            ViewBag.Solved_Complaint_Status = result3;

            return View();
        }
        #endregion

        public ActionResult Logout()

        {
            Session["Name"] = null;

            Session.RemoveAll();
            return   RedirectToAction("Login");
        }

    }
}