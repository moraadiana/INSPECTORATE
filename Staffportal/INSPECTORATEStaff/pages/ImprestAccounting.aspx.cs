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
    public partial class ImprestAccounting : System.Web.UI.Page
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
                    LoadImprests();
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
            string number = ddlImprests.SelectedValue.ToString();
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
        protected void LoadImprests()
        {
            string userID = Session["username"].ToString();
            try
            {
                this.ddlImprests.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetMyImprestsPosted";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                    ddlImprests.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Imprest--", String.Empty));
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

                                this.ddlImprests.Items.Add(li);
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
        protected void ddlImprests_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridviewData();
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            string imprestNo = ddlImprests.SelectedValue.ToString();
            try
            {
                if (!FileUpload1.HasFiles)
                {
                    Message("Warning! You must attach the documents!");
                    return;
                }
                string DocumentNo = "";
                string user = Session["username"].ToString();
                DateTime AttachDT = DateTime.Now;
                //string filext = Path.GetExtension(fileName).Split('.')[1].ToLower();
                //string attchby = "";
                //int identity=0;
                //string ftype = "";
                int tblId = 52178705;
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
                MyComponents.ObjNav.SaveMemoAttchmnts(DocumentNo, tblId, ftype, filext, AttachDT, fileName, fileName, Session["username"].ToString());
            //foreach (GridViewRow gvr in this.gvLines.Rows) ;
            }
                    catch (Exception Ex)
                    {
                        Message("ERROR: " + Ex.Message.ToString());
                        Ex.Data.Clear();
                    }
            string submitImprest = MyComponents.ObjNav.SubmitImprestSurrHeader(imprestNo);
            if (!String.IsNullOrEmpty(submitImprest))
            {
                string[] strdelimiters = new string[] { "::" };
                string[] staffLoginInfo_arr = submitImprest.Split(strdelimiters, StringSplitOptions.None);

                string DocumentNo = staffLoginInfo_arr[0];
            }

            foreach (GridViewRow gvr in this.gvLines.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;

                #region lines

                decimal Actual, cashd = 0;
                TextBox txtActualAmnt = gvr.FindControl("txtActual") as TextBox;
                TextBox txtCashAmnt = gvr.FindControl("txtCash") as TextBox;
                if (!string.IsNullOrEmpty(txtActualAmnt.Text.ToString()))
                {
                    Actual = Convert.ToDecimal(txtActualAmnt.Text.ToString());
                    cashd = Convert.ToDecimal(txtCashAmnt.Text.ToString());
                    if (!string.IsNullOrEmpty(txtCashAmnt.ToString()))
                    {
                        if (!MyComponents.IsNumeric(Actual.ToString()))
                        {
                            Message("Invalid Actual Amount!");
                            txtActualAmnt.Focus();
                            return;
                        }
                        if (!MyComponents.IsNumeric(cashd.ToString()))
                        {
                            Message("Invalid Cash Amount!");
                            txtCashAmnt.Focus();
                            return;
                        }
                        string DocumentNo = "";
                        MyComponents.ObjNav.SubmitImprestSurrDetails(DocumentNo, Actual, cashd, imprestNo);
                    }
                    #endregion
                }
                Message("Imprest accounting submitted successfully!");
            }
            try { }
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

        private void UploadToSharePoint(string p, string user, string password, string web_url, string impNo, string lib)  //p is path to file to load
        {
            string newUrl = "";
            try
            {
                string siteUrl = web_url;
                //Insert Credentials
                ClientContext context = new ClientContext(siteUrl);

                SecureString passWord = new SecureString();
                foreach (var c in password) passWord.AppendChar(c);
                context.Credentials = new SharePointOnlineCredentials(user, passWord);
                Web site = context.Web;

                //Get the required RootFolder
                string barRootFolderRelativeUrl = lib;
                Folder barFolder = site.GetFolderByServerRelativeUrl(barRootFolderRelativeUrl);

                //Create new subFolder to load files into
                string newFolderName = impNo; //+ DateTime.Now.ToString("yyyyMMddHHmm");
                barFolder.Folders.Add(newFolderName);
                barFolder.Update();

                //Add file to new Folder
                Folder currentRunFolder = site.GetFolderByServerRelativeUrl(barRootFolderRelativeUrl + "/" + newFolderName);
                FileCreationInformation newFile = new FileCreationInformation { Content = System.IO.File.ReadAllBytes(@p), Url = Path.GetFileName(@p), Overwrite = true };
                currentRunFolder.Files.Add(newFile);
                currentRunFolder.Update();

                context.ExecuteQuery();

                //Return the URL of the new uploaded file
                newUrl = siteUrl + barRootFolderRelativeUrl + "/" + newFolderName + "/" + Path.GetFileName(@p);
                //MessageBox.Show(newUrl, "Information");
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Error");
                Message("Error: " + ex.Message);
            }
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