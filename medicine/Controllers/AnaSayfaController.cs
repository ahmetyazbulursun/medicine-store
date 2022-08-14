using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using medicine.Models.Entity;

namespace medicine.Controllers
{
    public class AnaSayfaController : Controller
    {

        MedicineEntities db = new MedicineEntities();

        public ActionResult Index()
        {
            var value = db.Tbl_Urunler.ToList();

            // Total Products Count
            var totalProduct = db.Tbl_Urunler.Where(x => x.DURUM == true).Count().ToString();
            ViewBag.totalProduct = totalProduct;

            // Total Order Count
            var totalOrder = db.Tbl_Siparisler.Where(x => x.DURUM == true).Count().ToString();
            ViewBag.totalOrder = totalOrder;

            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;

            return View(value);
        }

        public PartialViewResult LastOrders()
        {
            var value = db.Tbl_Siparisler.Take(5).ToList();
            return PartialView(value);
        }

        public PartialViewResult LastProducts()
        {
            var value = db.Tbl_Urunler.Take(5).OrderByDescending(x => x.ID).Where(x => x.DURUM == true).ToList();
            return PartialView(value);
        }


    }
}