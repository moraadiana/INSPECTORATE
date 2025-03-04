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
    public partial class TransportListing : System.Web.UI.Page
    {
        public enum MessageType { Success, Error, Info, Warning };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    string L_ = "SELECT *,(CASE [Status] WHEN 0 THEN 'Open' WHEN 1 THEN 'Pending Approval' " +
                                "WHEN 2 THEN 'Approved' WHEN 3 THEN 'Transport' WHEN 4 THEN 'Closed' END)[Status Description] " +
                                "FROM [" + MyComponents.Company_Name + "$FLT-Transport Requisition] " +
                                "WHERE ([Emp No]=@Employee_No) order by [Date of Request] ASC";
                    //Open,Pending Approval,Approved,Cancelled
                    var cmd = new SqlCommand(L_, conn);
                    cmd.Parameters.AddWithValue("@Employee_No", Session["username"]);
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
                                                            <td><a href='Transport Requisition.aspx?Tp=old&nn={0}' class='text-success'>{0}</a></td>
                                                            <td>{1}</td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                            <td>{6}</td>
                                                            <td>{7}</td>
                                                            <td><span class='label label-{9}'>{8}</span></td>
                                                             <td class='small'>
                                                                <a href='TransportRequiz.aspx?Tp=old&nn={0}'><i class='fa fa-list  text-info'></i>&nbsp;<span class='text-info'> Details</span></a>
                                                            </td>
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

        protected bool HasPendingApplications()
        {
            bool b = false;

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetPendingApplications_Transport";
                    SqlCommand cmdPendingApplications = new SqlCommand
                    {
                        CommandText = sqlStmt,
                        Connection = connToNAV,
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdPendingApplications.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdPendingApplications.Parameters.AddWithValue("@EmployeeNo", "'" + Session["username"].ToString() + "'");
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

        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        protected void btnTravelRequest_ServerClick(object sender, EventArgs e)
        {
            #region HasPendingApplications?
            //ShowMessage("A problem has occurred while submitting data", MessageType.Error);
            bool HasPendingApplications_ = HasPendingApplications();
            if (HasPendingApplications_)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                dvMdlContentFail.Visible = true;
                //ScriptManager.RegisterStartupScript(this, GetType(), "msg", "$(function(){ $('.msg-modal').modal('show'); })", true);
                return;
            }
            else
            {
                Response.Redirect("TransportRequiz.aspx?Tp=New");
            }
            #endregion
        }
    }
}