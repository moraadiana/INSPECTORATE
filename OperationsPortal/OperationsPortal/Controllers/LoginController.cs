using OperationsPortal.Models;
using OperationsPortal.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace OperationsPortal.Controllers
{
    public class LoginController : Controller
    {


        Operations webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            try
            {
                string custNo = account.custNo;
                string emailAddress = account.Email;
                string password = account.Password.Trim();
                bool isValid = webportals.CheckValidCustomerNo(custNo);
                Console.WriteLine($"CheckValidPensionerNo returned: {isValid}");
                if (webportals.CheckValidCustomerNo(custNo))
                {
                    string response = webportals.CheckCustomerLogin(custNo, password);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        string returnMsg = responseArr[0];
                        if (returnMsg == "SUCCESS")
                        {
                            string customerNo = responseArr[2];
                            string customerName = responseArr[3];
                            string customerEmail = responseArr[4];
                            // string vendorVat = responseArr[4];

                            Session["customerNo"] = customerNo;
                            Session["customerName"] = customerName;
                            Session["customerEmail"] = customerEmail;
                            // Session["VendorVat"] = vendorVat;

                            //string otp = GenerateOtp(6);
                            //Session["otp"] = otp;

                            //string subject = "Inspectorate Operations Portal OTP";
                            //string body = $"{otp} is your OTP Code for Inspectorate Operations portal.";
                            //Components.SendEmailAlerts(pensionerEmail, subject, body);
                            //return RedirectToAction("verifyotp");
                            return RedirectToAction("index", "dashboard");
                        }
                        else
                        {
                            TempData["error"] = returnMsg;
                            return View("index");
                        }
                    }
                }
                else
                {
                    TempData["error"] = "Invalid Vendor No. ";
                    return View("index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("index");
            }
            return View();
        }

        public ActionResult VerifyOTP()
        {
            if (Session["pensionerNo"] == null) return View("index");
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(OTP otp)
        {
            try
            {
                string generatedOtp = Session["otp"].ToString();
                string otpFromUser = otp.OTPCode.Trim();

                if (generatedOtp.ToLower() == otpFromUser.ToLower())
                {
                    return RedirectToAction("index", "dashboard");
                }
                else
                {
                    TempData["error"] = "Invalid OTP. Please try again later";
                    return View("verifyotp");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("verifyotp");
            }
        }

        public static string GenerateOtp(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}