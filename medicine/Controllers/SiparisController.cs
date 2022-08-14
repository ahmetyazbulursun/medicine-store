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
    public class SiparisController : Controller
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

            var value = from x in db.Tbl_Siparisler.Where(x => x.DURUM == true).OrderByDescending(x=>x.ID) select x;
            if (!string.IsNullOrEmpty(ara))
            {
                value = value.Where(x => x.SIPARISADI.ToLower().Contains(ara));
            }

            return View(value.ToList().ToPagedList(sayfa, 10));
        }

        public ActionResult SiparisOlustur(Tbl_Siparisler p, string kar)
        {
            p.TARIH = DateTime.Now;
            p.DURUM = true;
            db.Tbl_Siparisler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SiparisSil(Tbl_Siparisler p)
        {
            var value = db.Tbl_Siparisler.Find(p.ID);
            value.DURUM = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SiparisDetay(int id, int sayfa = 1)
        {
            HideMenu();

            var value = db.Tbl_SiparisDetay.Where(x => x.SIPARISID == id).ToList().ToPagedList(sayfa, 10);

            // Total Price On the Order
            var totalPrice = db.Tbl_SiparisDetay.Where(x => x.SIPARISID == id).Sum(x => x.FIYAT).ToString();
            ViewBag.totalPrice = totalPrice;


            Tbl_Siparisler order = db.Tbl_Siparisler.Where(x => x.ID == id).FirstOrDefault();

            // Total Price + KDV %8
            if (order.KDV == "8")
            {
                var kdv8 = Convert.ToDouble(totalPrice) * 8 / 100;
                var priceKdv8 = Convert.ToDouble(totalPrice) + kdv8;
                ViewBag.kdv8 = "/ KDV %8 Dahil Fiyat: " + priceKdv8 + " ₺";
            }

            // Total Price + KDV %18
            if (order.KDV == "18")
            {
                var kdv18 = Convert.ToDouble(totalPrice) * 0.18;
                var priceKdv18 = Convert.ToDouble(totalPrice) + kdv18;
                ViewBag.kdv18 = "/ KDV %18 Dahil Fiyat: " + priceKdv18 + " ₺";
            }

            return View(value);
        }

        public ActionResult Yazdir(int id)
        {
            var value = db.Tbl_SiparisDetay.Where(x => x.SIPARISID == id).ToList();

            // Order Name
            var orderName = db.Tbl_SiparisDetay.Where(x => x.SIPARISID == id).FirstOrDefault();
            ViewBag.orderName = orderName.Tbl_Siparisler.SIPARISADI;

            // Total Price On the Order
            var totalPrice = db.Tbl_SiparisDetay.Where(x => x.SIPARISID == id).Sum(x => x.FIYAT).ToString();
            ViewBag.totalPrice = totalPrice;


            Tbl_Siparisler order = db.Tbl_Siparisler.Where(x => x.ID == id).FirstOrDefault();

            if (order.KDV == "Yok")
            {
                var kdv1 = Convert.ToDouble(totalPrice) * 1;
                ViewBag.kdv1 = kdv1;
            }

            // Total Price + KDV %8
            if (order.KDV == "8")
            {
                var kdv8 = Convert.ToDouble(totalPrice) * 8 / 100;
                var priceKdv8 = Convert.ToDouble(totalPrice) + kdv8;
                ViewBag.tableName8 = "KDV:";
                ViewBag.kdv8 = priceKdv8;
                ViewBag.tl = "₺";
                ViewBag.percent8 = "%8";
            }

            // Total Price + KDV %18
            if (order.KDV == "18")
            {
                var kdv18 = Convert.ToDouble(totalPrice) * 0.18;
                var priceKdv18 = Convert.ToDouble(totalPrice) + kdv18;
                ViewBag.tableName18 = "KDV:";
                ViewBag.kdv18 = priceKdv18;
                ViewBag.tl = "₺";
                ViewBag.percent18 = "%18";
            }



            return View(value);
        }

        [HttpGet]
        public ActionResult UrunEkle(int id)
        {
            HideMenu();

            var value = db.Tbl_SiparisDetay.Find(id);

            List<SelectListItem> urun = (from x in db.Tbl_Urunler.Where(x => x.DURUM == true).ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.URUNAD,
                                             Value = x.ID.ToString()
                                         }
                                            ).ToList();
            ViewBag.dgr = urun;

            return View("UrunEkle", value);
        }

        [HttpPost]
        public ActionResult UrunEkle(Tbl_SiparisDetay p, int id, int price = 0, int adet = 0, int sonuc = 0)
        {
            var product = db.Tbl_Urunler.Where(x => x.ID == p.Tbl_Urunler.ID).FirstOrDefault();
            var order = db.Tbl_Siparisler.Find(p.ID);

            // Adding Profit
            Tbl_Siparisler profitedOrder = db.Tbl_Siparisler.Where(x => x.ID == id).FirstOrDefault();
            var fiyat = product.FIYAT;
            var profit = Convert.ToInt32(profitedOrder.KAR);
            price = (Convert.ToInt32(fiyat) * profit) / 100;
            var y = fiyat + price;
            
            // Total Price
            sonuc = Convert.ToInt32(y) * adet;
            p.FIYAT = sonuc;


            // Falling Out Of Stock
            var stock = product.STOK;
            var fallingOutOfStock = stock - adet;
            product.STOK = fallingOutOfStock;

            //-------------------------------------------->
            // Convert Date To String (Month)
            var month = DateTime.Now.Month;
            string stringMonth = Convert.ToString(month);

            if(stringMonth == "1")
            {
                stringMonth = "Ocak";
            }
            if (stringMonth == "2")
            {
                stringMonth = "Şubat";
            }
            if (stringMonth == "3")
            {
                stringMonth = "Mart";
            }
            if (stringMonth == "4")
            {
                stringMonth = "Nisan";
            }
            if (stringMonth == "5")
            {
                stringMonth = "Mayıs";
            }
            if (stringMonth == "6")
            {
                stringMonth = "Haziran";
            }
            if (stringMonth == "7")
            {
                stringMonth = "Temmuz";
            }
            if (stringMonth == "8")
            {
                stringMonth = "Ağustos";
            }
            if (stringMonth == "9")
            {
                stringMonth = "Eylül";
            }
            if (stringMonth == "10")
            {
                stringMonth = "Ekim";
            }
            if (stringMonth == "11")
            {
                stringMonth = "Kasım";
            }
            if (stringMonth == "12")
            {
                stringMonth = "Aralık";
            }
            // <---------------------------

            // Convert Date To String (Year)
            var year = DateTime.Now.Year;

            p.Tbl_Urunler = product;
            p.SIPARISID = order.ID;
            p.AY = stringMonth;
            p.YIL = Convert.ToString(year);
            p.BIRIMFIYAT = y;


            db.Tbl_SiparisDetay.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UrunSil(int id)
        {
            var value = db.Tbl_SiparisDetay.Find(id);
            db.Tbl_SiparisDetay.Remove(value);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}   