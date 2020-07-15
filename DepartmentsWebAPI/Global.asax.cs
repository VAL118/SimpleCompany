using System.Data.Entity;
using System.Web.Http;
using SimpleCompanyDAL.EF;

namespace DepartmentsWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new DataInitializer());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
