using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages 
{
    public partial class StoresLines : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null || string.IsNullOrEmpty(Request.QueryString["Tp"].ToString()))
                {
                    Response.Redirect("~/Default.aspx");
                }
                //LoadStores();
                string paged = Request.QueryString["Tp"].ToString();
                if (paged == "old")
                {
                   // MultiView1.ActiveViewIndex = 1;
                    Session["ReqNo"] = Request.QueryString["An"].ToString();
                    //BindGridviewData(Request.QueryString["An"].ToString());
                    if (ApprovalSent(Request.QueryString["An"].ToString()) == "2")
                    {
                        if (ApprovedRejected(Request.QueryString["An"].ToString()) == true)
                        {
                            //btnApproval.Visible = false;
                            //btncancel.Visible = false;
                            ////lbnAddLine.Visible = false;
                        }
                        else
                        {
                            //btnApproval.Visible = false;
                            //btncancel.Visible = true;
                            //lbnAddLine.Visible = false;
                        }
                    }
                    else
                    {
                        //btnApproval.Visible = true;
                        //btncancel.Visible = false;
                    }
                }
                else
                {
                    MultiView1.ActiveViewIndex = 0;
                    LoadDrops();
                    lblNo.Text = GetStoreReqNo();
                    lbluserId.Text = Session["username"].ToString();
                    lblstatus.Text = "New";
                }
            }
        }
        protected bool Approval(string ReqNUM)
        {
            bool approve = false;
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetStoresReqApprovalStaus";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ReqNo", "'" + ReqNUM + "'");

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            approve = true;
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return approve;
        }
        protected string ApprovalSent(string ReqNUM)
        {
            string approve = "";
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetStrqSentForApproval";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ReqNo", "'" + ReqNUM + "'");

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                approve = sdr["Status"].ToString();
                            }
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return approve;
        }
        protected bool ApprovedRejected(string ReqNUM)
        {
            bool approve = false;
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetStrqApprovedRejected";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ReqNo", "'" + ReqNUM + "'");

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            approve = true;
                        }
                    }

                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return approve;
        }
        protected void LoadDrops()
        {
            //LoadPrograms();
            LoadResponsibilityCenters();
            LoadRegions();
        }
        protected void LoadRegions()
        {
            try
            {
                this.ddlregions.Items.Clear();

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
                    li = new ListItem("");
                    this.ddlregions.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlregions.Items.Add(li);
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
        /*
  
        protected void LoadStores()
        {
            try
            {
                this.ddlissuingstore.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetIssuingStore";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("---SELECT---", "");
                    this.ddlissuingstore.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlissuingstore.Items.Add(li);
                            }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }*/
        /*
        protected void LoadPrograms()
        {
            try
            {
                this.ddlBranch.Items.Clear();
                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadImprestProgram";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ddlBranch.Items.Insert(0, new ListItem(String.Empty));

                    ListItem li = null;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Name"].ToString() + " ",
                                    dr["Code"].ToString()
                                );

                                this.ddlBranch.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }*/

        protected void LoadResponsibilityCenters()
        {
            try
            {
                this.ddlresponibilitycentres.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadStoresResponsibilityCentre";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("");
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
        /*
        private void BindGridviewData(string number)
        {
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spStoresReqLines";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@ReqNo", "'" + number + "'");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        //cmd.Parameters.AddWithValue("",);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            //gvLines.DataSource = dt;
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
        }*/
        public static string GetStoreReqNo()
        {
            //string finalreturned = "";
            string newAppNo = "";
            Int32 LastNumUsed = 0;

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    SqlCommand cmds = new SqlCommand();
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.Connection = connToNAV;
                    cmds.CommandText = "spGetStoreReqNo";
                    cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    using (SqlDataReader rdr = cmds.ExecuteReader())
                    {
                        if (rdr.HasRows == true)
                        {
                            rdr.Read();
                            newAppNo = rdr["AppNo"].ToString();
                        }
                    }
                    connToNAV.Close();
                }
                // UpdateApplicationNo(newAppNo, conn);
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
                cmds.CommandText = "spUpdateReqNo";
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //Collect data
                string RqNum = lblNo.Text;
                //string MyDate = dtRequestDate.SelectedDate.ToLongDateString();
                string RequiredDate = dtRequiredDate.Text;
                if (dtRequiredDate.Text == null)
                {
                    Message("Warning! Required date cannot be bull.");
                    dtRequiredDate.Focus();
                    return;
                }
                //string userID = MyComponents.UserID;
                string userID = Session["username"].ToString();
                string CampusCode = "INSPECTORATE";
                string function = "INSPECTORATE";
                string DeptCode = ddlregions.SelectedItem.Value;
                string depts = ddlregions.SelectedItem.Text.ToString().Replace("'", "");
                string Rcentre = ddlresponibilitycentres.SelectedItem.Value;
                string userd = Session["username"].ToString();
                //string ReqType = "'";
                string Purpose = txtHdescription.Text.ToString().Replace("'", "").Trim();
                if (Purpose.Length > 1000)
                {
                    Message("Purpose cannot have more than 1000 characters");
                    txtHdescription.Focus();
                    return;
                }
                #region SendforApproval

                try
                {
                    
                    string DocumentNo = GetStoreReqNo();
                    string user = Session["username"].ToString();
                    DateTime AttachDT = DateTime.Now;
                    //string filext = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    //string attchby = "";
                    //int identity=0;
                    //string ftype = "";
                    int tblId = 52178702;
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
                    string DoCfilename = Path.GetFileName(FileUpload1.PostedFile.FileName).Replace(" ", "_");
                    FileUpload1.SaveAs(fileName.TrimEnd('.', 'p', 'd', 'f'));
                    //fileName.TrimEnd('.','p','d','f');
                    //TrimEnd();
                    MyComponents.ObjNav.SaveMemoAttchmnts(DocumentNo, tblId, ftype, filext, AttachDT, fileName, fileName, userd);
                    //foreach (GridViewRow gvr in this.gvLines.Rows) ;
                }
                catch (Exception Ex)
                {
                    //Message("ERROR: " + Ex.Message.ToString());
                    //Ex.Data.Clear();
                }

                string Submitted = MyComponents.ObjNav.StoresRequisitionCreate(Session["username"].ToString(), userd, Convert.ToDateTime(RequiredDate), Purpose, DeptCode, CampusCode, depts, function, Rcentre);

                if (!String.IsNullOrEmpty(Submitted))
                {
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = Submitted.Split(strdelimiters, StringSplitOptions.None);

                    Session["ReqNo"] = staffLoginInfo_arr[0];
                    lblNo.Text = Session["ReqNo"].ToString();
                }

                Session["ReqDate"] = RequiredDate;
                Session["Branch"] = CampusCode;
                Session["RCentre"] = Rcentre;
                Session["Dept"] = DeptCode;
                //connToNAV.Close();
                sendapproval();
                Message("Stores Requisition " + RqNum + " created successfully");
                sendapproval();
                //btncancel.Visible = false;
                // newView();
            }
            catch (Exception exception)
            {
                Message("ERROR: " + exception.Message.ToString());
                exception.Data.Clear();
            }
            #endregion
        } 

        /*
        protected void newView()
        {
            MultiView1.ActiveViewIndex = 1;
            newLines.Visible = true;
            lbnAddLine.Visible = false;
            lblLNo.Text = Session["ReqNo"].ToString();
            AdvanceTypes();
            BindGridviewData(Session["ReqNo"].ToString());
        }
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlType.SelectedValue.ToString()))
                {
                    Message("Please select type.");
                    ddlType.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ddlItems.SelectedValue.ToString()))
                {
                    Message("Please select item to request.");
                    ddlItems.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ddlissuingstore.SelectedValue.ToString()))
                {
                    Message("Issuing Store cannot be null.");
                    ddlissuingstore.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtUnit.Text.ToString()))
                {
                    Message("Unit of measure cannot be null.");
                    txtUnit.Focus();
                    return;
                }
                if (txtAmnt.Text.ToString() == "0" || string.IsNullOrEmpty(txtAmnt.Text.ToString()))
                {
                   Message("Unit cost cannot be zero or null.");
                    txtAmnt.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtQty.Text.ToString()))
                {
                    Message("Quantity Requested cannot be null.");
                    txtQty.Focus();
                    return;
                }
               
                decimal Quantity = Convert.ToDecimal(txtQty.Text.ToString());
                int type = Convert.ToInt32(ddlType.SelectedValue);
                decimal Amnt = Convert.ToDecimal(txtAmnt.Text.ToString());
                
                string Item = ddlItems.SelectedValue;
                string ItemDesc = ddlItems.SelectedItem.Text.ToString().Trim();
                string Units = txtUnit.Text.ToString();

                // string mytype = type.Text;
                string SREQNo = lblLNo.Text.ToString();
                decimal LAmnt = (Convert.ToDecimal(Amnt) * Convert.ToDecimal(Quantity));
                string ISTore = ddlissuingstore.SelectedValue.ToString();

                try
                {
                    #region commented - using webservice
                    string staffLoginInfo = MyComponents.ObjNav.InsertStoreRequisitionLines(SREQNo, Item, type, ItemDesc, Amnt, LAmnt, Quantity, Units, ISTore); 

                    if (!String.IsNullOrEmpty(staffLoginInfo))
                    {
                        string returnMsg = "";
                        string[] strdelimiters = new string[] { "::" };
                        string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                        returnMsg = staffLoginInfo_arr[0];
                        if (returnMsg == "SUCCESS")
                        {
                            Message("Line added successfully!");
                            ddlItems.SelectedIndex = 0;
                            ddlissuingstore.SelectedIndex = 0;
                            txtQty.Text = null;
                            txtAmnt.Text = null;
                            txtUnit.Text = null;
                            BindGridviewData(Session["ReqNo"].ToString());
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }

        protected void lbnAddLine_Click(object sender, EventArgs e)
        {
            lblLNo.Text = Request.QueryString["An"].ToString();
            AdvanceTypes();
            newLines.Visible = true;
            lbnAddLine.Visible = false;
        }
        protected void AdvanceTypes()
        {
            try
            {
                this.ddlItems.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetStoresReqATypes";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
                    li = new ListItem("----Select---", "");
                    this.ddlItems.Items.Add(li);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Description"].ToString().ToUpper(),
                                    dr["No_"].ToString()
                                );

                                this.ddlItems.Items.Add(li);
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

        protected void lbnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbnAddLine.Visible = true;
        }
        */
        protected string sendapproval()
        {
            try
            {
                /*int i = gvLines.Rows.Count;
                if (i == 0)
                {
                    Message("warning! Please add lines first!");
                    return;
                }*/
                MyComponents.ObjNav.StoreRequisitionApprovalRequest(Session["ReqNo"].ToString());
                Response.Redirect("StoresListing.aspx");
                return "";
            }
            catch (Exception ex)
            {
                Message("ERROR! "+ ex.Message.ToString());
                ex.Data.Clear();
                    return "";
            }
        }

        protected void UpdateApproval(SqlConnection conn, string num, string status)
        {
            try
            {
                string stsmnts = null;
                SqlCommand cmd = new SqlCommand();
                stsmnts = "spUpdateStoresApproval";
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
        
       
        /*
        protected void btncancel_Click(object sender, EventArgs e)
        {
            string Numm = Request.QueryString["An"].ToString();

            try
            {

                MyComponents.ObjNav.CancelStoreRequisition(Numm);

                //btnApproval.Visible = true;
                btncancel.Visible = false;

                Response.Redirect("StoresListing.aspx");

            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }

        protected void ddlItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUnitCost();
        }
        protected void getUnitCost()
        {
            string item = ddlItems.SelectedValue.ToString();
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string stsmnts = null;
                    SqlCommand cmd = new SqlCommand();
                    stsmnts = "spGetStoresUnitCost";
                    cmd.Connection = connToNAV;
                    cmd.CommandText = stsmnts;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@Item", "'" + item + "'");
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            txtAmnt.Text = Convert.ToInt32(Convert.ToDouble(sdr["Unit Cost"])).ToString();
                            txtUnit.Text = sdr["Base Unit of Measure"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void gvLines_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void cancelz(object sender, EventArgs e)
        {
            string message = "Are you sure you want to remove this line?";
            ClientScript.RegisterOnSubmitStatement(this.GetType(), "confirm", "return confirm('" + message + "');");
            string[] arg = new string[2];
            arg = (sender as LinkButton).CommandArgument.ToString().Split(';');
            int LineNo = Convert.ToInt32(arg[0]);
            try
            {
                MyComponents.ObjNav.RemoveStoreReqLine(LineNo); 
                Message("Line deleted successfully");
                BindGridviewData(Session["ReqNo"].ToString());
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }
        protected void gvLines_DataBound(object sender, EventArgs e)
        {
            bool appp = Approval(Session["ReqNo"].ToString());
            for (int i = 0; i <= gvLines.Rows.Count - 1; i++)
            {
                LinkButton lbtnCancl = (LinkButton)gvLines.Rows[i].FindControl("lbtnCancll");
                if (appp == false)
                {
                    lbtnCancl.Visible = false;
                }
                else if (appp == true)
                {
                    lbtnCancl.Visible = true;
                }
            }
        }*/
    }
}