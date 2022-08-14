using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using medicine.Models.Entity;
using PagedList;
using PagedList.Mvc;

namespace medicine.Controllers
{
    public class UrunlerController : Controller
    {

        MedicineEntities db = new MedicineEntities();

        public void HideMenu()
        {
            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;
        }

        public ActionResult Index(int sayfa = 1, string ara = "")
        {
            HideMenu();

            var value = from x in db.Tbl_Urunler.Where(x => x.DURUM == true) select x;
            if (!string.IsNullOrEmpty(ara))
            {
                value = value.Where(x => x.URUNAD.ToLower().Contains(ara) || x.BARKOD.ToLower().Contains(ara));
            }

            return View(value.ToList().ToPagedList(sayfa, 25));
        }

        [HttpGet]
        public ActionResult UrunEkle()
        {
            HideMenu();

            List<SelectListItem> category = (from x in db.Tbl_Kategoriler.Where(x => x.DURUM == true).ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.KATEGORIADI,
                                             Value = x.ID.ToString()
                                         }
                                            ).ToList();
            ViewBag.dgr = category;

            return View();
        }

        [HttpPost]
        public ActionResult UrunEkle(Tbl_Urunler p)
        {
            var category = db.Tbl_Kategoriler.Where(x => x.ID == p.Tbl_Kategoriler.ID).FirstOrDefault();

            p.Tbl_Kategoriler = category;
            p.DURUM = true;
            db.Tbl_Urunler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UrunDetay(int id)
        {
            HideMenu();

            Tbl_Urunler t = db.Tbl_Urunler.Find(id);
            return View(t);
        }

        public ActionResult UrunSil(int id)
        {
            HideMenu();

            Tbl_Urunler t = db.Tbl_Urunler.Find(id);
            t.DURUM = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UrunGuncelle(int id)
        {
            HideMenu();

            var value = db.Tbl_Urunler.Find(id);

            List<SelectListItem> category = (from x in db.Tbl_Kategoriler.Where(x => x.DURUM == true).ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.KATEGORIADI,
                                                 Value = x.ID.ToString()
                                             }
                                           ).ToList();
            ViewBag.dgr = category;

            return View("UrunGuncelle", value);
        }

        public ActionResult UrunGuncelleme(Tbl_Urunler p)
        {
            var value = db.Tbl_Urunler.Find(p.ID);
            var category = db.Tbl_Kategoriler.Where(x => x.ID == p.Tbl_Kategoriler.ID).FirstOrDefault();

            value.URUNAD = p.URUNAD;
            value.ACIKLAMA = p.ACIKLAMA;
            value.BARKOD = p.BARKOD;
            value.STOK = p.STOK;
            value.FIYAT = p.FIYAT;
            value.KATEGORI = category.ID;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}