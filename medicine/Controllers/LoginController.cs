using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using medicine.Models.Entity;

namespace medicine.Controllers
{
    [AllowAnonymous]

    public class LoginController : Controller
    {

        MedicineEntities db = new MedicineEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Tbl_Admin p)
        {
            var value = db.Tbl_Admin.FirstOrDefault(x => x.KULLANICIADI == p.KULLANICIADI && x.PAROLA == p.PAROLA);

            if(value != null)
            {
                FormsAuthentication.SetAuthCookie(value.KULLANICIADI, false);
                Session["KULLANICIADI"] = value.KULLANICIADI.ToString();
                Session["PAROLA"] = value.PAROLA.ToString();
                Session["ID"] = value.ID.ToString();
                Session["YETKI"] = value.YETKI.ToString();
                return RedirectToAction("Index", "AnaSayfa");
            }

            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


    }
}