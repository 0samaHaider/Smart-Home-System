using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FYP_App.Models;
using PagedList;

namespace FYP_App.Controllers
{
    public class UserVerifyController : Controller
    {
        #region User_Index_By_Admin
        [HttpGet]
        public ActionResult Index(int? i)
        {
            using (DbModels db1 = new DbModels())
            {
                var drafts = db1.Sign_Up.ToList().ToPagedList(i ?? 1, 5);
                return View(drafts);
            }
        }
        #endregion

        #region Garbage_Code

        //// GET: UserVerify/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserVerify/Create
        //[HttpPost]
        //public ActionResult Create(Sign_Up sign)
        //{
        //    // TODO: Add insert logic here
        //    using (DbModels db = new DbModels())
        //    {
        //        db.Sign_Up.Add(sign);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        //// GET: UserVerify/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    using (DbModels db = new DbModels())
        //    {
        //        return View(db.Sign_Up.Where(x => x.ID == id).FirstOrDefault());

        //    }
        //}

        //// POST: UserVerify/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, Sign_Up sign)
        //{
        //    try
        //    {
        //        using (DbModels db = new DbModels())
        //        {
        //            db.Entry(sign).State = EntityState.Modified;

        //            db.SaveChanges();

        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        #endregion

        #region User_Delete_by_Admin
        [HttpGet]
        public ActionResult Delete(int id, Sign_Up sign)
        {
            using (DbModels db = new DbModels())
            {
                sign = db.Sign_Up.Where(x => x.ID == id).FirstOrDefault();
                db.Sign_Up.Remove(sign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region User_Create_By_Admin
        [HttpGet]
        public PartialViewResult Creates()
        {
            return PartialView("Creates", new Models.Sign_Up());
        }
        [HttpPost]
        public JsonResult Creates(Sign_Up sign_Up)
        {
            if (ModelState.IsValid)
            {
                DbModels sd = new DbModels();
                sd.Sign_Up.Add(sign_Up);
                sd.SaveChanges();
                return Json(sign_Up, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json("<script>alert(\"some message\")</script>");
            }

        }
        #endregion

        #region User_Edit_By_Admin
        [HttpGet]
        public PartialViewResult Editt(Int32 ID)
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
        public JsonResult Editt(Sign_Up emp)
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


    
