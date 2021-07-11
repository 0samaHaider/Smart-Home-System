using System;
using System.Linq;
using System.Web.Mvc;
using FYP_App.Models;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Web.UI.WebControls;
using PagedList;
using System.Net.Mail;
using System.Net;
using FYP_App.Services;
using System.Data;
using System.Data.Entity.Infrastructure;
using ClosedXML.Excel;
using System.IO;

namespace FYP_App.Controllers
{
    public class ComplaintController : Controller
    {
        #region Database_Objcts
        readonly SqlConnection con = new SqlConnection();
        readonly SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        #endregion

        #region Connectionstring
        void Connectionstring()
        {
            con.ConnectionString = "Data Source=DESKTOP-BSAK185; Initial Catalog =FYP database; Integrated Security = True";
        }
        #endregion

        #region Complaint_Edit_By_Admin
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (DbModel1s db = new DbModel1s())
            {
                return View(db.Complaint_DB.Where(x => x.Complaint_ID == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Edit(Complaint_DB complaint)
        {
            try
            {
                using (DbModel1s db = new DbModel1s())
                {
                    db.Entry(complaint).State = EntityState.Modified;
                    db.SaveChanges();
                    Email_Sender email_Sender = new Email_Sender();
                    email_Sender.Complaint_Email(complaint.User_Email, complaint.Complaint_ID, complaint.Status);               
                }
                return RedirectToAction("Admin_Index");

            }

            catch
            {
                return RedirectToAction("Admin_Index");

              
            }
           // return vi("Admin_Index");
        }

        #endregion

        #region Admin_Delete
        [HttpGet]
        public ActionResult Admin_Delete(int id, Complaint_DB complaint_DB)
        {
            using (DbModel1s db = new DbModel1s())
            {
                complaint_DB = db.Complaint_DB.Where(x => x.Complaint_ID == id).FirstOrDefault();
                db.Complaint_DB.Remove(complaint_DB);
                db.SaveChanges();
                return RedirectToAction("Admin_Index");
            }
        }
        #endregion

        #region Complaint_Admin_Details
        [HttpGet]
        public ActionResult Admin_Details(int id)
        {
            using (DbModel1s db = new DbModel1s())
            {
                return View(db.Complaint_DB.Where(x => x.Complaint_ID == id).FirstOrDefault());
            }
        }
        #endregion

        #region Complaint_Admin_Index
        [HttpGet]
        public ActionResult Admin_Index(int? i)
        {
            try
            {
                using (DbModel1s db1 = new DbModel1s())
                {
                    var draftss = db1.Complaint_DB.ToList().OrderByDescending(d => d.Complaint_ID).ToPagedList(i ?? 1, 5);
                    return View(draftss);
                }
            }
            catch
            {
                return View("Login");
            }
        }
        #endregion

        #region Complaint_User_Index
        [HttpGet]
        public void RetrieveUserInfo(string Name)
        {
            try
            {
                Name = Session["Name"].ToString();
                using (DbModel1s db1 = new DbModel1s())
                {
                    Connectionstring();
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "Select * from  Sign_Up where Email='" + Name + "'";
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        DateTime now = DateTime.Now;
                        string id = dr.GetValue(0).ToString();
                        string name = dr.GetValue(1).ToString();
                        string email = dr.GetValue(2).ToString();
                        

                        ViewBag.Id = id;
                        ViewBag.name = name;
                        ViewBag.email = email;
                        ViewBag.date = now.ToString("MM/dd/yyyy hh:mm tt"); 
                    }
                }
            }
            catch
            {
                //session expire so we should go to logiin page again !!
                RedirectToAction("login");
            }
        }
        [HttpGet]
        public ActionResult Index(string Name, int? i)
        {
            try
            {
                using (DbModel1s db1 = new DbModel1s())
                {
                    if (Session["Name"] == null)
                    {
                        return RedirectToAction("LogIn", "Login");

                    }
                    else
                    {
                        Name = Session["Name"].ToString();
                        RetrieveUserInfo(Name);
                        var drafts = db1.Complaint_DB.Where(d => d.User_Email == Name).ToList().OrderByDescending(d => d.Complaint_ID).ToPagedList(i ?? 1, 5);
                        return View(drafts);
                    }
                }
            }
            catch
            {
                return RedirectToAction("LogIn", "Login"); 
            }
        }
        #endregion

        #region Complaint_Details_By_User
        [HttpGet]
        public ActionResult Details(int id)
        {
            using (DbModel1s db = new DbModel1s())
            {
                return View(db.Complaint_DB.Where(x => x.Complaint_ID == id).FirstOrDefault());

            }
        }
        #endregion

        #region Complaint_Create_By_user

        public ActionResult Create(string Name)
        {
            RetrieveUserInfo(Name);
            return View();
        }
        [HttpPost]
        public ActionResult Create(Complaint_DB complaint_DB, string Name)
        {
            try
            {
                using (DbModel1s db = new DbModel1s())
                {
                    RetrieveUserInfo(Name);
                    db.Complaint_DB.Add(complaint_DB);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Complaint_delete_By_user
        [HttpGet]
        public ActionResult Delete(int id, Complaint_DB complaint_DB)
        {
            using (DbModel1s db = new DbModel1s())
            {
                complaint_DB = db.Complaint_DB.Where(x => x.Complaint_ID == id).FirstOrDefault();
                db.Complaint_DB.Remove(complaint_DB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Complaint_To_Excel
        public ActionResult xl()
        {
            DbModel1s entities = new DbModel1s();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Complaint_ID"),
                                            new DataColumn("User_Email"),
                                            new DataColumn("User_Name"),
                                            new DataColumn("Date") ,
                                            new DataColumn("Subject") ,
                                            new DataColumn("Description"),
                                            new DataColumn("Complaint Status")
            });

            var complaint = from complaints in entities.Complaint_DB
                        select complaints;

            foreach (var complaints in complaint)
            {
                dt.Rows.Add(complaints.Complaint_ID, complaints.User_Email, complaints.User_Name, complaints.Date, complaints.Subject, complaints.Description, complaints.Status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Complaint.xlsx");
                }
            }
        }
        #endregion


    }
}
