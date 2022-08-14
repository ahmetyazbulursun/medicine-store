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
    public class RaporlarController : Controller
    {

        MedicineEntities db = new MedicineEntities();

        public void HideMenu()
        {
            var authority = Session["YETKI"].ToString();
            ViewBag.userAuthority = authority;
        }

        public ActionResult AylıkRapor(string ara = "", string ara2 = "")
        {
            HideMenu();

            var value = from x in db.Tbl_SiparisDetay select x;
            if (!string.IsNullOrEmpty(ara))
            {
                value = value.Where(x => x.AY.Contains(ara));
            }
            if (!string.IsNullOrEmpty(ara2))
            {
                value = value.Where(x => x.YIL.Contains(ara2));
            }
            return View(value.ToList());
        }

    }
}