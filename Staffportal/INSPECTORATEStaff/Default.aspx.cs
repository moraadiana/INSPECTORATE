using INSPECTORATEStaff.NAVWS;
using OpenQA.Selenium;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using Windows.UI.Core;

namespace INSPECTORATEStaff
{
    public partial class _Default : Page
    {
       
        string[] strLimiters = new string[] { "::" };
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
        private void LoginForUnchangedPass()
        {
            try
            {
                string pass = txtpassword.Value.ToString();
                string user = txtusername.Value.ToString();
                string response = MyComponents.ObjNav.LoginForUnchangedPassword(user);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        string staffEmail = responseArr[2];
                        Response.Redirect($"ResetPassword.aspx?staffNo={staffNo}&email={staffEmail}");
                    }
                    else
                    {
                        LblError.Text = returnMsg;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void LoginForUnchangedPass1()
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
            try
            {
                string username = txtusername.Value.ToString();
                if (string.IsNullOrEmpty(username))
                {
                    LblError.Text = "Kindly input Staff Number";
                    txtusername.Focus();
                    return;
                }

                if (!ValidStaffNo(username))
                {
                    LblError.Text = "Invalid Staff No";
                    return;
                }
                string newPassword = GenerateRandomPassword(10);
                string response = MyComponents.ObjNav.UpdateStaffAutoGenPassword(username, newPassword);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response != "SUCCESS")
                    {
                        LblError.Text = "Failed to reset the password. Please try again.";
                        return;
                    }

                }
                string email = GetStaffEmail(username);
                //string staffPassword = GetStaffPassword(username);
                string subject = "Inspectorate Staff Portal Password Reset";
                string body = $"Use this password to log into Inspectorate Staff portal .<br/> <br/>Auto generated Portal password: <strong>{newPassword}</strong> <br/> <br/>Do not reply to this email.";
                MyComponents.SendMyEmail(email, subject, body);
                lblSuccess.Text = $"Auto generated password has been sent to your email address {email}";
                return;


              
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void lbtnForgot_Click2(object sender, EventArgs e)
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
                    MyComponents.SendMyEmail(email, "Inspectorate - Web Portal Password", body);
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