using OperationsPortal.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string[] strLimiters2 = new string[] { "[]" };
        private readonly string[] strLimiters = new string[] { "::" };
   

        Operations webportals = Components.ObjNav;
        
        public ActionResult Index()
        {
            if (Session["customerNo"] == null) return RedirectToAction("index", "login");
            string username = Session["customerNo"].ToString();
            GetMemberData(username);
            return View();
        }
       
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("index", "login");
        }

        private void GetMemberData(string username)
        {
            try
            {
                string response = webportals.GetCustomerProfileDetails(username);
                if (response != null)
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    Session["Email"] = responseArr[0];
                    Session["PhoneNo"] = responseArr[1];
                    //  Session["Designation"] = responseArr[2];
                    Session["Address"] = responseArr[2];
                    Session["Country"] = responseArr[3];
                    Session["Name"] = responseArr[4];
                    
                    //Session["MemberName"]

                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}