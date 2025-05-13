using OperationsPortal.Models;
using OperationsPortal.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
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
                Console.WriteLine($"CheckValidCustomerNo returned: {isValid}");
                if (webportals.CheckValidCustomerNo(custNo))
                {
                    string response = webportals.CheckCustomerLogin(custNo, password);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        string returnMsg = responseArr[0];
                        if (returnMsg == "SUCCESS")
                        {
                            string customerNo = responseArr[1];
                            string customerName = responseArr[2];
                            string customerEmail = responseArr[3];
                            

                            Session["customerNo"] = customerNo;
                            Session["customerName"] = customerName;
                            Session["customerEmail"] = customerEmail;


                            //string otp = GenerateOtp(6);
                            //Session["otp"] = otp;

                            //string subject = "Inspectorate Operations Portal OTP";
                            //string body = $"{otp} is your OTP Code for Inspectorate Operations portal.";
                            //Components.SendEmailAlerts(customerEmail, subject, body);
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
                    TempData["error"] = "Invalid Client No. ";
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
            if (Session["customerNo"] == null) return View("index");
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
        public ActionResult ResetPassword(string email, string customerNo)
        {
            // If email is provided in query string, store it in session
            if (!string.IsNullOrEmpty(email))
            {
                Session["EmailAddress"] = email;
            }

            if (!string.IsNullOrEmpty(customerNo))
            {
                Session["customerNo"] = customerNo;
            }

            // Retrieve stored session values if they exist
            ViewBag.EmailAddress = Session["EmailAddress"] as string;
            ViewBag.customerNo = Session["customerNo"] as string;

            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            try
            {
                //if (Session["customerNo"] == null)
                //{
                //    TempData["Error"] = "Session expired. Please log in again.";
                //    return RedirectToAction("index", "login");
                //}
                string newPassword = reset.Password;
                string confirmPassword = reset.PasswordConfirmation;
                string customerNo = reset.customerNo;
                //string customerNo = Session["customerNo"].ToString();

                string response = webportals.UpdateCustomerPassword(customerNo, newPassword);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["Success"] = "Password has been updated successfully";
                        return RedirectToAction("index", "login");
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while updating your password. Please try again later.";
                        return RedirectToAction("resetpassword", "login");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }
        public ActionResult ForgotPassword(string email, string customerNo)
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ResetPassword reset)
        {
            try
            {

                string newPassword = GenerateRandomPassword(10);
                string clientNo = reset.customerNo;
                string clientEmail = reset.Email;
                //string email = Components.ObjNav.GetCustomerEmail(customerNo);
                string response = Components.ObjNav.UpdateCustomerAutoGenPassword(clientNo, newPassword);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response != "SUCCESS")
                    {
                        TempData["Error"] = "Failed to reset the password. Please try again.";
                        View("forgotpassword");
                    }

                }
                string subject = "Inspectorate Operations Portal Password Reset";
                string body = $"Use this password to log into Inspectorate Operations  Portal.<br/><br/>Auto generated Portal password: <strong>{newPassword}</strong> <br/> <br/>Do not reply to this email.";
                Components.SendEmailAlerts(clientEmail, subject, body);
                
                TempData["Success"] = $"Auto generated password has been sent to your email address {clientEmail}";
                //  return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("forgotpassword");
            }
            return View("index");
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$!";
            StringBuilder password = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] byteBuffer = new byte[1];

                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(byteBuffer);
                    int index = byteBuffer[0] % validChars.Length;
                    password.Append(validChars[index]);
                }
            }

            return password.ToString();
        }
        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}