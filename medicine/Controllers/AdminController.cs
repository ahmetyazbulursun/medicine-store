using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using medicine.Models.Entity;

namespace medicine.Controllers
{
    public class AdminController : Controller
    {

        MedicineEntities db = new MedicineEntities();
        public void HideMenu()
        {
            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;
        }

        public ActionResult Index(int sayfa = 1)
        {
            HideMenu();

            var value = db.Tbl_Admin.ToList().ToPagedList(sayfa, 10);
            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;
            return View(value);
        }

        [HttpGet]
        public ActionResult AdminEkle()
        {
            HideMenu();

            return View();
        }

        [HttpPost]
        public ActionResult AdminEkle(Tbl_Admin p)
        {
            db.Tbl_Admin.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AdminSil(int id)
        {
            var value = db.Tbl_Admin.Find(id);
            db.Tbl_Admin.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult AdminGuncelle(int id)
        {
            HideMenu();

            var value = db.Tbl_Admin.Find(id);
            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;
            return View("AdminGuncelle", value);
        }

        public ActionResult AdminGuncelleme(Tbl_Admin p)
        {
            var value = db.Tbl_Admin.Find(p.ID);

            value.KULLANICIADI = p.KULLANICIADI;
            value.PAROLA = p.PAROLA;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}