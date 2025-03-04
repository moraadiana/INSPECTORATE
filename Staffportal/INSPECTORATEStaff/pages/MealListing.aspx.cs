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
    public partial class MealListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            MealBookings();
        }

        protected void MealBookings()
        {
            var htmlStr = string.Empty;
            string userID = MyComponents.UserID;
            try
            {
                using (var conn = MyComponents.getconnToNAV())
                {
                    string meal_sql = "spGetMealBookingAll";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = meal_sql;
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");

                    cmd.ExecuteNonQuery();

                    var myAdapter = new SqlDataAdapter(cmd);

                    var myTable = new DataTable();
                    myAdapter.Fill(myTable);

                    gvMealBookings.DataSource = myTable;
                    gvMealBookings.AutoGenerateColumns = false;

                    gvMealBookings.DataBind();
                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
        }

        protected string getCssClass(string status, string mode)
        {
            string statusCls = "";
            switch (status)
            {
                case "New":
                    statusCls = "warning";
                    break;
                case "Pending Approval":
                    statusCls = "primary";
                    break;
                case "Cancelled":
                    statusCls = "danger";
                    break;
                case "Rejected":
                    statusCls = "danger";
                    break;
                case "Approved":
                    statusCls = "success";
                    break;
            }
            if (mode == "sonly")
            {
                statusCls = "label label-" + statusCls;
            }
            return statusCls;
        }

        protected void gvMealBookings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string refNo = Convert.ToString(gvMealBookings.DataKeys[e.RowIndex]["Booking Id"]);
            using (var conn = MyComponents.getconnToNAV())
            {
                string del_mealbooking = "spDeleteMealBooking";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = del_mealbooking;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@BookingID", "'" + refNo + "'");

                cmd.ExecuteNonQuery();
                MealBookings();
            }
        }

        protected void gvMealBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int rowIndex = gvr.RowIndex;
            GridViewRow row = gvMealBookings.Rows[rowIndex];
            string refNo = "", statusCls = "", statusDesc = "";

            if (e.CommandName.Equals("Redirect"))
            {
                refNo = Convert.ToString(gvMealBookings.DataKeys[row.RowIndex]["Booking Id"]);
                statusDesc = Convert.ToString(gvMealBookings.DataKeys[row.RowIndex]["StatusDesc"]);
                statusCls = getCssClass(statusDesc, "sonly");

                if (!String.IsNullOrEmpty(refNo))
                {
                    AES2 AES2 = new AES2();
                    string No_AES = AES2.Encrypt(HttpUtility.UrlEncode(refNo));
                    string Status_AES = AES2.Encrypt(HttpUtility.UrlEncode(statusCls));
                    string mode_AES = AES2.Encrypt(HttpUtility.UrlEncode("edit"));
                    Response.Redirect("MealBookingNew.aspx?An=" + No_AES + "&Tp=old");
                }
            }

            if (e.CommandName.Equals("ViewDetails"))
            {
                refNo = Convert.ToString(gvMealBookings.DataKeys[row.RowIndex]["Booking Id"]);
                // MealBooking(refNo);
                //MealItems(refNo);

                ScriptManager.RegisterStartupScript(this, GetType(), "apMsg", "$(function(){ $('.detail-modal').modal('show'); })", true);
            }
        }

        protected void gvMealBookings_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = Convert.ToString(gvMealBookings.DataKeys[e.Row.RowIndex]["StatusDesc"]);
                if (status != "New")
                {
                    Label lblEdit = (Label)e.Row.Cells[9].FindControl("lblEdt");
                    Label lblDel = (Label)e.Row.Cells[9].FindControl("lblDel");
                    Label lblVw = (Label)e.Row.Cells[9].FindControl("lblView");
                    lblEdit.Visible = false; lblDel.Visible = false; lblVw.Visible = true;
                }
            }
        }

        protected string StrEnc(string val)
        {
            string str_enc;
            AES2 AES2 = new AES2();
            str_enc = AES2.Encrypt(HttpUtility.UrlEncode(val));
            return str_enc;
        }

        #region View Details

        #endregion
    }
}