using INSPECTORATEStaff;
using Microsoft.OData.Edm;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace INSPECTORATEStaff.pages
{
    public partial class StoreqAccounting : System.Web.UI.Page
    {
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
                    LoadStoreqs();
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
            string number = ddlStoreqs.SelectedValue.ToString();
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spStoreqLines";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@StrqNo", "'" + MyComponents.ValidateEntry(number) + "'");
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
        protected void LoadStoreqs()
        {
            string userID = Session["username"].ToString();
            try
            {
                this.ddlStoreqs.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetMyStoreqsPosted";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                    ddlStoreqs.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Storeq--", String.Empty));
                    System.Web.UI.WebControls.ListItem li = null;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new System.Web.UI.WebControls.ListItem(
                                    dr["No_"].ToString() + " ",
                                    dr["No_"].ToString()
                                );

                                this.ddlStoreqs.Items.Add(li);
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
        protected void ddlStoreqs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridviewData();
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            //string DocumentNo = "";
            //DateTime AttachDT = DateTime.Now;
            string StoreqNo = ddlStoreqs.SelectedValue.ToString();
            try
            {
                string submitStoreq = MyComponents.ObjNav.SubmitStoreqHeader(StoreqNo);
                if (!String.IsNullOrEmpty(submitStoreq))
                {
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = submitStoreq.Split(strdelimiters, StringSplitOptions.None);

                    //No = staffLoginInfo_arr[0];
                }
                foreach (GridViewRow gvr in this.gvLines.Rows)
                {
                    if (gvr.RowType != DataControlRowType.DataRow)
                        continue;

                    #region lines

                    String Actual;
                    TextBox txtActualAmnt = FindControl("txtActual") as TextBox;
                    //String SRNNo = ddlStoreqs.SelectedValue.ToString();
                    String Remark = "";
                    String Aknow = "";

                    TextBox txtCashAmnt = gvr.FindControl("txtCash") as TextBox;
                    if (!string.IsNullOrEmpty(txtActualAmnt.Text.ToString()))
                    {
                        Actual = txtActualAmnt.Text.ToString();
                        //String No = "";
                        //String confDate = "";
                        //String Daterec = "";
                        //cashd = Convert.ToDecimal(txtCashAmnt.Text.ToString());
                        //if (!string.IsNullOrEmpty(txtCashAmnt.ToString()))
                        {
                            if (!MyComponents.IsNumeric(Actual.ToString()))
                            {
                                Message("Invalid Value please enter a number!");
                                txtActualAmnt.Focus();
                                return;
                            }
                            if (MyComponents.IsNumeric(Remark.ToString()))
                            {
                                Message("Invalid Cash Amount!");
                                txtCashAmnt.Focus();
                                return;//SubmitStoreqDetails(ActualSpent; SRNNo; Aknow; Remark)
                            }
                            //MyComponents.ObjNav.SubmitStoreqDetails(Actual; StoreqNo; Aknow; Remark);
                            
                        }
                    } 
                    #endregion
                }
                Message("Storeq accounting submitted successfully!");
        }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
}

private static ClientContext LogOn(string userName, string password, Uri url)
{
    ClientContext clientContext = null;
    ClientContext ctx;
    try
    {
        clientContext = new ClientContext(url);

        // Condition to check whether the user name is null or empty.
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            SecureString securestring = new SecureString();
            password.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));
            clientContext.Credentials = new System.Net.NetworkCredential(userName, securestring);
            clientContext.ExecuteQuery();
        }
        else
        {
            clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            clientContext.ExecuteQuery();
        }

        ctx = clientContext;
    }
    finally
    {
        if (clientContext != null)
        {
            clientContext.Dispose();
        }
    }

    return ctx;
}
public void Message(string strMsg)
{
    string strScript = null;
    strScript = "<script>";
    strScript = strScript + "alert('" + strMsg + "');";
    strScript = strScript + "</script>";
    Page.RegisterStartupScript("ClientScript", strScript.ToString());
}
protected void lbtnBack_Click(object sender, EventArgs e)
{

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
