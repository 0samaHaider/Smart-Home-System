using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FYP_App.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYP_App.Controllers
{
    public class SensorController : Controller
    {
        #region BasePath
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBBto0oXxnvec1ANLb6LCyCOCFRCTHiA2ic6zdat",
            BasePath = "https://fyp-firebase-67d9c-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        #endregion

        #region Sensor_View
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Sensors");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Sensor>();
            try
            {
                foreach (var item in data)
                {
                  
                    list.Add(JsonConvert.DeserializeObject<Sensor>(((JProperty)item).Value.ToString()));
                }
               
                

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "No Data Here");
            }
            return View(list);
        }
        public ActionResult Admin_Dashboard()
        {
            DbModel1s db1 = new DbModel1s();
            DbModels db2 = new DbModels();
            ViewBag.Countapproveduser = db2.Sign_Up.Count(a => a.Status == "Approved");
            ViewBag.Countuser = db2.Sign_Up.Count();
            ViewBag.Counttotal = db1.Complaint_DB.Count();
            ViewBag.Count = db1.Complaint_DB.Count(a => a.Status == "Solved");
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Sensors");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Sensor>();
            try
            {
                foreach (var item in data)
                {

                    list.Add(JsonConvert.DeserializeObject<Sensor>(((JProperty)item).Value.ToString()));
                }



            }
            catch
            {
                ModelState.AddModelError(string.Empty, "No Data Here");
            }
            return View(list);
        }
        #endregion
    }
}