using INSPECTORATEStaff;
using INSPECTORATEStaff.NAVWS;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Windows.UI.Xaml;

namespace INSPECTORATEStaff.pages
{
    public partial class Payslip : System.Web.UI.Page
    {
        string[] strLimiters2 = new string[] { "[]" };
        WebPortals webportals = MyComponents.ObjNav;
        string[] strLimiters = new string[] { "::" };
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadYears();
                LoadMonths();
                lbtnViewpayslip_Click();
                // GetPayslip(DdMonth.SelectedValue, ddlYear.SelectedValue);
            }
        }
        protected void LoadYears()
        {
            try
            {

                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetPayslipYears";
                    SqlCommand cmdProgStage = new SqlCommand();
                    cmdProgStage.CommandText = sqlStmt;
                    cmdProgStage.Connection = connToNAV;
                    cmdProgStage.CommandType = CommandType.StoredProcedure;
                    cmdProgStage.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    using (SqlDataReader sqlReaderStages = cmdProgStage.ExecuteReader())
                    {
                        if (sqlReaderStages.HasRows)
                        {
                            ddlYear.DataSource = sqlReaderStages;
                            ddlYear.DataTextField = "Period Year";
                            ddlYear.DataValueField = "Period Year";
                            ddlYear.DataBind();
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
        protected void LoadYears1()
        {
           
            try
            {
                connection = MyComponents.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPayslipYears",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    ddlYear.DataSource = reader;
                    ddlYear.DataTextField = "Period Year";
                    ddlYear.DataValueField = "Period Year";
                    ddlYear.DataBind();
                }
            }
            catch (Exception ex)
            {

                ex.Data.Clear();
            }
        }

        protected void LoadMonths()
        {
            
            try
            {
                DdMonth.Items.Clear();
                string year = ddlYear.SelectedValue;
                Console.WriteLine($"Selected Year: {year}");
                int CurrentYear = Convert.ToInt32(year);

                string payslipMonths = webportals.GetPayslipMonths(CurrentYear);
                if (!string.IsNullOrEmpty(payslipMonths))
                {
                    string[] monthsArr = payslipMonths.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string months in monthsArr)
                    {

                        string[] responseArr = months.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 2)
                        {
                            string monthNumber = responseArr[0];
                            string monthName = responseArr[1];


                            ListItem li = new ListItem(monthName, monthNumber);
                            DdMonth.Items.Add(li);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
       
        private void lbtnViewpayslip_Click()
        {
            try
            {
                string username = Session["username"].ToString();
                var filename = Session["username"].ToString().Replace(@"/", @"");
                var month = DdMonth.SelectedValue;

                if (month == "12")
                {
                    month = "12";

                }
                else if (month == "11")
                {
                    month = "11";
                }
                else if (month == "10")
                {
                    month = "10";
                }
                else if (month == "")
                {
                    month = "01";
                }
                else
                {
                    month = "0" + month;
                }

                var myDate = month + "/01/" + ddlYear.SelectedValue;
                var period = DateTime.ParseExact(myDate, "M/dd/yyyy", CultureInfo.InvariantCulture);

                var filePath = Server.MapPath("~/Download/") + String.Format("PAYSLIP-{0}.pdf", filename);

                // Check if directory exists, if not create it
                if (!Directory.Exists(Server.MapPath("~/Download/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Download/"));
                }
                webportals.GeneratePayslipReport2(username, period, String.Format(@"PAYSLIP-{0}.pdf", filename));

               // myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format(@"PAYSLIP-{0}.pdf", filename)));
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Payslip generated successfully.");
                    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP-{0}.pdf", filename)));
                }
                else
                {
                    throw new FileNotFoundException("Payslip PDF was not found after generation.");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }



        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMonths();
            lbtnViewpayslip_Click();
        }

        protected void DdMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbtnViewpayslip_Click();
        }
    }
}