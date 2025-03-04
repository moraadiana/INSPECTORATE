using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace INSPECTORATEStaff.pages

{
    public partial class BackToOffice : System.Web.UI.Page
    {
        private readonly object dvMdlContentFail;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["username"] == null)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    LoadLeaves();
                    BindGridviewData();
                }
            }
            catch (Exception Ex)
            {

                Ex.Data.Clear();
            }
        }
        private void BindGridviewData()
        {
            string number = ddlLeaves.SelectedValue.ToString();
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spbackto_officedata";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@LvNo", "'" + MyComponents.ValidateEntry(number) + "'");
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
        protected void LoadLeaves()
        {
            string userID = Session["username"].ToString();
            try
            {
                this.ddlLeaves.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetMyLeavesPosted";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                    ddlLeaves.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Leave--", String.Empty));
                    System.Web.UI.WebControls.ListItem li = null;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new System.Web.UI.WebControls.ListItem(
                                    dr["No_"].ToString(),
                                    dr["No_"].ToString()
                                );

                                this.ddlLeaves.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                //cSite.SendErrorToDeveloper(ex);
                ex.Data.Clear();
            }
        }
        protected void ddlLeaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridviewData();
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
            //dvMdlContentFail.Visible = true;
            //dvMdlContentPass.Visible = false;
        }

        public void ExceptionMsg(string Msg)
        {
            lbtnApply.Visible = false;
            Message(Msg);
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            string LeaveNo = ddlLeaves.SelectedValue.ToString();
            try
            {
                string submitBacktoOffice = MyComponents.ObjNav.SubmitbacktoOffice(LeaveNo);
                if (!String.IsNullOrEmpty(submitBacktoOffice))
                {
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = submitBacktoOffice.Split(strdelimiters, StringSplitOptions.None);

                    string DocumentNo = staffLoginInfo_arr[0];
                }

                foreach (GridViewRow gvr in this.gvLines.Rows)
                {
                    if (gvr.RowType != DataControlRowType.DataRow)
                        continue;

                    Date Actual, returndt;
                    TextBox txtActual = gvr.FindControl("txtActual") as TextBox;
                    TextBox txtreturndt = gvr.FindControl("txtreturndt") as TextBox;
                    if (!string.IsNullOrEmpty(txtActual.Text.ToString()))
                    {
                        Actual = DateTime.ParseExact(txtActual.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //cashd = Convert.ToDecimal(txtCash.Text.ToString());
                        returndt = DateTime.ParseExact(txtreturndt.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(txtreturndt.ToString()))
                        {
                            if (!MyComponents.IsNumeric(Actual.ToString()))
                            {
                                Message("Invalid Actual Amount!");
                                txtActual.Focus();
                                return;
                            }
                            if (!MyComponents.IsNumeric(returndt.ToString()))
                            {
                                Message("Invalid Cash Amount!");
                                txtreturndt.Focus();
                                return;
                            }
                            string DocumentNo = "";
                            MyComponents.ObjNav.Back2officedetails(DocumentNo, Actual, returndt, LeaveNo);
                        }
                    }
                    Message("Back to Office Applied Successfully");
                }
            }
            catch (Exception exception)
            {
                Message("ERROR: " + exception.Message.ToString());
                exception.Data.Clear();
            }

        }


        protected void gvLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row.
                DropDownList ddlRecpts = (e.Row.FindControl("ddlReceipts") as DropDownList);
                ddlRecpts.DataSource = GetData();
                ddlRecpts.DataTextField = "No";
                ddlRecpts.DataValueField = "No";
                ddlRecpts.DataBind();

                //Add Default Item in the DropDownList.
                ddlRecpts.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please select"));

                //Select the Country of Customer in DropDownList.
                //string country = (e.Row.FindControl("ddlReceipts") as Label).Text;
                //ddlRecpts.Items.FindByValue(country).Selected = true;
            }
        }
        private DataSet GetData()
        {
            string username = Session["username"].ToString();
            using (SqlConnection con = MyComponents.getconnToNAV())
            {
                string stst = "spGetMyReceipts";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = stst;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@Username", "'" + username + "'");

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        protected void ddlReceipts_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(sender as DropDownList).NamingContainer;
            string rNo = ((DropDownList)row.FindControl("ddlReceipts")).SelectedItem.Value;
            DataSet ds = GetMyData(rNo);
            //string country = (e.Row.FindControl("ddlReceipts") as Label).Text;
            //ddlRecpts.Items.FindByValue(country).Selected = true;
            row.Cells[0].Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        private DataSet GetMyData(string Receipts)
        {
            using (SqlConnection con = MyComponents.getconnToNAV())
            {
                string stst = "spGetMyReceiptsAmount";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = stst;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@ReceiptNo", "'" + Receipts + "'");

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
    }
}
//#endregion