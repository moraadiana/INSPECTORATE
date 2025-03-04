using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class Approvals_Transport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ActionPassed"] != null)
            {
                int ActionState = (Int32)Session["ActionPassed"];
                if (ActionState == 1)
                {
                    dvMdlContentRejConfirm.Visible = false;
                    dvMdlContentAprConfirm.Visible = true;
                }
                else if (ActionState == 2)
                {
                    dvMdlContentAprConfirm.Visible = false;
                    dvMdlContentRejConfirm.Visible = true;
                }
                Session["ActionPassed"] = null;
                ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.msg-modal').modal('show'); })", true);
            }
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LeaveApprovalsList();
                MultiView1.SetActiveView(View1);
            }
        }
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            string userID = MyComponents.UserID;
            using (var conn = MyComponents.getconnToNAV())
            {
                string L_ = null;
                L_ = "spGetMyApprovals";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = L_;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@Supervisor", "'" + userID + "'");
                using (SqlDataReader drL = cmd.ExecuteReader())
                {
                    if (drL.HasRows)
                    {
                        while (drL.Read())
                        {
                            AES2 aes = new AES2();
                            htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td class='small'>{0}</td>
                                                            <td class='small'>{1}</td>
                                                            <td class='small'>{2}</td>
                                                            <td class='small'>{3}</td>
                                                            <td class='small'>{4}</td>
                                                            <td class='small'>{5}</td>
                                                            <td class='small'>{6}</td>
                                                            <td class='small'>{7}</td>
                                                            <td class='small'>
                                                                    <a href='LeaveHandler.ashx?appNo={8}&action=A'><i class='fa fa-check text-success'></i><span class='text-success'>  Approve</span></a>
                                                                    <a href='LeaveHandler.ashx?appNo={8}&action=R'><i class='fa fa-times  text-danger'></i><span class='text-danger'>  Reject</span></a>
                                                                    <a href='#'><i class='fa fa-times  text-danger'></i><span class='text-danger'>  View Details</span></a>
                                                            </td>
                                                     </tr>",
                                drL["No_"],
                                drL["Leave Type"],
                                Convert.ToDateTime(drL["Date"]).ToShortDateString(),
                                Convert.ToDouble(drL["Applied Days"]).ToString(CultureInfo.InvariantCulture),
                                Convert.ToDateTime(drL["Starting Date"]).ToShortDateString(),
                                Convert.ToDateTime(drL["End Date"]).ToShortDateString(),
                                drL["Purpose"],
                                drL["Employee Name"],
                                aes.Encrypt(drL["No_"].ToString())
                                );
                        }
                    }
                }
            }
            return htmlStr;
        }

        public void Message(string strMsg, string id)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            if (!String.IsNullOrEmpty(id.Trim()))
            {
                strScript = strScript + "$('#" + id + "').css('border','1px solid red');";
                strScript = strScript + "$('#" + id + "').click(function(){ $(this).css('border','1px solid lightgreen')})";
            }
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }

        public string[] GetStaffName_N_UserId()
        {
            string Name, UserId = "";
            string StaffNo = Session["username"].ToString();
            string[] myData = new string[2];
            try
            {

                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetName_N_UserId";
                    SqlCommand cmdGetDetails = new SqlCommand();
                    cmdGetDetails.CommandText = sqlStmt;
                    cmdGetDetails.Connection = connToNAV;
                    cmdGetDetails.CommandType = CommandType.StoredProcedure;
                    cmdGetDetails.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdGetDetails.Parameters.AddWithValue("@StaffNo", "'" + StaffNo + "'");
                    using (SqlDataReader dr = cmdGetDetails.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            Name = dr["First Name"].ToString() + " " + dr["Middle Name"].ToString() + " " + dr["Last Name"].ToString();
                            UserId = dr["User ID"].ToString();

                            myData[0] = Name;
                            myData[1] = UserId;
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return myData;
        }
        protected void LeaveApprovalsList()
        {
            var htmlStr = string.Empty;
            // string InternalUser = Session["internalUserID"].ToString(); //@"KUCSERVER\RAYIEKO";
            string InternalUser = MyComponents.UserID; //@"KUCSERVER\RAYIEKO";
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {

                    string mealApproval_sql = "spGetMyApprovals_Transport";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = mealApproval_sql;
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@Supervisor", "'" + InternalUser + "'");

                    cmd.ExecuteNonQuery();

                    var myAdapter = new SqlDataAdapter(cmd);

                    var myTable = new DataTable();
                    myAdapter.Fill(myTable);

                    gvLeaveApprovals.DataSource = myTable;
                    gvLeaveApprovals.AutoGenerateColumns = false;

                    gvLeaveApprovals.DataBind();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
        }

        protected void gvLeaveApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            //int rowIndex = gvr.RowIndex + 1;
            int rowIndex = gvr.RowIndex;
            GridViewRow row = gvLeaveApprovals.Rows[rowIndex];
            string appCode = gvr.Cells[1].Text;
            HttpContext.Current.Session["leaveAppCode"] = appCode;

            if (e.CommandName.Equals("ViewDetails"))
            {
                TransportAppDetails(appCode);
                MultiView1.SetActiveView(View2);
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none","<script>$('#mymodal').modal('show');</script>", false);
                //ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.detail-modal').modal('show'); })", true);
            }
        }

        #region View Details

        protected void TransportAppDetails(string LeaveAppCode)
        {
            string query = ""; string t = ""; string applNo = ""; DateTime appDate, from, to = DateTime.Now;
            string Purpose = ""; string daysApplied = ""; Decimal days = 0; Boolean isNumber = false;
            string Commence, Destination = "";
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetTransportAppDetails";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@TransCode", "'" + LeaveAppCode + "'");

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            t = "<br /> <table id='trainAppDetails'  style='border: silver thin ridge; width: 100%;'>";

                            while (dr.Read())
                            {
                                //dr.Read();

                                appDate = Convert.ToDateTime(dr["Date of Request"].ToString());
                                Commence = dr["Commencement"].ToString();
                                Destination = dr["Destination"].ToString();
                                Purpose = dr["Purpose of Trip"].ToString();
                                from = Convert.ToDateTime(dr["Date of Trip"].ToString());
                                daysApplied = dr["No of Days Requested"].ToString();
                                if (MyComponents.IsNumeric(daysApplied))
                                {
                                    days = Convert.ToDecimal(daysApplied);
                                    days = Math.Round(days, 2);
                                    daysApplied = days.ToString();
                                }

                                t += "<tr><td>Application Date.:</td><td> <strong>" + appDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US")) + "</strong></td>";
                                t += "<td width='10px'> &nbsp; &nbsp; &nbsp; </td>";
                                t += "<tr><td>Employee Name:</td><td> <strong>" + dr["Employee Name"].ToString() + "</strong></td>";
                                t += "</tr>";

                                t += "<tr><td>Commencement:</td><td><strong>" + Commence + "</strong></td>";
                                t += "<td width='10px'> &nbsp; &nbsp; &nbsp; </td>";
                                t += "<tr><td>Destination:</td><td><strong>" + Destination + "</strong></td>";
                                t += "<td width='10px'> &nbsp; &nbsp; &nbsp; </td>";
                                t += "<tr><td>Status:</td><td><strong>" + dr["Status Description"].ToString() + "</strong></td>";
                                t += "</tr>";

                                t += "<tr><td>Purpose Of Trip:</td><td><strong>" + Purpose + "</strong></td>";
                                t += "<td width='10px'> &nbsp; &nbsp; &nbsp; </td>";
                                //t += "<tr style='display: none;'><td>Applicant Comments:</td><td><strong>" + dr["Applicant Comments"].ToString() + "</strong></td>";
                                //t += "</tr>";

                                t += "<tr><td>Date of Trip:</td><td> <strong>" + from.ToString("D", CultureInfo.CreateSpecificCulture("en-US")) + "</strong></td>";
                                t += "<td width='10px'> &nbsp; &nbsp; &nbsp; </td>";
                                t += "<tr><td>Number of Days:</td><td><strong>" + daysApplied + "</strong></td>";
                                t += "</tr>";
                            }
                            t = t + "</table>";
                        }

                        else
                        {
                            t = "<br /> Transport details not available at the moment.";
                        }

                        this.litViewTraining.Text = t;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        #endregion


        protected void btnApprovelBooking(object sender, EventArgs e)
        {
            string userID = MyComponents.UserID;
            #region Validations
            if (String.IsNullOrEmpty(txtApproverComments.Text.Trim()))
            {
                Message("Please accompany your action with comments.", "txtbd");
                ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.detail-modal').modal('show'); })", true);
                return;
            }

            #endregion

            try
            {
                string appCode = HttpContext.Current.Session["leaveAppCode"].ToString();
                InsertApproverComments(appCode);
                MyComponents.ObjNav.ApproveDocument(appCode, userID);

                Message("Application: " + appCode + " APPROVED successfully.", "txtbd");

                ScriptManager.RegisterStartupScript(this, GetType(), "msgbox", "$('.msg-modal').modal('show'); setTimeout(function(){ $('.msg-modal').modal('hide'); }, 2000);", true);
                //MealBookings();
                LeaveApprovalsList();
                MultiView1.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                char[] c = "' ".ToCharArray();
                Message(exception.Message.Replace(c[0], c[1]), "txtbd");
                // cSite.SendErrorToDeveloper(exception);
                exception.Data.Clear();
            }
        }
        protected void btnCancelBooking(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View1);
        }

        protected void btnRejectlBooking(object sender, EventArgs e)
        {
            string userID = MyComponents.UserID;
            #region Validations
            if (String.IsNullOrEmpty(txtApproverComments.Text.Trim()))
            {
                Message("Please accompany your action with comments.", "txtbd");
                ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.detail-modal').modal('show'); })", true);
                return;
            }

            #endregion

            try
            {
                string appCode = HttpContext.Current.Session["leaveAppCode"].ToString();
                InsertApproverComments(appCode);
                MyComponents.ObjNav.RejectDocument(appCode, userID);

                Message("Application: " + appCode + " REJECTED successfully.", "txtbd");

                ScriptManager.RegisterStartupScript(this, GetType(), "msgbox", "$('.msg-modal').modal('show'); setTimeout(function(){ $('.msg-modal').modal('hide'); }, 2000);", true);
                //MealBookings();
                LeaveApprovalsList();
                MultiView1.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                char[] c = "' ".ToCharArray();
                Message(exception.Message.Replace(c[0], c[1]), "txtbd");
                // cSite.SendErrorToDeveloper(exception);
                exception.Data.Clear();
            }
        }

        public void InsertApproverComments(string appCode)
        {
            string userID = MyComponents.UserID;
            try
            {
                if (txtApproverComments.Text.Length > 250)
                {
                    Message("Kindly note that the maximum length for Comments characters is 250, and you have exceeded.", "txtbd");
                    return;
                }
                MyComponents.ObjNav.InsertApproverComments(appCode, userID, txtApproverComments.Text);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}