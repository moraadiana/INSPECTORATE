using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class LeaveApplication : System.Web.UI.Page
    {
        WebPortals webportals = MyComponents.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        string[] StaffDetails = new string[3];
        string[] RelieverDetails = new string[3];
        public static string StaffName = "";
        public static string StaffUserId = "";
        string LeaveNum = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");

                }
                Fill_DropDownLeaveTypes();
                LoadLeaveLabel();
                LoadLeaveBal();
                LoadRelievers();
                LoadRelievers2();
                LoadRelievers3();
                LoadResponsibilityCenters();
                string query = Request.QueryString["query"];
                string leaveNo = null;
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                if (query == "new")
                {
                    leaveNo = null;
                    //lbtnSubmit.Visible = true;
                }
                else if (query == "old")
                {
                    leaveNo = Request.QueryString["appNo"].ToString();
                    string response = webportals.GetLeaveDetails(leaveNo);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(':');
                        string EmployeeNo = responseArr[0];
                        string EmployeeName = responseArr[1];
                        string Date = responseArr[2];
                        string AppliedDays = responseArr[3];
                        string StartingDate = responseArr[4];
                        string EndingDate = responseArr[5];
                        string Purpose = responseArr[6];
                        string LeaveType = responseArr[7];
                        string ReturnDate = responseArr[8];
                        string userId = responseArr[9];
                         string RelieverNo1 = responseArr[10];
                       string RelieverName1 = responseArr[11];
                         string resCenter = responseArr[12];
                        string RelieverNo2 = responseArr[13];
                        string RelieverName2 = responseArr[14];
                        string RelieverNo3 = responseArr[15];
                        string RelieverName3 = responseArr[16];
                        DdLeaveType.SelectedValue = LeaveType;
                        TxtAppliedDays.Text = AppliedDays;

                       TxtPurpose.Text = Purpose;
                       //lblEndDate.Text = EndingDate;
                        ddlresponibilitycentres.SelectedValue = resCenter;
                        ddlReliever.SelectedValue = RelieverNo1;
                        ddlReliever2.SelectedValue = RelieverNo2;
                        ddlReliever3.SelectedValue = RelieverNo3;
                       // txtStartDate.Text = StartingDate;
                        //lblReturnDate.Text = ReturnDate;
                        LoadLeaveBal();
                    }

                    if (approvalStatus == "Open" || approvalStatus == "Pending")
                    {
                        lbtnApply.Visible = true;
                        //lbtnSubmit.Visible = true;
                    }

                    else
                    {
                        lbtnApply.Visible = false;
                    }
                }

                //string grade = MyComponents.EmployeeGrade;
                //if (!string.IsNullOrEmpty(grade))
                //{
                //    ExceptionMsg("Your account has no Grade set up. Kindly, contact ICT Department for help.");
                //    return;
                //}
                //else
                //{
                //    if (Convert.ToInt16(grade) <= 10)
                //    {
                //        Fill_DropDownLeaveTypes();
                //    }
                //    else
                //    {
                //        Fill_DropDownLeaveTypesHigherGrade();
                //    }

                //    LoadLeaveLabel();
                //    LoadLeaveBal();
                //    LoadRelievers();
                //}
            }
         
        }
        public void Fill_DropDownLeaveTypes()
        {
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetLeaveTypes";
                    SqlCommand command = new SqlCommand();
                    command.CommandText = sqlStmt;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    command.Parameters.AddWithValue("@gender", MyComponents.EmployeeGender);
                    ListItem li = null;
                    li = new ListItem("--select--", "0");
                    this.DdLeaveType.Items.Add(li);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            DdLeaveType.DataSource = dr;
                            DdLeaveType.DataTextField = "Description";
                            DdLeaveType.DataValueField = "Code";
                            DdLeaveType.DataBind();
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception Ex)
            {
                Message("ERROR: " + Ex.Message.ToString());
                HttpContext.Current.Response.Write(Ex);
                //Ex.Data.Clear();
            }
        }

        public void Fill_DropDownLeaveTypesHigherGrade()
        {
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetLeaveTypesHigher";
                    SqlCommand cmdIntakeCode = new SqlCommand
                    {
                        CommandText = sqlStmt,
                        Connection = connToNAV,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdIntakeCode.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdIntakeCode.Parameters.AddWithValue("@gender", MyComponents.EmployeeGender);
                    using (SqlDataReader sqlReaderIntakeCode = cmdIntakeCode.ExecuteReader())
                    {
                        if (sqlReaderIntakeCode.HasRows)
                        {
                            DdLeaveType.DataSource = sqlReaderIntakeCode;
                            DdLeaveType.DataTextField = "Code";
                            DdLeaveType.DataValueField = "Code";
                            DdLeaveType.DataBind();
                        }
                    }

                    connToNAV.Close();
                }
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }
        protected void LoadRelievers()
        {
            try
            {
                this.ddlReliever.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    //sqlStmt = "spLoadRegions";spGetDepartmentList
                    sqlStmt = "spGetRelievers";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    // cmd.Parameters.AddWithValue("@Department", "'"++"'");
                    ListItem li = null;
                    li = new ListItem("--select--", "0");
                    this.ddlReliever.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString(),
                                    dr["No_"].ToString()
                                );

                                this.ddlReliever.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        
         protected void LoadRelievers2()
        {
            try
            {
                this.ddlReliever2.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    //sqlStmt = "spLoadRegions";spGetDepartmentList
                    sqlStmt = "spGetRelievers";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    // cmd.Parameters.AddWithValue("@Department", "'"++"'");
                    ListItem li = null;
                    li = new ListItem("--select--","");
                    this.ddlReliever2.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString(),
                                    dr["No_"].ToString()
                                );

                                this.ddlReliever2.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
         
        
         protected void LoadRelievers3()
        {
            try
            {
                this.ddlReliever3.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    //sqlStmt = "spLoadRegions";spGetDepartmentList
                    sqlStmt = "spGetRelievers";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    // cmd.Parameters.AddWithValue("@Department", "'"++"'");
                    ListItem li = null;
                    li = new ListItem("-----select----","");
                    this.ddlReliever3.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString(),
                                    dr["No_"].ToString()
                                );

                                this.ddlReliever3.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
         
        protected void LoadResponsibilityCenters()
        {
            try
            {
                this.ddlresponibilitycentres.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadResponsibilityCentre";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("------SELECT------", "");
                    this.ddlresponibilitycentres.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Name"].ToString(),
                                    dr["Code"].ToString()
                                );

                                this.ddlresponibilitycentres.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void LoadLeaveLabel()
        {
            string LeaveType = DdLeaveType.SelectedValue.ToString();
            //lblLeaveType.Text = LeaveType;
        }
        private void LoadLeaveBal()
        {
            try
            {
                string EmployeeNo = Session["username"].ToString();
                string LeaveType = DdLeaveType.SelectedValue.ToString();
                int Year = Convert.ToInt32(DateTime.Now.Year);
                string CurrentLeavePeriod = Year.ToString();
                //leaveMsg.Visible = false;
                //BtnApply.Visible = true;

                string availabledays = "";
               //availabledays = MyComponents.ObjNav.AvailableLeaveDays(EmployeeNo, LeaveType);
                availabledays = MyComponents.ObjNav.AvailableLeaveDayss(EmployeeNo, LeaveType, CurrentLeavePeriod);
                if (!string.IsNullOrEmpty(availabledays))
                {
                    double leavedays = Convert.ToDouble(availabledays);
                    lblLeaveBal.Text = leavedays.ToString();
                    if (leavedays <= 0)
                    {
                        lblLeaveBal.Text = "Not Available";
                        lbtnApply.Visible = false;
                        lbtnBack.Visible = true;
                        //lblSelectedLeaveType.Text = DdLeaveType.SelectedItem.Text;
                    }
                }
                else
                {
                    lblLeaveBal.Text = "Not Available";
                    lbtnApply.Visible = false;
                    lbtnBack.Visible = true;
                    //lblSelectedLeaveType.Text = DdLeaveType.SelectedItem.Text;
                }

            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }

        }

        public static string spGetLeaveReqNo()
        {
            //string finalreturned = "";
            string newAppNo = "";

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    SqlCommand cmds = new SqlCommand();
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.Connection = connToNAV;
                    cmds.CommandText = "spGetLeaveReqNo";
                    cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    using (SqlDataReader rdr = cmds.ExecuteReader())
                    {
                        if (rdr.HasRows == true)
                        {
                            rdr.Read();
                            newAppNo = rdr["No_"].ToString();
                        }
                    }
                    connToNAV.Close();
                }
                //UpdateApplicationNo(newAppNo, conn);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return newAppNo;
        }
        private void DefaultDays(string leavetype)
        {
            try
            {
                using (SqlConnection conn = MyComponents.getconnToNAV())
                {
                    //select Student Stages

                    string sqlStmt2 = null;
                    sqlStmt2 = "spMyLoaveDefault";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt2;
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@LeaveType", "'" + leavetype + "'");
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            lblLeaveBal.Text = Convert.ToInt32(dr["Days"]).ToString();
                            lbtnApply.Visible = true;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }
        public void Message1(string strMsg)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }
        public void Message(string strMsg)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
            dvMdlContentFail.Visible = true;
            dvMdlContentPass.Visible = false;
        }

        private void SuccessMessage(string message)
        {
            string page = "LeaveListsing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        public void ExceptionMsg(string Msg)
        {
            lbtnApply.Visible = false;
            Message(Msg);
        }
        protected bool HasPendingApplications()
        {
            bool b = false;

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetPendingApplications_Leave";
                    SqlCommand cmdPendingApplications = new SqlCommand
                    {
                        CommandText = sqlStmt,
                        Connection = connToNAV,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdPendingApplications.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdPendingApplications.Parameters.AddWithValue("@EmployeeNo", "'" + Session["username"] + "'");
                    using (SqlDataReader sqlReaderPendingApplications = cmdPendingApplications.ExecuteReader())
                    {
                        if (sqlReaderPendingApplications.HasRows)
                        {
                            b = true;
                        }
                    }

                    connToNAV.Close();
                }
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }

            return b;
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //txtStartDate_TextChanged(null,null);

                #region HasPendingApplications?

                bool HasPendingApplications_ = HasPendingApplications();

                if (HasPendingApplications_)
                {
                    Message1("You have a Pending Leave Application. Please Cancel/Wait for it to be Approved and Try Again.");
                    //dvMdlContentFail.Visible = true;
                    //ScriptManager.RegisterStartupScript(this, GetType(), "msg", "$(function(){ $('.msg-modal').modal('show'); })", true);
                    return;
                }
                if (CheckLeaveStatus())
                {
                    Message1("Sorry, You already have a leave requisition that is pending approval");

                }
                //Validate Leave days
                var appliedDays = TxtAppliedDays.Text.ToString();
                if (lblLeaveBal.Text != "Sorry, You have exhausted your leave days. You may select a different leave Type")
                {
                    if (Convert.ToInt32(appliedDays) > Convert.ToInt32(lblLeaveBal.Text))
                    {
                        Message1("Sorry, Applied days cannot be more than the available leave days");
                        TxtAppliedDays.Focus();
                        return;
                    }
                }
                else
                {
                    Message1("Sorry, Your Leave balance is exhausted, Consult your system administrator");
                }
                #endregion 
                #region Validation

                DateTime startingDate;

                string Reliever = "", RelieverName = "";
                string Reliever2 = "", RelieverName2 = "";
                string Reliever3 = "", RelieverName3 = "";

                if (string.IsNullOrEmpty(ddlReliever.SelectedValue))
                {
                    Message1("Please enter a Reliever.");
                    ddlReliever.Focus();
                    return;
                }
                else
                {
                    Reliever = ddlReliever.SelectedValue;
                    RelieverName = ddlReliever.SelectedItem.Text;
                    Reliever2 = ddlReliever2.SelectedValue;
                    RelieverName2 = ddlReliever2.SelectedItem.Text;
                    Reliever3 = ddlReliever3.SelectedValue;
                    RelieverName3 = ddlReliever3.SelectedItem.Text;
                }
                string rCentre = ddlresponibilitycentres.SelectedValue.ToString();
                if(string.IsNullOrEmpty(rCentre) )
                {
                    Message1("Warning! You must select responsibility centre.");
                    ddlresponibilitycentres.Focus();
                    return;
                }
                if (Convert.ToInt32(appliedDays) > Convert.ToInt32(lblLeaveBal.Text))
                {
                    Message1("Days applied cannot be more than the leave balance.");
                    TxtAppliedDays.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(appliedDays))
                {
                    Message1("Please enter the applied days.");
                    TxtAppliedDays.Focus();
                    return;
                }
                if (!MyComponents.IsNumeric(appliedDays))
                {
                    Message1("Applied days accepts numeric numbers only.");
                    TxtAppliedDays.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    Message1("Please select the start date.");
                    //dtPicker.Focus(); 
                    return;
                }
                else
                {
                    startingDate = DateTime.ParseExact(txtStartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(lblEndDate.Text))
                {
                    Message1("You cannot continue without End Date.");
                    //dtPicker.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(lblReturnDate.Text)) 
                {
                    Message1("You cannot Continue without Return date.");
                    //dtPicker.Focus();
                    return;
                }

                #endregion

                #region Convert.ToDateTime

                var endDate = DateTime.ParseExact(lblEndDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var newEndDate = "";
                newEndDate = endDate.ToString("yyyy-MM-dd");

                var returndate = DateTime.ParseExact(lblReturnDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
                var newReturnDate = "";
                newReturnDate = returndate.ToString("yyyy-MM-dd");
                #endregion
                try
                {
                    if (!FileUpload1.HasFiles)
                    {
                        Message("Warning! You must attach the documents!");
                        return;
                    }
                    string DocumentNo = spGetLeaveReqNo();
                    string user = Session["username"].ToString();
                    DateTime AttachDT = DateTime.Now;
                    //string filext = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    //string attchby = "";
                    //int identity=0;
                    //string ftype = "";
                    int tblId = 61125;
                    string fileName = Path.Combine(Server.MapPath("~/ImprestDocs/") + FileUpload1.FileName);
                    string filext = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    string ftype = "";
                    if (filext == "pdf")
                    {
                        ftype = "2";
                    }
                    if (filext == "jpg")
                    {
                        ftype = "1";
                    }
                    if (filext == "xlsx")
                    {
                        ftype = "4";
                    }
                    if (filext == "docx")
                    {
                        ftype = "3";
                    }
                    string DoCfilename = Path.GetFileName(FileUpload1.PostedFile.FileName.TrimEnd('.', 'p', 'd', 'f')).Replace(" ", "_");
                    FileUpload1.SaveAs(fileName);
                    //fileName.TrimEnd('.','p','d','f');
                    //TrimEnd();
                    MyComponents.ObjNav.SaveMemoAttchmnts(DocumentNo, tblId, ftype, filext, AttachDT, fileName, fileName, user);
                    //foreach (GridViewRow gvr in this.gvLines.Rows) ;
                }
                catch (Exception Ex)
                {
                    Message("ERROR: " + Ex.Message.ToString());
                    Ex.Data.Clear();
                }
                #region SendforApproval
                string LeaveNo = null;
                string query = Request.QueryString["query"];
                if (query == "old")
                {
                    LeaveNo = Request.QueryString["appNo"]; // Get the leaveNo from the query string
                }

                /*  try
                  {
                      MyComponents.ObjNav.HRLeaveApplication(Session["username"].ToString(), DdLeaveType.SelectedValue, Convert.ToDecimal(appliedDays), Convert.ToDateTime(startingDate), Convert.ToDateTime(newEndDate), Convert.ToDateTime(newReturnDate), TxtPurpose.Text, Reliever, RelieverName, rCentre, Reliever2, RelieverName2, Reliever3, RelieverName3);
                  }
                  catch (Exception exception)
                  {
                      Message("ERROR: " + exception.Message.ToString());
                      exception.Data.Clear();
                  }*/
                #endregion
               
               
                string response = MyComponents.ObjNav.HRLeaveApplication(LeaveNo ?? string.Empty,Session["username"].ToString(), DdLeaveType.SelectedValue, Convert.ToDecimal(appliedDays), Convert.ToDateTime(startingDate), Convert.ToDateTime(newEndDate), Convert.ToDateTime(newReturnDate), TxtPurpose.Text, Reliever, RelieverName, rCentre, Reliever2, RelieverName2, Reliever3, RelieverName3);
                //= webportals.HRMLeaveApplication1(LeaveNo ?? string.Empty, username, reliever, leaveType, Convert.ToDecimal(appliedDays), Convert.ToDateTime(startDate), endDate, returnDate, purpose, resCenter);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string leaveNo = responseArr[1];
                        SuccessMessage("Leave application has been sent for approval successfully.");
                        return;
                       /*
                              // TODO: Sent email to reliever
  string relieverEmail = GetRelieverEmail(reliever);
                              string subject = "NCIA Leave Relieval Request";
                              string body = $"Hello {ddlReliver.SelectedItem}" +
                                  $"<br/><br/>" +
                                  $"You have been requested by {Session["staffName"]} to be a reliever." +
                                  $"<br/><br/>" +
                                  $"Do not reply to this email.";
                              Components.SentEmailAlerts(relieverEmail, subject, body);
                              SuccessMessage("Leave application has been sent for approval successfully.");
                              return;
                          }
                          else
                          {
                              Message(approval);
                              return;
                          }*/
                    }
                    else
                    {
                        Message("An error occured while applying for the leave. Please try again later!");
                        return;
                    }
                }

                Session["Success"] = "1"; //0-fail,1-pass,2-error_msg
                Response.Redirect("LeaveListsing.aspx");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                dvMdlContentFail.Visible = false;
                dvMdlContentPass.Visible = true;
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
            //Response.Redirect("LeaveListing.aspx");
        }
        private bool CheckLeaveStatus()
        {
            bool b = false;
            string LeaveType = DdLeaveType.SelectedValue;
            try
            {
                string staffNo = Session["username"].ToString();
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spCheckLeaveStatus";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@StaffNo", "'" + staffNo + "'");
                    cmd.Parameters.AddWithValue("@Type", "'" + LeaveType + "'");

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        if (dr.HasRows)
                        {
                            dr.Read();
                            b = true;
                        }
                    }

                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return b;
        }
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("LeaveListsing.aspx");
        }
        protected void DdLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLeaveBal();
        }

        protected void txtStartDate_TextChanged1(object sender, EventArgs e)
        {
            try
            {
                string startDate = txtStartDate.Text;
                string appliedDays = TxtAppliedDays.Text;
                string leaveType = DdLeaveType.SelectedValue.ToString();
                DateTime applicationDate = DateTime.Now;

                // Ensure applied days is not empty
                if (string.IsNullOrEmpty(appliedDays))
                {
                    Message("Applied days cannot be empty.");
                    TxtAppliedDays.Focus();
                    return;
                }

                // Validate and parse start date
                if (!DateTime.TryParse(startDate, out DateTime startingDate))
                {
                    Message("Invalid date format. Please enter a valid start date.");
                    txtStartDate.Text = string.Empty;
                    return;
                }

                // Check if the start date is at least 3 days from the application date
                if ((startingDate - applicationDate).TotalDays < 3)
                {
                    Message("Start date must be at least 3 days from the application date.");
                    txtStartDate.Text = string.Empty;
                    return;
                }

                // Validate start date using webportals
                MyComponents.ObjNav.ValidateStartDate(startingDate);

                // Calculate end date and return date
                var endDate = MyComponents.ObjNav.CalcEndDate(startingDate, Convert.ToInt32(appliedDays), leaveType).ToString("yyyy-MM-dd");
                var returnDate = MyComponents.ObjNav.CalcReturnDate(Convert.ToDateTime(endDate), leaveType).ToString("yyyy-MM-dd");

                // Update labels
                lblEndDate.Text = endDate;
                lblReturnDate.Text = returnDate;
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                txtStartDate.Text = string.Empty;
                lblEndDate.Text = string.Empty;
                lblReturnDate.Text = string.Empty;
            }
        }
        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            var appliedDays = TxtAppliedDays.Text.ToString();
            string HasStartDate = txtStartDate.Text;

            if (String.IsNullOrEmpty(HasStartDate)) { return; }
            try
            {
                MyComponents.ObjNav.ValidateStartDate(Convert.ToDateTime(txtStartDate.Text)); //dtPicker.SelectedDate
            }
            catch (Exception exception)
            {
                Message(exception.Message);
                exception.Data.Clear();
                return;
            }
            if (string.IsNullOrEmpty(appliedDays))
            {
                Message("Please enter the applied days");
                TxtAppliedDays.Focus();
                return;

            }
            if (!MyComponents.IsNumeric(appliedDays))
            {

                Message("Applied days accepts numeric numbers only");
                TxtAppliedDays.Focus();
                return;
            }

            var endDate = "";

            try
            {
                endDate =
                    MyComponents.ObjNav.CalcEndDate(Convert.ToDateTime(txtStartDate.Text), Convert.ToInt16(appliedDays), DdLeaveType.SelectedValue)
                        .ToString("d");
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
            var returndate = string.Empty;
            try
            {
                returndate =
                    MyComponents.ObjNav.CalcReturnDate(Convert.ToDateTime(endDate), DdLeaveType.SelectedValue).ToString("d");
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
            lblEndDate.Text = Convert.ToDateTime(endDate).ToString("dd-MM-yyyy");
            lblReturnDate.Text = Convert.ToDateTime(returndate).ToString("dd-MM-yyyy");
            ScriptManager.RegisterStartupScript(this, GetType(), "startDate", "$(function () { $('.leavestartdate').datetimepicker('update', '" + txtStartDate.Text + "'); })", true);
        }
    }
}