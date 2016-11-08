using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {

            string language = Request.QueryString["language"];

            if (String.IsNullOrEmpty(language))
            {
                if (Request.Cookies["language"] != null)
                {
                    language = Request.Cookies["language"].Value;
                }
                else
                {
                    language = "zh-cn";
                }

            }
            Response.Cookies["language"].Value = language;
            Response.Cookies["language"].Expires = DateTime.MaxValue;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);


        }
    }
}
