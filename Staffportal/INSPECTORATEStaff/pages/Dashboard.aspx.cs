using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                GetStaffDetailsService(Session["username"].ToString());
                GetProfilePicFromNav(Session["username"].ToString());
            }
        }
        public void GetProfilePicFromNav(string username)
        {
            string ProfilePicBase64 = "", profilePic = "";
            try
            {
                ProfilePicBase64 = MyComponents.ObjNav.GetProfilePicture(username);
                ImgProfilePic.ImageUrl = "data:image/png;base64," + ProfilePicBase64;
                if (ProfilePicBase64 == "")
                {
                    ImgProfilePic.Visible = false;
                    ImgProfileDefault.Visible = true;
                    if (lblGender.Text == "Male")
                    {
                        profilePic = "profile_m";
                    }
                    else
                    {
                        profilePic = "profile_f";
                    }
                    ImgProfileDefault.ImageUrl = "~/images/" + profilePic + ".png";
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        public string TotalLeave()
        {
            string username = MyComponents.UserID;
            var totalDeposits = string.Empty;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    //isnull(Sum([Amount]),0)
                    string s = null;
                    s = "spGetMyApprovals_LeaveTotal";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = s;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    command.Parameters.AddWithValue("@Supervisor", "'" + username + "'");
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                totalDeposits = dr["TotalReqs"].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                e.Data.Clear();
            }
            return totalDeposits;
        }
        public string TotalImprest()
        {
            string username = MyComponents.UserID;
            var totalDeposits = string.Empty;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    //isnull(Sum([Amount]),0)
                    string s = null;
                    s = "spGetMyApprovals_ImprestTotal";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = s;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    command.Parameters.AddWithValue("@Supervisor", "'" + username + "'");
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                totalDeposits = dr["TotalReqs"].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e);
                //e.Data.Clear();
            }
            return totalDeposits;
        }
        public string TotalTransport()
        {
            string username = MyComponents.UserID;
            var totalDeposits = string.Empty;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    //isnull(Sum([Amount]),0)
                    string s = null;
                    s = "spGetMyApprovals_TransportTotal";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = s;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    command.Parameters.AddWithValue("@Supervisor", "'" + username + "'");
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                totalDeposits = dr["TotalReqs"].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                e.Data.Clear();
            }
            return totalDeposits;
        }
        public string TotalStores()
        {
            string username = MyComponents.UserID;
            var totalDeposits = string.Empty;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    //isnull(Sum([Amount]),0)
                    string s = null;
                    s = "spGetMyApprovals_StoresTotal";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = s;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    command.Parameters.AddWithValue("@Supervisor", "'" + username + "'");
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                totalDeposits = dr["TotalReqs"].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                e.Data.Clear();
            }
            return totalDeposits;
        }
        protected void GetStaffDetailsService(string username)
        {
            try
            {
                string[] strdelimiters = new string[] { "::" };
                string staffLoginInfo = MyComponents.ObjNav.GetStaffProfileDetails(username);

                //returnMsg::changedPassword::staffNo::staffUserID::staffName
                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    string idNum = "", Citizen = "", adddss = "", JobTitle = "", email = "", Tttle = "", DoB = "", gennder = "", phneNo = "", title = "";

                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    idNum = staffLoginInfo_arr[0];
                    Citizen = staffLoginInfo_arr[1];
                    adddss = staffLoginInfo_arr[2];
                    JobTitle = staffLoginInfo_arr[3];
                    email = staffLoginInfo_arr[4];
                    Tttle = staffLoginInfo_arr[5];
                    DoB = staffLoginInfo_arr[6];
                    gennder = staffLoginInfo_arr[7];
                    phneNo = staffLoginInfo_arr[8];
                    title = staffLoginInfo_arr[9];


                    LblStaffName.Text = Tttle.ToString() + " " + Session["StaffName"].ToString();
                    LblEmployeeNo.Text = Session["username"].ToString();
                    //LblDesignation.Text = sqlReaderDetails["Job Title"].ToString();
                    LblIDNo.Text = idNum.ToString();
                    LblCitizenship.Text = Citizen.ToString();
                    LblPostalAddress.Text = adddss.ToString();
                    LblDesignation.Text = JobTitle.ToString();
                    lblEmail.Text = email.ToString();
                    lblPhoneNo.Text = phneNo.ToString();
                    lblGender.Text = gennder.ToString();
                    LblTitle.Text = title.ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        //private void LoadStaffDetails(string username)
        //{

        //    //stored procedure spSelectApplicationFormQualification
        //    using (SqlConnection connToNAV = MyComponents.getconnToNAV())
        //    {
        //        string sqlStmt = null;
        //        sqlStmt = "spGetStaffDetails";
        //        SqlCommand cmdGetStaffDetails = new SqlCommand();
        //        cmdGetStaffDetails.CommandText = sqlStmt;
        //        cmdGetStaffDetails.Connection = connToNAV;
        //        cmdGetStaffDetails.CommandType = CommandType.StoredProcedure;
        //        cmdGetStaffDetails.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
        //        cmdGetStaffDetails.Parameters.AddWithValue("@username", "'" + username.ToUpper() + "'");

        //        using (SqlDataReader sqlReaderDetails = cmdGetStaffDetails.ExecuteReader())
        //        {
        //            if (sqlReaderDetails.HasRows)
        //            {
        //                sqlReaderDetails.Read();
        //                LblStaffName.Text = Session["StaffName"].ToString();
        //                LblEmployeeNo.Text = Session["username"].ToString();
        //                //LblDesignation.Text = sqlReaderDetails["Job Title"].ToString();
        //                LblIDNo.Text = sqlReaderDetails["ID Number"].ToString();
        //                LblCitizenship.Text = sqlReaderDetails["Citizenship"].ToString();
        //                LblPostalAddress.Text = sqlReaderDetails["Postal Address"].ToString();
        //                LblDesignation.Text = sqlReaderDetails["Job Title"].ToString();
        //                lblEmail.Text = sqlReaderDetails["Company E-Mail"].ToString();
        //                lblPhoneNo.Text = sqlReaderDetails["Cellular Phone Number"].ToString();
        //                string gender = sqlReaderDetails["Gender"].ToString();
        //                if (gender == "1")
        //                {
        //                    lblGender.Text = "Male";
        //                }
        //                else if (gender == "2")
        //                {
        //                    lblGender.Text = "Female";
        //                }
        //                string title = sqlReaderDetails["Title"].ToString();
        //                if (title == "0")
        //                {
        //                    LblTitle.Text = "Mr. ";
        //                }
        //                else if (title == "1")
        //                {
        //                    LblTitle.Text = "Mrs. ";
        //                }
        //                else if (title == "2")
        //                {
        //                    LblTitle.Text = "Miss. ";
        //                }
        //                else if (title == "3")
        //                {
        //                    LblTitle.Text = "Ms. ";
        //                }
        //                else if (title == "4")
        //                {
        //                    LblTitle.Text = "Dr. ";
        //                }
        //                else if (title == "5")
        //                {
        //                    LblTitle.Text = "Eng.";
        //                }
        //                else if (title == "6")
        //                {
        //                    LblTitle.Text = "Cc. ";
        //                }
        //                else if (title == "7")
        //                {
        //                    LblTitle.Text = "Prof. ";
        //                }
        //                else if (title == "8")
        //                {
        //                    LblTitle.Text = "Rev. ";
        //                }
        //            }
        //        }
        //        connToNAV.Close();
        //    }
        //}
    }
}