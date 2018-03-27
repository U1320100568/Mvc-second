using DatabaseFirstTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatabaseFirstTest.Helpers;

namespace DatabaseFirstTest.Controllers
{
    public class HomeController : Controller
    {
        private UserTestEntities db= new UserTestEntities() { };

        public ActionResult Index()
        {
            string UserName= AccountHelper.UserName;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logon()
        {
            ViewBag.Message = "Logon";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logon(Logon logon)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            /**/
            var user = db.User.FirstOrDefault(x => x.Name == logon.Account);
            if (user == null)
            {
                ModelState.AddModelError("", "請輸入正確帳號密碼");
            }
            //return RedirectToAction("Index", "Users");
            ViewBag.Msg = logon.Remember;

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: logon.Account,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: false,
                userData:logon.Account,
                cookiePath: FormsAuthentication.FormsCookiePath
                );
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
            //FormsIdentity id = (FormsIdentity)HttpContext.User.Identity;
           // string[] roles = ticket.UserData.Split(',');
            //HttpContext.User = new GenericPrincipal(id, roles);
            
            return RedirectToAction("Index","Home");
        }

        public ActionResult Logout() {

            FormsAuthentication.SignOut();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }
    }
}