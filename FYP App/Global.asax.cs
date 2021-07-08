using FYP_App.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FYP_App
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
        HttpContext ctx = HttpContext.Current;

        protected void Application_Start()
        {
            
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Application_Error(object sender, EventArgs e)
        {
            HttpContext con = HttpContext.Current;
            var v = Server.GetLastError();

            var HttpEx = v as HttpException;
            if (HttpEx.GetHttpCode() == 500 || HttpEx.GetHttpCode() == 400)
            {

                #region SaveFile
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("Page :           " + con.Request.Url.ToString());
                //sb.AppendLine("Error Message :  " + v.Message);

                //// Here save text file containing this error details
                //string fileName = Path.Combine(Server.MapPath("~/Errors")+ ".txt");
                //File.WriteAllText(fileName, sb.ToString());
                #endregion
                Server.TransferRequest("Login/Error");
                //Server.Transfer("Error");
            }
            else
            {
                Server.TransferRequest("Login/Error");
            }
        }
    }
}
