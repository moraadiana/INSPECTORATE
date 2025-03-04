using INSPECTORATEStaff;
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
    public partial class PurchaseReqLines : System.Web.UI.Page
    {
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
                    MultiView1.ActiveViewIndex = 1;
                    Session["ReqNo"] = Request.QueryString["An"].ToString();
                    BindGridviewData(Request.QueryString["An"].ToString());
                    if (ApprovalSent(Request.QueryString["An"].ToString()) == true)
                    {
                        if (ApprovedRejected(Request.QueryString["An"].ToString()) == true)
                        {
                            btnApproval.Visible = false;
                            btncancel.Visible = false;
                            lbnAddLine.Visible = false;
                        }
                        else
                        {
                            btnApproval.Visible = false;
                            btncancel.Visible = true;
                            lbnAddLine.Visible = false;
                        }
                    }
                    else
                    {
                        btnApproval.Visible = true;
                        btncancel.Visible = false;
                        lbnAddLine.Visible = true;
                    }
                }
                else
                {
                    MultiView1.ActiveViewIndex = 0;
                    LoadDrops();
                    lblNo.Text = GetPrnReqNo();
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
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.GetApprovalStatus(ReqNUM.ToString());

                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "Created")
                    {
                        approve = true;
                    }
                    else if (returnMsg == "Open")
                    {
                        approve = true;
                    }
                }
                else
                {
                    approve = true;
                }
                #endregion}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return approve;
        }
        protected bool ApprovalSent(string ReqNUM)
        {
            bool approve = false;
            try
            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.GetApprovalStatus(ReqNUM.ToString());

                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    
                    approve = true;
                }
                #endregion}
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
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.GetApprovalStatus(ReqNUM.ToString());

                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = staffLoginInfo_arr[0];
                    if (returnMsg == "Rejected" || returnMsg == "Approved")
                    {
                        approve = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return approve;
        }
        protected void LoadDrops()
        {
            LoadBusinessCodes();
            LoadResponsibilityCenters();
            LoadDepartment();
        }    
        protected void LoadDepartment()
        {
            try
            {
                this.ddlDepart.Items.Clear();

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
                    li = new ListItem("--select--", "");
                    this.ddlDepart.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlDepart.Items.Add(li);
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

        protected void LoadBusinessCodes()
        {
            try
            {
                this.ddlBizCode.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadImprestProgram";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlBizCode.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Name"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlBizCode.Items.Add(li);
                            }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void LoadResponsibilityCenters()
        {
            try
            {
                this.ddlRCentre.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadResponsibilityCentre";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlRCentre.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Code"].ToString(),
                                    dr["Code"].ToString()
                                );

                                this.ddlRCentre.Items.Add(li);
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
        private void BindGridviewData(string number)
        {
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetPRNLines";
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
        public static string GetPrnReqNo()
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
                    cmds.CommandText = "spGetPurchaseReqNo";
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
                cmds.CommandText = "spUpdatePurchaseReqNo";
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
                string userID = Session["username"].ToString();
                string BizCode = ddlBizCode.SelectedItem.Value;
                string Biz = ddlBizCode.SelectedItem.Text.ToString().Replace("'", "");
                string DeptCode = ddlDepart.SelectedValue;
                string depts = ddlDepart.SelectedItem.Text.ToString().Replace("'", "");
                string Rcentre = ddlRCentre.SelectedItem.Value;

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
                    MyComponents.ObjNav.PurchaseHeaderCreate(BizCode, DeptCode, Rcentre, userID, Purpose);
                    string lastPRN = getLastPRN(userID);
                    Session["ReqNo"] = lastPRN;
                    lblNo.Text = Session["ReqNo"].ToString();
                    Message("Purchase Requisition Number " + RqNum + " created successfully");
                    btncancel.Visible = false;
                    newView();
                }
                catch (Exception exception)
                {
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
        protected string getLastPRN(string userID)
        {
            string La = "";
            try
            {
                #region commented - using webservice
                string staffPassChanged = MyComponents.ObjNav.GetLastPRNNo(userID);

                if (!String.IsNullOrEmpty(staffPassChanged))
                {
                    string[] strdelimiters = new string[] { "::" };
                    string[] staffLoginInfo_arr = staffPassChanged.Split(strdelimiters, StringSplitOptions.None);

                    La = staffLoginInfo_arr[0];
                }
                #endregion}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return La;
        }
        protected void newView()
        {
            MultiView1.ActiveViewIndex = 1;
            newLines.Visible = true;
            lbnAddLine.Visible = false;
            lblLNo.Text = Session["ReqNo"].ToString();
            LoadItemss();
            LoadLocations();
            LoadUnitsOFM();
            BindGridviewData(Session["ReqNo"].ToString());
        }
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                string type = ddlType.SelectedValue;
                if (string.IsNullOrEmpty(type.ToString()))
                {
                    Message("Type cannot be null.");
                    ddlType.Focus();
                    return;
                }

                string Functions = ddlItems.SelectedValue;
                if (string.IsNullOrEmpty(Functions.ToString()))
                {
                    Message("Item cannot be null.");
                    ddlItems.Focus();
                    return;
                }

                string FunctionsDesc = ddlItems.SelectedItem.Text.ToString().Trim();
                if (string.IsNullOrEmpty(FunctionsDesc.ToString()))
                {
                    Message("Item description cannot be null.");
                    ddlItems.Focus();
                    return;
                }

                string LocationNo = ddlLoc.SelectedValue.ToString();
                if (string.IsNullOrEmpty(LocationNo.ToString()))
                {
                    Message("Location cannot be null.");
                    ddlLoc.Focus();
                    return;
                }

                string Units = ddlUnits.SelectedValue.ToString();
                if (string.IsNullOrEmpty(Units.ToString()))
                {
                    Message("Unit of measure cannot be null.");
                    ddlUnits.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtQty.Text.ToString()))
                {
                    Message("Quantity Requested cannot be null.");
                    txtQty.Focus();
                    return;
                }
                decimal Quantity = Convert.ToDecimal(txtQty.Text.ToString());

                // string mytype = type.Text;
                string SREQNo = lblLNo.Text.ToString();
                DateTime DateExp = DateTime.Today;

                //Insert/ Update stored procedure
                MyComponents.ObjNav.SubmitPurchaseLine(Convert.ToInt32(type), SREQNo, Functions, LocationNo, DateExp, FunctionsDesc, Units, Quantity);
                Message("Line added successfully!");
                ddlType.SelectedIndex = 0;
                ddlItems.SelectedIndex = 0;
                ddlUnits.SelectedIndex = 0;
                ddlLoc.SelectedIndex = 0;
                txtQty.Text = null;

                BindGridviewData(Session["ReqNo"].ToString());
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
            LoadHDetails();
            LoadItemss();
            LoadLocations();
            LoadUnitsOFM();
            newLines.Visible = true;
            lbnAddLine.Visible = false;
        }
        protected void LoadItemss()
        {
            try
            {
                this.ddlItems.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    if (ddlType.SelectedValue == "0")
                    {
                        sqlStmt = "spGetStdTexts";
                    }
                    else if (ddlType.SelectedValue == "1")
                    {
                        sqlStmt = "spGetPRNGLAccnts";
                    }
                    else if (ddlType.SelectedValue == "2")
                    {
                        sqlStmt = "spGetPRNItems";
                    }
                    else if (ddlType.SelectedValue == "3")
                    {
                        sqlStmt = "spGetPRNFixedAssets";
                    }
                    else if (ddlType.SelectedValue == "4")
                    {
                        sqlStmt = "spGetPRNChargeItems";
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlItems.Items.Add(li);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Description"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlItems.Items.Add(li);
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
        protected void LoadLocations()
        {
            try
            {
                this.ddlLoc.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetPRNLocation";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlLoc.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Name"].ToString(),
                                    dr["Code"].ToString()
                                );

                                this.ddlLoc.Items.Add(li);
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
        protected void LoadUnitsOFM()
        {
            try
            {
                this.ddlUnits.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetPRNUnitsOfMsre";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlUnits.Items.Add(li);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Code"].ToString(),
                                    dr["Code"].ToString()
                                );

                                this.ddlUnits.Items.Add(li);
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
        protected void LoadHDetails()
        {
            string RequiNo = "";
            if (Session["ReqNo"] == null)
            {
                RequiNo = Request.QueryString["An"].ToString();
                Session["ReqNo"] = RequiNo;
            }
            else
            {
                RequiNo = Session["ReqNo"].ToString();
            }

            try
            {
                string[] strdelimiters = new string[] { "::" };
                string staffLoginInfo = MyComponents.ObjNav.GetPRNHeaderDetails(RequiNo);

                if (!String.IsNullOrEmpty(staffLoginInfo))
                {
                    DateTime expeDate = DateTime.Today; ;

                    string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                    expeDate = Convert.ToDateTime(staffLoginInfo_arr[0]);
                    Session["ReqDate"] = expeDate;
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

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            string username = Session["username"].ToString();
            //Session["username"].ToString()
            try
            {
                int i = gvLines.Rows.Count;
                if (i == 0)
                {
                    Message("warning! Please add lines first!");
                    return;
                }
                LoadHDetails();
                MyComponents.ObjNav.PRNApprovalRequest(Session["ReqNo"].ToString());
                Response.Redirect("PurchaseReqListing.aspx");
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
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

        protected void btncancel_Click(object sender, EventArgs e)
        {
            string Numm = Request.QueryString["An"].ToString();
            try
            {
                MyComponents.ObjNav.CancelPrnApprovalRequest(Numm);

                Message("Approval request successfully cancelled!");
                btnApproval.Visible = true;
                btncancel.Visible = false;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
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
                            // txtAmnt.Text = Convert.ToInt32(Convert.ToDouble(sdr["Unit Cost"])).ToString();
                            //  txtUnit.Text = sdr["Base Unit of Measure"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItemss();
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
                MyComponents.ObjNav.RemovePurchaseLine(LineNo);
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
        }
    }
}