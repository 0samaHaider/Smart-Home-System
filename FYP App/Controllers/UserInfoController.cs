using System;

using System.Linq;

using System.Web.Mvc;
using FYP_App.Models;

using System.Data.Entity;
using PagedList;

namespace FYP_App.Controllers
{
    public class UserInfoController : Controller
    {
        #region Dashboard_Admin
        [HttpGet]
        public ActionResult Admin()
        {
            DbModel1s db1 = new DbModel1s();
            DbModels db2 = new DbModels();
            ViewBag.Countapproveduser = db2.Sign_Up.Count(a => a.Status == "Approved");
            ViewBag.Countuser = db2.Sign_Up.Count();
            ViewBag.Counttotal = db1.Complaint_DB.Count();
            ViewBag.Count = db1.Complaint_DB.Count(a => a.Status == "Solved");
            return View();
        }
        #endregion

        #region UserProfile_Index
        [HttpGet]
        public ActionResult Index(string Name)
        {
            try
            {
                using (DbModels db1 = new DbModels())
                {
                    Name = Session["Name"].ToString();
                    var drafts = db1.Sign_Up.Where(d => d.Email == Name).ToList();
                    return View(drafts);
                }
            }
            catch
            {
                return View("Login");
            }
        }
        #endregion


        #region User_Profile_Update_By_User
        [HttpGet]
        public PartialViewResult Update(Int32 ID)
        {
            DbModels sd = new DbModels();
            Sign_Up emp = sd.Sign_Up.Where(x => x.ID == ID).FirstOrDefault();
            Crudclass empclass = new Crudclass();
            empclass.Name = emp.Name;
            empclass.Password = emp.Password;
            empclass.Phone = emp.Phone;
            empclass.Status = emp.Status;
            return PartialView(empclass);
        }
        [HttpPost]
        public JsonResult Update(Sign_Up emp)
        {

            DbModels sd = new DbModels();
            int empp = emp.ID;
            Sign_Up emptb = sd.Sign_Up.Where(x => x.ID == empp).FirstOrDefault();
            Crudclass empclass = new Crudclass();
            emptb.Name = emp.Name;
            emptb.Password = emp.Password;
            emptb.Phone = emp.Phone;
            emptb.Status = emp.Status;
            sd.SaveChanges();

            return Json(emptb, JsonRequestBehavior.AllowGet);
        }



        #endregion
    }
}
