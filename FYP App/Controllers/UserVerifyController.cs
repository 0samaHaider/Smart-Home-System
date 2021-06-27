using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FYP_App.Models;
using PagedList;
using FYP_App.Services;
using Syncfusion.XlsIO;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using ClosedXML.Excel;

namespace FYP_App.Controllers
{
    public class UserVerifyController : Controller
    {

        public ActionResult xl()
        {
            DbModels entities = new DbModels();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("ID"),
                                            new DataColumn("User Name"),
                                            new DataColumn("User Email"),
                                            new DataColumn("User Phone") ,
                                            new DataColumn("User Password") ,
                                            new DataColumn("User Account status") });

            var users = from user in entities.Sign_Up
                            select user;

            foreach (var user in users)
            {
                dt.Rows.Add(user.ID, user.Name, user.Email, user.Phone, user.Password,user.Status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
                }
            }
        }
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

                return Json("error");
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
            empclass.Email = emp.Email;
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
            emptb.Email = emp.Email;
            sd.SaveChanges();
            Email_Sender email_Sender = new Email_Sender();
            email_Sender.Profile_Update(emptb.Email);
            return Json(emptb, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //public void ExportToExcel()
        //{
        //    List<Sign_Up> emplist = .Select(x => new Sign_Up
        //    {
        //        EmployeeId = x.EmployeeId,
        //        EmployeeName = x.EmployeeName,
        //        Email = x.Email,
        //        Phone = x.Phone,
        //        Experience = x.Experience
        //    }).ToList();

        //    ExcelPackage pck = new ExcelPackage();
        //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

        //    ws.Cells["A1"].Value = "Communication";
        //    ws.Cells["B1"].Value = "Com1";

        //    ws.Cells["A2"].Value = "Report";
        //    ws.Cells["B2"].Value = "Report1";

        //    ws.Cells["A3"].Value = "Date";
        //    ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

        //    ws.Cells["A6"].Value = "EmployeeId";
        //    ws.Cells["B6"].Value = "EmployeeName";
        //    ws.Cells["C6"].Value = "Email";
        //    ws.Cells["D6"].Value = "Phone";
        //    ws.Cells["E6"].Value = "Experience";

        //    int rowStart = 7;
        //    foreach (var item in emplist)
        //    {
        //        if (item.Experience < 5)
        //        {
        //            ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));

        //        }

        //        ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeId;
        //        ws.Cells[string.Format("B{0}", rowStart)].Value = item.EmployeeName;
        //        ws.Cells[string.Format("C{0}", rowStart)].Value = item.Email;
        //        ws.Cells[string.Format("D{0}", rowStart)].Value = item.Phone;
        //        ws.Cells[string.Format("E{0}", rowStart)].Value = item.Experience;
        //        rowStart++;
        //    }

        //    ws.Cells["A:AZ"].AutoFitColumns();
        //    Response.Clear();
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
        //    Response.BinaryWrite(pck.GetAsByteArray());
        //    Response.End();

        //}

    }
}


    
