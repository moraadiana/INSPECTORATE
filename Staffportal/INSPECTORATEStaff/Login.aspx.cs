using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SupplierPortal
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusername.Focus();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LbtnLogin_Click();
        }

        protected void LbtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtPassword.Text.ToString().Trim().Replace("'", "");
            string user = txtusername.Text.ToString().Trim().Replace("'", "");
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(user))
            {
                LblError.Text = "Username or Password cannot be null!";
                return;
            }
            //Check Password Change Status
            if (ChangedPassStatus())
            {
                LoginForChangedPass();
            }
            else
            {
                LoginForUnchangedPass();
            }
        }

        protected void LoginForChangedPass()
        {
            string pass = txtPassword.Text.ToString().Trim().Replace("'", "");
            string user = txtusername.Text.ToString().Trim().Replace("'", "");
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.CheckBidderLogin(user, pass); 

                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string returnMsg = "", changedPassword = "", vendorNo = "", vendorName = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    changedPassword = staffLoginInfo_arr[1];

                    if (returnMsg == "Login")
                    {
                        vendorNo = staffLoginInfo_arr[1];
                        vendorName = staffLoginInfo_arr[2];

                        Session["username"] = vendorNo;
                        Session["StaffName"] = vendorName;
                        Response.Redirect("~/pages/Dashboard.aspx");
                    }
                    else
                    {
                        LblError.Text = returnMsg;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void LoginForUnchangedPass()
        {
            string pass = txtPassword.Text.ToString().Trim().Replace("'", "");
            string user = txtusername.Text.ToString().Trim().Replace("'", "");
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.CheckBidderLoginForUnchangedPass(user, pass);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string returnMsg = "", staffNo = "", email = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "Login")
                    {
                        staffNo = staffLoginInfo_arr[1];
                        email = staffLoginInfo_arr[2];

                        Response.Redirect("ResetPassword.aspx?sd=" + staffNo + "&em=" + email);
                        return;
                    }
                    else
                    {
                        LblError.Text = returnMsg;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private bool CheckValidBidderNo(string vendorNo)
        {
            //stored procedure spSelectApplicationFormQualification

            bool r = false;
            try
            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.CheckValidBidderNo(vendorNo);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "Yes")
                    {
                        r = true;
                    }
                }
                #endregion}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return r;
        }

        public void SendMyEmail(string address, string subject, string message)
        {
            Boolean x = false;

            string a = "";
            try
            {
                string email = "dynamicsselfservice@gmail.com";
                string password = "self@service444";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

                x = true;
                a = "Sent";

            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
                Message("Error, failed to send email. Contact System Administrator!");
                throw;
            }
        }
        private void UpdatePass(string Password)
        {
            try
            {
                MyComponents.ObjNav.UpdateStaffPass(txtusername.Text.ToString(), Password);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        public void Message(string strMsg)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }
        private bool ChangedPassStatus()
        {
            string username = txtusername.Text.ToString().ToUpper();
            bool b = false;
            try
            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.CheckBidderPasswordChanged(username);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "Yes")
                    {
                        b = true;
                    }
                }
                #endregion}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return b;
        }
        protected void lbtnForgot_Click(object sender, EventArgs e)
        {
            string user = txtusername.Text.ToString().Trim().Replace("'", "");
            if (string.IsNullOrEmpty(user))
            {
                LblError.Text = "Please enter your username!";
                txtusername.Focus();
                return;
            }
            //Check Password Change Status
            if (CheckValidBidderNo(user) == false)
            {
                LblError.Text = "Invalid Username";
                txtusername.Focus();
                return;
            }
            if (ChangedPassStatus() == false)
            {
                LblError.Text = "Warning! Your account is not active, Please use your MSU email address as your password to activate your account!";
                txtusername.Focus();
                return;
            }
            string email = GetBidderMail(user);
            if (email.Length < 3)
            {
                LblError.Text = "Error! Please visit the HR office to update your official email.";
                return;
            }
            #region LoginSp
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.GetBidderCurrentPassword(user);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string password = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);


                    password = staffLoginInfo_arr[0];

                    //Pick Email & send alert
                    string body = "Your Portal Password has been successfully reset, Use the below password to login<br/><br/>";
                    body += "<b>Password: " + password + " </b> <br /><br />";
                    SendMyEmail(email, "NACADA - Web Portal Password", body);
                    LblError.Text = "Your password has been sent to: " + email.ToLower();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            #endregion
        }
        private string GetBidderMail(string vendorNo)
        {
            string r = "";
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.GetBidderMail(vendorNo);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string email = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);
                    email = staffLoginInfo_arr[0];
                    r = email;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return r;
        }
    }
}