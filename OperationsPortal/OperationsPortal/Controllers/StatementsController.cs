using OperationsPortal.NAVWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsPortal.Controllers
{
    public class StatementsController : Controller
    {
      
        private readonly string[] strLimiters2 = new string[] { "[]" };
        private readonly string[] strLimiters = new string[] { "::" };
        

        Operations webportals = Components.ObjNav;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WarehouseReceipt(string customerNo)
        {
            customerNo = Session["pensionerNo"]?.ToString();

            if (customerNo == null) return RedirectToAction("index", "login");
            string fileName = customerNo.Replace(@"/", @"");
            string pdfFileName = $"WarehouseReceipt-{fileName}.pdf";

            string path = Server.MapPath("~/Downloads/");
            string pdfFilePath = Path.Combine(path, pdfFileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // Create the Downloads folder if it doesn't exist
            }
            DateTime period = DateTime.Today;

            try
            {
                webportals.GenerateWarehouseReceipt(path, pdfFileName, customerNo);
              
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                ViewBag.Error = "An error occurred while generating the life certificate.";
                return View();
            }

            if (System.IO.File.Exists(pdfFilePath))
            {
                
                ViewBag.PdfUrl = Url.Content($"~/Downloads/{pdfFileName}");
            }
            else
            {
                ViewBag.Error = "Warehouse receipt generation failed. File not found.";
            }
            return View();

        }
    }
}