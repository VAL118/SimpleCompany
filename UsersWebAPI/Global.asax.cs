using System.Data.Entity;
using System.Web.Http;
using SimpleCompanyDAL.EF;

namespace UsersWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Initialization of the database with test data.
            Database.SetInitializer(new DataInitializer());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
