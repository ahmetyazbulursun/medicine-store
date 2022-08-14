using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using medicine.Models;
using medicine.Models.Entity;
using PagedList;
using PagedList.Mvc;

namespace medicine.Controllers
{
    public class CeklerController : Controller
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

            // Fetch all files in the Folder
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/material-pro-lite-master/pdf"));

            // Copy File Names To Model Collection
            List<FileModel> files = new List<FileModel>();
            foreach(string filePath in filePaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }   

            return View(files);
        }

        public ActionResult PdfEkle()
        {
            HideMenu();

            return View();
        }
        
        [HttpPost]
        public ActionResult PdfEkleme(HttpPostedFileBase dosya, Tbl_Cekler p)
        {
            if(dosya.ContentLength > 0)
            {
                string dosyayolu = Path.Combine(Server.MapPath("~/material-pro-lite-master/pdf"), Path.GetFileName(dosya.FileName));
                dosya.SaveAs(dosyayolu);
            }
            
            db.Tbl_Cekler.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public FileResult PdfIndir(string fileName)
        {
            // Build the File Path
            string path = Server.MapPath("~/material-pro-lite-master/pdf/") + fileName;

            // Read the File Data Into Byte Array
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            // Send the File to Download
            return File(bytes, "application/octet-stream", fileName);
        }


    }
}