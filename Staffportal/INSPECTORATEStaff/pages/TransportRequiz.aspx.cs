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
    public partial class TransportRequiz : System.Web.UI.Page
    {
        string LeaveNum = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                string paged = Request.QueryString["Tp"].ToString();
                if (paged == "old")
                {
                    Session["ReqNo"] = Request.QueryString["nn"].ToString();
                    Fill_TransportGroups();
                    MultiView1.SetActiveView(vwDone);
                    if (HasPendingApplications() == 0)
                    {
                        lbtnApproval.Visible = true;
                        lbtnCancel.Visible = false;
                        lbtnCAll.Visible = true;
                    }
                    if (HasPendingApplications() == 1)
                    {
                        lbtnApproval.Visible = false;
                        lbtnCancel.Visible = true;
                        lbtnCAll.Visible = true;
                    }
                    if (HasPendingApplications() > 1)
                    {
                        lbtnApproval.Visible = false;
                        lbtnCancel.Visible = false;
                        lbtnCAll.Visible = false;
                    }
                }
                else
                {
                    Fill_TransportGroups();
                    MultiView1.SetActiveView(vwApply);
                    //btncancel.Visible = false;
                    //lbnAddLine.Visible = true;
                }
            }
        }
        protected int HasPendingApplications()
        {
            int? b = null;
            string paged = Request.QueryString["nn"].ToString();
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetSentForApprovals_Transport";
                    SqlCommand cmdPendingApplications = new SqlCommand
                    {
                        CommandText = sqlStmt,
                        Connection = connToNAV,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdPendingApplications.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdPendingApplications.Parameters.AddWithValue("@DocumentNo", "'" + paged + "'");
                    using (SqlDataReader sqlReaderPendingApplications = cmdPendingApplications.ExecuteReader())
                    {
                        if (sqlReaderPendingApplications.HasRows)
                        {
                            sqlReaderPendingApplications.Read();
                            b = Convert.ToInt32(sqlReaderPendingApplications["Status"]);
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }

            return Convert.ToInt32(b);
        }
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            //if  Session["ReqNo"]
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    string L_ = "SELECT *,(CASE [Status] WHEN 0 THEN 'Open' WHEN 1 THEN 'Pending Approval' " +
                                "WHEN 2 THEN 'Approved' WHEN 3 THEN 'Transport' WHEN 4 THEN 'Closed' END)[Status Description] " +
                                "FROM [" + MyComponents.Company_Name + "$FLT-Transport Requisition] " +
                                "WHERE ([Emp No]=@Employee_No) AND ([Transport Requisition No]=@TranNo) order by [Transport Requisition No] DESC";
                    //Open,Pending Approval,Approved,Cancelled
                    var cmd = new SqlCommand(L_, conn);
                    cmd.Parameters.AddWithValue("@Employee_No", Session["username"]);
                    cmd.Parameters.AddWithValue("@TranNo", Session["ReqNo"]);
                    using (SqlDataReader drL = cmd.ExecuteReader())
                    {
                        if (drL.HasRows)
                        {
                            while (drL.Read())
                            {
                                //Open,Released,Pending Approval,Pending Prepayment,Cancelled,Posted
                                //cSite.ExternalUserID, txtDestination.Text.Trim(), txtCommence.Text.Trim(), dtPicker.SelectedDate, txtPurpose.Text, Convert.ToInt16(txtNoDays.Text), Convert.ToInt16(txtNoPassengers.Text)
                                //Convert.ToInt16(ddRequest.SelectedValue),Convert.ToInt16(ddTravelType.SelectedValue)
                                var statusCls = "default";
                                string status = drL["Status Description"].ToString();
                                string mode = "v"; //view
                                switch (status)
                                {
                                    case "Open":
                                        statusCls = "warning";
                                        mode = "e"; //edit
                                        break;
                                    case "Pending Approval":
                                        statusCls = "primary";
                                        break;
                                    case "Approved":
                                        statusCls = "success";
                                        break;
                                    case "Transport":
                                        statusCls = "info";
                                        break;
                                    case "Closed":
                                        statusCls = "danger";
                                        break;
                                }
                                AES2 AES = new AES2();
                                htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td><a href='#' class='text-success'>{0}</a></td>
                                                            <td>{1}</td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                            <td>{6}</td>
                                                            <td>{7}</td>
                                                            <td><span class='label label-{9}'>{8}</span></td>
                                                     </tr>",
                                    drL["Transport Requisition No"],
                                    drL["Commencement"],
                                    drL["Destination"],
                                    drL["Vehicle Allocated"],
                                    drL["Driver Allocated"],
                                    Convert.ToDateTime(drL["Date of Request"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Date of Trip"]).ToShortDateString(),
                                    drL["No of Days Requested"],
                                    //drL["No of Passengers"],
                                    drL["Status Description"],
                                    statusCls,
                                    drL["Status"],
                                    mode,
                                    AES.Encrypt(drL["Transport Requisition No"].ToString())
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
            return htmlStr;
        }
        public void Fill_TransportGroups()
        {
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetTransportGroups";
                    SqlCommand cmdIntakeCode = new SqlCommand
                    {
                        CommandText = sqlStmt,
                        Connection = connToNAV,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdIntakeCode.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    using (SqlDataReader sqlReaderIntakeCode = cmdIntakeCode.ExecuteReader())
                    {
                        if (sqlReaderIntakeCode.HasRows)
                        {
                            ddlGroup.DataSource = sqlReaderIntakeCode;
                            ddlGroup.DataTextField = "code";
                            ddlGroup.DataValueField = "code";
                            ddlGroup.DataBind();
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

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["StaffName"].ToString().Replace("'", "");
                string NoOfDays = txtDays.Text.ToString();
                if (NoOfDays == null)
                {
                    Message("Warning! Number of days required cannot be bull.");
                    txtDays.Focus();
                    return;
                }
                string travelNature = ddlNature.SelectedValue.ToString();
                string group = ddlGroup.SelectedValue.ToString();
                string From = txtFrom.Text.ToString();
                if (From == null)
                {
                    Message("Warning! Departure place cannot be bull.");
                    txtFrom.Focus();
                    return;
                }
                string description = txtDestination.Text.ToString();
                if (description == null)
                {
                    Message("Warning! Destination cannot be bull.");
                    txtDestination.Focus();
                    return;
                }
                string dateofTrip = txtDateofTrip.Text.ToString();
                if (dateofTrip == null)
                {
                    Message("Warning! Destination cannot be bull.");
                    txtDateofTrip.Focus();
                    return;
                }
                string passangers = txtPassa.Text.ToString();
                if (passangers == null)
                {
                    Message("Warning! Number of passangers cannot be bull.");
                    txtPassa.Focus();
                    return;
                }
                string userID = MyComponents.UserID;
                if (userID.Length < 3)
                {
                    Message("Your userID is not setup in the system. Please visit Administrator.");
                    return;
                }
                string Purpose = TxtPurpose.Text.ToString().Replace("'", "").Trim();
                if (Purpose.Length > 200)
                {
                    Message("Purpose cannot have more than 200 characters");
                    TxtPurpose.Focus();
                    return;
                }
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    LeaveNum = GetTransNo(connToNAV);
                    connToNAV.Close();
                }
                //sp save requisition header
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spInsertTransportRequisition";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@TransNo", "'" + LeaveNum + "'");
                    cmd.Parameters.AddWithValue("@From", "'" + From + "'");
                    cmd.Parameters.AddWithValue("@Destination", "'" + description + "'");
                    cmd.Parameters.AddWithValue("@StaffUserId", "'" + userID + "'");
                    cmd.Parameters.AddWithValue("@DateOfTrip", "'" + dateofTrip + "'");
                    cmd.Parameters.AddWithValue("@Reason", "'" + Purpose + "'");
                    cmd.Parameters.AddWithValue("@DaysReq", "'" + NoOfDays + "'");
                    cmd.Parameters.AddWithValue("@NatureOfTrip", "'" + travelNature + "'");
                    cmd.Parameters.AddWithValue("@Group", "'" + group + "'");
                    cmd.Parameters.AddWithValue("@Staffname", "'" + staffName + "'");
                    cmd.Parameters.AddWithValue("@Passangers", "'" + passangers + "'");
                    cmd.Parameters.AddWithValue("@Username", "'" + username + "'");

                    cmd.ExecuteNonQuery();
                    UpdateApplicationNo(LeaveNum, connToNAV);
                    Session["ReqNo"] = LeaveNum;
                    connToNAV.Close();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                    //Message("Transport Requisition Number " + LeaveNum + " created successfully");
                    MultiView1.SetActiveView(vwDone);
                    //newView();
                    lbtnApproval.Visible = true;
                    lbtnCancel.Visible = false;
                    lbtnCAll.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Message("ERROR: " + Ex.Message.ToString());
                Ex.Data.Clear();
            }
        }
        public static string GetTransNo(SqlConnection conn)
        {
            //string finalreturned = "";
            string newAppNo = "";
            Int32 LastNumUsed = 0;
            try
            {
                SqlCommand cmds = new SqlCommand();
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection = conn;
                cmds.CommandText = "spGetTransportNo";
                cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                using (SqlDataReader rdr = cmds.ExecuteReader())
                {
                    if (rdr.HasRows == true)
                    {
                        rdr.Read();
                        newAppNo = rdr["AppNo"].ToString();
                    }
                }
                UpdateApplicationNo(newAppNo, conn);
                conn.Close();
                //Call update function
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return newAppNo;
        }
        public static void UpdateApplicationNo(string LastNoUsed, SqlConnection conn)
        {
            try
            {
                SqlCommand cmds = new SqlCommand();
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection = conn;
                cmds.CommandText = "spUpdateTransportNo";
                cmds.Parameters.AddWithValue("@LastNoUsed", "'" + LastNoUsed + "'");
                cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmds.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }
        private void Message(string p)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + p + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }

        protected void lbtnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                MyComponents.ObjNav.TransportRequisitionApprovalRequest(Session["ReqNo"].ToString());
                //InsertLeaveApprovalEntries(MyComponents.UserID, Session["ReqNo"].ToString());
                Response.Redirect("TransportListing.aspx");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                dvMdlContentFail.Visible = false;
                dvMdlContentPass.Visible = true;
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            string Numm = Session["ReqNo"].ToString();
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string stsmnts = null;
                    SqlCommand cmd = new SqlCommand();
                    stsmnts = "spDeleteImprestApproval";
                    cmd.Connection = connToNAV;
                    cmd.CommandText = stsmnts;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ImpNo", "'" + Numm + "'");
                    int deleted = cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                    //Message("Approval request successfully cancelled!");
                    UpdateApproval(connToNAV, Numm, "0");
                    lbtnApproval.Visible = true;
                    lbtnCancel.Visible = false;
                    lbtnCAll.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void UpdateApproval(SqlConnection conn, string num, string status)
        {
            try
            {
                string stsmnts = null;
                SqlCommand cmd = new SqlCommand();
                stsmnts = "spUpdateTransportApproval";
                cmd.CommandText = stsmnts;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@DocNo", "'" + num + "'");
                cmd.Parameters.AddWithValue("@Status", "'" + status + "'");
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void lbtnCAll_Click(object sender, EventArgs e)
        {
            string Numm = Session["ReqNo"].ToString();
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string stsmnts = null;
                    SqlCommand cmd = new SqlCommand();
                    stsmnts = "spCancelTransportReq";
                    cmd.Connection = connToNAV;
                    cmd.CommandText = stsmnts;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@DocNo", "'" + Numm + "'");
                    int deleted = cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                    //Message("Approval request successfully cancelled!");
                    lbtnApproval.Visible = false;
                    lbtnCancel.Visible = false;
                    lbtnCAll.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}