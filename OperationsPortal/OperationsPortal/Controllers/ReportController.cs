using OperationsPortal.NAVWS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsPortal.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        private readonly string[] strLimiters2 = new string[] { "[]" };
        private readonly string[] strLimiters = new string[] { "::" };


        Operations webportals = Components.ObjNav;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WarehouseReceipt(string receiptNo,string contractNo)
        {
            Session["receiptNo"] = receiptNo;
            Session["contractNo"] = contractNo;
            if (Session["customerNo"] == null)
                return RedirectToAction("index", "login");
            try
            {
                string fileName = receiptNo.Replace(@"/", @"");
                string pdfFileName = $"WarehouseReceipt-{fileName}.pdf";

                string path = Server.MapPath("~/Downloads/");
                if (string.IsNullOrEmpty(path))
                    throw new Exception("Resolved path is null or empty.");

                string pdfFilePath = Path.Combine(path, pdfFileName);
                Debug.WriteLine($"Resolved path: {path}");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (System.IO.File.Exists(pdfFilePath))
                    System.IO.File.Delete(pdfFilePath);

                webportals.GenerateWarehouseReceipt(path, pdfFileName, receiptNo,contractNo);
                TempData["PdfUrl"] = Url.Content($"~/Downloads/{pdfFileName}");
                ViewBag.PdfUrl = TempData["PdfUrl"];


            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return View();
        }
    }
}