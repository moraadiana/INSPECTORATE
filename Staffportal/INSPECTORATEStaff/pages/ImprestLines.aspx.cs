using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class ImprestLines : System.Web.UI.Page
    {
        public string ImpNo, IReqDate, IPurpose, ICompas, IRespon, IDept = "";
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
                    //MultiView1.ActiveViewIndex = 1;
                    MultiView1.ActiveViewIndex = 0;
                    Session["ReqNo"] = Request.QueryString["An"].ToString();
                    BindGridviewData(Request.QueryString["An"].ToString());
                    if (ApprovalSent(Request.QueryString["An"].ToString()) == "8")
                    {
                        if (ApprovedRejected(Request.QueryString["An"].ToString()) == true)
                        {
                            //btnApproval.Visible = false;
                            //btncancel.Visible = false;
                            //lbnAddLine.Visible = false;
                        }
                        else
                        {
                            //btnApproval.Visible = false;
                            //btncancel.Visible = true;
                            //lbnAddLine.Visible = false;
                        }
                    } 
                    else if (ApprovalSent(Request.QueryString["An"].ToString()) == "9")
                    {
                        //btnApproval.Visible = false;
                        //btncancel.Visible = false;
                        //lbnAddLine.Visible = false;
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
                    lblNo.Text = spGetImpReqNo();
                    //lbluserId.Text = MyComponents.UserID;
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
                    sqlStmt = "spGetImprestReqApprovalStaus";
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
            // LoadCurrency();
            //LoadPrograms();
            LoadResponsibilityCenters();
            LoadRegions();
        }
        //protected void LoadCurrency()
        //{
        //    try
        //    {
        //        this.ddlCurrency.Items.Clear();

        //        using (SqlConnection connToNav = MyComponents.getconnToNAV())
        //        {

        //            string q = null;
        //            SqlCommand cmd = new SqlCommand();
        //            q = "spGetCurrency";
        //            cmd.CommandText = q;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Connection = connToNav;
        //            cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

        //            ListItem li = null;
        //            li = new ListItem("--SELECT--", "");
        //            this.ddlCurrency.Items.Add(li);

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                if (dr.HasRows)
        //                    while (dr.Read())
        //                    {
        //                        li = new ListItem(
        //                            dr["Description"].ToString().ToUpper(),
        //                            dr["Code"].ToString()
        //                        );

        //                        this.ddlCurrency.Items.Add(li);
        //                    }
        //            }
        //            connToNav.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //        ex.Data.Clear();
        //    }
        //}
        /*
        protected void LoadPrograms()
        {
            try
            {
                this.ddlPrograms.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spLoadImprestProgram";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    //ddlPrograms.Items.Insert(0, new ListItem(String.Empty));
                    ListItem li = null;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(
                                    dr["Name"].ToString(),
                                    dr["Code"].ToString()
                                );

                                this.ddlPrograms.Items.Add(li);
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
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spLoadResponsibilityCentre";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
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
        protected void LoadRegions()
        {
            try
            {
                this.ddlregions.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {

                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetDepartmentList";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
                    //li = new ListItem();
                    //this.ddlregions.Items.Add(li);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Code"].ToString(),
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
        protected void AdvanceTypes()
        {
            try
            {
                this.ddlAdvancType.Items.Clear();

                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {

                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spLoadAdvancedTypes";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);

                    ListItem li = null;
                    li = new ListItem("--Select--", "");
                    this.ddlAdvancType.Items.Add(li);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new ListItem(

                                    dr["Code"].ToString() + " - " + dr["Description"].ToString().ToUpper(),
                                    dr["Code"].ToString()
                                );

                                this.ddlAdvancType.Items.Add(li);
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
        public static string spGetImpReqNo()
        {
            //string finalreturned = "";
            string newAppNo = "";

            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    SqlCommand cmds = new SqlCommand();
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.Connection = connToNAV;
                    cmds.CommandText = "spGetImpReqNo";
                    cmds.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    using (SqlDataReader rdr = cmds.ExecuteReader())
                    {
                        if (rdr.HasRows == true)
                        {
                            rdr.Read();
                            newAppNo = rdr["No_"].ToString();
                        }
                    }
                    connToNAV.Close();
                }
                //UpdateApplicationNo(newAppNo, conn);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return newAppNo;
        }


        private void BindGridviewData(string number)
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
                            //gvLines.DataSource = dt;
                            //gvLines.DataBind();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //string userID = MyComponents.UserID;
               // string RequiNo = Session["ReqNo"].ToString();
                string userID = Session["username"].ToString();
                string username = Session["username"].ToString();
                string userz = MyComponents.Customer;
                string campusCode = "INSPECTORATE";
                string CampusName = "INSPECTORATE";
                string RequiredDate = dtRequiredDate.Text.ToString();
                string ResponsibilityCentre = ddlresponibilitycentres.SelectedValue.ToString();
                string RegionsCode = ddlregions.SelectedValue;
                string RegionsCodeName = ddlregions.SelectedItem.Text.ToString().Replace("'", "");
                string AccntType = "Customer";

                if (userID.Length < 2)
                {
                    Message("Error! You do not have user ID please visit HR office to update your profile");
                    return;
                }

                string Purpose = txtHdescription.Text.ToString().Replace("'", "").Trim();
                if (Purpose.Length > 1000)
                {
                    Message("Purpose cannot have more than 1000 characters");
                    txtHdescription.Focus();
                    return;
                }
                {
                    //string DocumentNo = "";
                    try
                    {
                        string DocumentNo = spGetImpReqNo();
                        string user = Session["username"].ToString();
                        DateTime AttachDT = DateTime.Now;
                        //string filext = Path.GetExtension(fileName).Split('.')[1].ToLower();
                        //string attchby = "";
                        //int identity=0;
                        //string ftype = "";
                        int tblId = 1173;
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
                    Message("Imprest request made successfully!");
                    string Submitted = MyComponents.ObjNav.ImprestRequisitionCreate(userz, campusCode, RegionsCode, CampusName, RegionsCodeName, ResponsibilityCentre, AccntType, username, Purpose);

                    //returnMsg::changedPassword::staffNo::staffUserID::staffName
                    if (!String.IsNullOrEmpty(Submitted))
                    {
                        string[] strdelimiters = new string[] { "::" };
                        string[] staffLoginInfo_arr = Submitted.Split(strdelimiters, StringSplitOptions.None);

                        Session["ReqNo"] = staffLoginInfo_arr[0];
                    }

                    Session["ReqDate"] = RequiredDate;
                    Session["Purpose"] = Purpose;
                    Session["Campus"] = campusCode;
                    Session["Dept"] = RegionsCode;

                    sendapproval();
                    Message("Imprest created successfully!");
                    
                    //btncancel.Visible = false;
                    //newView();
                }
            }
            catch (Exception Ex)
            {
                Message("ERROR: " + Ex.Message.ToString());
                Ex.Data.Clear();
            }
        }

        /*
        protected void lbnAddLine_Click(object sender, EventArgs e)
        {
            lblLNo.Text = Session["ReqNo"].ToString();
            LoadHDetails();
            AdvanceTypes();
         //   newLines.Visible = true;
            lbnAddLine.Visible = false;
           // lbnClose.Visible = true;
        }*/
        /*
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
            string purp = "";
            if (Session["Purpose"] == null)
            {
                purp = Request.QueryString["An"].ToString();
                Session["Purpose"] = purp;
            }
            else
            {
                purp = Session["Purpose"].ToString();
            }
            string dat = "";
            if (Session["ReqDate"] == null)
            {
                dat = Request.QueryString["An"].ToString();
                Session["ReqDate"] = dat;
            }
            else
            {
                dat = Session["ReqDate"].ToString();
            }
            string cump = "";
            if (Session["Campus"] == null)
            {
                cump = Request.QueryString["An"].ToString();
                Session["Campus"] = cump;
            }
            else
            {
                cump = Session["Campus"].ToString();
            }
            string dept = "";
            if (Session["Dept"] == null)
            {
                dept = Request.QueryString["An"].ToString();
                Session["Dept"] = dept;
            }
            else
            {
                dept = Session["Dept"].ToString();
            }
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetReqHeaderDetails";
                    SqlCommand cmdGetStaffDetails = new SqlCommand();
                    cmdGetStaffDetails.CommandText = sqlStmt;
                    cmdGetStaffDetails.Connection = connToNAV;
                    cmdGetStaffDetails.CommandType = CommandType.StoredProcedure;
                    cmdGetStaffDetails.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdGetStaffDetails.Parameters.AddWithValue("@ReqNo", "'" + RequiNo + "'");

                    using (SqlDataReader sdr = cmdGetStaffDetails.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            Session["ReqNo"] = sdr["No_"].ToString();
                            Session["ReqDate"] = sdr["Date"].ToString();
                            Session["Purpose"] = sdr["Purpose"].ToString();
                            Session["Campus"] = sdr["Global Dimension 1 Code"].ToString();
                            Session["Dept"] = sdr["Shortcut Dimension 2 Code"].ToString();
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
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
                //LoadHDetails();
                MyComponents.ObjNav.ImprestRequisitionApprovalRequest(Session["ReqNo"].ToString());
                Response.Redirect("ImprestListing.aspx");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
                dvMdlContentFail.Visible = false;
                dvMdlContentPass.Visible = true;
                return "";
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
                return  "";
            }
        }
        protected void UpdateApproval(SqlConnection conn, string num, string status)
        {
            try
            {
                string stsmnts = null;
                SqlCommand cmd = new SqlCommand();
                stsmnts = "spUpdateImpresApproval";
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
        /*protected void lbnClose_Click(object sender, EventArgs e)
        {
            //newLines.Visible = false;
            lbnAddLine.Visible = true;
            lbnClose.Visible = false;
        }*/

        protected void btncancel_Click(object sender, EventArgs e)
        {
            string Numm = Request.QueryString["An"].ToString();

            try
            {

                MyComponents.ObjNav.CancelImprestRequisition(Numm);

                //btnApproval.Visible = true;
                //btncancel.Visible = false;

                Response.Redirect("ImprestListing.aspx");

            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
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
        }*/
        public static void UpdateApplicationNo(string LastNoUsed, SqlConnection conn)
        {
            try
            {
                SqlCommand cmds = new SqlCommand();
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection = conn;
                cmds.CommandText = "spUpdateImprestNo";
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
        protected string[] AccountDetails(string code)
        {
            string AccountCode, AccountDesc = "";
            string[] myData = new string[2];
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spLoadImprestAccDetails";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlStmt;
                    cmd.Connection = connToNAV;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@Code", "'" + code + "'");

                    using (SqlDataReader sqlReaderDetails = cmd.ExecuteReader())
                    {
                        if (sqlReaderDetails.HasRows)
                        {
                            sqlReaderDetails.Read();
                            AccountCode = sqlReaderDetails["G_L Account"].ToString();
                            AccountDesc = sqlReaderDetails["Description"].ToString();

                            myData[0] = AccountCode;
                            myData[1] = AccountDesc;
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
        /*
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAmnt.Text == null)
                {
                    Message("Amount cannot be null!");
                    return;
                }
                string ATypes = ddlAdvancType.SelectedValue;
                string Amnt = txtAmnt.Text;

                string[] accnt = AccountDetails(ATypes);
                string Num = accnt[0];
                string name = accnt[1];
                string i = ImpNo;

                
                #region commented - using webservice
                string staffLoginInfo = MyComponents.ObjNav.InsertImprestRequisitionLines(Session["ReqNo"].ToString(), ATypes, Num, name, Convert.ToDecimal(Amnt), Session["username"].ToString());

                if (!String.IsNullOrEmpty(staffLoginInfo))
                    {
                        string returnMsg = "";
                        string[] strdelimiters = new string[] { "::" };
                        string[] staffLoginInfo_arr = staffLoginInfo.Split(strdelimiters, StringSplitOptions.None);

                        returnMsg = staffLoginInfo_arr[0];
                        if (returnMsg == "SUCCESS")
                        {
                        Message("Line added successfully!");
                        txtAmnt.Text = null;
                        BindGridviewData(Session["ReqNo"].ToString());
                    }
                        }
                    #endregion
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex);
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }
        protected void cancelz(object sender, EventArgs e)
        {
            string message = "Are you sure you want to remove this line?";
            ClientScript.RegisterOnSubmitStatement(this.GetType(), "confirm", "return confirm('" + message + "');");
            string[] arg = new string[2];
            arg = (sender as LinkButton).CommandArgument.ToString().Split(';');
            string Atype = arg[0];
            string reqNo = Session["ReqNo"].ToString();
            try
            {
                MyComponents.ObjNav.RemoveImprestReqLine(reqNo, Atype); 
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