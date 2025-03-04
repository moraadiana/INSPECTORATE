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
    public partial class Approvals_Imprest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                ImprestApprovalsList();
                MultiView1.SetActiveView(View1);
            }
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
        protected void ImprestApprovalsList()
        {
            var htmlStr = string.Empty;
            string InternalUser = MyComponents.UserID;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {

                    string mealApproval_sql = "spGetMyApprovals_Imprest";
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
                ImprestLines(appCode);
                MultiView1.SetActiveView(View2);
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none","<script>$('#mymodal').modal('show');</script>", false);
                //ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.detail-modal').modal('show'); })", true);
            }
        }

        #region View Details
        private void ImprestLines(string number)
        {
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spImprestLines";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ImpNo", "'" + MyComponents.ValidateEntry(number) + "'");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        //cmd.Parameters.AddWithValue("",);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvLines.DataSource = dt;
                            gvLines.DataBind();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        #endregion
        protected void btnApprovelImprest(object sender, EventArgs e)
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
                MyComponents.ObjNav.ApproveDocument(appCode, "IMPREST");

                Message("Application: " + appCode + " APPROVED successfully.", "txtbd");

                ScriptManager.RegisterStartupScript(this, GetType(), "msgbox", "$('.msg-modal').modal('show'); setTimeout(function(){ $('.msg-modal').modal('hide'); }, 2000);", true);

                ImprestApprovalsList();
                MultiView1.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                char[] c = "' ".ToCharArray();
                Message(exception.Message.Replace(c[0], c[1]), "txtbd");
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
                ImprestApprovalsList();
                MultiView1.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                char[] c = "' ".ToCharArray();
                Message(exception.Message.Replace(c[0], c[1]), "txtbd");
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