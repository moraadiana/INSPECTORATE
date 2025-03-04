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
    public partial class VenueBooking : System.Web.UI.Page
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
                    Fill_Departments();
                    Fill_Venues();
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
                    Fill_Departments();
                    Fill_Venues();
                    MultiView1.SetActiveView(vwApply);
                    lbluserId.Text = MyComponents.UserID;
                    lblstatus.Text = "New";
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
                    sqlStmt = "spGetSentForApprovals_Venue";
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
            string userID = MyComponents.UserID;
            //if  Session["ReqNo"]
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    string L_ = "SELECT *,(CASE [Status] WHEN 0 THEN 'New' WHEN 1 THEN 'Pending Approval' " +
                                "WHEN 2 THEN 'Approved' WHEN 3 THEN 'Cancelled' WHEN 4 THEN 'Rejected' END)[Status Description] " +
                                "FROM [" + MyComponents.Company_Name + "$Gen-Venue Booking] " +
                                "WHERE ([Requested By]=@Employee_No) AND ([Booking Id]=@TranNo) order by [Booking Id] DESC";
                    //Open,Pending Approval,Approved,Cancelled
                    var cmd = new SqlCommand(L_, conn);
                    cmd.Parameters.AddWithValue("@Employee_No", userID);
                    cmd.Parameters.AddWithValue("@TranNo", Session["ReqNo"]);
                    using (SqlDataReader drL = cmd.ExecuteReader())
                    {
                        if (drL.HasRows)
                        {
                            while (drL.Read())
                            {
                                var statusCls = "default";
                                string status = drL["Status Description"].ToString();
                                string mode = "v"; //view
                                switch (status)
                                {
                                    case "New":
                                        statusCls = "warning";
                                        mode = "e"; //edit
                                        break;
                                    case "Pending Approval":
                                        statusCls = "primary";
                                        break;
                                    case "Approved":
                                        statusCls = "success";
                                        break;
                                    case "Cancelled":
                                        statusCls = "info";
                                        break;
                                    case "Rejected":
                                        statusCls = "danger";
                                        break;
                                }
                                AES2 AES = new AES2();
                                htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td><a href='VenueBooking.aspx?Tp=old&nn={0}' class='text-success'>{0}</a></td>
                                                            <td>{1}</td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                            <td>{6}</td>
                                                            <td><span class='label label-{8}'>{7}</span></td>
                                                     </tr>",
                                    drL["Booking Id"],
                                    drL["Meeting Description"],
                                    drL["Venue"],
                                    Convert.ToDateTime(drL["Request Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Booking Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Required Time"]).ToShortTimeString(),
                                    drL["Pax"],
                                    //drL["No of Passengers"],
                                    drL["Status Description"],
                                    statusCls,
                                    drL["Status"],
                                    mode,
                                    AES.Encrypt(drL["Booking Id"].ToString())
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
        public void Fill_Departments()
        {
            try
            {
                this.ddlDeparts.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    //sqlStmt = "spLoadRegions";spGetDepartmentList
                    sqlStmt = "spGetDepartmentList";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--select--", "0");
                    this.ddlDeparts.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlDeparts.Items.Add(li);
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
        public void Fill_Venues()
        {
            try
            {
                this.ddlVenue.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    //sqlStmt = "spLoadRegions";spGetDepartmentList
                    sqlStmt = "spGetVenueList";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--select--", "0");
                    this.ddlVenue.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Description"].ToString().ToUpper(),
                                    dr["Venue Code"].ToString()
                                );

                                this.ddlVenue.Items.Add(li);
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
        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //Collect data
                //string RqNum = lblNo.Text;
                string CntcNme = Session["StaffName"].ToString();
                string deptsName = ddlDeparts.SelectedItem.Text.Replace("'", "");
                string depts = ddlDeparts.SelectedValue.ToString();
                if (depts == "0")
                {
                    Message("Warning! Please select department.");
                    ddlDeparts.Focus();
                    return;
                }
                string Venue = ddlVenue.SelectedValue;
                if (string.IsNullOrEmpty(Venue))
                {
                    Message("Warning! Please select venue.");
                    ddlVenue.Focus();
                    return;
                }
                int Pax = Convert.ToInt32(txtPax.Text.ToString());
                if (string.IsNullOrEmpty(txtPax.Text))
                {
                    Message("Warning! Number of people cannot be bull.");
                    txtPax.Focus();
                    return;
                }
                string CntctNo = txtCntct.Text.ToString();
                if (string.IsNullOrEmpty(CntctNo))
                {
                    Message("Warning! Contact number cannot be bull.");
                    txtCntct.Focus();
                    return;
                }
                string Email = txtmail.Text.ToString();
                if (string.IsNullOrEmpty(Email))
                {
                    Message("Warning! Contact email cannot be bull.");
                    txtmail.Focus();
                    return;
                }
                string BkingDate = txtbd.Text.ToString();
                if (string.IsNullOrEmpty(BkingDate))
                {
                    Message("Warning! Please select booking date.");
                    txtbd.Focus();
                    return;
                }
                string str_RequiredTime = txtrt.Text.ToString();//hh:mm
                var time = TimeSpan.Parse(str_RequiredTime);//hh:mm:ss
                var RequiredTime = DateTime.Today.Add(time);//yyyy-mm-dd hh:mm:ss
                if (string.IsNullOrEmpty(str_RequiredTime))
                {
                    Message("Warning! Required time cannot be bull.");
                    txtrt.Focus();
                    return;
                }

                string userID = MyComponents.UserID;

                string Purpose = txtDesc.Text.ToString().Replace("'", "").Trim();
                if (Purpose.Length > 200)
                {
                    Message("Description cannot have more than 200 characters");
                    txtDesc.Focus();
                    return;
                }
                #region SendforApproval

                try
                {
                    //MyComponents.ObjNav.VenueRequisitionCreate(depts, Convert.ToDateTime(BkingDate), Purpose, RequiredTime, Venue, CntcNme, CntctNo, Email, userID, Pax);

                    Session["ReqNo"] = LeaveNum;
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
                catch (Exception exception)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = true;
                    dvMdlContentPass.Visible = false;
                    exception.Data.Clear();
                    return;
                }
                #endregion
            }
            catch (Exception Ex)
            {
                Message("ERROR: " + Ex.Message.ToString());
                Ex.Data.Clear();
            }
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
                using (SqlConnection conn = MyComponents.getconnToNAV())
                {
                    UpdateApproval(conn, Session["ReqNo"].ToString(), "1");
                    conn.Close();
                    Response.Redirect("VenueListing.aspx");
                }

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
                stsmnts = "spUpdateVenueApproval";
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
                    stsmnts = "spCancelVenueReq";
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