using OpenQA.Selenium;
using System;
using System.Web.UI;
using Windows.UI.Core;

namespace INSPECTORATEStaff
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusername.Focus();
        }

        protected void LbtnLogin_Click(object sender, EventArgs e)
        {
            string pass = txtpassword.Value.ToString();
            string user = txtusername.Value.ToString();
            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(user))
            {
                LblError.Text = "Username or Password cannot be null!";
                return;
            }
            if (ValidStaffNo(user) == false)
            {
                LblError.Text = "Invalid Staff No";
                txtusername.Focus();
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
                //LblError.Text = "Your Password has been sent to "+TxtPass.Text+"";
            }
           
        }

        protected void LoginForChangedPass()
        {
            string pass = txtpassword.Value.ToString();
            string user = txtusername.Value.ToString();
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.CheckStaffLogin(user, pass); 

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string returnMsg = "", changedPassword = "", staffNo = "", staffName = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    changedPassword = staffLoginInfo_arr[1];
                    if (returnMsg == "SUCCESS")
                    {
                        staffNo = staffLoginInfo_arr[2];
                        staffName = staffLoginInfo_arr[3];

                        Session["username"] = staffNo;
                        Session["StaffName"] = staffName;
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
            string pass = txtpassword.Value.ToString();
            string user = txtusername.Value.ToString();
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.CheckStaffLoginForUnchangedPass(user, pass);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string returnMsg = "", staffNo = "", email = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "SUCCESS")
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
        private bool ValidStaffNo(string staffNo)
        {

            bool r = false;
            try

            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.CheckValidStaffNo(staffNo);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        r = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return r;
        }


        private void UpdatePass(string Password)
        {
            try
            {
                MyComponents.ObjNav.UpdateStaffPass(txtusername.Value.ToString(), Password);
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
            string username = txtusername.Value.ToString().ToUpper();
            bool b = false;
            try
            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.CheckStaffPasswordChanged(username);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[1];
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
            string user = txtusername.Value.ToString();
            if (string.IsNullOrEmpty(user))
            {
                LblError.Text = "Please enter your PF Number!";
                txtusername.Focus();
                return;
            }
            //Check Password Change Status
            if (ValidStaffNo(user) == false)
            {
                LblError.Text = "Invalid Staff No";
                txtusername.Focus();
                return;
            }
            if (ChangedPassStatus() == false)
            {
                LblError.Text = "Warning! Your account is not active, Please use your BFL email address as your password to activate your account!";
                txtusername.Focus();
                return;
            }
            string email = GetStaffEmail(user);
            if (email.Length < 3)
            {
                LblError.Text = "Error! Please visit the HR office to update your official email.";
                return;
            }
            #region LoginSp
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.GetCurrentPassword(user);

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
                    MyComponents.SendMyEmail(email, "BFL - Web Portal Password", body);
                    LblError.Text = "Your password has been sent to: " + email.ToUpper();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            #endregion
        }
        private string GetStaffEmail(string staffNo)
        {
            string r = "";
            try
            {
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.GetStaffMail(staffNo);

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