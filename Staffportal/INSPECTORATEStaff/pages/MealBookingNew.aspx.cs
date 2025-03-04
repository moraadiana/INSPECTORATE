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
    public partial class MealBookingNew : System.Web.UI.Page
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
                    string bbb = HttpUtility.UrlDecode(Request.QueryString["An"]).ToString();

                    AES2 AES2 = new AES2();
                    if (String.IsNullOrEmpty(bbb)) { Response.Redirect("MealListing.aspx"); }
                    string md = AES2.Decrypt(bbb);
                    Session["ReqNo"] = md.ToString();

                    MultiView1.ActiveViewIndex = 1;
                    BindGridviewData(md);
                    if (ApprovalSent(md) == true)
                    {
                        if (ApprovedRejected(md) == true)
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
                    //lblNo.Text = GetSoreReqNo();
                    lbluserId.Text = MyComponents.UserID;
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
        protected bool ApprovalSent(string ReqNUM)
        {
            bool approve = false;
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetImprestSentForApproval";
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
        protected bool ApprovedRejected(string ReqNUM)
        {
            bool approve = false;
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetImprestApprovedRejected";
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
                    li = new ListItem("--select--", "0");
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
        private void BindGridviewData(string number)
        {
            try
            {
                using (SqlConnection con = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spMealReqLines";
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
        public static string GetMealReqNo()
        {
            //string finalreturned = "";
            string newAppNo = "";
            string username = MyComponents.UserID;

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    SqlCommand cmds = new SqlCommand();
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.Connection = connToNAV;
                    cmds.CommandText = "spGetMealID";
                    cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmds.Parameters.AddWithValue("@username", "'" + username + "'");
                    using (SqlDataReader rdr = cmds.ExecuteReader())
                    {
                        if (rdr.HasRows == true)
                        {
                            rdr.Read();
                            newAppNo = rdr["Booking Id"].ToString();
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
        private void Message(string p)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + p + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }
        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //Collect data
                //string RqNum = lblNo.Text;
                string CntcNme = Session["StaffName"].ToString();
                string deptsName = ddlregions.SelectedItem.Text.Replace("'", "");
                string depts = ddlregions.SelectedValue.ToString();
                if (depts == "0")
                {
                    Message("Warning! Please select department.");
                    ddlregions.Focus();
                    return;
                }
                string Venue = txtVenue.Text.ToString().Replace("'", "");
                if (string.IsNullOrEmpty(Venue))
                {
                    Message("Warning! Venue cannot be bull.");
                    txtVenue.Focus();
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
                    Message("Travel description cannot have more than 200 characters");
                    txtDesc.Focus();
                    return;
                }
                #region SendforApproval

                try
                {
                    //MyComponents.ObjNav.MealRequisitionCreate(Session["username"].ToString(), depts, Convert.ToDateTime(BkingDate), Purpose, RequiredTime, Venue, CntcNme, CntctNo, Email, Pax, deptsName, userID, RequiredTime);

                    Session["ReqNo"] = GetMealReqNo();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                    btncancel.Visible = false;
                    newView();
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
        protected void newView()
        {
            MultiView1.ActiveViewIndex = 1;
            newLines.Visible = true;
            lbnAddLine.Visible = false;
            lblLNo.Text = Session["ReqNo"].ToString();
            LoadMeals();
            BindGridviewData(Session["ReqNo"].ToString());
        }
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                string Quantity = txtQnty.Text.ToString();
                string Amnt = lblPrice.Text.ToString();
                if (string.IsNullOrEmpty(Quantity))
                {
                    Message("Quantity Requested cannot be null.");
                    txtQnty.Focus();
                    return;
                }
                string MealNo = ddlMeals.SelectedValue;
                string MealDesc = ddlMeals.SelectedItem.Text.ToString().Trim();

                // string mytype = type.Text;
                string SREQNo = lblLNo.Text.ToString();
                string LAmnt = Convert.ToString(Convert.ToDouble(Amnt) * Convert.ToDouble(Quantity));

                #region SendforApproval

                try
                {
                   //MyComponents.ObjNav.MealReqLines(SREQNo, MealNo, MealDesc, Convert.ToDecimal(Quantity), Convert.ToDecimal(Amnt), Convert.ToDecimal(LAmnt));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                    txtQnty.Text = null;
                    //lblPrice.Text = null;
                    BindGridviewData(Session["ReqNo"].ToString());
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
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }

        protected void lbnAddLine_Click(object sender, EventArgs e)
        {
            string bbb = HttpUtility.UrlDecode(Request.QueryString["An"]).ToString();

            AES2 AES2 = new AES2();
            if (String.IsNullOrEmpty(bbb)) { Response.Redirect("MealListing.aspx"); }
            string md = AES2.Decrypt(bbb);
            lblLNo.Text = md;
            //LoadHDetails();
            LoadMeals();
            newLines.Visible = true;
            lbnAddLine.Visible = false;
        }
        protected void LoadMeals()
        {
            try
            {
                this.ddlMeals.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetFood";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNav;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
                    li = new ListItem("----Select---", "");
                    this.ddlMeals.Items.Add(li);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Description"].ToString().ToUpper(),
                                    dr["No_"].ToString()
                                );

                                this.ddlMeals.Items.Add(li);
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
        protected void lbnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbnAddLine.Visible = true;
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                int i = gvLines.Rows.Count;
                if (i == 0)
                {
                    Message("warning! Please add lines first!");
                    return;
                }
                // LoadHDetails();
               // MyComponents.ObjNav.MealBookingApprovalRequest(Session["ReqNo"].ToString());
                //InsertLeaveApprovalEntries(MyComponents.UserID, Session["ReqNo"].ToString());
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                dvMdlContentFail.Visible = false;
                dvMdlContentPass.Visible = true;
                Response.Redirect("MealListing.aspx");
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
                stsmnts = "spUpdateMealApproval";
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

        private string GetEmail(string UserId)
        {
            string email = "";
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetApproverEmail";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@approverId", "'" + UserId + "'");

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            email = dr["Company E-Mail"].ToString();
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
            return email;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            string bbb = HttpUtility.UrlDecode(Request.QueryString["An"]).ToString();

            AES2 AES2 = new AES2();
            if (String.IsNullOrEmpty(bbb)) { Response.Redirect("MealListing.aspx"); }
            string md = AES2.Decrypt(bbb);
            string Numm = md;
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

                    //Message("Approval request successfully cancelled!");
                    UpdateApproval(connToNAV, Numm, "0");
                    btnApproval.Visible = true;
                    btncancel.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                    dvMdlContentFail.Visible = false;
                    dvMdlContentPass.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void getUnitCost()
        {
            string item = ddlMeals.SelectedValue.ToString();
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
                            lblPrice.Text = Convert.ToInt32(Convert.ToDouble(sdr["Unit Price"])).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUnitCost();
        }
    }
}