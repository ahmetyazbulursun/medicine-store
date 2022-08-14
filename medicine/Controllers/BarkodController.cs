using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using medicine.Models.Entity;

namespace medicine.Controllers
{
    public class BarkodController : Controller
    {

        MedicineEntities db = new MedicineEntities();

        public ActionResult Index()
        {
            return View();
        }
    }
}