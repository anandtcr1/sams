using sams.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace sams.Controllers
{
    public class DBConnection
    {

        //public static string ConnectionString = @"Server=tcp:samsdevelopment20200411113014dbserver.database.windows.net,1433;Initial Catalog=sams-dev;Persist Security Info=False;User ID=samsdevadmin;Password=RealEstate@2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //public static string ConnectionString = @"Data Source = DESKTOP-C0QETOJ\SQLEXPRESS; Initial Catalog = sam1; User id = sa; Password=123;";

        //public static string ConnectionString = @"Data Source = DESKTOP-C0QETOJ\SQLEXPRESS; Initial Catalog = sams_prod_19_10_2020; User id = sa; Password=123;";
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        public static string ConnectionString = Helper.SamsConnectionString;
        //public static string ConnectionString = Helper.SamsConnectionStringLocal;
        //public static string ConnectionString = Helper.SamsConnectionStringSandBox;
        //public static string ConnectionString = Helper.SamsConnectionStringQa;
    }
}