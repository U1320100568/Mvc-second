using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DatabaseFirstTest.Helpers
{
    public class AccountHelper 
    {
        public static string UserName
        {
            get {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return string.Empty;
                }
                else
                {
                    
                    return identity.Name;
                }
            }
        } 
    }
}