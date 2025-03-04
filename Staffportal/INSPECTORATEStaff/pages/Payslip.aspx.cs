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
                if (Session["username"] == null)
                {
                    throw new Exception("Session expired or user not logged in.");
                }

                string username = Session["username"].ToString();
                string filename = username.Replace("/", ""); // Avoid invalid characters

                string month = DdMonth.SelectedValue.PadLeft(2, '0'); // Ensure two-digit month format
               string myDate = month + "/01/" + ddlYear.SelectedValue;
                string[] formats = { "MM/dd/yyyy", "dd/MM/yyyy", "yyyy-MM-dd" };
                if (!DateTime.TryParseExact(myDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime period))
                {
                    throw new FormatException($"Invalid date format: {myDate}");
                }
               // DateTime period= DateTime.ParseExact(myDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                System.Diagnostics.Debug.WriteLine($"period: {period}");
                System.Diagnostics.Debug.WriteLine($"Running on .NET Version: {Environment.Version}");

                string filePath = Server.MapPath("~/Download/") + $"PAYSLIP-{filename}.pdf";

                // Ensure directory exists
                if (!Directory.Exists(Server.MapPath("~/Download/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Download/"));
                }

                // Generate the payslip
                MyComponents.ObjNav.GeneratePayslipReport(username, period, $"PAYSLIP-{filename}.pdf");

                // Check if file was generated
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Payslip generated successfully.");
                    myPDF.Attributes.Add("src", ResolveUrl($"~/Download/PAYSLIP-{filename}.pdf"));
                }
                else
                {
                    throw new FileNotFoundException("Payslip PDF was not found after generation.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
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
//private void lbtnViewpayslip_Click2()
//{
//    try
//    {
//        string username = Session["username"].ToString();

//        var filename = Session["username"].ToString().Replace(@"/", @"");
//        // DateTime period = DateTime.ParseExact($"{ddlMonth.SelectedValue.PadLeft(2, '0')}/01/{ddlYear.SelectedValue}", "MM/dd/yyyy", CultureInfo.InvariantCulture);
//        var month = DdMonth.SelectedValue;

//        if (month == "12")
//        {
//            month = "12";

//        }
//        else if (month == "11")
//        {
//            month = "11";
//        }
//        else if (month == "10")
//        {
//            month = "10";
//        }
//        else if (month == "")
//        {
//            month = "01";
//        }
//        else
//        {
//            month = "0" + month;
//        }

//        var myDate = month + "/01/" + ddlYear.SelectedValue;
//        var period = DateTime.ParseExact(myDate, "M/dd/yyyy", CultureInfo.InvariantCulture);

//        try
//        {
//            string returnstring = "";
//            //MyComponents.ObjNav.Generatep9Report(period, employee, String.Format("p9Form{0}.pdf", filename), ref returnstring);

//            //  MyComponents.ObjNav.GeneratePaySlipReport1(username, period, String.Format("PAYSLIP{0}.pdf", filename), ref returnstring);
//            MyComponents.ObjNav.GeneratePaySlipReport1(Session["username"].ToString(), period, String.Format("PAYSLIP{0}.pdf", filename));

//            myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP{0}.pdf", filename)));

//            // byte[] bytes = Convert.FromBase64String(returnstring);
//            string path = HostingEnvironment.MapPath("~/Download/" + $"PAYSLIP{filename}.pdf");


//            if (System.IO.File.Exists(path))
//            {
//                System.IO.File.Delete(path);
//            }
//            FileStream stream = new FileStream(path, FileMode.CreateNew);
//            BinaryWriter writer = new BinaryWriter(stream);
//            //  writer.Write(bytes, 0, bytes.Length);
//            writer.Close();
//            myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP{0}.pdf", filename)));
//        }
//        catch (Exception exception)
//        {
//            exception.Data.Clear();

//        }

//    }
//    catch (Exception ex)
//    {
//        ex.Data.Clear();
//    }
//}
//protected void lbtnViewpayslip_Click1()
//{
//    //var s =Convert.ToDateTime(period.ToString("M/dd/yyyy", CultureInfo.InvariantCulture));
//    try
//    {
//        //  var filename = Session["username"].ToString().Replace(@"/", @"");
//        string username = Session["username"].ToString();
//        var filename = Session["username"].ToString().Replace(@"/", @"");
//        string pdfFileName = String.Format(@"PAYSLIP{0}.pdf", filename);
//        var month = DdMonth.SelectedValue;

//        if (month == "12")
//        {
//            month = "12";

//        }
//        else if (month == "11")
//        {
//            month = "11";
//        }
//        else if (month == "10")
//        {
//            month = "10";
//        }
//        else
//        {
//            month = "0" + month;

//        }

//        var myDate = month + "/01/" + ddlYear.SelectedValue;


//        var period = DateTime.ParseExact(myDate, "M/dd/yyyy", CultureInfo.InvariantCulture);
//        string path = HostingEnvironment.MapPath("~/Download/" + $"PAYSLIP{filename}.pdf");
//        // Check if directory exists, if not create it

//        if (!Directory.Exists(Server.MapPath("~/Download/")))
//        {
//            Directory.CreateDirectory(Server.MapPath("~/Download/"));
//        }
//        MyComponents.ObjNav.GeneratePaySlipReport1(Session["username"].ToString(), period, String.Format("PAYSLIP{0}.pdf", filename));
//        myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP{0}.pdf", filename)));
//        //if (File.Exists(filePath))
//        //{
//        //    System.Diagnostics.Debug.WriteLine("Payslip generated successfully.");
//        //    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP-{0}.pdf", filename)));
//        //}
//        if (System.IO.File.Exists(path))
//        {
//            System.IO.File.Delete(path);
//        }
//        else
//        {
//            throw new FileNotFoundException("Payslip PDF was not found after generation.");
//        }


//    }
//    catch (Exception exception)
//    {
//        exception.Data.Clear();
//        HttpContext.Current.Response.Write(exception);
//    }
//}